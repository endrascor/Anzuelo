using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ProductoConEstacionesDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public ICollection<PedidoEstacionDTO> Estaciones { get; set; } = new List<PedidoEstacionDTO>();
    }
}
