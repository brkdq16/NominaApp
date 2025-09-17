using Microsoft.Extensions.DependencyInjection;
using NominaApp.Infrastructure;
using NominaApp.Console.UI;

namespace NominaApp.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configurar Dependency Injection
            var services = new ServiceCollection();
            services.AddInfrastructure();

            var serviceProvider = services.BuildServiceProvider();

            // Mostrar banner inicial
            MostrarBanner();

            // Crear y ejecutar menú principal
            var menuPrincipal = new MenuPrincipal(serviceProvider);
            await menuPrincipal.EjecutarAsync();

            System.Console.WriteLine("\n¡Gracias por usar el Sistema de Nómina!");
            System.Console.WriteLine("Presiona cualquier tecla para salir...");
            System.Console.ReadKey();
        }

        private static void MostrarBanner()
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            System.Console.WriteLine("║                                                          ║");
            System.Console.WriteLine("║              🏢 SISTEMA DE GESTIÓN DE NÓMINA             ║");
            System.Console.WriteLine("║                                                          ║");
            System.Console.WriteLine("║                    Versión 1.0                          ║");
            System.Console.WriteLine("║                Clean Architecture                        ║");
            System.Console.WriteLine("║                                                          ║");
            System.Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            System.Console.ResetColor();
            System.Console.WriteLine();
        }
    }
}