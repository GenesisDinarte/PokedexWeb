using PokedexWeb.Models.PokeApi;

namespace PokedexWeb.ServiceGateway;

// SG: contrato para hablar con el servicio EXTERNO (PokeAPI)
public interface IPokeApiServicio
{
    Task<PokemonRespuesta?> ObtenerPokemonAsync(string nombreOId);
    Task<ListaRespuesta?> ObtenerListaAsync(int offset, int limit);
    Task<TipoRespuesta?> ObtenerTipoAsync(string tipo);
}
