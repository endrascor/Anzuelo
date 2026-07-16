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
    public class ServiceCategoriaCombo : IServiceCategoriaCombo
    {
        private readonly IRepositoryCategoriaCombo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCategoriaCombo> _logger;

        public ServiceCategoriaCombo(IRepositoryCategoriaCombo repository, IMapper mapper, ILogger<ServiceCategoriaCombo> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<CategoriaComboDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CategoriaComboDTO>>(list);
            return collection;
        }
    }
}
