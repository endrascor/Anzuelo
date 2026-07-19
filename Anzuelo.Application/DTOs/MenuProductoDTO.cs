using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record MenuProductoDTO
    {
        [Display(Name = "Producto")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar un producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Descuento")]
        [Range(
            0,
            100,
            ErrorMessage =
                "El descuento debe estar entre 0 y 100")]
        public decimal Descuento { get; set; }

        [ValidateNever]
        public string NombreProducto { get; set; }
            = string.Empty;
    }
}
