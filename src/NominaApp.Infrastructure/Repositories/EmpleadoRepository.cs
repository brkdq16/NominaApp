using NominaApp.Domain.Entities;
using NominaApp.Domain.Interfaces;
using NominaApp.Infrastructure.Data;

namespace NominaApp.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly InMemoryContext _context;

        public EmpleadoRepository(InMemoryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<IEnumerable<Empleado>> ObtenerTodosAsync()
        {
            var empleados = _context.Empleados.AsEnumerable();
            return Task.FromResult(empleados);
        }

        public Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            var empleado = _context.Empleados.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(empleado);
        }

        public Task<Empleado?> ObtenerPorNSSAsync(string numeroSeguroSocial)
        {
            var empleado = _context.Empleados
                .FirstOrDefault(e => e.NumeroSeguroSocial.Valor == numeroSeguroSocial);
            return Task.FromResult(empleado);
        }

        public Task<int> AgregarAsync(Empleado empleado)
        {
            _context.Agregar(empleado);
            return Task.FromResult(empleado.Id);
        }

        public Task ActualizarAsync(Empleado empleado)
        {
            _context.Actualizar(empleado);
            return Task.CompletedTask;
        }

        public Task EliminarAsync(int id)
        {
            _context.Eliminar(id);
            return Task.CompletedTask;
        }

        public Task<bool> ExisteNSSAsync(string numeroSeguroSocial)
        {
            var existe = _context.Empleados
                .Any(e => e.NumeroSeguroSocial.Valor == numeroSeguroSocial);
            return Task.FromResult(existe);
        }
    }
}