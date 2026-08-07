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
    public class ServiceEstadoPedido : IServiceEstadoPedido
    {
        private readonly IRepositoryEstadoPedido _repository;
        private readonly IMapper _mapper;

        public ServiceEstadoPedido(IRepositoryEstadoPedido repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<EstadoPedidoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstadoPedidoDTO>>(list);
        }

        public async Task<EstadoPedidoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<EstadoPedidoDTO>(@object);
        }
    }
}
