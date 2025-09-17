using NominaApp.Domain.Enums;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Domain.Entities
{
    public class EmpleadoAsalariadoPorComision : Empleado
    {
        public Dinero VentasBrutas { get; private set; }
        public decimal TarifaComision { get; private set; }
        public Dinero SalarioBase { get; private set; }

        public EmpleadoAsalariadoPorComision(string primerNombre, string apellidoPaterno,
                                           NumeroSeguroSocial numeroSeguroSocial,
                                           Dinero ventasBrutas, decimal tarifaComision,
                                           Dinero salarioBase)
            : base(primerNombre, apellidoPaterno, numeroSeguroSocial, TipoEmpleado.AsalariadoPorComision)
        {
            VentasBrutas = ventasBrutas ?? throw new ArgumentNullException(nameof(ventasBrutas));
            SalarioBase = salarioBase ?? throw new ArgumentNullException(nameof(salarioBase));

            if (tarifaComision < 0 || tarifaComision > 1)
                throw new ArgumentException("La tarifa de comisión debe estar entre 0 y 1");

            TarifaComision = tarifaComision;
        }

        public override Dinero CalcularPagoSemanal()
        {
            // Fórmula: (ventasBrutas × tarifaComision) + salarioBase + (salarioBase × 0.10)
            var comision = VentasBrutas * TarifaComision;
            var bonificacion = SalarioBase * 0.10m;

            return comision + SalarioBase + bonificacion;
        }

        public void ActualizarVentas(Dinero nuevasVentas)
        {
            VentasBrutas = nuevasVentas ?? throw new ArgumentNullException(nameof(nuevasVentas));
        }

        public override string ToString() =>
            $"{base.ToString()} - Salario: {SalarioBase} - Ventas: {VentasBrutas} - Comisión: {TarifaComision:P}";
    }
}