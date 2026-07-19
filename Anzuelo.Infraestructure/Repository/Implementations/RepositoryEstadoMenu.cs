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
    public class RepositoryEstadoMenu : IRepositoryEstadoMenu
    {
        private readonly AnzueloContext _context;

        public RepositoryEstadoMenu(AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<EstadoMenu>> ListAsync()
        {
            return await _context.Set<EstadoMenu>()

                .ToListAsync();
        }
    }
}
