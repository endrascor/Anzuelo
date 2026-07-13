using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Anzuelo.Application.DTOs
{
    public record PreparacionEstacionDTO
    {
        [Display(Name = "Estación")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdEstacionCocina { get; set; }

        [Display(Name = "Estación")]
        [ValidateNever]
        public string? NombreEstacion { get; set; }

        [Display(Name = "Orden")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(1, 99, ErrorMessage = "El orden mínimo es {0}")]
        public int NumeroOrden { get; set; }

        [Display(Name = "Tiempo Estimado (min)")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(1, 999, ErrorMessage = "El tiempo mínimo es {0}")]
        public int TiempoEstimadoMinutos { get; set; }
    }
}