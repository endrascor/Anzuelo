using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;

namespace Anzuelo.Application.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(
                    dest => dest.NombreEstado,
                    opt => opt.MapFrom(src =>
                        src.IdEstadoUsuarioNavigation != null
                            ? src.IdEstadoUsuarioNavigation.NombreEstado
                            : string.Empty)
                )
                .ForMember(
                    dest => dest.NombreRol,
                    opt => opt.MapFrom(src =>
                        src.IdRolNavigation != null
                            ? src.IdRolNavigation.NombreRol
                            : string.Empty)
                );

            CreateMap<UsuarioDTO, Usuario>()

                .ForMember(
                    dest => dest.IdUsuario,
                    opt => opt.Ignore()
                )

                .ForMember(
                    dest => dest.IdRolNavigation,
                    opt => opt.Ignore()
                )

                .ForMember(
                    dest => dest.IdEstadoUsuarioNavigation,
                    opt => opt.Ignore()
                )

                .ForMember(
                    dest => dest.Direccion,
                    opt => opt.Ignore()
                )

                .ForMember(
                    dest => dest.IdPedido,
                    opt => opt.Ignore()
                )

                .ForMember(
                    dest => dest.PasswordTemporal,
                    opt => opt.Ignore()
                );
        }
    }
}