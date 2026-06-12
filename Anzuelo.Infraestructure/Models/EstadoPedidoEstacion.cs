using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstadoPedidoEstacion
{
    public int IdEstadoPedidoEstacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<PedidoEstacion> PedidoEstacion { get; set; } = new List<PedidoEstacion>();
}
