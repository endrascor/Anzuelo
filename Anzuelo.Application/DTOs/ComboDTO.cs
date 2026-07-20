using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Anzuelo.Application.DTOs
{
    public record ComboDTO
    {
        [Display(Name = "Identificador Combo")]
        [ValidateNever]
        public int IdCombo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(20, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Precio Total")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(0.01, 999999999, ErrorMessage = "El valor mínimo es {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioTotal { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int? IdCategoriaCombo { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int? IdEstadoCombo { get; set; }

        [Display(Name = "Imagen Combo")]
        public byte[]? Imagen { get; set; }

        [Display(Name = "Categoría")]
        [ValidateNever]
        public string? NombreCategoria { get; set; }

        [Display(Name = "Estado")]
        [ValidateNever]
        public string? NombreEstado { get; set; }

        [ValidateNever]
        public ICollection<byte[]> ImagenesProductos { get; set; } = new List<byte[]>();

        [Display(Name = "Productos")]
        [ValidateNever]
        public ICollection<ComboProductoDTO> Productos { get; set; } = new List<ComboProductoDTO>();
    }
}