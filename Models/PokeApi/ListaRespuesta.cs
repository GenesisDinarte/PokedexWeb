using System.Text.Json.Serialization;

namespace PokedexWeb.Models.PokeApi;

// JSON de GET https://pokeapi.co/api/v2/pokemon?offset=0&limit=20
public class ListaRespuesta
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("results")]
    public List<RecursoConNombre> Results { get; set; } = new();
}

// JSON de GET https://pokeapi.co/api/v2/type/{tipo}
public class TipoRespuesta
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("pokemon")]
    public List<PokemonDeTipo> Pokemon { get; set; } = new();
}

public class PokemonDeTipo
{
    [JsonPropertyName("pokemon")]
    public RecursoConNombre Pokemon { get; set; } = new();
}
