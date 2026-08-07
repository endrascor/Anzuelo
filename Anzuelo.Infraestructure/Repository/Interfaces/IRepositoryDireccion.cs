using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryDireccion
    {
        Task<ICollection<Direccion>> ListByUsuarioAsync(int idUsuario);
        Task<Direccion> FindByIdAsync(int id);
        Task<int> AddAsync(Direccion entity);
    }
}
