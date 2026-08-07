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
    public class RepositoryDireccion : IRepositoryDireccion
    {
        private readonly AnzueloContext _context;
        public RepositoryDireccion(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Direccion>> ListByUsuarioAsync(int idUsuario)
        {
            var collection = await _context.Set<Direccion>()
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();
            return collection;
        }
        public async Task<Direccion> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Direccion>()
                .FirstOrDefaultAsync(x => x.IdDireccion == id);
            return @object!;
        }
        public async Task<int> AddAsync(Direccion entity)
        {
            await _context.Set<Direccion>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdDireccion;
        }
    }
}
