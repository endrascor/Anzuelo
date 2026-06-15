using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record class ComboDTO
    {
        public int IdCombo { get; init; }
        public string Nombre { get; init; } = null!;
        public string Descripcion { get; init; } = null!;
        public decimal PrecioTotal { get; init; }
        public string NombreCategoria { get; init; } = null!;
        public string NombreEstado { get; init; } = null!;
        public ICollection<ComboProductoDTO> Productos { get; init; } = new List<ComboProductoDTO>();
    }
}
