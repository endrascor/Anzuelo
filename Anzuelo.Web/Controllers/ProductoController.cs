using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;

        public ProductoController(IServiceProducto serviceProducto)
        {
            _serviceProducto = serviceProducto;

        }


        // GET: ProductoController
        public async Task<ActionResult> Index()
        {
            var collection=await _serviceProducto.ListAync();
            return View(collection);
        }

        public async Task<ActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceProducto.ListAync();
            return View(collection.ToPagedList(page ?? 1, 5));
        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object= await _serviceProducto.FindByIdAsync(id);
            return View(@object);
        }
    }
}
