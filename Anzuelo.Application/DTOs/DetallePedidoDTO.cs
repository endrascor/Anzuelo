using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record DetallePedidoDTO
    {
        [ValidateNever]
        public int IdDetallePedido { get; set; }
        public int? IdProducto { get; set; }
        public int? IdCombo { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(1, 999, ErrorMessage = "La cantidad debe ser al menos {1}")]
        public int Cantidad { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string? Observaciones { get; set; }
        [ValidateNever]
        public decimal PrecioUnitario { get; set; }
        [ValidateNever]
        public decimal Subtotal { get; set; }
        [ValidateNever]
        public decimal Impuesto { get; set; }

        [ValidateNever]
        public string? Nombre { get; set; }
    }
}
