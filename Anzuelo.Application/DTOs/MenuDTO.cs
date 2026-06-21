using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record MenuDTO
    {
        public int IdMenu { get; init; }
        public string NombreMenu { get; init; } = null!;
        public string Descripcion { get; init; } = null!;
        public string NombreEstado { get; init; } = null!;
        public DateTime FechaInicio { get; init; }
        public DateTime FechaFinal { get; init; }
        public DateTime HoraInicio { get; init; }
        public DateTime HoraFinal { get; init; }
        public string DescripcionDisponibilidad { get; init; } = null!;
        public ICollection<MenuCategoriaProductoDTO> ProductosPorCategoria { get; init; } = new List<MenuCategoriaProductoDTO>();
        public ICollection<MenuCategoriaComboDTO> CombosPorCategoria { get; init; } = new List<MenuCategoriaComboDTO>();
    }
}
