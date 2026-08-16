using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceFacturaPedido
    {
        Task<byte[]> GenerarFacturaAsync(int idPedido);
        Task<bool> EnviarFacturaAsync(int idPedido, string email);
    }
}
