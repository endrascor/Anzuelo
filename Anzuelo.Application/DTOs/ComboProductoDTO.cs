using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record class ComboProductoDTO
    {
        public int IdProducto { get; init; }
        public string NombreProducto { get; init; } = null!;
        public int Cantidad { get; init; }
    }
}
