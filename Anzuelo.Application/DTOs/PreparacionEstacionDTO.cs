using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record PreparacionEstacionDTO
    {
        public int NumeroOrden { get; init; }
        public string NombreEstacion { get; init; } = null!;
        public int TiempoEstimadoMinutos { get; init; }
    }
}
