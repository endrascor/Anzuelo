namespace Anzuelo.Application.DTOs
{
    public record PedidoEstacionDTO
    {
        public int IdPedidoEstacion { get; set; }

        public int IdPedido { get; set; }

        public int IdDetallePedido { get; set; }

        public int IdProducto { get; set; }

        public string? NombreProducto { get; set; }

        public bool EsCombo { get; set; }

        public string? NombreCombo { get; set; }

        public int IdEstacionCocina { get; set; }

        public string? NombreEstacion { get; set; }

        public int IdEstadoPedidoEstacion { get; set; }

        public string? NombreEstadoPedidoEstacion { get; set; }

        public int OrdenProceso { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int TiempoEstimadoMinutos { get; set; }

        public bool PuedeIniciar { get; set; }

        public bool PuedeFinalizar { get; set; }
        public string? ClaseEstado { get; set; }
    }
}
