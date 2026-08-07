using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEstadoPedido
    {
        Task<ICollection<EstadoPedido>> ListAsync();
        Task<EstadoPedido> FindByIdAsync(int id);
    }
}
