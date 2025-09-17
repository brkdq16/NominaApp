namespace NominaApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IEmpleadoRepository Empleados { get; }
        Task<int> SaveChangesAsync();
    }
}