using NominaApp.Domain.Enums;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Domain.Entities
{
    public class EmpleadoAsalariado : Empleado
    {
        public Dinero SalarioSemanal { get; private set; }

        public EmpleadoAsalariado(string primerNombre, string apellidoPaterno,
                                 NumeroSeguroSocial numeroSeguroSocial, Dinero salarioSemanal)
            : base(primerNombre, apellidoPaterno, numeroSeguroSocial, TipoEmpleado.Asalariado)
        {
            SalarioSemanal = salarioSemanal ?? throw new ArgumentNullException(nameof(salarioSemanal));
        }

        public override Dinero CalcularPagoSemanal()
        {
            return SalarioSemanal;
        }

        public override string ToString() =>
            $"{base.ToString()} - Salario: {SalarioSemanal}";
    }
}