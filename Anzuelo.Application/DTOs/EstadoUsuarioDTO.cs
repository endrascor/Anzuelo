using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public class EstadoUsuarioDTO
    {
        public int IdEstadoUsuario { get; set; }
        public string NombreEstado { get; set; } = null!;
    }
}
