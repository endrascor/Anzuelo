using Anzuelo.Application.Services.Interfaces;
using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Anzuelo.Web.BackgroundServices
{
    public sealed class ActualizarMenuCronService :
        BackgroundService
    {
        private readonly IServiceScopeFactory
            _serviceScopeFactory;

        private readonly ILogger<ActualizarMenuCronService>
            _logger;

        private readonly CronExpression
            _cronExpression;

        private readonly TimeZoneInfo
            _zonaHoraria;

        public ActualizarMenuCronService(
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration,
            ILogger<ActualizarMenuCronService> logger)
        {
            _serviceScopeFactory =
                serviceScopeFactory;

            _logger =
                logger;

            var expresionCron =
    configuration[
        "TareasProgramadas:ActualizarMenu:Cron"]
    ?? "0 5 * * *";

            _cronExpression =
                CronExpression.Parse(
                    expresionCron);

            _zonaHoraria =
                ObtenerZonaHorariaCostaRica();
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "La tarea programada de menús ha iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var ahoraUtc =
                    DateTimeOffset.UtcNow;

                var siguienteEjecucion =
                    _cronExpression
                        .GetNextOccurrence(
                            ahoraUtc,
                            _zonaHoraria);

                if (siguienteEjecucion == null)
                {
                    _logger.LogWarning(
                        "No fue posible calcular la siguiente " +
                        "ejecución de la tarea.");

                    return;
                }

                var tiempoEspera =
                    siguienteEjecucion.Value -
                    ahoraUtc;

                try
                {
                    if (tiempoEspera > TimeSpan.Zero)
                    {
                        await Task.Delay(
                            tiempoEspera,
                            stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    /*
                     * BackgroundService es singleton.
                     * Creamos un scope para utilizar
                     * servicios Scoped como DbContext,
                     * repositorios y servicios.
                     */

                    await using var scope =
                        _serviceScopeFactory
                            .CreateAsyncScope();

                    var servicioActualizarMenu =
                        scope.ServiceProvider
                            .GetRequiredService<
                                IServiceActualizarMenu>();

                    var ahoraCostaRica =
                        TimeZoneInfo.ConvertTime(
                            DateTimeOffset.UtcNow,
                            _zonaHoraria);

                    await servicioActualizarMenu
                        .RevisarMenuDisponiblesAsync(
                            ahoraCostaRica.DateTime,
                            stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Ocurrió un error al ejecutar " +
                        "la tarea programada de menús.");
                }
            }

            _logger.LogInformation(
                "La tarea programada de menús ha finalizado.");
        }

        private static TimeZoneInfo
            ObtenerZonaHorariaCostaRica()
        {
            /*
             * Nombre utilizado normalmente
             * en Linux y macOS.
             */

            try
            {
                return TimeZoneInfo
                    .FindSystemTimeZoneById(
                        "America/Costa_Rica");
            }
            catch (TimeZoneNotFoundException)
            {
                /*
                 * Nombre utilizado normalmente
                 * en Windows.
                 */

                return TimeZoneInfo
                    .FindSystemTimeZoneById(
                        "Central America Standard Time");
            }
        }
    }
}