using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ComboProductoDTO
    {
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdProducto { get; set; }

        [Display(Name = "Producto")]
        [ValidateNever]
        public string? NombreProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(1, 999, ErrorMessage = "La cantidad mínima es {0}")]
        public int Cantidad { get; set; }

        [ValidateNever]
        public byte[]? Imagen { get; set; }
    }
}
