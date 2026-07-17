using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record IngredienteDTO
    {
        [Display(Name = "Ingrediente")]
        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar un ingrediente")]
        public int IdIngrediente { get; set; }

        [Display(Name = "Nombre del ingrediente")]
        [ValidateNever]
        public string NombreIngrediente { get; set; } = null!;

        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue,
            ErrorMessage = "{0} debe ser mayor que cero")]
        public int Cantidad { get; set; }
    }
}
