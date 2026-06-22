using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Anzuelo.Application.Profiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()
                .ForMember(dest => dest.NombreCategoria,
                    orig => orig.MapFrom(o => o.IdCategoriaComboNavigation != null ? o.IdCategoriaComboNavigation.Descripcion : string.Empty))
                .ForMember(dest => dest.NombreEstado,
                    orig => orig.MapFrom(o => o.IdEstadoComboNavigation != null ? o.IdEstadoComboNavigation.Descripcion : string.Empty))
                .ForMember(dest => dest.Productos,
                     orig => orig.MapFrom(o => o.ComboProducto))
                .ForMember(dest => dest.ImagenesProductos,
                    orig => orig.MapFrom(o => o.ComboProducto
                   .Select(cp => cp.IdProductoNavigation != null && cp.IdProductoNavigation.ImagenProducto != null
                    ? cp.IdProductoNavigation.ImagenProducto.FirstOrDefault()!.Imagen
                    : null)
                    .Where(img => img != null)
                    .ToList()))
                .ReverseMap();

            CreateMap<ComboProducto, ComboProductoDTO>()
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation != null ? src.IdProductoNavigation.Nombre : string.Empty))
                .ForMember(dest => dest.Imagen,
                    opt => opt.MapFrom(src =>
                    src.IdProductoNavigation != null && src.IdProductoNavigation.ImagenProducto != null
                    ? src.IdProductoNavigation.ImagenProducto.Select(i => i.Imagen).FirstOrDefault()
                    : null))
                .ReverseMap();
        }
    }
}
