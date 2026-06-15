using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Anzuelo.Web.Controllers
{
    public class PreparacionController : Controller
    {
        private readonly IServicePreparacion _servicePreparacion;
        public PreparacionController(IServicePreparacion servicePreparacion)
        {
            _servicePreparacion = servicePreparacion;
        }

        //GET: PreparacionController
        public async Task<ActionResult> Index()
        {
            var collection = await _servicePreparacion.ListAync();
            return View(collection);
        }

        // GET: PreparacionController/Details/1
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _servicePreparacion.FindByIdAsync(id);
            return View(@object);
        }
    }
}
