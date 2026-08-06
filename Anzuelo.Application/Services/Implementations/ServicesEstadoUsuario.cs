using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceEstadoUsuario : IServiceEstadoUsuario
    {
        private readonly IRepositoryEstadoUsuario _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceEstadoUsuario> _logger;

        public ServiceEstadoUsuario(IRepositoryEstadoUsuario repository, IMapper mapper, ILogger<ServiceEstadoUsuario> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<EstadoUsuarioDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstadoUsuarioDTO>>(list);
        }
    }
}
