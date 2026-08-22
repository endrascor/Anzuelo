using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using System.Net.Http.Json;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceTipoCambio : IServiceTipoCambio
    {
        private readonly HttpClient _httpClient;

        public ServiceTipoCambio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TipoCambioDTO> ObtenerTipoCambioAsync()
        {
            var tipoCambio =
                await _httpClient.GetFromJsonAsync<TipoCambioDTO>(
                    "https://api.hacienda.go.cr/indicadores/tc/dolar");

            if (tipoCambio == null)
            {
                throw new Exception(
                    "No se pudo obtener el tipo de cambio.");
            }

            return tipoCambio;
        }
    }
}