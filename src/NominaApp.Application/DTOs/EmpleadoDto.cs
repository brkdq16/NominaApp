using NominaApp.Domain.Enums;

namespace NominaApp.Application.DTOs
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string PrimerNombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string NumeroSeguroSocial { get; set; } = string.Empty;
        public TipoEmpleado TipoEmpleado { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public decimal PagoSemanal { get; set; }
        public string DetallesPago { get; set; } = string.Empty;
    }
}