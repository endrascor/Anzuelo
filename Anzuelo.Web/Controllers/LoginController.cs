using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Web.Util;
using Anzuelo.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Anzuelo.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            IServiceUsuario serviceUsuario,
            ILogger<LoginController> logger)
        {
            _serviceUsuario = serviceUsuario;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View(new ViewModelLogin());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(
            ViewModelLogin viewModelLogin)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join(
                    "; ",
                    ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)
                );

                _logger.LogInformation(
                    "Error en login de {Usuario}. Errores: {Errores}",
                    viewModelLogin.User,
                    errors
                );

                // Debe regresar al formulario LogIn
                return View("LogIn", viewModelLogin);
            }

            var usuarioLog = await _serviceUsuario.LoginAsync(
                viewModelLogin.User,
                viewModelLogin.Password
            );

            if (usuarioLog == null)
            {
                ViewBag.Message = "Correo o contraseña incorrectos.";

                _logger.LogInformation(
                    "Error de acceso para el usuario {Usuario}",
                    viewModelLogin.User
                );

                // Debe regresar al formulario LogIn
                return View("LogIn", viewModelLogin);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Name,
                    $"{usuarioLog.Nombre} {usuarioLog.Apellido1} {usuarioLog.Apellido2}"
                ),

                new Claim(
                    ClaimTypes.Role,
                    usuarioLog.NombreRol
                ),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuarioLog.IdUsuario.ToString()
                )
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var properties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            _logger.LogInformation(
                "Conexión correcta de {Usuario}",
                viewModelLogin.User
            );

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            _logger.LogInformation(
                "Desconexión correcta de {Usuario}",
                User.Identity?.Name
            );

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Index", "Login");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}