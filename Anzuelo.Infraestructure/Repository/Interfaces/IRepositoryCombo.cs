using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCombo
    {
        Task<ICollection<Combo>> ListAsync();
        Task<Combo> FindByIdAsync(int id);
        Task<int> AddAsync(Combo entity);
        Task UpdateAsync(Combo entity);
        Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null);
    }
}
