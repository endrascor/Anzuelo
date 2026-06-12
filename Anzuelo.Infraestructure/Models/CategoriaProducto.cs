using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class CategoriaProducto
{
    public int IdCategoriaProducto { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
