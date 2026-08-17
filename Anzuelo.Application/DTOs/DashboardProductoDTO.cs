using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record DashboardProductoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public int Cantidad { get; set; }
        public int Posicion { get; set; }
    }
}
