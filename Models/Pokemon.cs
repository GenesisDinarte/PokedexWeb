namespace PokedexWeb.Models;

// Modelo PROPIO: lo que nuestra API devuelve (limpio y en español)
public class Pokemon
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal AlturaMetros { get; set; }
    public decimal PesoKg { get; set; }
    public int ExperienciaBase { get; set; }
    public string? Imagen { get; set; }
    public List<string> Tipos { get; set; } = new();
    public List<string> Habilidades { get; set; } = new();
    public List<Estadistica> Estadisticas { get; set; } = new();
    public int TotalEstadisticas => Estadisticas.Sum(e => e.Valor);
}

public class Estadistica
{
    public string Nombre { get; set; } = string.Empty;
    public int Valor { get; set; }
}

// Versión corta para listados (no requiere una llamada extra por Pokémon)
public class PokemonResumen
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Imagen { get; set; } = string.Empty;
}

public class PaginaPokemon
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Cantidad { get; set; }
    public int TotalPaginas => (int)Math.Ceiling(Total / (double)Cantidad);
    public List<PokemonResumen> Resultados { get; set; } = new();
}

public class Comparacion
{
    public Pokemon Pokemon1 { get; set; } = new();
    public Pokemon Pokemon2 { get; set; } = new();
    public string Ganador { get; set; } = string.Empty;
    public int Diferencia { get; set; }
}
