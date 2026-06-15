using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record IngredienteDTO
    {
        public int IdIngrediente { get; init; }

        public string NombreIngrediente { get; init; } = null!;

        public decimal Cantidad { get; init; }
    }
}
