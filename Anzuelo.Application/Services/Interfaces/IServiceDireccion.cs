using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceDireccion
    {
        Task<ICollection<DireccionDTO>> ListByUsuarioAsync(int idUsuario);
        Task<DireccionDTO> FindByIdAsync(int id);
        Task<int> AddAsync(DireccionDTO dto);
    }
}
