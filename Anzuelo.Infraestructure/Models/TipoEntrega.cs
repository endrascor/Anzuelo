using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class TipoEntrega
{
    public int IdTipoEntrega { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
