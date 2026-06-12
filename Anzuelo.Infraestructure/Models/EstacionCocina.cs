using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstacionCocina
{
    public int IdEstacionCocina { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<PedidoEstacion> PedidoEstacion { get; set; } = new List<PedidoEstacion>();

    public virtual ICollection<PreparacionEstacion> PreparacionEstacion { get; set; } = new List<PreparacionEstacion>();
}
