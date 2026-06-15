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
    public class RepositoryCombo : IRepositoryCombo
    {
        private readonly AnzueloContext _context;
        public RepositoryCombo(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Combo>> ListAsync()
        {
            var collection = await _context.Set<Combo>()
                .Include(x => x.IdCategoriaComboNavigation)
                .Include(x => x.IdEstadoComboNavigation)
                .OrderByDescending(x => x.Nombre)
                .ToListAsync();
            return collection;
        }

        public async Task<Combo> FindByIdAsync(int id)
        {
            var @Object = await _context.Set<Combo>()
                .Include(x => x.IdCategoriaComboNavigation)
                .Include(x => x.IdEstadoComboNavigation)
                .Include(x => x.ComboProducto)
                .ThenInclude(x => x.IdProductoNavigation)
                .FirstOrDefaultAsync(x => x.IdCombo == id);
            return @Object;
        }
    }
}
