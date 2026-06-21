using Anzuelo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record MenuCategoriaComboDTO
    {
        public string Categoria { get; init; } = null!;
        public List<ComboDTO> Combos { get; init; } = new();
    }
}
