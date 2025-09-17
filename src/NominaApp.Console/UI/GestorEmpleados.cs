using Microsoft.Extensions.DependencyInjection;
using NominaApp.Application.DTOs;
using NominaApp.Application.Interfaces;
using NominaApp.Domain.Enums;

namespace NominaApp.Console.UI
{
    public class GestorEmpleados
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly INominaService _nominaService;

        public GestorEmpleados(IServiceProvider serviceProvider)
        {
            _empleadoService = serviceProvider.GetRequiredService<IEmpleadoService>();
            _nominaService = serviceProvider.GetRequiredService<INominaService>();
        }

        public async Task CrearEmpleadoAsync()
        {
            System.Console.Clear();
            MostrarTitulo("CREAR NUEVO EMPLEADO");

            try
            {
                var request = new CrearEmpleadoRequest();

                // Datos básicos
                System.Console.Write("Primer nombre: ");
                request.PrimerNombre = System.Console.ReadLine()?.Trim() ?? "";

                System.Console.Write("Apellido paterno: ");
                request.ApellidoPaterno = System.Console.ReadLine()?.Trim() ?? "";

                System.Console.Write("Número de Seguro Social: ");
                request.NumeroSeguroSocial = System.Console.ReadLine()?.Trim() ?? "";

                // Verificar si ya existe el NSS
                if (await _empleadoService.ExisteNSSAsync(request.NumeroSeguroSocial))
                {
                    MostrarError("Ya existe un empleado con ese número de seguro social.");
                    return;
                }

                // Seleccionar tipo de empleado
                request.TipoEmpleado = SeleccionarTipoEmpleado();

                // Capturar datos específicos según el tipo
                await CapturarDatosEspecificos(request);

                // Crear empleado
                var empleadoId = await _empleadoService.CrearEmpleadoAsync(request);

                MostrarExito($"Empleado creado exitosamente con ID: {empleadoId}");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al crear empleado: {ex.Message}");
            }
        }

        public async Task ListarEmpleadosAsync()
        {
            System.Console.Clear();
            MostrarTitulo("LISTA DE EMPLEADOS");

            try
            {
                var empleados = await _empleadoService.ObtenerTodosLosEmpleadosAsync();
                var listaEmpleados = empleados.ToList();

                if (!listaEmpleados.Any())
                {
                    System.Console.WriteLine("No hay empleados registrados.");
                    return;
                }

                System.Console.WriteLine();
                foreach (var empleado in listaEmpleados)
                {
                    MostrarDetalleEmpleado(empleado);
                    System.Console.WriteLine(new string('-', 80));
                }

                System.Console.WriteLine($"\nTotal de empleados: {listaEmpleados.Count}");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al obtener empleados: {ex.Message}");
            }
        }

