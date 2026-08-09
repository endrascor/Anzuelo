using Anzuelo.Application.DTOs;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServicePedidoEstacion
    {
        Task<ICollection<PedidoEstacionDTO>> ListAsync();

        Task<PedidoEstacionDTO?> FindByIdAsync(int id);

        Task<bool> IniciarAsync(int id);

        Task<bool> FinalizarAsync(int id);
    }
}