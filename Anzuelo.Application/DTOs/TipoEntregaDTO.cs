using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record TipoEntregaDTO
    {
        public int IdTipoEntrega { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = null!;
    }
}
