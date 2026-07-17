using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceEstadoProducto
    {
        Task<ICollection<EstadoProductoDTO>> ListAync();
    }
}
