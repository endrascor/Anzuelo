using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServicePedido
    {
        Task<PedidoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PedidoDTO dto, int idUsuarioLogueado, string rolUsuarioLogueado);
        Task<ICollection<PedidoDTO>> ListHistorialAsync(int idUsuarioLogueado, string rolUsuarioLogueado, System.DateTime? fecha, int? idEstadoPedido);
        Task<bool> PerteneceAlUsuarioAsync(int idPedido, int idUsuario);
    }
}
