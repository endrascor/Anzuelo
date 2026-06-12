using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class PedidoEstacion
{
    public int IdPedidoEstacion { get; set; }

    public int IdDetallePedido { get; set; }

    public int IdEstacionCocina { get; set; }

    public int IdEstadoPedidoEstacion { get; set; }

    public int OrdenProceso { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public int TiempoEstimadoMinutos { get; set; }

    public virtual DetallePedido IdDetallePedidoNavigation { get; set; } = null!;

    public virtual EstacionCocina IdEstacionCocinaNavigation { get; set; } = null!;

    public virtual EstadoPedidoEstacion IdEstadoPedidoEstacionNavigation { get; set; } = null!;
}
