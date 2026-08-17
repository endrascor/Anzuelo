using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
        public record DashboardDTO
        {
            public ICollection<DashboardProductoDTO> Productos { get; set; } = new List<DashboardProductoDTO>();

            public ICollection<DashboardEstadoDTO> Estados { get; set; } = new List<DashboardEstadoDTO>();
        }
}
