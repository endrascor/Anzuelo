using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryTipoEntrega : IRepositoryTipoEntrega
    {
        private readonly AnzueloContext _context;
        public RepositoryTipoEntrega(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<TipoEntrega>> ListAsync()
        {
            var collection = await _context.Set<TipoEntrega>()
                .OrderBy(x => x.IdTipoEntrega)
                .ToListAsync();
            return collection;
        }
        public async Task<TipoEntrega> FindByIdAsync(int id)
        {
            var @object = await _context.Set<TipoEntrega>()
                .FirstOrDefaultAsync(x => x.IdTipoEntrega == id);
            return @object!;
        }
    }
}
