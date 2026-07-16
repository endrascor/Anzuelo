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
    public class RepositoryCategoriaCombo : IRepositoryCategoriaCombo
    {
        private readonly AnzueloContext _context;

        public RepositoryCategoriaCombo(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CategoriaCombo>> ListAsync()
        {
            var collection = await _context.Set<CategoriaCombo>()
                .ToListAsync();
            return collection;
        }
    }
}
