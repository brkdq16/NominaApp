using NominaApp.Application.DTOs;

namespace NominaApp.Application.Interfaces
{
    public interface INominaService
    {
        Task<ReporteNominaDto> GenerarReporteNominaAsync();
        Task<decimal> CalcularTotalNominaAsync();
        Task<Dictionary<string, decimal>> ObtenerNominaPorTiposAsync();
        Task<decimal> CalcularPagoEmpleadoAsync(int empleadoId);
    }
}