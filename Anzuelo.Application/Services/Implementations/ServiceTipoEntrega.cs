using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceTipoEntrega : IServiceTipoEntrega
    {
        private readonly IRepositoryTipoEntrega _repository;
        private readonly IMapper _mapper;

        public ServiceTipoEntrega(IRepositoryTipoEntrega repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<TipoEntregaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<TipoEntregaDTO>>(list);
        }

        public async Task<TipoEntregaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<TipoEntregaDTO>(@object);
        }
    }
}
