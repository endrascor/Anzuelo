using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Anzuelo.Application.DTOs;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceEstacionCocina : IServiceEstacionCocina
    {
        private readonly IRepositoryEstacionCocina _repositoryEstacionCocina;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceEstacionCocina> _logger;

        public ServiceEstacionCocina(IRepositoryEstacionCocina repositoryEstacionCocina, IMapper mapper, ILogger<ServiceEstacionCocina> logger)
        {
            _repositoryEstacionCocina = repositoryEstacionCocina;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ICollection<EstacionCocinaDTO>> ListAync()
        {
            var list = await _repositoryEstacionCocina.ListAsync();
            return _mapper.Map<ICollection<EstacionCocinaDTO>>(list);
        }
    }
}
