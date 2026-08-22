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
            var resultado =
                await _servicePedidoEstacion
                    .IniciarAsync(id);

            if (!resultado)
            {
                return BadRequest(
                    "No se pudo iniciar la etapa.");
            }

            return Json(new
            {
                success = true
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id)
        {
            var resultado =
                await _servicePedidoEstacion
                    .FinalizarAsync(id);

            if (!resultado)
            {
                return BadRequest(
                    "No se pudo finalizar la etapa.");
            }

            return Json(new
            {
                success = true
            });
        }
    }
}