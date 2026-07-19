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

        public RepositoryMenu(
            AnzueloContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Menu>>
            ListAsync()
        {
            return await _context.Menu
                .AsNoTracking()

                .Include(menu =>
                    menu.IdEstadoMenuNavigation)

                .Include(menu =>
                    menu.IdDisponibilidadNavigation)

                    .ThenInclude(disponibilidad =>
                        disponibilidad
                            .IdDisponibilidadDiaNavigation)

                .OrderByDescending(menu =>
                    menu
                        .IdDisponibilidadNavigation
                        .FechaInicio)

                .ToListAsync();
        }

        public async Task<Menu?>FindByIdAsync(int id)
        {
            return await _context.Menu
                .AsNoTracking()

                .Include(menu =>
                    menu.IdEstadoMenuNavigation)

                .Include(menu =>
                    menu.IdDisponibilidadNavigation)

                    .ThenInclude(disponibilidad =>
                        disponibilidad
                            .IdDisponibilidadDiaNavigation)

                .Include(menu =>
                    menu.MenuProducto)

                    .ThenInclude(relacion =>
                        relacion.IdProductoNavigation)

                        .ThenInclude(producto =>
                            producto
                                .IdCategoriaProductoNavigation)

                .Include(menu =>
                    menu.MenuProducto)

                    .ThenInclude(relacion =>
                        relacion.IdProductoNavigation)

                        .ThenInclude(producto =>
                            producto.ImagenProducto)

                .Include(menu =>
                    menu.MenuCombo)

                    .ThenInclude(relacion =>
                        relacion.IdComboNavigation)

                        .ThenInclude(combo =>
                            combo.IdCategoriaComboNavigation)

                .Include(menu =>
                    menu.MenuCombo)

                    .ThenInclude(relacion =>
                        relacion.IdComboNavigation)

                        .ThenInclude(combo =>
                            combo.ComboProducto)

                            .ThenInclude(comboProducto =>
                                comboProducto
                                    .IdProductoNavigation)

                                    .ThenInclude(producto =>
                                        producto.ImagenProducto)

                .FirstOrDefaultAsync(menu =>
                    menu.IdMenu == id);
        }

        public async Task<Menu?>
            GetMenuDisponibleAsync()
        {
            var ahora = DateTime.Now;

            var fechaActual =
                ahora.Date;

            var horaActual =
                ahora.TimeOfDay;

            var diaActual =
                ahora.DayOfWeek == DayOfWeek.Sunday
                    ? 7
                    : (int)ahora.DayOfWeek;

            return await _context.Menu
                .AsNoTracking()

                .Include(menu =>
                    menu.IdDisponibilidadNavigation)

                    .ThenInclude(disponibilidad =>
                        disponibilidad
                            .IdDisponibilidadDiaNavigation)

                .Include(menu =>
                    menu.IdEstadoMenuNavigation)

                .Include(menu =>
                    menu.MenuProducto)

                    .ThenInclude(relacion =>
                        relacion.IdProductoNavigation)

                        .ThenInclude(producto =>
                            producto
                                .IdCategoriaProductoNavigation)

                .Include(menu =>
                    menu.MenuProducto)

                    .ThenInclude(relacion =>
                        relacion.IdProductoNavigation)

                        .ThenInclude(producto =>
                            producto.ImagenProducto)

                .Include(menu =>
                    menu.MenuCombo)

                    .ThenInclude(relacion =>
                        relacion.IdComboNavigation)

                        .ThenInclude(combo =>
                            combo.IdCategoriaComboNavigation)

                .Include(menu =>
                    menu.MenuCombo)

                    .ThenInclude(relacion =>
                        relacion.IdComboNavigation)

                        .ThenInclude(combo =>
                            combo.ComboProducto)

                            .ThenInclude(comboProducto =>
                                comboProducto
                                    .IdProductoNavigation)

                                    .ThenInclude(producto =>
                                        producto.ImagenProducto)

                .Where(menu =>
                    menu.IdDisponibilidadNavigation != null &&

                    menu
                        .IdDisponibilidadNavigation
                        .IdDisponibilidadDiaNavigation != null &&

                    menu
                        .IdDisponibilidadNavigation
                        .FechaInicio.Date <= fechaActual &&

                    menu
                        .IdDisponibilidadNavigation
                        .FechaFinal.Date >= fechaActual &&

                    menu
                        .IdDisponibilidadNavigation
                        .HoraInicio.TimeOfDay <= horaActual &&

                    horaActual <= menu
                        .IdDisponibilidadNavigation
                        .HoraFinal.TimeOfDay &&

                    menu
                        .IdDisponibilidadNavigation
                        .IdDisponibilidadDia == diaActual)

                .OrderByDescending(menu =>
                    menu
                        .IdDisponibilidadNavigation
                        .FechaInicio)

                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(Menu entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (entity.IdDisponibilidadNavigation == null)
            {
                throw new InvalidOperationException(
                    "El menú debe contener una disponibilidad.");
            }

            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            try
            {

                entity.IdMenu = 0;
                entity.IdDisponibilidad = 0;

                entity
                    .IdDisponibilidadNavigation
                    .IdDisponibilidad = 0;

                foreach (var producto in entity.MenuProducto)
                {
                    producto.IdMenu = 0;
                    producto.IdMenuNavigation = entity;
                }

                foreach (var combo in entity.MenuCombo)
                {
                    combo.IdMenu = 0;
                    combo.IdMenuNavigation = entity;
                }

                await _context.Menu.AddAsync(entity);

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return entity.IdMenu;
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(
            Menu entity)
        {
            await using var transaccion =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                var existente =
                    await _context.Menu

                        .Include(menu =>
                            menu.IdDisponibilidadNavigation)

                        .Include(menu =>
                            menu.MenuProducto)

                        .Include(menu =>
                            menu.MenuCombo)

                        .FirstOrDefaultAsync(menu =>
                            menu.IdMenu ==
                            entity.IdMenu);

                if (existente == null)
                {
                    throw new KeyNotFoundException(
                        $"No se encontró el menú con ID {entity.IdMenu}.");
                }

                existente.NombreMenu =
                    entity.NombreMenu;

                existente.Descripcion =
                    entity.Descripcion;

                existente.IdEstadoMenu =
                    entity.IdEstadoMenu;

                if (
                    existente
                        .IdDisponibilidadNavigation ==
                    null)
                {
                    existente
                        .IdDisponibilidadNavigation =
                            new Disponibilidad();
                }

                existente
                    .IdDisponibilidadNavigation
                    .FechaInicio =
                        entity
                            .IdDisponibilidadNavigation
                            .FechaInicio;

                existente
                    .IdDisponibilidadNavigation
                    .FechaFinal =
                        entity
                            .IdDisponibilidadNavigation
                            .FechaFinal;

                existente
                    .IdDisponibilidadNavigation
                    .HoraInicio =
                        entity
                            .IdDisponibilidadNavigation
                            .HoraInicio;

                existente
                    .IdDisponibilidadNavigation
                    .HoraFinal =
                        entity
                            .IdDisponibilidadNavigation
                            .HoraFinal;

                existente
                    .IdDisponibilidadNavigation
                    .Descripcion =
                        entity
                            .IdDisponibilidadNavigation
                            .Descripcion;

                existente
                    .IdDisponibilidadNavigation
                    .IdDisponibilidadDia =
                        entity
                            .IdDisponibilidadNavigation
                            .IdDisponibilidadDia;

                ActualizarProductos(
                    existente,
                    entity.MenuProducto);

                ActualizarCombos(
                    existente,
                    entity.MenuCombo);

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();
            }
            catch
            {
                await transaccion.RollbackAsync();

                throw;
            }
        }

        private void ActualizarProductos(
            Menu menuExistente,
            ICollection<MenuProducto>
                productosRecibidos)
        {
            var productosNuevos =
                productosRecibidos

                    .Where(relacion =>
                        relacion.IdProducto > 0)

                    .GroupBy(relacion =>
                        relacion.IdProducto)

                    .ToDictionary(
                        grupo =>
                            grupo.Key,

                        grupo =>
                            grupo.Last().Descuento);

            var productosAEliminar =
                menuExistente
                    .MenuProducto

                    .Where(relacion =>
                        !productosNuevos
                            .ContainsKey(
                                relacion.IdProducto))

                    .ToList();

            _context.MenuProducto
                .RemoveRange(
                    productosAEliminar);

            foreach (
                var productoNuevo
                in productosNuevos)
            {
                var relacionExistente =
                    menuExistente
                        .MenuProducto

                        .FirstOrDefault(relacion =>
                            relacion.IdProducto ==
                            productoNuevo.Key);

                if (relacionExistente != null)
                {
                    relacionExistente.Descuento =
                        productoNuevo.Value;
                }
                else
                {
                    menuExistente
                        .MenuProducto
                        .Add(
                            new MenuProducto
                            {
                                IdMenu =
                                    menuExistente.IdMenu,

                                IdProducto =
                                    productoNuevo.Key,

                                Descuento =
                                    productoNuevo.Value
                            });
                }
            }
        }

        private void ActualizarCombos(
            Menu menuExistente,
            ICollection<MenuCombo>
                combosRecibidos)
        {
            var combosNuevos =
                combosRecibidos

                    .Where(relacion =>
                        relacion.IdCombo > 0)

                    .GroupBy(relacion =>
                        relacion.IdCombo)

                    .ToDictionary(
                        grupo =>
                            grupo.Key,

                        grupo =>
                            grupo.Last().Descuento);

            var combosAEliminar =
                menuExistente
                    .MenuCombo

                    .Where(relacion =>
                        !combosNuevos
                            .ContainsKey(
                                relacion.IdCombo))

                    .ToList();

            _context.MenuCombo
                .RemoveRange(
                    combosAEliminar);

            foreach (
                var comboNuevo
                in combosNuevos)
            {
                var relacionExistente =
                    menuExistente
                        .MenuCombo

                        .FirstOrDefault(relacion =>
                            relacion.IdCombo ==
                            comboNuevo.Key);

                if (relacionExistente != null)
                {
                    relacionExistente.Descuento =
                        comboNuevo.Value;
                }
                else
                {
                    menuExistente
                        .MenuCombo
                        .Add(
                            new MenuCombo
                            {
                                IdMenu =
                                    menuExistente.IdMenu,

                                IdCombo =
                                    comboNuevo.Key,

                                Descuento =
                                    comboNuevo.Value
                            });
                }
            }
        }
    }
}
