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
    public class RepositoryCategoriaProducto : IRepositoryCategoriaProducto
    {
        private readonly AnzueloContext _context;

        public RepositoryCategoriaProducto(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CategoriaProducto>> ListAsync()
        {
            var collection = await _context.Set<CategoriaProducto>()
                .ToListAsync();
            return collection;
        }
    }
}
