using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryPedidoEstacion :
        IRepositoryPedidoEstacion
    {
        private readonly AnzueloContext _context;

        public RepositoryPedidoEstacion(
            AnzueloContext context)
        {
            _context = context;
        }

        public async Task<PedidoEstacion?>
            FindByIdAsync(int id)
        {
            return await _context.PedidoEstacion

                .Include(x =>
                    x.IdProductoNavigation)

                .Include(x =>
                    x.IdDetallePedidoNavigation)
                    .ThenInclude(x =>
                        x.IdPedidoNavigation)

                .Include(x =>
                    x.IdDetallePedidoNavigation)
                    .ThenInclude(x =>
                        x.IdComboNavigation)

                .Include(x =>
                    x.IdEstacionCocinaNavigation)

                .Include(x =>
                    x.IdEstadoPedidoEstacionNavigation)

                .FirstOrDefaultAsync(
                    x => x.IdPedidoEstacion == id);
        }

        public async Task<ICollection<PedidoEstacion>>
            ListAsync()
        {
            return await _context.PedidoEstacion

                .AsNoTracking()

                .Include(x =>
                    x.IdProductoNavigation)

                .Include(x =>
                    x.IdDetallePedidoNavigation)
                    .ThenInclude(x =>
                        x.IdPedidoNavigation)

                .Include(x =>
                    x.IdDetallePedidoNavigation)
                    .ThenInclude(x =>
                        x.IdComboNavigation)

                .Include(x =>
                    x.IdEstacionCocinaNavigation)

                .Include(x =>
                    x.IdEstadoPedidoEstacionNavigation)

                .OrderBy(x =>
                    x.IdDetallePedidoNavigation.IdPedido)

                .ThenBy(x =>
                    x.IdDetallePedido)

                .ThenBy(x =>
                    x.IdProducto)

                .ThenBy(x =>
                    x.OrdenProceso)

                .ToListAsync();
        }

        public async Task<ICollection<PedidoEstacion>>
            ListProcesoProductoAsync(
                int idDetallePedido,
                int idProducto)
        {
            return await _context.PedidoEstacion

                .AsNoTracking()

                .Include(x =>
                    x.IdEstadoPedidoEstacionNavigation)

                .Where(x =>
                    x.IdDetallePedido ==
                    idDetallePedido &&
                    x.IdProducto ==
                    idProducto)

                .OrderBy(x =>
                    x.OrdenProceso)

                .ToListAsync();
        }

        public async Task<int?> FindEstadoIdAsync(
            string textoDescripcion)
        {
            var texto =
                textoDescripcion.Trim().ToLower();

            return await _context
                .EstadoPedidoEstacion

                .AsNoTracking()

                .Where(x =>
                    x.Descripcion
                        .ToLower()
                        .Contains(texto))

                .Select(x =>
                    (int?)x.IdEstadoPedidoEstacion)

                .FirstOrDefaultAsync();
        }

        public async Task<bool>
            TodasFinalizadasPorPedidoAsync(
                int idPedido,
                int idEstadoFinalizado)
        {
            var query =
                _context.PedidoEstacion
                    .AsNoTracking()
                    .Where(x =>
                        x.IdDetallePedidoNavigation
                            .IdPedido ==
                        idPedido);

            var existen =
                await query.AnyAsync();

            if (!existen)
                return false;

            return await query.AllAsync(
                x => x.IdEstadoPedidoEstacion ==
                     idEstadoFinalizado);
        }

        public async Task UpdateAsync(
            PedidoEstacion entity)
        {
            _context.PedidoEstacion.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}