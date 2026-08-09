using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedido
    {
        Task<Pedido> FindByIdAsync(int id);
        Task<int> AddAsync(Pedido entity, int idUsuarioCliente, int? idUsuarioEncargado);
        Task<ICollection<Pedido>> ListByClienteAsync(int idUsuarioCliente);
        Task<ICollection<Pedido>> ListAsync(DateTime? fecha, int? idEstadoPedido);
    }
}
