using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IServiceMenu _serviceMenu;

        private readonly IServiceEstadoMenu
            _serviceEstadoMenu;

        private readonly IServiceDisponibilidadDia
            _serviceDisponibilidadDia;

        private readonly IServiceProducto
            _serviceProducto;

        private readonly IServiceCombo
            _serviceCombo;

        public MenuController(
            IServiceMenu serviceMenu,
            IServiceEstadoMenu serviceEstadoMenu,
            IServiceDisponibilidadDia serviceDisponibilidadDia,
            IServiceProducto serviceProducto,
            IServiceCombo serviceCombo)
        {
            _serviceMenu =
                serviceMenu;

            _serviceEstadoMenu =
                serviceEstadoMenu;

            _serviceDisponibilidadDia =
                serviceDisponibilidadDia;

            _serviceProducto =
                serviceProducto;

            _serviceCombo =
                serviceCombo;
        }

        /*
         * MÉTODOS EXISTENTES.
         * No se modifica su funcionamiento.
         */

        // GET: MenuController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceMenu.ListAync();
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
            var @object =
                await _serviceMenu.GetMenuDisponibleAsync();

            return View(@object);
        }

        /*
         * CARGA DE LISTAS PARA CREATE Y EDIT.
         */

        private async Task CargarListasAsync(
            MenuDTO? menu = null)
        {
            var estados =
                await _serviceEstadoMenu.ListAync();

            var dias =
                await _serviceDisponibilidadDia.ListAync();

            var productos =
                await _serviceProducto.ListAync();

            var combos =
                await _serviceCombo.ListAync();

            ViewBag.ListEstados =
                new SelectList(
                    estados,
                    "IdEstadoMenu",
                    "Descripcion",
                    menu?.IdEstadoMenu);

            ViewBag.ListDias =
                new SelectList(
                    dias,
                    "IdDisponibilidadDia",
                    "DiaSemana",
                    menu?.IdDisponibilidadDia);

            ViewBag.ListProductos =
                productos
                    .Select(producto =>
                        new SelectListItem
                        {
                            Value =
                                producto.IdProducto.ToString(),

                            Text =
                                $"{producto.Nombre} - ₡{producto.Precio:N2}"
                        })
                    .ToList();

            ViewBag.ListCombos =
                combos
                    .Select(combo =>
                        new SelectListItem
                        {
                            Value =
                                combo.IdCombo.ToString(),

                            Text =
                                $"{combo.Nombre} - ₡{combo.PrecioTotal:N2}"
                        })
                    .ToList();
        }

        /*
         * CREATE
         */

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var fechaActual =
                DateTime.Today;

            var dto =
                new MenuDTO
                {
                    FechaInicio =
                        fechaActual,

                    FechaFinal =
                        fechaActual,

                    HoraInicio =
                        fechaActual.AddHours(8),

                    HoraFinal =
                        fechaActual.AddHours(17),

                    Productos =
                        new List<MenuProductoDTO>(),

                    Combos =
                        new List<MenuComboDTO>()
                };

            await CargarListasAsync(dto);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MenuDTO dto)
        {
            ValidarMenu(dto);

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(dto);

                return View(dto);
            }

            await _serviceMenu.AddAsync(dto);

            return RedirectToAction(
                nameof(IndexAdmin));
        }

        /*
         * EDIT
         */

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var menu =
                await _serviceMenu.FindByIdAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            await CargarListasAsync(menu);

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            MenuDTO dto)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            dto.IdMenu = id;

            ValidarMenu(dto);

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(dto);

                return View(dto);
            }

            await _serviceMenu.UpdateAsync(
                id,
                dto);

            return RedirectToAction(
                nameof(IndexAdmin));
        }

        /*
         * VALIDACIONES ADICIONALES.
         */

        private void ValidarMenu(
            MenuDTO dto)
        {
            ValidarFechas(dto);

            ValidarHoras(dto);

            ValidarElementosMenu(dto);

            ValidarDescuentos(dto);

            ValidarElementosIncompletos(dto);
        }

        private void ValidarFechas(
            MenuDTO dto)
        {
            if (
                dto.FechaInicio ==
                default)
            {
                ModelState.AddModelError(
                    nameof(dto.FechaInicio),
                    "Debe ingresar la fecha inicial.");
            }

            if (
                dto.FechaFinal ==
                default)
            {
                ModelState.AddModelError(
                    nameof(dto.FechaFinal),
                    "Debe ingresar la fecha final.");
            }

            if (
                dto.FechaInicio != default &&
                dto.FechaFinal != default &&
                dto.FechaFinal.Date <
                dto.FechaInicio.Date)
            {
                ModelState.AddModelError(
                    nameof(dto.FechaFinal),
                    "La fecha final no puede ser anterior a la fecha inicial.");
            }
        }

        private void ValidarHoras(
            MenuDTO dto)
        {
            if (
                dto.HoraInicio ==
                default)
            {
                ModelState.AddModelError(
                    nameof(dto.HoraInicio),
                    "Debe ingresar la hora inicial.");
            }

            if (
                dto.HoraFinal ==
                default)
            {
                ModelState.AddModelError(
                    nameof(dto.HoraFinal),
                    "Debe ingresar la hora final.");
            }

            if (
                dto.HoraInicio != default &&
                dto.HoraFinal != default &&
                dto.HoraFinal.TimeOfDay <=
                dto.HoraInicio.TimeOfDay)
            {
                ModelState.AddModelError(
                    nameof(dto.HoraFinal),
                    "La hora final debe ser posterior a la hora inicial.");
            }
        }

        private void ValidarElementosMenu(
            MenuDTO dto)
        {
            bool tieneProducto =
                dto.Productos != null &&
                dto.Productos.Any(producto =>
                    producto.IdProducto > 0);

            bool tieneCombo =
                dto.Combos != null &&
                dto.Combos.Any(combo =>
                    combo.IdCombo > 0);

            if (
                !tieneProducto &&
                !tieneCombo)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Debe agregar al menos un producto o un combo.");
            }
        }

        private void ValidarDescuentos(
            MenuDTO dto)
        {
            bool descuentoProductoInvalido =
                dto.Productos?.Any(producto =>
                    producto.Descuento < 0 ||
                    producto.Descuento > 100)
                ?? false;

            bool descuentoComboInvalido =
                dto.Combos?.Any(combo =>
                    combo.Descuento < 0 ||
                    combo.Descuento > 100)
                ?? false;

            if (
                descuentoProductoInvalido ||
                descuentoComboInvalido)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Los descuentos deben estar entre 0 y 100.");
            }
        }

        private void ValidarElementosIncompletos(
            MenuDTO dto)
        {
            bool tieneProductoIncompleto =
                dto.Productos?.Any(producto =>
                    producto.IdProducto <= 0)
                ?? false;

            bool tieneComboIncompleto =
                dto.Combos?.Any(combo =>
                    combo.IdCombo <= 0)
                ?? false;

            if (tieneProductoIncompleto)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Complete o quite las filas de productos vacías.");
            }

            if (tieneComboIncompleto)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Complete o quite las filas de combos vacías.");
            }
        }
    }
}
