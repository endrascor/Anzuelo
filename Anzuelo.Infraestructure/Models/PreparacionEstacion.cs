using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class PreparacionEstacion
{
    public int IdPreparacion { get; set; }

    public int IdEstacionCocina { get; set; }

    public int NumeroOrden { get; set; }

    public int TiempoEstimadoMinutos { get; set; }

    public virtual EstacionCocina IdEstacionCocinaNavigation { get; set; } = null!;

    public virtual Preparacion IdPreparacionNavigation { get; set; } = null!;
}
