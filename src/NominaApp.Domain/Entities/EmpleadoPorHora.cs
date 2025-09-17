using NominaApp.Domain.Enums;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Domain.Entities
{
    public class EmpleadoPorHoras : Empleado
    {
        public Dinero SueldoPorHora { get; private set; }
        public decimal HorasTrabajadas { get; private set; }

        public EmpleadoPorHoras(string primerNombre, string apellidoPaterno,
                               NumeroSeguroSocial numeroSeguroSocial,
                               Dinero sueldoPorHora, decimal horasTrabajadas)
            : base(primerNombre, apellidoPaterno, numeroSeguroSocial, TipoEmpleado.PorHoras)
        {
            SueldoPorHora = sueldoPorHora ?? throw new ArgumentNullException(nameof(sueldoPorHora));

            if (horasTrabajadas < 0)
                throw new ArgumentException("Las horas trabajadas no pueden ser negativas");

            HorasTrabajadas = horasTrabajadas;
        }

        public override Dinero CalcularPagoSemanal()
        {
            if (HorasTrabajadas <= 40)
            {
                // Pago normal
                return SueldoPorHora * (decimal)HorasTrabajadas;
            }
            else
            {
                // Pago con tiempo extra (1.5x después de 40 horas)
                var pagoNormal = SueldoPorHora * 40m;
                var horasExtra = (decimal)HorasTrabajadas - 40m;
                var pagoExtra = SueldoPorHora * 1.5m * horasExtra;

                return pagoNormal + pagoExtra;
            }
        }

        public void ActualizarHorasTrabajadas(decimal nuevasHoras)
        {
            if (nuevasHoras < 0)
                throw new ArgumentException("Las horas trabajadas no pueden ser negativas");

            HorasTrabajadas = nuevasHoras;
        }

        public override string ToString() =>
            $"{base.ToString()} - ${SueldoPorHora}/hr - {HorasTrabajadas} horas";
    }
}