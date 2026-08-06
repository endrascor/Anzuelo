using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anzuelo.Application.DTOs;
namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
      Task<ICollection<UsuarioDTO>> ListAsync();
      Task<UsuarioDTO> FindByIdAsync(int id);
      Task<UsuarioDTO> LoginAsync(string id, string password);
      Task<string> AddAsync(UsuarioDTO dto);
    }
}
