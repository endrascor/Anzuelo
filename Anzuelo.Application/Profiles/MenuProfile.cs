using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {

            CreateMap<Menu, MenuDTO>()
            .ForMember(d => d.NombreEstado,
                o => o.MapFrom(s => s.IdEstadoMenuNavigation.Descripcion))

            .ForMember(d => d.FechaInicio,
                o => o.MapFrom(s => s.IdDisponibilidadNavigation.FechaInicio))

            .ForMember(d => d.FechaFinal,
                o => o.MapFrom(s => s.IdDisponibilidadNavigation.FechaFinal))

            .ForMember(d => d.HoraInicio,
                o => o.MapFrom(s => s.IdDisponibilidadNavigation.HoraInicio))

            .ForMember(d => d.HoraFinal,
                o => o.MapFrom(s => s.IdDisponibilidadNavigation.HoraFinal))

            .ForMember(d => d.DescripcionDisponibilidad,
                o => o.MapFrom(s => s.IdDisponibilidadNavigation.Descripcion))

            .ForMember(d => d.ProductosPorCategoria,
                o => o.MapFrom(s =>
                    s.MenuProducto
                        .GroupBy(mp =>
                            mp.IdProductoNavigation.IdCategoriaProductoNavigation.Descripcion ?? "Sin categoría")
                        .Select(g => new MenuCategoriaProductoDTO
                        {
                            Categoria = g.Key,
                            Productos = g.Select(x => new ProductoDTO
                            {
                                IdProducto = x.IdProducto,
                                Nombre = x.IdProductoNavigation.Nombre,
                                Descripcion = x.IdProductoNavigation.Descripcion,
                                Precio = x.IdProductoNavigation.Precio,
                                NombreCategoria = g.Key,
                                Imagen = x.IdProductoNavigation.ImagenProducto
        .Select(i => i.Imagen)
        .FirstOrDefault()
                            }).ToList()
                        }).ToList()
                ))

            .ForMember(d => d.CombosPorCategoria,
    o => o.MapFrom(s =>
        s.MenuCombo
            .GroupBy(mc =>
                mc.IdComboNavigation.IdCategoriaComboNavigation.Descripcion ?? "Sin categoría")
            .Select(g => new MenuCategoriaComboDTO
            {
                Categoria = g.Key,

                Combos = g.Select(x => new ComboDTO
                {
                    IdCombo = x.IdCombo,
                    Nombre = x.IdComboNavigation.Nombre,
                    Descripcion = x.IdComboNavigation.Descripcion,
                    PrecioTotal = x.IdComboNavigation.PrecioTotal,
                    NombreCategoria = g.Key,

                    Productos = x.IdComboNavigation.ComboProducto
                        .Select(cp => new ComboProductoDTO
                        {
                            IdProducto = cp.IdProducto,
                            NombreProducto = cp.IdProductoNavigation.Nombre,
                            Cantidad = cp.Cantidad,

                            Imagen = cp.IdProductoNavigation.ImagenProducto
                                .Select(i => i.Imagen)
                                .FirstOrDefault()
                        })
                        .ToList()
                }).ToList()
            }).ToList()
    ));

        }
    }
}
