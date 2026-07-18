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
                .ForMember(dest => dest.Estaciones,
                    orig => orig.MapFrom(o => o.PreparacionEstacion))
                .ForMember(dest => dest.NombreProducto,
                    orig => orig.MapFrom(o => o.IdProductoNavigation != null ? o.IdProductoNavigation.Nombre : string.Empty))
                .ReverseMap();

            CreateMap<PreparacionEstacion, PreparacionEstacionDTO>()
                .ForMember(dest => dest.NombreEstacion,
                    orig => orig.MapFrom(o => o.IdEstacionCocinaNavigation != null ? o.IdEstacionCocinaNavigation.Descripcion : string.Empty))
                .ReverseMap();
        }
    }
}