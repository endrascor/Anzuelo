using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class ComboController : Controller
    {
        private readonly IServiceCombo _serviceCombo;
        public ComboController(IServiceCombo serviceCombo)
        {
            _serviceCombo = serviceCombo;
        }

        // GET: ComboController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceCombo.ListAync();
            return View(collection);
        }

        // GET: ComboController/Details/1 
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceCombo.FindByIdAsync(id);
            return View(@object);
        }
    }
}
