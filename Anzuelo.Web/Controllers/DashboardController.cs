using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anzuelo.Web.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class DashboardController : Controller
    {
        private readonly IServiceDashboard _serviceDashboard;

        public DashboardController(IServiceDashboard serviceDashboard)
        {
            _serviceDashboard = serviceDashboard;
        }

        public async Task<IActionResult> Index(string? tipo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inicio = fechaInicio ?? DateTime.Today;
            var fin = fechaFin ?? DateTime.Today;

            var dashboard = await _serviceDashboard.ObtenerDashboardAsync(tipo, inicio, fin);

            ViewBag.TipoSeleccionado = tipo;
            ViewBag.FechaInicio = inicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fin.ToString("yyyy-MM-dd");

            return View(dashboard);
        }
    }
}
