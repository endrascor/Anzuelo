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
    public class RepositoryMetodoPago : IRepositoryMetodoPago
    {
        private readonly AnzueloContext _context;
        public RepositoryMetodoPago(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<MetodoPago>> ListAsync()
        {
            var collection = await _context.Set<MetodoPago>()
                .OrderBy(x => x.IdMetodoPago)
                .ToListAsync();
            return collection;
        }
        public async Task<MetodoPago> FindByIdAsync(int id)
        {
            var @object = await _context.Set<MetodoPago>()
                .FirstOrDefaultAsync(x => x.IdMetodoPago == id);
            return @object!;
        }
    }
}
