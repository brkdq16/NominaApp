using NominaApp.Domain.Enums;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Domain.Entities
{
    public abstract class Empleado
    {
        public int Id { get; protected set; }
        public string PrimerNombre { get; protected set; }
        public string ApellidoPaterno { get; protected set; }
        public NumeroSeguroSocial NumeroSeguroSocial { get; protected set; }
        public TipoEmpleado TipoEmpleado { get; protected set; }

        protected Empleado(string primerNombre, string apellidoPaterno,
                          NumeroSeguroSocial numeroSeguroSocial, TipoEmpleado tipoEmpleado)
        {
            PrimerNombre = primerNombre ?? throw new ArgumentNullException(nameof(primerNombre));
            ApellidoPaterno = apellidoPaterno ?? throw new ArgumentNullException(nameof(apellidoPaterno));
            NumeroSeguroSocial = numeroSeguroSocial ?? throw new ArgumentNullException(nameof(numeroSeguroSocial));
            TipoEmpleado = tipoEmpleado;
        }

        // Método abstracto que cada tipo de empleado debe implementar
        public abstract Dinero CalcularPagoSemanal();

        public string NombreCompleto => $"{PrimerNombre} {ApellidoPaterno}";

        public override string ToString() =>
            $"{NombreCompleto} - {TipoEmpleado} - NSS: {NumeroSeguroSocial}";
    }
}