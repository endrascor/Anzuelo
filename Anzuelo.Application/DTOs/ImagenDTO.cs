using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ImagenDTO
    {
        [ValidateNever]
        public int IdImagenProducto { get; set; }

        [Display(Name = "Imagen")]
        [ValidateNever]
        public byte[]? Imagen { get; set; }
    }
}
