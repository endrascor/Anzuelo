using Anzuelo.Infraestructure.Models;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedido
    {
        Task<bool> PerteneceAlUsuarioAsync(int idPedido, int idUsuario);

        Task<Pedido> FindByIdAsync(int id);

        Task<ICollection<Pedido>> ListByClienteAsync(int idUsuarioCliente);

        Task<ICollection<Pedido>> ListAsync(
            DateTime? fecha,
            int? idEstadoPedido);

        Task<int> AddAsync(
            Pedido entity,
            int idUsuarioCliente,
            int? idUsuarioEncargado);

        Task UpdateAsync(Pedido entity);
    }
}
