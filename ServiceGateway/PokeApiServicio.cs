using System.Net;
using PokedexWeb.Models.PokeApi;

namespace PokedexWeb.ServiceGateway;

public class PokeApiServicio : IPokeApiServicio
{
    private readonly HttpClient _httpClient;

    // El HttpClient llega por inyección de dependencias,
    // ya configurado con la URL base (ver Program.cs)
    public PokeApiServicio(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PokemonRespuesta?> ObtenerPokemonAsync(string nombreOId)
    {
        // PokeAPI solo acepta nombres en minúscula
        var respuesta = await _httpClient.GetAsync($"pokemon/{nombreOId.Trim().ToLower()}");

        // 404 = el Pokémon no existe → devolvemos null en lugar de lanzar error
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            return null;

        respuesta.EnsureSuccessStatusCode();   // otro error (500, etc.) sí lanza excepción
        return await respuesta.Content.ReadFromJsonAsync<PokemonRespuesta>();
    }

    public async Task<ListaRespuesta?> ObtenerListaAsync(int offset, int limit)
    {
        return await _httpClient.GetFromJsonAsync<ListaRespuesta>($"pokemon?offset={offset}&limit={limit}");
    }

    public async Task<TipoRespuesta?> ObtenerTipoAsync(string tipo)
    {
        var respuesta = await _httpClient.GetAsync($"type/{tipo.Trim().ToLower()}");
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            return null;

        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadFromJsonAsync<TipoRespuesta>();
    }
}
