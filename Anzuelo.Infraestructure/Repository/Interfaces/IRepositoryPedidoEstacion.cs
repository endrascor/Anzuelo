using Anzuelo.Infraestructure.Models;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedidoEstacion
    {
        Task<PedidoEstacion?> FindByIdAsync(int id);

        Task<ICollection<PedidoEstacion>> ListAsync();

        Task<ICollection<PedidoEstacion>>
            ListProcesoProductoAsync(
                int idDetallePedido,
                int idProducto);

        Task<int?> FindEstadoIdAsync(
            string textoDescripcion);

        Task<bool> TodasFinalizadasPorPedidoAsync(
            int idPedido,
            int idEstadoFinalizado);

        Task UpdateAsync(
            PedidoEstacion entity);
    }
}