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
    public class RepositoryRol : IRepositoryRol
    {
        private readonly AnzueloContext _context;

        public RepositoryRol(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Rol>> ListAsync()
        {
            return await _context.Set<Rol>()

                .ToListAsync();
        }
    }
}
