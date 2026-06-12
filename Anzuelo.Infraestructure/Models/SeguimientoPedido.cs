using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class SeguimientoPedido
{
    public int IdSeguimientoPedido { get; set; }

    public int IdPedido { get; set; }

    public int IdEstadoPedido { get; set; }

    public DateTime FechaCambio { get; set; }

    public string Observacion { get; set; } = null!;

    public virtual EstadoPedido IdEstadoPedidoNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
