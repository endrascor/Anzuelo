using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record PreparacionDTO
    {
        public int IdPreparacion { get; init; }
        public string Descripcion { get; init; } = null!;
        public List<PreparacionEstacionDTO> Estaciones { get; init; } = new();
        public List<string> NombresProductos { get; init; } = new();
    }
}
