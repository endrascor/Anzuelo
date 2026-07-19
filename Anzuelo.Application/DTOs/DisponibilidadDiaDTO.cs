using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record DisponibilidadDiaDTO
    {
        [Display(Name = "Código")]
        public int IdDisponibilidadDia { get; set; }

        [Display(Name = "Día de la semana")]
        public string DiaSemana { get; set; }
            = string.Empty;
    }
}
