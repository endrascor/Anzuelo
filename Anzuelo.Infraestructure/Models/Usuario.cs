using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int IdRol { get; set; }

    public int IdEstadoUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string PasswordTemporal { get; set; } = null!;

    public virtual ICollection<Direccion> Direccion { get; set; } = new List<Direccion>();

    public virtual EstadoUsuario IdEstadoUsuarioNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> IdPedido { get; set; } = new List<Pedido>();
}
