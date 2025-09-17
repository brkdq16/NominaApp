namespace NominaApp.Application.DTOs
{
    public class ReporteNominaDto
    {
        public DateTime FechaGeneracion { get; set; }
        public List<EmpleadoDto> Empleados { get; set; } = new();
        public decimal TotalNomina { get; set; }
        public int TotalEmpleados { get; set; }
        public decimal PromedioSueldo { get; set; }

        public Dictionary<string, int> EmpleadosPorTipo { get; set; } = new();
        public Dictionary<string, decimal> NominaPorTipo { get; set; } = new();
    }
}