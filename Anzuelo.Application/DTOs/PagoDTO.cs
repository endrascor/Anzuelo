using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record PagoDTO
    {
        [ValidateNever]
        public int IdPago { get; set; }

        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdMetodoPago { get; set; }

        [ValidateNever]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Monto { get; set; }
        [Display(Name = "Monto recibido")]
        public decimal? MontoRecibido { get; set; }
        [ValidateNever]
        public decimal? Vuelto { get; set; }
        [Display(Name = "Últimos 4 dígitos")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Debe tener {1} dígitos")]
        public string? Ultimos4Tarjeta { get; set; }
        [ValidateNever]
        public string? NombreMetodoPago { get; set; }
    }
}
