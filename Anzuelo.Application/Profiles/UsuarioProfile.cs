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
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.IdRolNavigation.NombreRol))
                .ForMember(dest => dest.NombreEstado, opt => opt.MapFrom(src => src.IdEstadoUsuarioNavigation.NombreEstado))
                .ReverseMap();
        }
    }
}
