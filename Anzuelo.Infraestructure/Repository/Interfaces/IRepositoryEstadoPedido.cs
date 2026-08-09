using Anzuelo.Infraestructure.Models;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEstadoPedido
    {
        Task<ICollection<EstadoPedido>> ListAsync();

        Task<EstadoPedido> FindByIdAsync(int id);

        Task<EstadoPedido?> FindByDescripcionAsync(
            string descripcion);
    }
}