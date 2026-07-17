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
    public class ServiceIngrediente
        : IServiceIngrediente
    {
        private readonly IRepositoryIngrediente _repository;

        public ServiceIngrediente(
            IRepositoryIngrediente repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<IngredienteDTO>> ListAsync()
        {
            var ingredientes = await _repository.ListAsync();

            return ingredientes
                .Select(x => new IngredienteDTO
                {
                    IdIngrediente = x.IdIngrediente,
                    NombreIngrediente =
                        x.NombreIngrediente
                })
                .ToList();
        }

        public async Task<IngredienteDTO> AddAsync(string nombre)
        {
            var ingrediente =
                await _repository.AddAsync(nombre);

            return new IngredienteDTO
            {
                IdIngrediente =
                    ingrediente.IdIngrediente,

                NombreIngrediente =
                    ingrediente.NombreIngrediente
            };
        }
    }
}
