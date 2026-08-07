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
    public class ServiceMetodoPago : IServiceMetodoPago
    {
        private readonly IRepositoryMetodoPago _repository;
        private readonly IMapper _mapper;

        public ServiceMetodoPago(IRepositoryMetodoPago repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<MetodoPagoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<MetodoPagoDTO>>(list);
        }

        public async Task<MetodoPagoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<MetodoPagoDTO>(@object);
        }
    }
}
