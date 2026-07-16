using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceUsuario> _logger;
        public ServiceUsuario(IRepositoryUsuario repository, IMapper mapper,
                            ILogger<ServiceUsuario> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<UsuarioDTO>> ListAync()
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
    }
}
