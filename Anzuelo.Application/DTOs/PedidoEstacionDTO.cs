using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record PedidoEstacionDTO
    {
        public int IdPedidoEstacion { get; set; }
        public int IdEstacionCocina { get; set; }
        public string? NombreEstacion { get; set; }
        public int IdEstadoPedidoEstacion { get; set; }
        public string? NombreEstadoPedidoEstacion { get; set; }
        public int OrdenProceso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TiempoEstimadoMinutos { get; set; }
        public string? ClaseEstado { get; set; }
    }
}
