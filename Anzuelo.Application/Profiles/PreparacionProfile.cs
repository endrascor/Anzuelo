using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;

namespace Anzuelo.Application.Profiles
{
    public class PreparacionProfile : Profile
    {
        public PreparacionProfile()
        {
            CreateMap<Preparacion, PreparacionDTO>()
                .ForMember(dest => dest.IdPreparacion,
                    orig => orig.MapFrom(o => o.IdPreparacion))
                .ForMember(dest => dest.Descripcion,
                    orig => orig.MapFrom(o => o.Descripcion))
                .ForMember(dest => dest.Estaciones,
                    orig => orig.MapFrom(o => o.PreparacionEstacion))
                .ForMember(dest => dest.NombresProductos,
                    orig => orig.MapFrom(o => o.Producto.Select(p => p.Nombre).ToList()))
                .ReverseMap();

            CreateMap<PreparacionEstacion, PreparacionEstacionDTO>()
                .ForMember(dest => dest.NumeroOrden,
                    orig => orig.MapFrom(o => o.NumeroOrden))
                .ForMember(dest => dest.NombreEstacion,
                    orig => orig.MapFrom(o => o.IdEstacionCocinaNavigation.Descripcion))
                .ForMember(dest => dest.TiempoEstimadoMinutos,
                    orig => orig.MapFrom(o => o.TiempoEstimadoMinutos))
                .ReverseMap();
        }
    }
}