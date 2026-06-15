using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServicePreparacion : IServicePreparacion
    {
        private readonly IRepositoryPreparacion _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServicePreparacion> _logger;
        public ServicePreparacion(IRepositoryPreparacion repository, IMapper mapper,
                            ILogger<ServicePreparacion> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<PreparacionDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PreparacionDTO>>(list);
            return collection;
        }

        public async Task<PreparacionDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PreparacionDTO>(@object);
            return objectMapped;
        }
    }
}
