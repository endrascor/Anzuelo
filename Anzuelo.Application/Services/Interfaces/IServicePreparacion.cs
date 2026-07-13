using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServicePreparacion
    {
        Task<ICollection<PreparacionDTO>> ListAync();
        Task<PreparacionDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PreparacionDTO dto);
        Task UpdateAsync(PreparacionDTO dto);
    }
}
