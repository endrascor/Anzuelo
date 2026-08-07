using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<ICollection<Usuario>> ListAsync();
        Task<Usuario> FindByIdAsync(int id);
        Task<string> AddAsync(Usuario entity);
        Task UpdateAsync();
        Task<Usuario> LoginAsync(string id, string password);
        Task<ICollection<Usuario>> ListByRolAsync(string nombreRol);

    }
}
