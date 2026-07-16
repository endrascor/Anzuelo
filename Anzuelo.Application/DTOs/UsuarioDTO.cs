using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido1 { get; set; } = null!;
        public string Apellido2 { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NombreRol { get; set; } = null!;
        public string NombreEstado { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
