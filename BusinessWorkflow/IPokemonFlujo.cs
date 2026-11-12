using PokedexWeb.Models;

namespace PokedexWeb.BusinessWorkflow;

// BW: reglas de negocio que usa el controlador
public interface IPokemonFlujo
{
    Task<Pokemon?> ObtenerPokemonAsync(string nombreOId);
    Task<PaginaPokemon> ListarAsync(int pagina, int cantidad);
    Task<List<PokemonResumen>?> ListarPorTipoAsync(string tipo);
    Task<Comparacion?> CompararAsync(string pokemon1, string pokemon2);
}
