using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record DashboardEstadoDTO
    {
        public string Estado { get; set; } = null!;

        public int Cantidad { get; set; }
    }
}
