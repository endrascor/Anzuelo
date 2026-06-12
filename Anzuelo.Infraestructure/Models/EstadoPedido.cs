using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstadoPedido
{
    public int IdEstadoPedido { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();

    public virtual ICollection<SeguimientoPedido> SeguimientoPedido { get; set; } = new List<SeguimientoPedido>();
}
