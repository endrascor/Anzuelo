using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Combo
{
    public int IdCombo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioTotal { get; set; }

    public int IdCategoriaCombo { get; set; }

    public int IdEstadoCombo { get; set; }

    public virtual ICollection<ComboProducto> ComboProducto { get; set; } = new List<ComboProducto>();

    public virtual CategoriaCombo IdCategoriaComboNavigation { get; set; } = null!;

    public virtual EstadoCombo IdEstadoComboNavigation { get; set; } = null!;

    public virtual ICollection<MenuCombo> MenuCombo { get; set; } = new List<MenuCombo>();
    public byte[]? Imagen { get; set; }
}
