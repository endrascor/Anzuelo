using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Direccion
{
    public int IdDireccion { get; set; }

    public string Canton { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public string Distrito { get; set; } = null!;

    public string Detalle { get; set; } = null!;

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
