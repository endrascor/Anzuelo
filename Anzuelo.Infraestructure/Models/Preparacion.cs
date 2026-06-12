using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Preparacion
{
    public int IdPreparacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<PreparacionEstacion> PreparacionEstacion { get; set; } = new List<PreparacionEstacion>();

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
