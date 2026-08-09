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
    public class DetallePedidoProfile : Profile
    {
        public DetallePedidoProfile()
        {
            CreateMap<DetallePedido, DetallePedidoDTO>()
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(o =>
                        o.IdProductoNavigation != null ? o.IdProductoNavigation.Nombre :
                        o.IdComboNavigation != null ? o.IdComboNavigation.Nombre : string.Empty))
                .ForMember(dest => dest.Estaciones,
                    opt => opt.MapFrom(o => o.PedidoEstacion));

            CreateMap<DetallePedidoDTO, DetallePedido>()
                .ForMember(dest => dest.PedidoEstacion,
                    opt => opt.MapFrom(src => src.Estaciones));
        }
    }
}
