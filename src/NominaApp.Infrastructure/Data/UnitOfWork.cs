using NominaApp.Domain.Interfaces;
using NominaApp.Infrastructure.Repositories;

namespace NominaApp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InMemoryContext _context;
        private EmpleadoRepository? _empleadoRepository;

        public UnitOfWork(InMemoryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEmpleadoRepository Empleados =>
            _empleadoRepository ??= new EmpleadoRepository(_context);

        public Task<int> SaveChangesAsync()
        {
            // En una implementación real aquí se guardarían los cambios a la BD
            // Para el caso en memoria, los cambios ya están aplicados
            return Task.FromResult(_context.ContarEmpleados());
        }

        public void Dispose()
        {
            // Limpiar recursos si fuera necesario
        }
    }
}