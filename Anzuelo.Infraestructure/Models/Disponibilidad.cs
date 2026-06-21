using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anzuelo.Infraestructure.Models;

public partial class Disponibilidad
{
    public int IdDisponibilidad { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinal { get; set; }

    public DateTime HoraInicio { get; set; }

    public DateTime HoraFinal { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdDisponibilidadDia { get; set; }

    public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();

    public virtual DisponibilidadDia IdDisponibilidadDiaNavigation { get; set; } = null!;
}
