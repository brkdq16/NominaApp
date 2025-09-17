using Microsoft.Extensions.DependencyInjection;
using NominaApp.Application.Interfaces;
using NominaApp.Application.Services;
using NominaApp.Domain.Interfaces;
using NominaApp.Infrastructure.Data;

namespace NominaApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Contexto en memoria (Singleton para mantener datos durante la sesión)
            services.AddSingleton<InMemoryContext>();

            // Unit of Work y Repositorios
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Servicios de Aplicación
            services.AddScoped<IEmpleadoService, EmpleadoService>();
            services.AddScoped<INominaService, NominaService>();

            return services;
        }
    }
}