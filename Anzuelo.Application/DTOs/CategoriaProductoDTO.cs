using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record CategoriaProductoDTO
    {
        public int IdCategoriaProducto { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
