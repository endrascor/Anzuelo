namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceActualizarMenu
    {
        Task RevisarMenuDisponiblesAsync(
            DateTime fechaHora,
            CancellationToken cancellationToken);
    }
}
