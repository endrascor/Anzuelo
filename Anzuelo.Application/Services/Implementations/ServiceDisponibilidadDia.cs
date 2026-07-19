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
    public class ServiceDisponibilidadDia :
        IServiceDisponibilidadDia
    {
        private readonly
            IRepositoryDisponibilidadDia _repository;

        private readonly IMapper _mapper;

        private readonly
            ILogger<ServiceDisponibilidadDia> _logger;

        public ServiceDisponibilidadDia(
            IRepositoryDisponibilidadDia repository,
            IMapper mapper,
            ILogger<ServiceDisponibilidadDia> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<
            ICollection<DisponibilidadDiaDTO>>
            ListAync()
        {
            var dias =
                await _repository.ListAsync();

            return _mapper.Map<
                ICollection<DisponibilidadDiaDTO>>(
                    dias);
        }
    }
}
