using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceCategoriaProducto
    {
        Task<ICollection<CategoriaProductoDTO>> ListAync();
    }
}
