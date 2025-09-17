using Microsoft.Extensions.DependencyInjection;
using NominaApp.Application.Interfaces;

namespace NominaApp.Console.UI
{
    public class MenuPrincipal
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly GestorEmpleados _gestorEmpleados;
        private readonly GeneradorReportes _generadorReportes;

        public MenuPrincipal(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _gestorEmpleados = new GestorEmpleados(serviceProvider);
            _generadorReportes = new GeneradorReportes(serviceProvider);
        }

        public async Task EjecutarAsync()
        {
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                var opcion = LeerOpcion();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            await _gestorEmpleados.CrearEmpleadoAsync();
                            break;
                        case "2":
                            await _gestorEmpleados.ListarEmpleadosAsync();
                            break;
                        case "3":
                            await _gestorEmpleados.ActualizarDatosEmpleadoAsync();
                            break;
                        case "4":
                            await _gestorEmpleados.EliminarEmpleadoAsync();
                            break;
                        case "5":
                            await _generadorReportes.GenerarReporteCompletoAsync();
                            break;
                        case "6":
                            await _generadorReportes.MostrarEstadisticasAsync();
                            break;
                        case "7":
                            await _gestorEmpleados.CalcularPagoIndividualAsync();
                            break;
                        case "0":
                            continuar = false;
                            break;
                        default:
                            MostrarError("Opción no válida. Por favor, selecciona una opción del 0 al 7.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error: {ex.Message}");
                }

                if (continuar)
                {
                    System.Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    System.Console.ReadKey();
                }
            }
        }

        private void MostrarMenu()
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("═══════════════════════════════════════════════════════════");
            System.Console.WriteLine("                    MENÚ PRINCIPAL                        ");
            System.Console.WriteLine("═══════════════════════════════════════════════════════════");
            System.Console.ResetColor();

            System.Console.WriteLine();
            System.Console.WriteLine("👥 GESTIÓN DE EMPLEADOS:");
            System.Console.WriteLine("   1. Crear nuevo empleado");
            System.Console.WriteLine("   2. Listar todos los empleados");
            System.Console.WriteLine("   3. Actualizar datos de empleado");
            System.Console.WriteLine("   4. Eliminar empleado");

            System.Console.WriteLine();
            System.Console.WriteLine("📊 REPORTES Y CÁLCULOS:");
            System.Console.WriteLine("   5. Generar reporte completo de nómina");
            System.Console.WriteLine("   6. Mostrar estadísticas");
            System.Console.WriteLine("   7. Calcular pago individual");

            System.Console.WriteLine();
            System.Console.WriteLine("   0. Salir");

            System.Console.WriteLine();
            System.Console.Write("Selecciona una opción: ");
        }

        private string LeerOpcion()
        {
            return System.Console.ReadLine()?.Trim() ?? "";
        }

        private void MostrarError(string mensaje)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"\n❌ {mensaje}");
            System.Console.ResetColor();
        }
    }
}