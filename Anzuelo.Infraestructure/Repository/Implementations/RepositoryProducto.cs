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
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly AnzueloContext _context;
        public RepositoryProducto(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Producto>> ListAsync()
        {
            var collection = await _context.Set<Producto>()
                .Include(x => x.IdCategoriaProductoNavigation)
                .Include(x => x.IdEstadoProductoNavigation)
                .Include(x => x.ImagenProducto)
                .OrderBy(x => x.Nombre)
                .ToListAsync();
           return collection;
        }

        public async Task<Producto> FindByIdAsync(int id)
        {
            var @Object = await _context.Set<Producto>()
                .Include(x => x.IdCategoriaProductoNavigation)
        .Include(x => x.IdEstadoProductoNavigation)
        .Include(x => x.IdPreparacionNavigation)

        .Include(x => x.ImagenProducto)

        .Include(x => x.ProductoIngrediente)
            .ThenInclude(pi => pi.IdIngredienteNavigation)

        .Include(x => x.ComboProducto)
            .ThenInclude(cp => cp.IdComboNavigation)

        .Include(x => x.MenuProducto)
            .ThenInclude(mp => mp.IdMenuNavigation)

        .FirstOrDefaultAsync(x => x.IdProducto == id);
            return @Object;
        }
    }
}
