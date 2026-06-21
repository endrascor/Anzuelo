using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IServiceMenu _serviceMenu;

        public MenuController(IServiceMenu serviceMenu)
        {
            _serviceMenu = serviceMenu;

        }


        // GET: MenuController
        public async Task<ActionResult> Index()
        {
            var collection=await _serviceMenu.ListAync();
            return View(collection);
        }

        public async Task<ActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceMenu.ListAync();
            return View(collection.ToPagedList(page ?? 1, 5));
        }

        // GET: ProductoController/Details
        public async Task<ActionResult> Details()
        {
            var @object= await _serviceMenu.GetMenuDisponibleAsync();
            return View(@object);
        }
    }
}
