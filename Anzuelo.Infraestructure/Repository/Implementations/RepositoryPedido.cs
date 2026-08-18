using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryPedido : IRepositoryPedido
    {
        private readonly AnzueloContext _context;

        public RepositoryPedido(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<bool> PerteneceAlUsuarioAsync(int idPedido, int idUsuario)
        {
            return await _context.Set<Pedido>()
                .AnyAsync(p => p.IdPedido == idPedido && p.IdUsuario.Any(u => u.IdUsuario == idUsuario));
        }

        public async Task<Pedido> FindByIdAsync(int id)
        {
            var pedido = await _context.Set<Pedido>()
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdTipoEntregaNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.IdUsuario)
                    .ThenInclude(u => u.IdRolNavigation)
                .Include(p => p.Pago)
                    .ThenInclude(pa => pa.IdMetodoPagoNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.IdComboNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.PedidoEstacion)
                        .ThenInclude(pe => pe.IdProductoNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.PedidoEstacion)
                        .ThenInclude(pe => pe.IdEstacionCocinaNavigation)
                .Include(p => p.DetallePedido)
                    .ThenInclude(d => d.PedidoEstacion)
                        .ThenInclude(pe => pe.IdEstadoPedidoEstacionNavigation)
                .FirstOrDefaultAsync(p => p.IdPedido == id);
            return pedido!;
        }

        public async Task<ICollection<Pedido>> ListByClienteAsync(int idUsuarioCliente)
        {
            var collection = await _context.Set<Pedido>()
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdTipoEntregaNavigation)
                .Include(p => p.IdUsuario)
                    .ThenInclude(u => u.IdRolNavigation)
                .Where(p => p.IdUsuario.Any(u => u.IdUsuario == idUsuarioCliente))
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return collection;
        }

        public async Task<ICollection<Pedido>> ListAsync(DateTime? fecha, int? idEstadoPedido)
        {
            var query = _context.Set<Pedido>()
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdTipoEntregaNavigation)
                .Include(p => p.IdUsuario)
                    .ThenInclude(u => u.IdRolNavigation)
                .AsQueryable();

            if (fecha.HasValue)
            {
                query = query.Where(p => p.FechaPedido.Date == fecha.Value.Date);
            }

            if (idEstadoPedido.HasValue)
            {
                query = query.Where(p => p.IdEstadoPedido == idEstadoPedido.Value);
            }

            return await query
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Pedido entity, int idUsuarioCliente, int? idUsuarioEncargado)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var cliente = await _context.Set<Usuario>().FindAsync(idUsuarioCliente);
                entity.IdUsuario.Add(cliente!);

                if (idUsuarioEncargado.HasValue)
                {
                    var encargado = await _context.Set<Usuario>().FindAsync(idUsuarioEncargado.Value);
                    entity.IdUsuario.Add(encargado!);
                }

                await _context.Set<Pedido>().AddAsync(entity);
                await _context.SaveChangesAsync();
                var seguimiento = new SeguimientoPedido
                {
                    IdPedido = entity.IdPedido,
                    IdEstadoPedido = entity.IdEstadoPedido,
                    Observacion = "Pedido registrado"
                };
                await _context.Set<SeguimientoPedido>().AddAsync(seguimiento);
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();

                return entity.IdPedido;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(Pedido entity)
        {
            _context.Set<Pedido>().Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<DetallePedido>> ListDetallesParaDashboardAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<DetallePedido>()
                .AsNoTracking()
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdComboNavigation)
                .Include(d => d.IdPedidoNavigation)
                .Where(d => d.IdPedidoNavigation.FechaPedido.Date >= fechaInicio.Date &&
                            d.IdPedidoNavigation.FechaPedido.Date <= fechaFin.Date)
                .ToListAsync();
        }

        public async Task<ICollection<Pedido>> ListPedidosPorRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<Pedido>()
                .AsNoTracking()
                .Include(p => p.IdEstadoPedidoNavigation)
                .Where(p => p.FechaPedido.Date >= fechaInicio.Date &&
                            p.FechaPedido.Date <= fechaFin.Date)
                .ToListAsync();
        }
    }
}
