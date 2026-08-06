using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record RolDTO
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } = null!;

    }
}
