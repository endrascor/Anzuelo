using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Cocina
{
    public int IdCocina { get; set; }

    public int IdPreparacion { get; set; }

    public DateTime Tiempo { get; set; }

    public virtual Preparacion IdPreparacionNavigation { get; set; } = null!;
}
