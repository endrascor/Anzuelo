using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class EstadoUsuario
{
    public int IdEstadoUsuario { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
