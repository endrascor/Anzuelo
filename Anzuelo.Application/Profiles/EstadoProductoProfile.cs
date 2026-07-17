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
    public class EstadoProductoProfile : Profile
    {
        public EstadoProductoProfile()
        {
            CreateMap<EstadoProducto, EstadoProductoDTO>().ReverseMap();
        }
    }
}
