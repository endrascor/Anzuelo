using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anzuelo.Web.Controllers
{
    [Authorize(Roles = "Cocina,Admin")]
    public class PedidoEstacionController : Controller
    {
        private readonly IServicePedidoEstacion
            _servicePedidoEstacion;


        public PedidoEstacionController(
            IServicePedidoEstacion servicePedidoEstacion)
        {
            _servicePedidoEstacion =
                servicePedidoEstacion;
        }


        public async Task<ActionResult> Index()
        {
            var collection =
                await _servicePedidoEstacion.ListAsync();

            return View(collection);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Iniciar(int id)
        {
            await _servicePedidoEstacion
                .IniciarAsync(id);

            return RedirectToAction(
                nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id)
        {
            await _servicePedidoEstacion
                .FinalizarAsync(id);

            return RedirectToAction(
                nameof(Index));
        }
    }
}