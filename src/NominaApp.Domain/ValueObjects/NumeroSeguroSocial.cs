namespace NominaApp.Domain.ValueObjects
{
    public class NumeroSeguroSocial
    {
        public string Valor { get; private set; }

        public NumeroSeguroSocial(string numeroSeguroSocial)
        {
            if (string.IsNullOrWhiteSpace(numeroSeguroSocial))
                throw new ArgumentException("El número de seguro social no puede estar vacío");

            if (numeroSeguroSocial.Length < 9)
                throw new ArgumentException("El número de seguro social debe tener al menos 9 caracteres");

            Valor = numeroSeguroSocial;
        }

        public override string ToString() => Valor;

        public override bool Equals(object? obj)
        {
            if (obj is NumeroSeguroSocial otro)
                return Valor == otro.Valor;
            return false;
        }

        public override int GetHashCode() => Valor.GetHashCode();
    }
}