        public async Task ActualizarDatosEmpleadoAsync()
        {
            System.Console.Clear();
            MostrarTitulo("ACTUALIZAR DATOS DE EMPLEADO");

            try
            {
                System.Console.Write("Ingresa el ID del empleado: ");
                if (!int.TryParse(System.Console.ReadLine(), out int empleadoId))
                {
                    MostrarError("ID inválido.");
                    return;
                }

                var empleado = await _empleadoService.ObtenerEmpleadoPorIdAsync(empleadoId);
                if (empleado == null)
                {
                    MostrarError("Empleado no encontrado.");
                    return;
                }

                System.Console.WriteLine("\nEmpleado encontrado:");
                MostrarDetalleEmpleado(empleado);

                if (empleado.TipoEmpleado == TipoEmpleado.PorHoras)
                {
                    System.Console.Write("\nNuevas horas trabajadas: ");
                    if (decimal.TryParse(System.Console.ReadLine(), out decimal nuevasHoras))
                    {
                        await _empleadoService.ActualizarHorasTrabajadasAsync(empleadoId, nuevasHoras);
                        MostrarExito("Horas actualizadas exitosamente.");
                    }
                    else
                    {
                        MostrarError("Valor inválido para horas.");
                    }
                }
                else if (empleado.TipoEmpleado == TipoEmpleado.PorComision ||
                         empleado.TipoEmpleado == TipoEmpleado.AsalariadoPorComision)
                {
                    System.Console.Write("\nNuevas ventas: ");
                    if (decimal.TryParse(System.Console.ReadLine(), out decimal nuevasVentas))
                    {
                        await _empleadoService.ActualizarVentasAsync(empleadoId, nuevasVentas);
                        MostrarExito("Ventas actualizadas exitosamente.");
                    }
                    else
                    {
                        MostrarError("Valor inválido para ventas.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Este tipo de empleado no tiene datos actualizables.");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al actualizar empleado: {ex.Message}");
            }
        }

        public async Task EliminarEmpleadoAsync()
        {
            System.Console.Clear();
            MostrarTitulo("ELIMINAR EMPLEADO");

            try
            {
                System.Console.Write("Ingresa el ID del empleado a eliminar: ");
                if (!int.TryParse(System.Console.ReadLine(), out int empleadoId))
                {
                    MostrarError("ID inválido.");
                    return;
                }

                var empleado = await _empleadoService.ObtenerEmpleadoPorIdAsync(empleadoId);
                if (empleado == null)
                {
                    MostrarError("Empleado no encontrado.");
                    return;
                }

                System.Console.WriteLine("\nEmpleado a eliminar:");
                MostrarDetalleEmpleado(empleado);

                System.Console.Write("\n¿Estás seguro de que deseas eliminar este empleado? (s/N): ");
                var confirmacion = System.Console.ReadLine()?.ToLower();

                if (confirmacion == "s" || confirmacion == "si")
                {
                    await _empleadoService.EliminarEmpleadoAsync(empleadoId);
                    MostrarExito("Empleado eliminado exitosamente.");
                }
                else
                {
                    System.Console.WriteLine("Operación cancelada.");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar empleado: {ex.Message}");
            }
        }

        public async Task CalcularPagoIndividualAsync()
        {
            System.Console.Clear();
            MostrarTitulo("CALCULAR PAGO INDIVIDUAL");

            try
            {
                System.Console.Write("Ingresa el ID del empleado: ");
                if (!int.TryParse(System.Console.ReadLine(), out int empleadoId))
                {
                    MostrarError("ID inválido.");
                    return;
                }

                var empleado = await _empleadoService.ObtenerEmpleadoPorIdAsync(empleadoId);
                if (empleado == null)
                {
                    MostrarError("Empleado no encontrado.");
                    return;
                }

                var pago = await _nominaService.CalcularPagoEmpleadoAsync(empleadoId);

                System.Console.WriteLine();
                MostrarDetalleEmpleado(empleado);
                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"💰 PAGO SEMANAL: ${pago:F2}");
                System.Console.ResetColor();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al calcular pago: {ex.Message}");
            }
        }

        // Métodos auxiliares
        private TipoEmpleado SeleccionarTipoEmpleado()
        {
            while (true)
            {
                System.Console.WriteLine("\nTipo de empleado:");
                System.Console.WriteLine("1. Asalariado");
                System.Console.WriteLine("2. Por Horas");
                System.Console.WriteLine("3. Por Comisión");
                System.Console.WriteLine("4. Asalariado por Comisión");
                System.Console.Write("Selecciona el tipo (1-4): ");

                var opcion = System.Console.ReadLine();
                switch (opcion)
                {
                    case "1": return TipoEmpleado.Asalariado;
                    case "2": return TipoEmpleado.PorHoras;
                    case "3": return TipoEmpleado.PorComision;
                    case "4": return TipoEmpleado.AsalariadoPorComision;
                    default:
                        MostrarError("Opción inválida. Selecciona del 1 al 4.");
                        continue;
                }
            }
        }

        private async Task CapturarDatosEspecificos(CrearEmpleadoRequest request)
        {
            switch (request.TipoEmpleado)
            {
                case TipoEmpleado.Asalariado:
                    System.Console.Write("Salario semanal: $");
                    request.SalarioSemanal = LeerDecimal();
                    break;

                case TipoEmpleado.PorHoras:
                    System.Console.Write("Sueldo por hora: $");
                    request.SueldoPorHora = LeerDecimal();
                    System.Console.Write("Horas trabajadas: ");
                    request.HorasTrabajadas = LeerDecimal();
                    break;

                case TipoEmpleado.PorComision:
                    System.Console.Write("Ventas brutas: $");
                    request.VentasBrutas = LeerDecimal();
                    System.Console.Write("Tarifa de comisión (0.0 - 1.0): ");
                    request.TarifaComision = LeerDecimal();
                    break;

                case TipoEmpleado.AsalariadoPorComision:
                    System.Console.Write("Salario base: $");
                    request.SalarioBase = LeerDecimal();
                    System.Console.Write("Ventas brutas: $");
                    request.VentasBrutas = LeerDecimal();
                    System.Console.Write("Tarifa de comisión (0.0 - 1.0): ");
                    request.TarifaComision = LeerDecimal();
                    break;
            }
        }

        private decimal LeerDecimal()
        {
            while (true)
            {
                if (decimal.TryParse(System.Console.ReadLine(), out decimal valor))
                    return valor;

                System.Console.Write("Valor inválido. Intenta nuevamente: ");
            }
        }

        private void MostrarDetalleEmpleado(EmpleadoDto empleado)
        {
            System.Console.WriteLine($"ID: {empleado.Id} | {empleado.NombreCompleto}");
            System.Console.WriteLine($"NSS: {empleado.NumeroSeguroSocial}");
            System.Console.WriteLine($"Tipo: {empleado.TipoEmpleado}");
            System.Console.WriteLine($"Pago Semanal: ${empleado.PagoSemanal:F2}");
        }

        private void MostrarTitulo(string titulo)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(titulo);
            System.Console.WriteLine(new string('=', titulo.Length));
            System.Console.ResetColor();
        }

        private void MostrarError(string mensaje)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"❌ {mensaje}");
            System.Console.ResetColor();
        }

        private void MostrarExito(string mensaje)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"✅ {mensaje}");
            System.Console.ResetColor();
        }
    }
}