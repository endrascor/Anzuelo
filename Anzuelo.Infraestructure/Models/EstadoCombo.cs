using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstadoCombo
{
    public int IdEstadoCombo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Combo> Combo { get; set; } = new List<Combo>();
}
