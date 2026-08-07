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
    public class RepositoryEstadoPedido : IRepositoryEstadoPedido
    {
        private readonly AnzueloContext _context;
        public RepositoryEstadoPedido(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<EstadoPedido>> ListAsync()
        {
            var collection = await _context.Set<EstadoPedido>()
                .OrderBy(x => x.IdEstadoPedido)
                .ToListAsync();
            return collection;
        }
        public async Task<EstadoPedido> FindByIdAsync(int id)
        {
            var @object = await _context.Set<EstadoPedido>()
                .FirstOrDefaultAsync(x => x.IdEstadoPedido == id);
            return @object!;
        }
    }
}
