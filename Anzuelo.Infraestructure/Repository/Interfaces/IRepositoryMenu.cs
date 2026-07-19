using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryMenu
    {
        Task<ICollection<Menu>> ListAsync();

        Task<Menu?> FindByIdAsync(int id);

        Task<Menu?> GetMenuDisponibleAsync();

        Task<int> AddAsync(Menu entity);

        Task UpdateAsync(Menu entity);
    }
}
