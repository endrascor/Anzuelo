using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Anzuelo.Application.DTOs
{
    public record PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdEstadoPedido { get; set; }
        public string? NombreEstado { get; set; }
        [Display(Name = "Método de entrega")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int? IdTipoEntrega { get; set; }
        public string? NombreTipoEntrega { get; set; }
        public int? IdDireccion { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdUsuarioCliente { get; set; }
        public int? IdUsuarioEncargado { get; set; }
        public string? NombreCliente { get; set; }
        public string? CedulaCliente { get; set; }
        public string? NombreEncargado { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Subtotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Impuesto { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal CostoEnvio { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total { get; set; }
        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto o combo")]
        public ICollection<DetallePedidoDTO> Detalles { get; set; } = new List<DetallePedidoDTO>();
        public PagoDTO Pago { get; set; } = null!;
    }
}
