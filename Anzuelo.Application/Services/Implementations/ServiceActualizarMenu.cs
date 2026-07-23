using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Anzuelo.Application.Services
{
    public class ServiceActualizarMenu :
        IServiceActualizarMenu
    {
        private readonly IRepositoryMenu
            _repositoryMenu;

        private readonly ILogger<ServiceActualizarMenu>
            _logger;

        public ServiceActualizarMenu(
            IRepositoryMenu repositoryMenu,
            ILogger<ServiceActualizarMenu> logger)
        {
            _repositoryMenu =
                repositoryMenu;

            _logger =
                logger;
        }

        public async Task RevisarMenuDisponiblesAsync(
            DateTime fechaHora,
            CancellationToken cancellationToken)
        {
            var existeMenuDisponible =
                await _repositoryMenu
                    .ExisteMenuDisponibleAsync(
                        fechaHora,
                        cancellationToken);

            if (existeMenuDisponible)
            {
                _logger.LogInformation(
                    "Existe un menú disponible el {Fecha} " +
                    "a las {Hora}. No se modificaron los días.",
                    fechaHora.Date,
                    fechaHora.TimeOfDay);

                return;
            }

            var cantidadActualizada =
                await _repositoryMenu
                    .AvanzarDiaMenusAsync(
                        cancellationToken);

            _logger.LogInformation(
                "No se encontró un menú disponible. " +
                "Se actualizaron {Cantidad} disponibilidades.",
                cantidadActualizada);
        }
    }
}