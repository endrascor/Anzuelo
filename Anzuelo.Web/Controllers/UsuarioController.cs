using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;

        }


        // GET: UsuarioController
        public async Task<ActionResult> Index()
        {
            var collection=await _serviceUsuario.ListAync();
            return View(collection);
        }

        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object= await _serviceUsuario.FindByIdAsync(id);
            return View(@object);
        }
    }
}
