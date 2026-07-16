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
    public class ServiceEstadoCombo : IServiceEstadoCombo
    {
        private readonly IRepositoryEstadoCombo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceEstadoCombo> _logger;

        public ServiceEstadoCombo(IRepositoryEstadoCombo repository, IMapper mapper, ILogger<ServiceEstadoCombo> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<EstadoComboDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstadoComboDTO>>(list);
        }
    }
}
