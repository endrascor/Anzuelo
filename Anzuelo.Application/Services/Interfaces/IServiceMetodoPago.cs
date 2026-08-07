using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceMetodoPago
    {
        Task<ICollection<MetodoPagoDTO>> ListAsync();
        Task<MetodoPagoDTO> FindByIdAsync(int id);
    }
}
