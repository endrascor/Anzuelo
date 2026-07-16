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
    public class RepositoryEstacionCocina : IRepositoryEstacionCocina
    {
        private readonly AnzueloContext _context;

        public RepositoryEstacionCocina(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<EstacionCocina>> ListAsync()
        {
            var collection = await _context.Set<EstacionCocina>()
                .ToListAsync();
            return collection;
        }
    }
}
