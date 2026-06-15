using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPreparacion
    {
        Task<ICollection<Preparacion>> ListAsync();
        Task<Preparacion> FindByIdAsync(int id);
    }
}
