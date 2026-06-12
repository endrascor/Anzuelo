using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string NombreMenu { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public bool ActivoPublico { get; set; }

    public int IdDisponibilidad { get; set; }

    public int IdEstadoMenu { get; set; }

    public virtual Disponibilidad IdDisponibilidadNavigation { get; set; } = null!;

    public virtual EstadoMenu IdEstadoMenuNavigation { get; set; } = null!;

    public virtual ICollection<MenuCombo> MenuCombo { get; set; } = new List<MenuCombo>();

    public virtual ICollection<MenuProducto> MenuProducto { get; set; } = new List<MenuProducto>();
}
