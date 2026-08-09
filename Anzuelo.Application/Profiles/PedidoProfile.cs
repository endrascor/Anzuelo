using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;
using System.Linq;

namespace Anzuelo.Application.Profiles
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.NombreEstado,
                    opt => opt.MapFrom(o => o.IdEstadoPedidoNavigation != null ? o.IdEstadoPedidoNavigation.Descripcion : string.Empty))
                .ForMember(dest => dest.NombreTipoEntrega,
                    opt => opt.MapFrom(o => o.IdTipoEntregaNavigation != null ? o.IdTipoEntregaNavigation.Descripcion : string.Empty))
                .ForMember(dest => dest.Detalles,
                    opt => opt.MapFrom(o => o.DetallePedido))
                .ForMember(dest => dest.Pago,
                    opt => opt.MapFrom(o => o.Pago.FirstOrDefault()))
                .ForMember(dest => dest.NombreCliente, opt => opt.Ignore())
                .ForMember(dest => dest.CedulaCliente, opt => opt.Ignore())
                .ForMember(dest => dest.NombreEncargado, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioCliente, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioEncargado, opt => opt.Ignore());

            CreateMap<PedidoDTO, Pedido>()
                .ForMember(dest => dest.IdPedido, opt => opt.Ignore())
                .ForMember(dest => dest.IdTipoEntrega,
                    opt => opt.MapFrom(src => src.IdTipoEntrega ?? 0))
                .ForMember(dest => dest.DetallePedido,
                    opt => opt.MapFrom(src => src.Detalles))
                .ForMember(dest => dest.Pago,
                    opt => opt.MapFrom(src => new[] { src.Pago }))
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore());
        }
    }
}