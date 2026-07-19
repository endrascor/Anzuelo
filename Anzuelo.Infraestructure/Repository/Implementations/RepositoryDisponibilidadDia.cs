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
    public class RepositoryDisponibilidadDia : IRepositoryDisponibilidadDia
    {
        private readonly AnzueloContext _context;

        public RepositoryDisponibilidadDia(
            AnzueloContext context)
        {
            _context = context;
        }

        public async Task<
            ICollection<DisponibilidadDia>>
            ListAsync()
        {
            return await _context
                .Set<DisponibilidadDia>()
                .AsNoTracking()
                .OrderBy(x =>
                    x.IdDisponibilidadDia)
                .ToListAsync();
        }
    }
}
