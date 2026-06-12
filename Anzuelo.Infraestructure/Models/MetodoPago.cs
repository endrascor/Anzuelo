using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class MetodoPago
{
    public int IdMetodoPago { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pago { get; set; } = new List<Pago>();
}
