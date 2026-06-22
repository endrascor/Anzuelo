using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;
using System.Linq;

namespace Anzuelo.Application.Profiles
{
    public class PreparacionProfile : Profile
    {
        public PreparacionProfile()
        {
            CreateMap<Preparacion, PreparacionDTO>()
                .ForMember(dest => dest.Estaciones,
                    orig => orig.MapFrom(o => o.PreparacionEstacion))
                .ForMember(dest => dest.NombresProductos,
                    orig => orig.MapFrom(o => o.Producto != null ? o.Producto.Select(p => p.Nombre).ToList() : new System.Collections.Generic.List<string>()))
                .ReverseMap();

            CreateMap<PreparacionEstacion, PreparacionEstacionDTO>()
                .ForMember(dest => dest.NombreEstacion,
                    orig => orig.MapFrom(o => o.IdEstacionCocinaNavigation != null ? o.IdEstacionCocinaNavigation.Descripcion : string.Empty))
                .ReverseMap();
        }
    }
}
