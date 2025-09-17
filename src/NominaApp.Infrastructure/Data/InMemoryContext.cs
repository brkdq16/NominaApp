using NominaApp.Domain.Entities;

namespace NominaApp.Infrastructure.Data
{
    public class InMemoryContext
    {
        private readonly List<Empleado> _empleados;
        private int _siguienteId;

        public InMemoryContext()
        {
            _empleados = new List<Empleado>();
            _siguienteId = 1;
        }

        public IReadOnlyList<Empleado> Empleados => _empleados.AsReadOnly();

        public void Agregar(Empleado empleado)
        {
            // Asignar ID usando reflexión (hack para el ejemplo)
            var idProperty = typeof(Empleado).GetProperty("Id");
            idProperty?.SetValue(empleado, _siguienteId++);

            _empleados.Add(empleado);
        }

        public void Actualizar(Empleado empleado)
        {
            var index = _empleados.FindIndex(e => e.Id == empleado.Id);
            if (index >= 0)
            {
                _empleados[index] = empleado;
            }
        }

        public bool Eliminar(int id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            if (empleado != null)
            {
                _empleados.Remove(empleado);
                return true;
            }
            return false;
        }

        public void LimpiarTodo()
        {
            _empleados.Clear();
            _siguienteId = 1;
        }

        public int ContarEmpleados() => _empleados.Count;
    }
}