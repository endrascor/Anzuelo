using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryPreparacion : IRepositoryPreparacion
    {
        private readonly AnzueloContext _context;
        public RepositoryPreparacion(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Preparacion>> ListAsync()
        {
            var collection = await _context.Preparacion
            .Include(x => x.PreparacionEstacion)
            .ThenInclude(x => x.IdEstacionCocinaNavigation)
            .Include(x => x.Producto)
            .ToListAsync();

            return collection;
        }

        public async Task<Preparacion> FindByIdAsync(int id)
        {
            var @Object = await _context.Preparacion
            .Include(x => x.PreparacionEstacion)
                .ThenInclude(x => x.IdEstacionCocinaNavigation)
            .Include(x => x.Producto)
            .FirstOrDefaultAsync(x => x.IdPreparacion == id);

            return @Object;
        }
    }
}
