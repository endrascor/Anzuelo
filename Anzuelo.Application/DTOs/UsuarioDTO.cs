using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record UsuarioDTO
    {
        [Display(Name = "Codigo")]
        public int IdUsuario { get; init; }
        public string Cedula { get; init; } = null!;
        public string Nombre { get; init; } = null!;
        public string Apellido1 { get; init; } = null!;
        public string Apellido2 { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string NombreRol { get; init; } = null!;
        public string NombreEstado { get; init; } = null!;
    }
}
