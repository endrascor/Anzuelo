using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ImagenDTO
    {
        public int IdImagenProducto { get; init; }
        public byte[] Imagen { get; init; } = null!;
    }
}
