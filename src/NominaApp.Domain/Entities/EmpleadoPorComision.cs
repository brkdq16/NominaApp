using NominaApp.Domain.Enums;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Domain.Entities
{
    public class EmpleadoPorComision : Empleado
    {
        public Dinero VentasBrutas { get; private set; }
        public decimal TarifaComision { get; private set; }

        public EmpleadoPorComision(string primerNombre, string apellidoPaterno,
                                  NumeroSeguroSocial numeroSeguroSocial,
                                  Dinero ventasBrutas, decimal tarifaComision)
            : base(primerNombre, apellidoPaterno, numeroSeguroSocial, TipoEmpleado.PorComision)
        {
            VentasBrutas = ventasBrutas ?? throw new ArgumentNullException(nameof(ventasBrutas));

            if (tarifaComision < 0 || tarifaComision > 1)
                throw new ArgumentException("La tarifa de comisión debe estar entre 0 y 1");

            TarifaComision = tarifaComision;
        }

        public override Dinero CalcularPagoSemanal()
        {
            return VentasBrutas * TarifaComision;
        }

        public void ActualizarVentas(Dinero nuevasVentas)
        {
            VentasBrutas = nuevasVentas ?? throw new ArgumentNullException(nameof(nuevasVentas));
        }

        public override string ToString() =>
            $"{base.ToString()} - Ventas: {VentasBrutas} - Comisión: {TarifaComision:P}";
    }
}