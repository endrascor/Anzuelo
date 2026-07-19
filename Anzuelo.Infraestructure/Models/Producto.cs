using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int IdCategoriaProducto { get; set; }

    public int IdEstadoProducto { get; set; }

    public virtual ICollection<ComboProducto> ComboProducto { get; set; } = new List<ComboProducto>();

    public virtual CategoriaProducto IdCategoriaProductoNavigation { get; set; } = null!;

    public virtual EstadoProducto IdEstadoProductoNavigation { get; set; } = null!;

    public virtual Preparacion Preparacion { get; set; } = null!;

    public virtual ICollection<ImagenProducto> ImagenProducto { get; set; } = new List<ImagenProducto>();

    public virtual ICollection<MenuProducto> MenuProducto { get; set; } = new List<MenuProducto>();

    public virtual ICollection<ProductoIngrediente> ProductoIngrediente { get; set; } = new List<ProductoIngrediente>();
}
