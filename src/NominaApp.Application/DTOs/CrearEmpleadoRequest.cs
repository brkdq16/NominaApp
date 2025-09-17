using NominaApp.Domain.Enums;

namespace NominaApp.Application.DTOs
{
    public class CrearEmpleadoRequest
    {
        public string PrimerNombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string NumeroSeguroSocial { get; set; } = string.Empty;
        public TipoEmpleado TipoEmpleado { get; set; }

        // Para Empleado Asalariado
        public decimal? SalarioSemanal { get; set; }

        // Para Empleado Por Horas
        public decimal? SueldoPorHora { get; set; }
        public decimal? HorasTrabajadas { get; set; }

        // Para Empleado Por Comisión
        public decimal? VentasBrutas { get; set; }
        public decimal? TarifaComision { get; set; }

        // Para Empleado Asalariado Por Comisión
        public decimal? SalarioBase { get; set; }
    }
}