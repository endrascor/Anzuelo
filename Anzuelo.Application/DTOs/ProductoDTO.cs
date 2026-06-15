using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ProductoDTO
    {
        [Display(Name = "Codigo")]
        public int IdProducto { get; init; }
        public string Nombre { get; init; } = null!;
        public string Descripcion { get; init; } = null!;
        public decimal Precio { get; init; }

        public byte[]? Imagen { get; init; }

        [Display(Name = "Categoria")]
        public string NombreCategoria { get; init; } = null!;
        public string NombreEstado { get; init; } = null!;
        public ICollection<ImagenDTO> Imagenes { get; init; } = new List<ImagenDTO>();
        public ICollection<IngredienteDTO> Ingredientes { get; init; } = new List<IngredienteDTO>();
    }
}
