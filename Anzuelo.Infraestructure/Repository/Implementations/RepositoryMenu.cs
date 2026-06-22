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
    public class RepositoryMenu : IRepositoryMenu
    {
        private readonly AnzueloContext _context;
        public RepositoryMenu(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Menu>> ListAsync()
        {
            var collection = await _context.Set<Menu>()
                .Include(x => x.IdEstadoMenuNavigation)
                .Include(x => x.IdDisponibilidadNavigation)
                .OrderByDescending(x => x.IdDisponibilidadNavigation.FechaInicio)
                .ToListAsync();

            return collection;
        }

        public async Task<Menu> GetMenuDisponibleAsync()
        {
            var now = DateTime.Now;
            var currentDate = now.Date;
            var currentTime = now.TimeOfDay;
            var currentDay = now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;

            var @Object =  await _context.Set<Menu>()
                .AsNoTracking()
                .Include(x => x.IdDisponibilidadNavigation)
                    .ThenInclude(d => d.IdDisponibilidadDiaNavigation)
                .Include(x => x.IdEstadoMenuNavigation)
                .Include(x => x.MenuProducto)
                    .ThenInclude(mp => mp.IdProductoNavigation)
                        .ThenInclude(p => p.IdCategoriaProductoNavigation)
                .Include(x => x.MenuProducto)
                    .ThenInclude(mp => mp.IdProductoNavigation)
                        .ThenInclude(p => p.ImagenProducto)
                .Include(x => x.MenuCombo)
                    .ThenInclude(mc => mc.IdComboNavigation)
                        .ThenInclude(c => c.IdCategoriaComboNavigation)
                .Include(x => x.MenuCombo)
                    .ThenInclude(mc => mc.IdComboNavigation)
                        .ThenInclude(c => c.ComboProducto)
                            .ThenInclude(cp => cp.IdProductoNavigation)
                                .ThenInclude(p => p.ImagenProducto)
                .Where(x =>
                    x.IdDisponibilidadNavigation != null &&
                    x.IdDisponibilidadNavigation.IdDisponibilidadDiaNavigation != null &&
                    x.IdDisponibilidadNavigation.FechaInicio.Date <= currentDate &&
                    x.IdDisponibilidadNavigation.FechaFinal.Date >= currentDate &&
                    x.IdDisponibilidadNavigation.HoraInicio.TimeOfDay <= currentTime &&
                    currentTime <= x.IdDisponibilidadNavigation.HoraFinal.TimeOfDay &&
                    x.IdDisponibilidadNavigation.IdDisponibilidadDiaNavigation.IdDisponibilidadDia ==
                        currentDay
                )
                .OrderByDescending(x => x.IdDisponibilidadNavigation.FechaInicio)
                .FirstOrDefaultAsync();
            return @Object!;
        }
    }
}
