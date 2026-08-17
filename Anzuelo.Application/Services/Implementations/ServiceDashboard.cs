using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceDashboard : IServiceDashboard
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public ServiceDashboard(
            IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public async Task<DashboardDTO> ObtenerDashboardAsync(string? tipo, DateTime fechaInicio, DateTime fechaFin)
        {
            var detalles = await _repositoryPedido.ListDetallesParaDashboardAsync(fechaInicio, fechaFin);
            var pedidos = await _repositoryPedido.ListPedidosPorRangoFechaAsync(fechaInicio, fechaFin);

            var productos = detalles
                .Where(d => d.IdProductoNavigation != null || d.IdComboNavigation != null)
                .Select(d =>
                {
                    if (d.IdProductoNavigation != null)
                    {
                        return new DashboardProductoDTO
                        {
                            Id = d.IdProductoNavigation.IdProducto,
                            Nombre = d.IdProductoNavigation.Nombre,
                            Tipo = "Producto",
                            Cantidad = d.Cantidad
                        };
                    }

                    return new DashboardProductoDTO
                    {
                        Id = d.IdComboNavigation!.IdCombo,
                        Nombre = d.IdComboNavigation.Nombre,
                        Tipo = "Combo",
                        Cantidad = d.Cantidad
                    };
                });

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                productos = productos.Where(x => x.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));
            }

            var topProductos = productos
                .GroupBy(x => new { x.Id, x.Nombre, x.Tipo })
                .Select(g => new DashboardProductoDTO
                {
                    Id = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    Tipo = g.Key.Tipo,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(3)
                .Select((item, index) =>
                {
                    item.Posicion = index + 1;
                    return item;
                })
                .ToList();

            var estados = pedidos
                .Where(p => p.IdEstadoPedidoNavigation != null)
                .GroupBy(p => p.IdEstadoPedidoNavigation.Descripcion)
                .Select(g => new DashboardEstadoDTO
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .ToList();

            return new DashboardDTO
            {
                Productos = topProductos,
                Estados = estados
            };
        }
    }
}

