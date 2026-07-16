using Anzuelo.Infraestructure.Repository.Interfaces;
using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .OrderBy(x => x.Nombre)
                .ToListAsync();
            return collection;
        }

        public async Task<Usuario> FindByIdAsync(int id)
        {
            var @Object = await _context.Set<Usuario>()
                .Include(x => x.IdEstadoUsuarioNavigation)
                .Include(x => x.IdRolNavigation)
                .FirstOrDefaultAsync(x => x.IdUsuario == id);
            return @Object!;
        }
    }
}
