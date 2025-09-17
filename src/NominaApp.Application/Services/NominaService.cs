using NominaApp.Application.DTOs;
using NominaApp.Application.Interfaces;
using NominaApp.Domain.Interfaces;

namespace NominaApp.Application.Services
{
    public class NominaService : INominaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NominaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ReporteNominaDto> GenerarReporteNominaAsync()
        {
            var empleados = await _unitOfWork.Empleados.ObtenerTodosAsync();
            var empleadosDto = empleados.Select(e => new EmpleadoDto
            {
                Id = e.Id,
                PrimerNombre = e.PrimerNombre,
                ApellidoPaterno = e.ApellidoPaterno,
                NumeroSeguroSocial = e.NumeroSeguroSocial.ToString(),
                TipoEmpleado = e.TipoEmpleado,
                NombreCompleto = e.NombreCompleto,
                PagoSemanal = e.CalcularPagoSemanal().Cantidad,
                DetallesPago = e.ToString()
            }).ToList();

            var totalNomina = empleadosDto.Sum(e => e.PagoSemanal);
            var totalEmpleados = empleadosDto.Count;
            var promedioSueldo = totalEmpleados > 0 ? totalNomina / totalEmpleados : 0;

            var empleadosPorTipo = empleadosDto
                .GroupBy(e => e.TipoEmpleado.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            var nominaPorTipo = empleadosDto
                .GroupBy(e => e.TipoEmpleado.ToString())
                .ToDictionary(g => g.Key, g => g.Sum(e => e.PagoSemanal));

            return new ReporteNominaDto
            {
                FechaGeneracion = DateTime.Now,
                Empleados = empleadosDto,
                TotalNomina = totalNomina,
                TotalEmpleados = totalEmpleados,
                PromedioSueldo = promedioSueldo,
                EmpleadosPorTipo = empleadosPorTipo,
                NominaPorTipo = nominaPorTipo
            };
        }

        public async Task<decimal> CalcularTotalNominaAsync()
        {
            var empleados = await _unitOfWork.Empleados.ObtenerTodosAsync();
            return empleados.Sum(e => e.CalcularPagoSemanal().Cantidad);
        }

        public async Task<Dictionary<string, decimal>> ObtenerNominaPorTiposAsync()
        {
            var empleados = await _unitOfWork.Empleados.ObtenerTodosAsync();

            return empleados
                .GroupBy(e => e.TipoEmpleado.ToString())
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.CalcularPagoSemanal().Cantidad)
                );
        }

        public async Task<decimal> CalcularPagoEmpleadoAsync(int empleadoId)
        {
            var empleado = await _unitOfWork.Empleados.ObtenerPorIdAsync(empleadoId);

            if (empleado == null)
                throw new InvalidOperationException("Empleado no encontrado");

            return empleado.CalcularPagoSemanal().Cantidad;
        }
    }
}