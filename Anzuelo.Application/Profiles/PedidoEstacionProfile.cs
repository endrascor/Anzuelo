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
    public class PedidoEstacionProfile : Profile
    {
        public PedidoEstacionProfile()
        {
            CreateMap<PedidoEstacion, PedidoEstacionDTO>()
                .ForMember(dest => dest.NombreEstacion,
                    opt => opt.MapFrom(o => o.IdEstacionCocinaNavigation != null ? o.IdEstacionCocinaNavigation.Descripcion : string.Empty))
                .ForMember(dest => dest.NombreEstadoPedidoEstacion,
                    opt => opt.MapFrom(o => o.IdEstadoPedidoEstacionNavigation != null ? o.IdEstadoPedidoEstacionNavigation.Descripcion : string.Empty));

            CreateMap<PedidoEstacionDTO, PedidoEstacion>()
                .ForMember(dest => dest.IdPedidoEstacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdDetallePedido, opt => opt.Ignore());
        }
    }
}
