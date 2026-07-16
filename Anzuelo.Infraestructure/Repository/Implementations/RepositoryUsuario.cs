using Anzuelo.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly AnzueloContext _context;
        public RepositoryUsuario(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Usuario>> ListAsync()
        {
            var collection = await _context.Set<Usuario>()
                .Include(x => x.IdRolNavigation)
                .Include(x => x.IdEstadoUsuarioNavigation)
                .ToListAsync();
            return collection;
        }
        public async Task<Usuario> FindByIdAsync(int id)
        {
            var @Object = await _context.Set<Usuario>()
                .Include(x => x.IdRolNavigation)
                .Include(x => x.IdEstadoUsuarioNavigation)
                .FirstOrDefaultAsync(x => x.IdUsuario == id);
            return @Object!;
        }
    }
}
