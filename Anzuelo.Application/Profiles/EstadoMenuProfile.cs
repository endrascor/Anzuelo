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
    public class EstadoMenuProfile : Profile
    {
        public EstadoMenuProfile()
        {
            CreateMap<EstadoMenu, EstadoMenuDTO>().ReverseMap();
        }
    }
}
