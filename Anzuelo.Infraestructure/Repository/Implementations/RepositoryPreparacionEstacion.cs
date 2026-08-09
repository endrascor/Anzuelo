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
    public class RepositoryPreparacionEstacion : IRepositoryPreparacionEstacion
    {
        private readonly AnzueloContext _context;

        public RepositoryPreparacionEstacion(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PreparacionEstacion>> ListByProductoAsync(int idProducto)
        {
            return await _context.Set<PreparacionEstacion>()
                .Include(pe => pe.IdPreparacionNavigation)
                .Where(pe => pe.IdPreparacionNavigation.IdProducto == idProducto)
                .OrderBy(pe => pe.NumeroOrden)
                .ToListAsync();
        }
    }
}
