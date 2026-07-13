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
        public async Task<int> AddAsync(Preparacion entity)
        {
            await _context.Preparacion.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdPreparacion;
        }

        public async Task UpdateAsync(Preparacion entity)
        {
            var existente = await _context.Preparacion
                .Include(x => x.PreparacionEstacion)
                .FirstAsync(x => x.IdPreparacion == entity.IdPreparacion);

            existente.Descripcion = entity.Descripcion;

            _context.RemoveRange(existente.PreparacionEstacion);

            foreach (var estacion in entity.PreparacionEstacion)
            {
                estacion.IdPreparacion = entity.IdPreparacion;
                existente.PreparacionEstacion.Add(estacion);
            }

            await _context.SaveChangesAsync();
        }
    }
}
