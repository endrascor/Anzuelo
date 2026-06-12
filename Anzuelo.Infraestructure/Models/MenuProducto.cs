using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class MenuProducto
{
    public int IdMenu { get; set; }

    public int IdProducto { get; set; }

    public decimal Descuento { get; set; }

    public virtual Menu IdMenuNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
