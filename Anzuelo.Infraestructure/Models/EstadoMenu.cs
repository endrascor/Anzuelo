using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstadoMenu
{
    public int IdEstadoMenu { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();
}
