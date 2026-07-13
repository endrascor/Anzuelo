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
    public class ServiceCombo : IServiceCombo
    {
        private readonly IRepositoryCombo _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceCombo> _logger;
        public ServiceCombo(IRepositoryCombo repository, IMapper mapper,
                            ILogger<ServiceCombo> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ComboDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ComboDTO>>(list);
            return collection;
        }

        public async Task<ComboDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ComboDTO>(@object);
            return objectMapped;
        }

        public async Task<int> AddAsync(ComboDTO dto)
        {
            var entity = _mapper.Map<Combo>(dto);

            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ComboDTO dto)
        {
            var entity = _mapper.Map<Combo>(dto);

            await _repository.UpdateAsync(entity);

        }
    }
}