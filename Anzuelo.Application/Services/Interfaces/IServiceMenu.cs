using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceMenu
    {
        Task<ICollection<MenuDTO>>ListAync();

        Task<MenuDTO?> FindByIdAsync(int id);

        Task<MenuDTO?> GetMenuDisponibleAsync();

        Task<int> AddAsync(MenuDTO dto);

        Task UpdateAsync(int id,MenuDTO dto);
    }
}
