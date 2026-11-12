using System.Text.Json.Serialization;

namespace PokedexWeb.Models.PokeApi;

// Representa el JSON que devuelve GET https://pokeapi.co/api/v2/pokemon/{nombre}
// Solo mapeamos los campos que nos interesan; el resto se ignora.
public class PokemonRespuesta
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public int Height { get; set; }          // en decímetros

    [JsonPropertyName("weight")]
    public int Weight { get; set; }          // en hectogramos

    [JsonPropertyName("base_experience")]
    public int? BaseExperience { get; set; }

    [JsonPropertyName("sprites")]
    public Sprites Sprites { get; set; } = new();

    [JsonPropertyName("types")]
    public List<TipoSlot> Types { get; set; } = new();

    [JsonPropertyName("stats")]
    public List<StatSlot> Stats { get; set; } = new();

    [JsonPropertyName("abilities")]
    public List<HabilidadSlot> Abilities { get; set; } = new();
}

public class Sprites
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }

    [JsonPropertyName("other")]
    public OtrosSprites? Other { get; set; }
}

public class OtrosSprites
{
    [JsonPropertyName("official-artwork")]
    public ArteOficial? OfficialArtwork { get; set; }
}

public class ArteOficial
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }
}

public class TipoSlot
{
    [JsonPropertyName("slot")]
    public int Slot { get; set; }

    [JsonPropertyName("type")]
    public RecursoConNombre Type { get; set; } = new();
}

public class StatSlot
{
    [JsonPropertyName("base_stat")]
    public int BaseStat { get; set; }

    [JsonPropertyName("stat")]
    public RecursoConNombre Stat { get; set; } = new();
}

public class HabilidadSlot
{
    [JsonPropertyName("is_hidden")]
    public bool IsHidden { get; set; }

    [JsonPropertyName("ability")]
    public RecursoConNombre Ability { get; set; } = new();
}

// PokeAPI usa mucho este par { "name": "...", "url": "..." }
public class RecursoConNombre
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
