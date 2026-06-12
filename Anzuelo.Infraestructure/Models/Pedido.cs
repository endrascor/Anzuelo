using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public DateTime FechaPedido { get; set; }

    public int IdEstadoPedido { get; set; }

    public int IdTipoEntrega { get; set; }

    public int IdDireccion { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal CostoEnvio { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual EstadoPedido IdEstadoPedidoNavigation { get; set; } = null!;

    public virtual TipoEntrega IdTipoEntregaNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pago { get; set; } = new List<Pago>();

    public virtual ICollection<SeguimientoPedido> SeguimientoPedido { get; set; } = new List<SeguimientoPedido>();

    public virtual ICollection<Usuario> IdUsuario { get; set; } = new List<Usuario>();
}
