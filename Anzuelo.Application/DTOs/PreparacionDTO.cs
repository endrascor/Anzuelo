using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Anzuelo.Application.DTOs
{
    public record PreparacionDTO
    {
        [Display(Name = "Identificador Preparación")]
        [ValidateNever]
        public int IdPreparacion { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdProducto { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Producto")]
        [ValidateNever]
        public string NombreProducto { get; set; } = string.Empty;

        [Display(Name = "Estaciones")]
        [ValidateNever]
        public List<PreparacionEstacionDTO> Estaciones { get; set; } = new();
    }
}