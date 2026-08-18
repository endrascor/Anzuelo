using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anzuelo.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceRol _serviceRol;
        private readonly IServiceEstadoUsuario _serviceEstadoUsuario;

        public UsuarioController(
            IServiceUsuario serviceUsuario,
            IServiceRol serviceRol,
            IServiceEstadoUsuario serviceEstadoUsuario)
        {
            _serviceUsuario = serviceUsuario;
            _serviceRol = serviceRol;
            _serviceEstadoUsuario = serviceEstadoUsuario;
        }

        [Authorize(Roles = "Administrador")]
        // GET: Usuario
        public async Task<ActionResult> Index()
        {
            var collection =
                await _serviceUsuario.ListAsync();

            return View(collection);
        }


        // GET: Usuario/Details/1
        public async Task<ActionResult> Details(int id)
        {
            var usuario =
                await _serviceUsuario.FindByIdAsync(id);

            return View(usuario);
        }


        // GET: Usuario/Create
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarListas();

            return View(new UsuarioDTO());
        }

        // GET: Usuario/CreateAdmin
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            await CargarListas();

            return View(new UsuarioDTO());
        }


        // POST: Usuario/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioDTO dto)
        {

            dto.IdRol = 4;              
            dto.IdEstadoUsuario = 1;    

            ModelState.Remove(nameof(UsuarioDTO.NombreRol));
            ModelState.Remove(nameof(UsuarioDTO.NombreEstado));

            ModelState.Remove(nameof(UsuarioDTO.IdRol));
            ModelState.Remove(nameof(UsuarioDTO.IdEstadoUsuario));

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _serviceUsuario.AddAsync(dto);

            return RedirectToAction(
                "LogIn",
                "Login"
            );
        }

        // POST: Usuario/CreateAdmin
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateAdmin(UsuarioDTO dto)
        {

            ModelState.Remove(nameof(UsuarioDTO.NombreRol));
            ModelState.Remove(nameof(UsuarioDTO.NombreEstado));


            // Validar rol
            if (dto.IdRol <= 0)
            {
                ModelState.AddModelError(
                    nameof(dto.IdRol),
                    "Debe seleccionar un rol."
                );
            }


            // Validar estado
            if (dto.IdEstadoUsuario <= 0)
            {
                ModelState.AddModelError(
                    nameof(dto.IdEstadoUsuario),
                    "Debe seleccionar un estado."
                );
            }


            if (!ModelState.IsValid)
            {
                await CargarListas();

                return View(dto);
            }


            await _serviceUsuario.AddAsync(dto);

            return RedirectToAction(
                "LogIn",
                "Login"
            );
        }


        private async Task CargarListas()
        {
            ViewBag.ListRoles =
                await _serviceRol.ListAync();

            ViewBag.ListEstadosUsuario =
                await _serviceEstadoUsuario.ListAync();
        }
    }
}