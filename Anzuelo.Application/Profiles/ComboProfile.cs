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
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()
                .ForMember(dest => dest.IdCombo, orig => orig.MapFrom(o => o.IdCombo))
                .ForMember(dest => dest.Nombre, orig => orig.MapFrom(o => o.Nombre))
                .ForMember(dest => dest.Descripcion, orig => orig.MapFrom(o => o.Descripcion))
                .ForMember(dest => dest.PrecioTotal, orig => orig.MapFrom(o => o.PrecioTotal))
                .ForMember(dest => dest.NombreCategoria,
                    orig => orig.MapFrom(o => o.IdCategoriaComboNavigation.Descripcion))
                .ForMember(dest => dest.NombreEstado,
                    orig => orig.MapFrom(o => o.IdEstadoComboNavigation.Descripcion))
                .ForMember(dest => dest.Productos,
                    orig => orig.MapFrom(o => o.ComboProducto))
                .ReverseMap();

            CreateMap<ComboProducto, ComboProductoDTO>()
                .ForMember(dest => dest.IdProducto,
        opt => opt.MapFrom(src => src.IdProducto))

                .ForMember(dest => dest.NombreProducto,
        opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))

                .ForMember(dest => dest.Cantidad,
        opt => opt.MapFrom(src => src.Cantidad))

                .ForMember(dest => dest.Imagen,
        opt => opt.MapFrom(src =>
            src.IdProductoNavigation.ImagenProducto
                .Select(i => i.Imagen)
                .FirstOrDefault()))

                .ReverseMap();
        }
    }
}
