using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Application.Utils;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Anzuelo.Application.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;

        public ServiceUsuario(IRepositoryUsuario repository, IOptions<AppConfig> options, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<ICollection<UsuarioDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<UsuarioDTO>>(list);
            return collection;
        }
        public async Task<UsuarioDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<UsuarioDTO>(@object);
            return objectMapped;
        }
        public async Task<UsuarioDTO> LoginAsync(string id, string password)
        {
            UsuarioDTO usuarioDTO = null!;

            // Llave secreta
            string secret = _options.Value.Crypto.Secret;
            // Password encriptado
            string passwordEncrypted = Cryptography.Encrypt(password, secret);

            var @object = await _repository.LoginAsync(id, passwordEncrypted);

            if (@object != null)
            {
                usuarioDTO = _mapper.Map<UsuarioDTO>(@object);
            }

            return usuarioDTO;
        }

        public async Task<string> AddAsync(UsuarioDTO dto)
        {
            // Llave secreta
            string secret = _options.Value.Crypto.Secret;

            // Encriptar contraseña ingresada por el usuario
            string passwordEncrypted =
                Cryptography.Encrypt(dto.PasswordHash, secret);

            dto.PasswordHash = passwordEncrypted;

            // Fecha automática
            dto.FechaRegistro = DateTime.Now;

            // Mapear DTO -> Usuario
            var objectMapped =
                _mapper.Map<Usuario>(dto);

            string passwordTemporal =
            Guid.NewGuid()
            .ToString("N")
            .Substring(0, 10);

            objectMapped.PasswordTemporal =
                Cryptography.Encrypt(
                    passwordTemporal,
                    secret
                );

            return await _repository.AddAsync(objectMapped);
        }
    }
}
