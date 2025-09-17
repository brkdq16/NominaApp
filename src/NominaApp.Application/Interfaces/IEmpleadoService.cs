using NominaApp.Application.DTOs;

namespace NominaApp.Application.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> ObtenerTodosLosEmpleadosAsync();
        Task<EmpleadoDto?> ObtenerEmpleadoPorIdAsync(int id);
        Task<EmpleadoDto?> ObtenerEmpleadoPorNSSAsync(string nss);
        Task<int> CrearEmpleadoAsync(CrearEmpleadoRequest request);
        Task ActualizarHorasTrabajadasAsync(int empleadoId, decimal nuevasHoras);
        Task ActualizarVentasAsync(int empleadoId, decimal nuevasVentas);
        Task EliminarEmpleadoAsync(int id);
        Task<bool> ExisteNSSAsync(string nss);
    }
}