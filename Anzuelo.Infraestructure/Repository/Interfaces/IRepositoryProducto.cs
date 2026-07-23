using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<ICollection<Producto>> ListAsync();
        Task<Producto> FindByIdAsync(int id);
        Task<int> AddAsync(Producto entity);
        Task UpdateAsync(Producto entity);
        Task<bool> ExisteNombreAsync(
    string nombre,
    int? idProductoExcluir = null);
    }
}
