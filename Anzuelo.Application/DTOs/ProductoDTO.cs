using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record ProductoDTO
    {
        [Display(Name = "Código")]
        [ValidateNever]
        public int IdProducto { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(
            20,
            ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(
            45,
            ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Range(
            0.01,
            999999999,
            ErrorMessage = "{0} debe ser mayor que cero")]
        [DisplayFormat(
            ApplyFormatInEditMode = true,
            DataFormatString = "{0:C0}")]
        public decimal Precio { get; set; }

        /*
         * Llaves foráneas utilizadas en Create y Update.
         */

        [DisplayName("Categoría")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoriaProducto { get; set; }

        [DisplayName("Estado")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar un estado")]
        public int IdEstadoProducto { get; set; }

        [DisplayName("Preparación")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar una preparación")]
        public int IdPreparacion { get; set; }

        /*
         * Propiedades utilizadas para listado y detalle.
         */

        [Display(Name = "Imagen")]
        [ValidateNever]
        public byte[]? Imagen { get; set; }

        [Display(Name = "Categoría")]
        [ValidateNever]
        public string NombreCategoria { get; set; } = null!;

        [Display(Name = "Estado")]
        [ValidateNever]
        public string NombreEstado { get; set; } = null!;

        [Display(Name = "Preparación")]
        [ValidateNever]
        public string NombrePreparacion { get; set; } = null!;

        /*
         * Colecciones utilizadas para las imágenes
         * y los ingredientes del producto.
         */

        [ValidateNever]
        public ICollection<ImagenDTO> Imagenes { get; set; }
            = new List<ImagenDTO>();

        public ICollection<IngredienteDTO> Ingredientes { get; set; }
            = new List<IngredienteDTO>();
    }
}
