using Anzuelo.Application.DTOs;

namespace Anzuelo.Application.Services.Interfaces
{
    public interface IServiceTipoCambio
    {
        Task<TipoCambioDTO> ObtenerTipoCambioAsync();
    }
}