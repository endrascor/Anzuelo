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
    public record MenuDTO
    {
        [Display(Name = "Código")]
        [ValidateNever]
        public int IdMenu { get; set; }

        [Display(Name = "Nombre del menú")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [StringLength(
            50,
            ErrorMessage =
                "{0} no puede superar los {1} caracteres")]
        public string NombreMenu { get; set; }
            = string.Empty;

        [Display(Name = "Descripción")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [StringLength(
            200,
            ErrorMessage =
                "{0} no puede superar los {1} caracteres")]
        public string Descripcion { get; set; }
            = string.Empty;

        /*
         * Estado.
         */

        [DisplayName("Estado")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Debe seleccionar un estado")]
        public int IdEstadoMenu { get; set; }

        [Display(Name = "Estado")]
        [ValidateNever]
        public string NombreEstado { get; set; }
            = string.Empty;

        /*
         * Disponibilidad.
         */

        [ValidateNever]
        public int IdDisponibilidad { get; set; }

        [Display(Name = "Fecha inicial")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha final")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; set; }

        [Display(Name = "Hora inicial")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }

        [Display(Name = "Hora final")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.Time)]
        public DateTime HoraFinal { get; set; }

        [Display(Name = "Descripción de disponibilidad")]
        [Required(
            ErrorMessage = "{0} es un dato requerido")]
        [StringLength(
            100,
            ErrorMessage =
                "{0} no puede superar los {1} caracteres")]
        public string DescripcionDisponibilidad
        {
            get;
            set;
        } = string.Empty;

        [Display(Name = "Día disponible")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Debe seleccionar un día")]
        public int IdDisponibilidadDia { get; set; }

        [Display(Name = "Día")]
        [ValidateNever]
        public string NombreDia { get; set; }
            = string.Empty;

        /*
         * Relaciones editables.
         */

        public ICollection<MenuProductoDTO> Productos
        {
            get;
            set;
        } = new List<MenuProductoDTO>();

        public ICollection<MenuComboDTO> Combos
        {
            get;
            set;
        } = new List<MenuComboDTO>();

        /*
         * Propiedades utilizadas por el detalle actual.
         */

        [ValidateNever]
        public ICollection<MenuCategoriaProductoDTO>
            ProductosPorCategoria
        {
            get;
            set;
        } = new List<MenuCategoriaProductoDTO>();

        [ValidateNever]
        public ICollection<MenuCategoriaComboDTO>
            CombosPorCategoria
        {
            get;
            set;
        } = new List<MenuCategoriaComboDTO>();
    }
}
