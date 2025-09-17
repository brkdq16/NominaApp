using NominaApp.Application.DTOs;
using NominaApp.Application.Interfaces;
using NominaApp.Domain.Entities;
using NominaApp.Domain.Enums;
using NominaApp.Domain.Interfaces;
using NominaApp.Domain.ValueObjects;

namespace NominaApp.Application.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpleadoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<EmpleadoDto>> ObtenerTodosLosEmpleadosAsync()
        {
            var empleados = await _unitOfWork.Empleados.ObtenerTodosAsync();
            return empleados.Select(ConvertirADto);
        }

        public async Task<EmpleadoDto?> ObtenerEmpleadoPorIdAsync(int id)
        {
            var empleado = await _unitOfWork.Empleados.ObtenerPorIdAsync(id);
            return empleado != null ? ConvertirADto(empleado) : null;
        }

        public async Task<EmpleadoDto?> ObtenerEmpleadoPorNSSAsync(string nss)
        {
            var empleado = await _unitOfWork.Empleados.ObtenerPorNSSAsync(nss);
            return empleado != null ? ConvertirADto(empleado) : null;
        }

        public async Task<int> CrearEmpleadoAsync(CrearEmpleadoRequest request)
        {
            ValidarRequest(request);

            if (await _unitOfWork.Empleados.ExisteNSSAsync(request.NumeroSeguroSocial))
                throw new InvalidOperationException("Ya existe un empleado con ese número de seguro social");

            var nss = new NumeroSeguroSocial(request.NumeroSeguroSocial);
            Empleado empleado = request.TipoEmpleado switch
            {
                TipoEmpleado.Asalariado => new EmpleadoAsalariado(
                    request.PrimerNombre,
                    request.ApellidoPaterno,
                    nss,
                    new Dinero(request.SalarioSemanal!.Value)),

                TipoEmpleado.PorHoras => new EmpleadoPorHoras(
                    request.PrimerNombre,
                    request.ApellidoPaterno,
                    nss,
                    new Dinero(request.SueldoPorHora!.Value),
                    request.HorasTrabajadas!.Value),

                TipoEmpleado.PorComision => new EmpleadoPorComision(
                    request.PrimerNombre,
                    request.ApellidoPaterno,
                    nss,
                    new Dinero(request.VentasBrutas!.Value),
                    request.TarifaComision!.Value),

                TipoEmpleado.AsalariadoPorComision => new EmpleadoAsalariadoPorComision(
                    request.PrimerNombre,
                    request.ApellidoPaterno,
                    nss,
                    new Dinero(request.VentasBrutas!.Value),
                    request.TarifaComision!.Value,
                    new Dinero(request.SalarioBase!.Value)),

                _ => throw new ArgumentException("Tipo de empleado no válido")
            };

            var empleadoId = await _unitOfWork.Empleados.AgregarAsync(empleado);
            await _unitOfWork.SaveChangesAsync();
            return empleadoId;
        }

        public async Task ActualizarHorasTrabajadasAsync(int empleadoId, decimal nuevasHoras)
        {
            var empleado = await _unitOfWork.Empleados.ObtenerPorIdAsync(empleadoId);

            if (empleado == null)
                throw new InvalidOperationException("Empleado no encontrado");

            if (empleado is not EmpleadoPorHoras empleadoPorHoras)
                throw new InvalidOperationException("Solo se pueden actualizar horas para empleados por horas");

            empleadoPorHoras.ActualizarHorasTrabajadas(nuevasHoras);
            await _unitOfWork.Empleados.ActualizarAsync(empleado);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActualizarVentasAsync(int empleadoId, decimal nuevasVentas)
        {
            var empleado = await _unitOfWork.Empleados.ObtenerPorIdAsync(empleadoId);

            if (empleado == null)
                throw new InvalidOperationException("Empleado no encontrado");

            var dineroVentas = new Dinero(nuevasVentas);

            switch (empleado)
            {
                case EmpleadoPorComision empleadoPorComision:
                    empleadoPorComision.ActualizarVentas(dineroVentas);
                    break;
                case EmpleadoAsalariadoPorComision empleadoAsalariadoPorComision:
                    empleadoAsalariadoPorComision.ActualizarVentas(dineroVentas);
                    break;
                default:
                    throw new InvalidOperationException("Solo se pueden actualizar ventas para empleados por comisión");
            }

            await _unitOfWork.Empleados.ActualizarAsync(empleado);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarEmpleadoAsync(int id)
        {
            await _unitOfWork.Empleados.EliminarAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExisteNSSAsync(string nss)
        {
            return await _unitOfWork.Empleados.ExisteNSSAsync(nss);
        }

        private static EmpleadoDto ConvertirADto(Empleado empleado)
        {
            return new EmpleadoDto
            {
                Id = empleado.Id,
                PrimerNombre = empleado.PrimerNombre,
                ApellidoPaterno = empleado.ApellidoPaterno,
                NumeroSeguroSocial = empleado.NumeroSeguroSocial.ToString(),
                TipoEmpleado = empleado.TipoEmpleado,
                NombreCompleto = empleado.NombreCompleto,
                PagoSemanal = empleado.CalcularPagoSemanal().Cantidad,
                DetallesPago = empleado.ToString()
            };
        }

        private static void ValidarRequest(CrearEmpleadoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PrimerNombre))
                throw new ArgumentException("El primer nombre es requerido");

            if (string.IsNullOrWhiteSpace(request.ApellidoPaterno))
                throw new ArgumentException("El apellido paterno es requerido");

            if (string.IsNullOrWhiteSpace(request.NumeroSeguroSocial))
                throw new ArgumentException("El número de seguro social es requerido");

            switch (request.TipoEmpleado)
            {
                case TipoEmpleado.Asalariado:
                    if (!request.SalarioSemanal.HasValue || request.SalarioSemanal <= 0)
                        throw new ArgumentException("El salario semanal debe ser mayor a cero");
                    break;

                case TipoEmpleado.PorHoras:
                    if (!request.SueldoPorHora.HasValue || request.SueldoPorHora <= 0)
                        throw new ArgumentException("El sueldo por hora debe ser mayor a cero");
                    if (!request.HorasTrabajadas.HasValue || request.HorasTrabajadas < 0)
                        throw new ArgumentException("Las horas trabajadas no pueden ser negativas");
                    break;

                case TipoEmpleado.PorComision:
                    if (!request.VentasBrutas.HasValue || request.VentasBrutas < 0)
                        throw new ArgumentException("Las ventas brutas no pueden ser negativas");
                    if (!request.TarifaComision.HasValue || request.TarifaComision < 0 || request.TarifaComision > 1)
                        throw new ArgumentException("La tarifa de comisión debe estar entre 0 y 1");
                    break;

                case TipoEmpleado.AsalariadoPorComision:
                    if (!request.VentasBrutas.HasValue || request.VentasBrutas < 0)
                        throw new ArgumentException("Las ventas brutas no pueden ser negativas");
                    if (!request.TarifaComision.HasValue || request.TarifaComision < 0 || request.TarifaComision > 1)
                        throw new ArgumentException("La tarifa de comisión debe estar entre 0 y 1");
                    if (!request.SalarioBase.HasValue || request.SalarioBase <= 0)
                        throw new ArgumentException("El salario base debe ser mayor a cero");
                    break;
            }
        }
    }
}