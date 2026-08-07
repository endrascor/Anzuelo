using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceDireccion : IServiceDireccion
    {
        private readonly IRepositoryDireccion _repository;
        private readonly IMapper _mapper;

        public ServiceDireccion(IRepositoryDireccion repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<DireccionDTO>> ListByUsuarioAsync(int idUsuario)
        {
            var list = await _repository.ListByUsuarioAsync(idUsuario);
            return _mapper.Map<ICollection<DireccionDTO>>(list);
        }

        public async Task<DireccionDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<DireccionDTO>(@object);
        }

        public async Task<int> AddAsync(DireccionDTO dto)
        {
            var entity = _mapper.Map<Direccion>(dto);
            return await _repository.AddAsync(entity);
        }
    }
}
