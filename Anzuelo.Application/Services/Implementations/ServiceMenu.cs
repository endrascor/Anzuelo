using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceMenu : IServiceMenu
    {
        private readonly IRepositoryMenu _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceMenu> _logger;

        public ServiceMenu(
            IRepositoryMenu repository,
            IMapper mapper,
            ILogger<ServiceMenu> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<MenuDTO>>
            ListAync()
        {
            var lista =
                await _repository.ListAsync();

            return _mapper.Map<
                ICollection<MenuDTO>>(lista);
        }

        public async Task<MenuDTO?> FindByIdAsync(int id)
        {
            var menu =
                await _repository
                    .FindByIdAsync(id);

            if (menu == null)
            {
                return null;
            }

            return _mapper.Map<MenuDTO>(
                menu);
        }

        public async Task<MenuDTO?>
            GetMenuDisponibleAsync()
        {
            var menu =
                await _repository
                    .GetMenuDisponibleAsync();

            if (menu == null)
            {
                return null;
            }

            return _mapper.Map<MenuDTO>(
                menu);
        }

        public async Task<int> AddAsync(MenuDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var menu = _mapper.Map<Menu>(dto);

            menu.IdMenu = 0;
            menu.IdDisponibilidad = 0;

            var disponibilidad =
                CrearDisponibilidad(dto);

            disponibilidad.IdDisponibilidad = 0;

            menu.IdDisponibilidadNavigation =
                disponibilidad;

            menu.MenuProducto =
                CrearProductos(dto.Productos);

            menu.MenuCombo =
                CrearCombos(dto.Combos);

            return await _repository.AddAsync(menu);
        }

        public async Task UpdateAsync(int id,MenuDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "El identificador del menú no es válido.");
            }

            var menu =
                _mapper.Map<Menu>(dto);

            menu.IdMenu = id;

            menu.IdDisponibilidadNavigation =
                CrearDisponibilidad(dto);

            menu.MenuProducto =
                CrearProductos(dto.Productos);

            menu.MenuCombo =
                CrearCombos(dto.Combos);

            await _repository.UpdateAsync(menu);
        }

        private static Disponibilidad CrearDisponibilidad(MenuDTO dto)
        {
            return new Disponibilidad
            {
                FechaInicio =
                    dto.FechaInicio.Date,

                FechaFinal =
                    dto.FechaFinal.Date,

                HoraInicio =
                    dto.HoraInicio,

                HoraFinal =
                    dto.HoraFinal,

                Descripcion =
                    dto.DescripcionDisponibilidad.Trim(),

                IdDisponibilidadDia =
                    dto.IdDisponibilidadDia
            };
        }

        private static ICollection<MenuProducto> CrearProductos(ICollection<MenuProductoDTO>? productos)
        {
            if (productos == null)
            {
                return new List<MenuProducto>();
            }

            return productos
                .Where(producto =>
                    producto.IdProducto > 0)

                .GroupBy(producto =>
                    producto.IdProducto)

                .Select(grupo =>
                    new MenuProducto
                    {
                        IdProducto =
                            grupo.Key,

                        Descuento =
                            grupo.Last().Descuento
                    })

                .ToList();
        }

        private static ICollection<MenuCombo>CrearCombos(ICollection<MenuComboDTO>? combos)
        {
            if (combos == null)
            {
                return new List<MenuCombo>();
            }

            return combos
                .Where(combo =>
                    combo.IdCombo > 0)

                .GroupBy(combo =>
                    combo.IdCombo)

                .Select(grupo =>
                    new MenuCombo
                    {
                        IdCombo =
                            grupo.Key,

                        Descuento =
                            grupo.Last().Descuento
                    })

                .ToList();
        }
    }
}