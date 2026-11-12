using Microsoft.AspNetCore.Mvc;
using PokedexWeb.BusinessWorkflow;
using PokedexWeb.Models;

namespace PokedexWeb.Controllers;

public class PokedexController : Controller
{
    private readonly IPokemonFlujo _flujo;

    public PokedexController(IPokemonFlujo flujo)
    {
        _flujo = flujo;
    }

    // GET /Pokedex?pagina=2
    public async Task<IActionResult> Index(int pagina = 1)
    {
        var resultado = await _flujo.ListarAsync(pagina, 24);
        return View(resultado);
    }

    // GET /Pokedex/Detalle/pikachu
    public async Task<IActionResult> Detalle(string id)
    {
        var pokemon = await _flujo.ObtenerPokemonAsync(id);
        if (pokemon == null)
            return NotFound();   // 404

        return View(pokemon);
    }

    // GET /Pokedex/Buscar  → muestra el formulario vacío
    public IActionResult Buscar()
    {
        return View(new BusquedaFormulario());
    }

    // POST /Pokedex/Buscar → Model Binding llena "formulario"
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buscar(BusquedaFormulario formulario)
    {
        if (!ModelState.IsValid)
            return View(formulario);

        var pokemon = await _flujo.ObtenerPokemonAsync(formulario.Termino);
        if (pokemon == null)
        {
            ModelState.AddModelError(nameof(formulario.Termino),
                $"No se encontró ningún Pokémon llamado '{formulario.Termino}'");
            return View(formulario);
        }

        return RedirectToAction(nameof(Detalle), new { id = pokemon.Nombre });
    }

    // GET /Pokedex/Tipo/fire
    public async Task<IActionResult> Tipo(string id = "fire")
    {
        var lista = await _flujo.ListarPorTipoAsync(id);
        if (lista == null)
            return NotFound();

        ViewBag.Tipo = id;
        return View(lista);
    }

    // GET /Pokedex/Comparar
    public IActionResult Comparar()
    {
        return View(new ComparacionFormulario());
    }

    // POST /Pokedex/Comparar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Comparar(ComparacionFormulario formulario)
    {
        if (!ModelState.IsValid)
            return View(formulario);

        formulario.Resultado = await _flujo.CompararAsync(formulario.Pokemon1, formulario.Pokemon2);
        if (formulario.Resultado == null)
            ModelState.AddModelError(string.Empty, "Alguno de los dos Pokémon no existe. Revisa los nombres.");

        return View(formulario);
    }
}
