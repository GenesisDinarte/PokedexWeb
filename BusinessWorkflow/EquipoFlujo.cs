using PokedexWeb.DataAccess;
using PokedexWeb.Models;

namespace PokedexWeb.BusinessWorkflow;

public interface IEquipoFlujo
{
    List<MiembroEquipo> ObtenerEquipo();
    Task<(bool Exito, string Mensaje)> AgregarAsync(string nombre, string? apodo);
    (bool Exito, string Mensaje) CambiarApodo(int id, string? apodo);
    (bool Exito, string Mensaje) Eliminar(int id);
}

// BW: reglas del equipo. Usa el BW de Pokémon (que usa el SG) y el DA.
public class EquipoFlujo : IEquipoFlujo
{
    public const int MaximoEquipo = 6;

    private readonly IEquipoRepositorio _repositorio;
    private readonly IPokemonFlujo _pokemonFlujo;

    public EquipoFlujo(IEquipoRepositorio repositorio, IPokemonFlujo pokemonFlujo)
    {
        _repositorio = repositorio;
        _pokemonFlujo = pokemonFlujo;
    }

    public List<MiembroEquipo> ObtenerEquipo() => _repositorio.ObtenerTodos();

    public async Task<(bool Exito, string Mensaje)> AgregarAsync(string nombre, string? apodo)
    {
        // Regla 1: máximo 6 Pokémon
        if (_repositorio.ObtenerTodos().Count >= MaximoEquipo)
            return (false, $"Tu equipo ya tiene {MaximoEquipo} Pokémon. Libera uno primero.");

        // Regla 2: el Pokémon debe existir en PokeAPI
        var pokemon = await _pokemonFlujo.ObtenerPokemonAsync(nombre);
        if (pokemon == null)
            return (false, $"'{nombre}' no existe en PokeAPI.");

        // Regla 3: no se repiten Pokémon
        if (_repositorio.ObtenerPorId(pokemon.Id) != null)
            return (false, $"{pokemon.Nombre} ya está en tu equipo.");

        _repositorio.Agregar(new MiembroEquipo
        {
            Id = pokemon.Id,
            Nombre = pokemon.Nombre,
            Imagen = pokemon.Imagen,
            Tipos = pokemon.Tipos,
            TotalEstadisticas = pokemon.TotalEstadisticas,
            Apodo = string.IsNullOrWhiteSpace(apodo) ? null : apodo.Trim()
        });
        return (true, $"¡{pokemon.Nombre} se unió a tu equipo!");
    }

    public (bool Exito, string Mensaje) CambiarApodo(int id, string? apodo)
    {
        var miembro = _repositorio.ObtenerPorId(id);
        if (miembro == null)
            return (false, "Ese Pokémon no está en tu equipo.");

        miembro.Apodo = string.IsNullOrWhiteSpace(apodo) ? null : apodo.Trim();
        return (true, $"Apodo de {miembro.Nombre} actualizado.");
    }

    public (bool Exito, string Mensaje) Eliminar(int id)
    {
        var miembro = _repositorio.ObtenerPorId(id);
        if (miembro == null)
            return (false, "Ese Pokémon no está en tu equipo.");

        _repositorio.Eliminar(id);
        return (true, $"{miembro.Nombre} fue liberado. ¡Adiós, {miembro.Nombre}!");
    }
}
