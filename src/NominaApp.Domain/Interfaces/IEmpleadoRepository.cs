using NominaApp.Domain.Entities;

namespace NominaApp.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(int id);
        Task<Empleado?> ObtenerPorNSSAsync(string numeroSeguroSocial);
        Task<int> AgregarAsync(Empleado empleado);
        Task ActualizarAsync(Empleado empleado);
        Task EliminarAsync(int id);
        Task<bool> ExisteNSSAsync(string numeroSeguroSocial);
    }
}