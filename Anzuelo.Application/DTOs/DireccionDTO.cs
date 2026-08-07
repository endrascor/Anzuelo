using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.DTOs
{
    public record DireccionDTO
    {
        public int IdDireccion { get; set; }

        [Display(Name = "Cantón")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Canton { get; set; } = null!;

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Provincia { get; set; } = null!;

        [Display(Name = "Distrito")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Distrito { get; set; } = null!;

        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(45, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Detalle { get; set; } = null!;
        public int IdUsuario { get; set; }
    }
}
