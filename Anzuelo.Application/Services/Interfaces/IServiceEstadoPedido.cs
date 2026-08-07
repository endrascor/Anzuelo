using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceEstadoPedido
    {
        Task<ICollection<EstadoPedidoDTO>> ListAsync();
        Task<EstadoPedidoDTO> FindByIdAsync(int id);
    }
}
