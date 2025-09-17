namespace NominaApp.Domain.ValueObjects
{
    public class Dinero
    {
        public decimal Cantidad { get; private set; }

        public Dinero(decimal cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");

            Cantidad = Math.Round(cantidad, 2);
        }

        public static Dinero operator +(Dinero a, Dinero b)
        {
            return new Dinero(a.Cantidad + b.Cantidad);
        }

        public static Dinero operator *(Dinero dinero, decimal multiplicador)
        {
            return new Dinero(dinero.Cantidad * multiplicador);
        }

        public override string ToString() => $"${Cantidad:F2}";

        public override bool Equals(object? obj)
        {
            if (obj is Dinero otro)
                return Cantidad == otro.Cantidad;
            return false;
        }

        public override int GetHashCode() => Cantidad.GetHashCode();
    }
}