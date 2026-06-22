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
    var currentDay = (int)now.DayOfWeek;

    var menus = await _context.Set<Menu>()
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

        .ToListAsync();

    var menuDisponible = menus
        .Where(x =>
        {
            var d = x.IdDisponibilidadNavigation;

            if (d == null || d.IdDisponibilidadDiaNavigation == null)
                return false;

            var fechaValida =
                d.FechaInicio.Date <= currentDate &&
                d.FechaFinal.Date >= currentDate;

            var horaInicio = d.HoraInicio.TimeOfDay;
            var horaFinal = d.HoraFinal.TimeOfDay;

            var horaValida =
                horaInicio <= currentTime &&
                currentTime <= horaFinal;

            var dayToMatch = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Monday => 1,
                DayOfWeek.Tuesday => 2,
                DayOfWeek.Wednesday => 3,
                DayOfWeek.Thursday => 4,
                DayOfWeek.Friday => 5,
                DayOfWeek.Saturday => 6,
                DayOfWeek.Sunday => 7
            };

            var diaValido =
                d.IdDisponibilidadDiaNavigation.IdDisponibilidadDia == dayToMatch;

            return fechaValida && horaValida && diaValido;
        })
        .OrderByDescending(x => x.IdDisponibilidadNavigation.FechaInicio)
        .FirstOrDefault();
}
    }
}
