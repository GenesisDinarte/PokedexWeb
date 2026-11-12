using System.ComponentModel.DataAnnotations;

namespace PokedexWeb.Models;

// Formulario de búsqueda (Model Binding + validaciones de la Semana 2)
public class BusquedaFormulario
{
    [Required(ErrorMessage = "Escribe el nombre o número del Pokémon")]
    [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
    [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Solo letras, números y guiones (ej: mr-mime)")]
    [Display(Name = "Nombre o número")]
    public string Termino { get; set; } = string.Empty;
}

// Formulario para comparar dos Pokémon + el resultado
public class ComparacionFormulario
{
    [Required(ErrorMessage = "Indica el primer Pokémon")]
    [Display(Name = "Pokémon 1")]
    public string Pokemon1 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indica el segundo Pokémon")]
    [Display(Name = "Pokémon 2")]
    public string Pokemon2 { get; set; } = string.Empty;

    // Se llena después de consultar la API (no viene del formulario)
    public Comparacion? Resultado { get; set; }
}

// Un Pokémon guardado en "Mi equipo"
public class MiembroEquipo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Imagen { get; set; }
    public List<string> Tipos { get; set; } = new();
    public int TotalEstadisticas { get; set; }

    [StringLength(20, ErrorMessage = "El apodo admite máximo 20 caracteres")]
    public string? Apodo { get; set; }
}
