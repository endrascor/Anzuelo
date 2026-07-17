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
    public class ServiceCategoriaProducto : IServiceCategoriaProducto
    {
        private readonly IRepositoryCategoriaProducto _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCategoriaProducto> _logger;

        public ServiceCategoriaProducto(IRepositoryCategoriaProducto repository, IMapper mapper, ILogger<ServiceCategoriaProducto> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<CategoriaProductoDTO>> ListAync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CategoriaProductoDTO>>(list);
            return collection;
        }
    }
}
