using Microsoft.AspNetCore.Mvc;
using PokedexWeb.BusinessWorkflow;

namespace PokedexWeb.Controllers;

public class EquipoController : Controller
{
    private readonly IEquipoFlujo _flujo;

    public EquipoController(IEquipoFlujo flujo)
    {
        _flujo = flujo;
    }

    // GET /Equipo  → READ
    public IActionResult Index()
    {
        return View(_flujo.ObtenerEquipo());
    }

    // POST /Equipo/Agregar  → CREATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Agregar(string nombre, string? apodo)
    {
        var (exito, mensaje) = await _flujo.AgregarAsync(nombre, apodo);
        GuardarMensaje(exito, mensaje);

        // Si falló, regresamos al detalle del Pokémon para que vea el error
        return exito
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Detalle", "Pokedex", new { id = nombre });
    }

    // POST /Equipo/CambiarApodo  → UPDATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CambiarApodo(int id, string? apodo)
    {
        var (exito, mensaje) = _flujo.CambiarApodo(id, apodo);
        GuardarMensaje(exito, mensaje);
        return RedirectToAction(nameof(Index));
    }

    // POST /Equipo/Eliminar  → DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        var (exito, mensaje) = _flujo.Eliminar(id);
        GuardarMensaje(exito, mensaje);
        return RedirectToAction(nameof(Index));
    }

    // TempData sobrevive a un Redirect → ideal para mensajes de "éxito/error"
    private void GuardarMensaje(bool exito, string mensaje)
    {
        TempData["Mensaje"] = mensaje;
        TempData["TipoMensaje"] = exito ? "success" : "danger";
    }
}
