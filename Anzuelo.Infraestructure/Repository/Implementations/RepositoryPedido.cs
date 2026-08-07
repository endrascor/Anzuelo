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
                .FirstOrDefaultAsync(p => p.IdPedido == id);
            return pedido!;
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
    }
}
