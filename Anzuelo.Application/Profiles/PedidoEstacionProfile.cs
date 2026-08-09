using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;

namespace Anzuelo.Application.Profiles
{
    public class PedidoEstacionProfile : Profile
    {
        public PedidoEstacionProfile()
        {
            CreateMap<PedidoEstacion, PedidoEstacionDTO>()

                .ForMember(
                    dest => dest.IdPedido,
                    opt => opt.MapFrom(o =>
                        o.IdDetallePedidoNavigation.IdPedido))

                .ForMember(
                    dest => dest.NombreProducto,
                    opt => opt.MapFrom(o =>
                        o.IdProductoNavigation != null
                            ? o.IdProductoNavigation.Nombre
                            : string.Empty))

                .ForMember(
                    dest => dest.EsCombo,
                    opt => opt.MapFrom(o =>
                        o.IdDetallePedidoNavigation.IdCombo.HasValue))

                .ForMember(
                    dest => dest.NombreCombo,
                    opt => opt.MapFrom(o =>
                        o.IdDetallePedidoNavigation.IdComboNavigation != null
                            ? o.IdDetallePedidoNavigation.IdComboNavigation.Nombre
                            : string.Empty))

                .ForMember(
                    dest => dest.NombreEstacion,
                    opt => opt.MapFrom(o =>
                        o.IdEstacionCocinaNavigation != null
                            ? o.IdEstacionCocinaNavigation.Descripcion
                            : string.Empty))

                .ForMember(
                    dest => dest.NombreEstadoPedidoEstacion,
                    opt => opt.MapFrom(o =>
                        o.IdEstadoPedidoEstacionNavigation != null
                            ? o.IdEstadoPedidoEstacionNavigation.Descripcion
                            : string.Empty))

                .ForMember(
                    dest => dest.PuedeIniciar,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.PuedeFinalizar,
                    opt => opt.Ignore());


            CreateMap<PedidoEstacionDTO, PedidoEstacion>()

                .ForMember(
                    dest => dest.IdPedidoEstacion,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdDetallePedido,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdProductoNavigation,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdDetallePedidoNavigation,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdEstacionCocinaNavigation,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdEstadoPedidoEstacionNavigation,
                    opt => opt.Ignore());
        }
    }
}