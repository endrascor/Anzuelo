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
    public class RepositoryIngrediente
        : IRepositoryIngrediente
    {
        private readonly AnzueloContext _context;

        public RepositoryIngrediente(
            AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Ingrediente>> ListAsync()
        {
            return await _context.Set<Ingrediente>()
                .OrderBy(x => x.NombreIngrediente)
                .ToListAsync();
        }

        public async Task<Ingrediente> AddAsync(string nombre)
        {
            string nombreLimpio = nombre.Trim();

            var existente = await _context.Set<Ingrediente>()
                .FirstOrDefaultAsync(x =>
                    x.NombreIngrediente == nombreLimpio);

            if (existente != null)
            {
                return existente;
            }

            var ingrediente = new Ingrediente
            {
                NombreIngrediente = nombreLimpio
            };

            await _context.Set<Ingrediente>()
                .AddAsync(ingrediente);

            await _context.SaveChangesAsync();

            return ingrediente;
        }
    }
}
