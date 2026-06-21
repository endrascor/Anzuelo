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
    public class ServiceMenu : IServiceMenu
    {
        private readonly IRepositoryMenu _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceMenu> _logger;
        public ServiceMenu(IRepositoryMenu repository, IMapper mapper,
                            ILogger<ServiceMenu> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<MenuDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<MenuDTO>>(list);
            return collection;
        }

        public async Task<MenuDTO> GetMenuDisponibleAsync()
        {
            var @object = await _repository.GetMenuDisponibleAsync();
            var objectMapped = _mapper.Map<MenuDTO>(@object);
            return objectMapped;
        }
    }
}