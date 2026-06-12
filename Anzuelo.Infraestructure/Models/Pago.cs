using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdPedido { get; set; }

    public int IdMetodoPago { get; set; }

    public decimal Monto { get; set; }

    public decimal MontoRecibido { get; set; }

    public decimal Vuelto { get; set; }

    public string Ultimos4Tarjeta { get; set; } = null!;

    public DateTime FechaPago { get; set; }

    public virtual MetodoPago IdMetodoPagoNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
