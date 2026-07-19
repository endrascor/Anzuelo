using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
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
         * LISTADO
         */

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var collection =
                await _serviceMenu.ListAync();

            return View(collection);
        }

        /*
         * LISTADO ADMINISTRATIVO
         */

        [HttpGet]
        public async Task<ActionResult> IndexAdmin(
            int? page)
        {
            var collection =
                await _serviceMenu.ListAync();

            return View(
                collection.ToPagedList(
                    page ?? 1,
                    5));
        }

        /*
         * DETALLE DEL MENÚ DISPONIBLE
         */

        [HttpGet]
        public async Task<ActionResult> Details()
        {
            var menu =
                await _serviceMenu
                    .GetMenuDisponibleAsync();

            return View(menu);
        }

        /*
         * CARGAR LISTAS
         */

        private async Task CargarListasAsync(
            MenuDTO? menu = null)
        {
            var estados =
                await _serviceEstadoMenu
                    .ListAync();

            var dias =
                await _serviceDisponibilidadDia
                    .ListAync();

            var productos =
                await _serviceProducto
                    .ListAync();

            var combos =
                await _serviceCombo
                    .ListAync();

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
                                producto
                                    .IdProducto
                                    .ToString(),

                            Text =
                                $"{producto.Nombre} - " +
                                $"₡{producto.Precio:N2}"
                        })
                    .ToList();

            ViewBag.ListCombos =
                combos
                    .Select(combo =>
                        new SelectListItem
                        {
                            Value =
                                combo
                                    .IdCombo
                                    .ToString(),

                            Text =
                                $"{combo.Nombre} - " +
                                $"₡{combo.PrecioTotal:N2}"
                        })
                    .ToList();
        }

        /*
         * CREATE GET
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

        /*
         * CREATE POST
         */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MenuDTO dto)
        {
            NormalizarColecciones(dto);

            CorregirDescuentosDesdeFormulario(
                dto);

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
         * EDIT GET
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
                await _serviceMenu
                    .FindByIdAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            NormalizarColecciones(menu);

            await CargarListasAsync(menu);

            return View(menu);
        }

        /*
         * EDIT POST
         */

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

            dto.IdMenu =
                id;

            /*
             * Evita conservar un valor anterior
             * dentro de ModelState.
             */
            ModelState.Remove(
                nameof(MenuDTO.IdMenu));

            NormalizarColecciones(dto);

            /*
             * Convierte correctamente valores como:
             *
             * 10.00
             * 10,00
             */
            CorregirDescuentosDesdeFormulario(
                dto);

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
         * EVITAR COLECCIONES NULAS
         */

        private static void NormalizarColecciones(
            MenuDTO dto)
        {
            dto.Productos =
                dto.Productos?.ToList()
                ?? new List<MenuProductoDTO>();

            dto.Combos =
                dto.Combos?.ToList()
                ?? new List<MenuComboDTO>();
        }

        /*
         * CORREGIR DECIMALES RECIBIDOS
         *
         * El navegador normalmente envía:
         * 10.00
         *
         * Algunas configuraciones regionales
         * esperan:
         * 10,00
         *
         * Este método acepta ambos formatos.
         */

        private void CorregirDescuentosDesdeFormulario(
            MenuDTO dto)
        {
            var productos =
                dto.Productos.ToList();

            for (
                int indice = 0;
                indice < productos.Count;
                indice++)
            {
                var nombreCampo =
                    $"Productos[{indice}].Descuento";

                ModelState.Remove(
                    nombreCampo);

                if (
                    !Request.Form.TryGetValue(
                        nombreCampo,
                        out var valorRecibido))
                {
                    ModelState.AddModelError(
                        nombreCampo,
                        "Debe ingresar el descuento.");

                    continue;
                }

                if (
                    TryParseDecimal(
                        valorRecibido.ToString(),
                        out decimal descuento))
                {
                    productos[indice].Descuento =
                        descuento;
                }
                else
                {
                    ModelState.AddModelError(
                        nombreCampo,
                        "El descuento no tiene un formato válido.");
                }
            }

            dto.Productos =
                productos;

            var combos =
                dto.Combos.ToList();

            for (
                int indice = 0;
                indice < combos.Count;
                indice++)
            {
                var nombreCampo =
                    $"Combos[{indice}].Descuento";

                ModelState.Remove(
                    nombreCampo);

                if (
                    !Request.Form.TryGetValue(
                        nombreCampo,
                        out var valorRecibido))
                {
                    ModelState.AddModelError(
                        nombreCampo,
                        "Debe ingresar el descuento.");

                    continue;
                }

                if (
                    TryParseDecimal(
                        valorRecibido.ToString(),
                        out decimal descuento))
                {
                    combos[indice].Descuento =
                        descuento;
                }
                else
                {
                    ModelState.AddModelError(
                        nombreCampo,
                        "El descuento no tiene un formato válido.");
                }
            }

            dto.Combos =
                combos;
        }

        /*
         * CONVERSIÓN DE DECIMAL
         */

        private static bool TryParseDecimal(
            string valor,
            out decimal resultado)
        {
            resultado = 0;

            if (string.IsNullOrWhiteSpace(valor))
            {
                return false;
            }

            /*
             * Convierte una coma decimal en punto
             * para interpretar siempre el valor
             * utilizando formato invariable.
             */
            var valorNormalizado =
                valor
                    .Trim()
                    .Replace(',', '.');

            return decimal.TryParse(
                valorNormalizado,
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out resultado);
        }

        /*
         * VALIDACIONES
         */

        private void ValidarMenu(
            MenuDTO dto)
        {
            ValidarFechas(dto);

            ValidarHoras(dto);

            ValidarElementos(dto);

            ValidarDescuentos(dto);

            ValidarDuplicados(dto);
        }

        private void ValidarFechas(
            MenuDTO dto)
        {
            if (
                dto.FechaInicio != default &&
                dto.FechaFinal != default &&
                dto.FechaFinal.Date <
                dto.FechaInicio.Date)
            {
                ModelState.AddModelError(
                    nameof(dto.FechaFinal),
                    "La fecha final no puede ser " +
                    "anterior a la fecha inicial.");
            }
        }

        private void ValidarHoras(
            MenuDTO dto)
        {
            if (
                dto.HoraInicio != default &&
                dto.HoraFinal != default &&
                dto.HoraFinal.TimeOfDay <=
                dto.HoraInicio.TimeOfDay)
            {
                ModelState.AddModelError(
                    nameof(dto.HoraFinal),
                    "La hora final debe ser posterior " +
                    "a la hora inicial.");
            }
        }

        private void ValidarElementos(
            MenuDTO dto)
        {
            bool tieneProductos =
                dto.Productos.Any(producto =>
                    producto.IdProducto > 0);

            bool tieneCombos =
                dto.Combos.Any(combo =>
                    combo.IdCombo > 0);

            if (
                !tieneProductos &&
                !tieneCombos)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Debe agregar al menos un producto " +
                    "o un combo.");
            }
        }

        private void ValidarDescuentos(
            MenuDTO dto)
        {
            bool productoInvalido =
                dto.Productos.Any(producto =>
                    producto.Descuento < 0 ||
                    producto.Descuento > 100);

            bool comboInvalido =
                dto.Combos.Any(combo =>
                    combo.Descuento < 0 ||
                    combo.Descuento > 100);

            if (
                productoInvalido ||
                comboInvalido)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "Los descuentos deben estar " +
                    "entre 0 y 100.");
            }
        }

        private void ValidarDuplicados(
            MenuDTO dto)
        {
            bool productosDuplicados =
                dto.Productos
                    .Where(producto =>
                        producto.IdProducto > 0)
                    .GroupBy(producto =>
                        producto.IdProducto)
                    .Any(grupo =>
                        grupo.Count() > 1);

            bool combosDuplicados =
                dto.Combos
                    .Where(combo =>
                        combo.IdCombo > 0)
                    .GroupBy(combo =>
                        combo.IdCombo)
                    .Any(grupo =>
                        grupo.Count() > 1);

            if (productosDuplicados)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "No puede agregar el mismo producto " +
                    "más de una vez.");
            }

            if (combosDuplicados)
            {
                ModelState.AddModelError(
                    "ElementosMenu",
                    "No puede agregar el mismo combo " +
                    "más de una vez.");
            }
        }
    }
}
