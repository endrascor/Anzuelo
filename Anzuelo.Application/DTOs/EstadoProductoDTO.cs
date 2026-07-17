using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public class EstadoProductoDTO
    {
        public int IdEstadoProducto { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
