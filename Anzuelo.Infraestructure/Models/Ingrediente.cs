using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string NombreIngrediente { get; set; } = null!;

    public virtual ICollection<ProductoIngrediente> ProductoIngrediente { get; set; } = new List<ProductoIngrediente>();
}
