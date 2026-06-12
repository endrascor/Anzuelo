using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class ImagenProducto
{
    public int IdImagenProducto { get; set; }

    public int IdProducto { get; set; }

    public string Imagen { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
