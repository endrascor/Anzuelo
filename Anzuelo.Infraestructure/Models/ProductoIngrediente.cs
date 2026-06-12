using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class ProductoIngrediente
{
    public int IdProducto { get; set; }

    public int IdIngrediente { get; set; }

    public int Cantidad { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
