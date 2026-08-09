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
    public class PagoProfile : Profile
    {
        public PagoProfile()
        {
            CreateMap<Pago, PagoDTO>()
                .ForMember(dest => dest.NombreMetodoPago,
                    opt => opt.MapFrom(o => o.IdMetodoPagoNavigation != null ? o.IdMetodoPagoNavigation.Descripcion : string.Empty));

            CreateMap<PagoDTO, Pago>();
        }
    }
}
