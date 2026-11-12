using PokedexWeb.Models;

namespace PokedexWeb.DataAccess;

// DA: guarda el equipo. Por ahora en memoria (lista estática como en MascotaController);
// más adelante se puede cambiar por una base de datos SIN tocar el BW.
public interface IEquipoRepositorio
{
    List<MiembroEquipo> ObtenerTodos();
    MiembroEquipo? ObtenerPorId(int id);
    void Agregar(MiembroEquipo miembro);
    void Eliminar(int id);
}

public class EquipoRepositorio : IEquipoRepositorio
{
    private static readonly List<MiembroEquipo> _equipo = new();

    public List<MiembroEquipo> ObtenerTodos() => _equipo.ToList();

    public MiembroEquipo? ObtenerPorId(int id) => _equipo.Find(m => m.Id == id);

    public void Agregar(MiembroEquipo miembro) => _equipo.Add(miembro);

    public void Eliminar(int id) => _equipo.RemoveAll(m => m.Id == id);
}
