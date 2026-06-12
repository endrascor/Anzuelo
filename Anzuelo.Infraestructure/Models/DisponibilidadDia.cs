using System;
using System.Collections.Generic;

namespace Anzuelo.Infraestructure.Models;

public partial class DisponibilidadDia
{
    public int IdDisponibilidadDia { get; set; }

    public string DiaSemana { get; set; } = null!;
}
