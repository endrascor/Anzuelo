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
    public class EstadoComboProfile : Profile
    {
        public EstadoComboProfile()
        {
            CreateMap<EstadoCombo, EstadoComboDTO>().ReverseMap();
        }
    }
}
