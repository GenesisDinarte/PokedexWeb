using PokedexWeb.Models;
using PokedexWeb.Models.PokeApi;
using PokedexWeb.ServiceGateway;

namespace PokedexWeb.BusinessWorkflow;

public class PokemonFlujo : IPokemonFlujo
{
    private const string UrlImagen =
        "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{0}.png";

    private readonly IPokeApiServicio _pokeApi;

    public PokemonFlujo(IPokeApiServicio pokeApi)
    {
        _pokeApi = pokeApi;
    }

    public async Task<Pokemon?> ObtenerPokemonAsync(string nombreOId)
    {
        var respuesta = await _pokeApi.ObtenerPokemonAsync(nombreOId);
        return respuesta == null ? null : Mapear(respuesta);
    }

    public async Task<PaginaPokemon> ListarAsync(int pagina, int cantidad)
    {
        // Regla de negocio: página mínima 1, cantidad entre 1 y 50
        pagina = Math.Max(1, pagina);
        cantidad = Math.Clamp(cantidad, 1, 50);

        var offset = (pagina - 1) * cantidad;
        var lista = await _pokeApi.ObtenerListaAsync(offset, cantidad);

        return new PaginaPokemon
        {
            Total = lista?.Count ?? 0,
            Pagina = pagina,
            Cantidad = cantidad,
            Resultados = lista?.Results.Select(CrearResumen).ToList() ?? new()
        };
    }

    public async Task<List<PokemonResumen>?> ListarPorTipoAsync(string tipo)
    {
        var respuesta = await _pokeApi.ObtenerTipoAsync(tipo);
        return respuesta?.Pokemon.Select(p => CrearResumen(p.Pokemon)).ToList();
    }

    public async Task<Comparacion?> CompararAsync(string pokemon1, string pokemon2)
    {
        // Las dos llamadas a PokeAPI se hacen en paralelo
        var tarea1 = ObtenerPokemonAsync(pokemon1);
        var tarea2 = ObtenerPokemonAsync(pokemon2);
        await Task.WhenAll(tarea1, tarea2);

        var p1 = tarea1.Result;
        var p2 = tarea2.Result;
        if (p1 == null || p2 == null)
            return null;

        return new Comparacion
        {
            Pokemon1 = p1,
            Pokemon2 = p2,
            Ganador = p1.TotalEstadisticas == p2.TotalEstadisticas ? "Empate"
                    : p1.TotalEstadisticas > p2.TotalEstadisticas ? p1.Nombre : p2.Nombre,
            Diferencia = Math.Abs(p1.TotalEstadisticas - p2.TotalEstadisticas)
        };
    }

    // ── Mapeo: JSON de PokeAPI (inglés) → nuestro modelo (español) ──
    private static Pokemon Mapear(PokemonRespuesta r) => new()
    {
        Id = r.Id,
        Nombre = r.Name,
        AlturaMetros = r.Height / 10m,     // decímetros → metros
        PesoKg = r.Weight / 10m,           // hectogramos → kg
        ExperienciaBase = r.BaseExperience ?? 0,
        Imagen = r.Sprites.Other?.OfficialArtwork?.FrontDefault ?? r.Sprites.FrontDefault,
        Tipos = r.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
        Habilidades = r.Abilities
            .Select(a => a.IsHidden ? $"{a.Ability.Name} (oculta)" : a.Ability.Name)
            .ToList(),
        Estadisticas = r.Stats
            .Select(s => new Estadistica { Nombre = s.Stat.Name, Valor = s.BaseStat })
            .ToList()
    };

    // La lista solo trae { name, url }. El Id viene al final de la url:
    // https://pokeapi.co/api/v2/pokemon/25/  →  25
    private static PokemonResumen CrearResumen(RecursoConNombre recurso)
    {
        var id = int.Parse(recurso.Url.TrimEnd('/').Split('/').Last());
        return new PokemonResumen
        {
            Id = id,
            Nombre = recurso.Name,
            Imagen = string.Format(UrlImagen, id)
        };
    }
}
