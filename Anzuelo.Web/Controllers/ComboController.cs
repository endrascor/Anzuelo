using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class ComboController : Controller
    {
        private readonly IServiceCombo _serviceCombo;
        private readonly IServiceEstadoCombo _serviceEstadoCombo;
        private readonly IServiceCategoriaCombo _ServiceCategoriaCombo;
        private readonly IServiceProducto _serviceProducto;
        private readonly ILogger<ComboController> _logger;
        public ComboController(IServiceCombo serviceCombo, IServiceCategoriaCombo serviceCategoriaCombo, IServiceEstadoCombo serviceEstadoCombo, IServiceProducto serviceProducto, ILogger<ComboController> logger)
        {
            _serviceCombo = serviceCombo;
            _serviceEstadoCombo = serviceEstadoCombo;
            _ServiceCategoriaCombo = serviceCategoriaCombo;
            _serviceProducto = serviceProducto;
            _logger = logger;
        }

        // GET: ComboController
        public async Task<ActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceCombo.ListAync();
            return View(collection);
        }

        // GET: ComboController/IndexAdmin
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceCombo.ListAync();
            return View(collection.ToPagedList(page ?? 1, 5));
        }

        // GET: ComboController/Details/1
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceCombo.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Combo no existente");
                }

                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CargarProductosSeleccionados(ComboDTO dto, int[] ids, int[] cantidades)
        {
            dto.Productos.Clear();
            if (ids == null) return;
            for (int i = 0; i < ids.Length; i++)
            {
                dto.Productos.Add(new ComboProductoDTO
                {
                    IdProducto = ids[i],
                    Cantidad = cantidades != null && cantidades.Length > i ? cantidades[i] : 1
                });
            }
        }

        private async Task CargarListasAsync()
        {
            ViewBag.ListCategorias = await _ServiceCategoriaCombo.ListAync();
            ViewBag.ListEstados = await _serviceEstadoCombo.ListAync();
            ViewBag.ListProductos = await _serviceProducto.ListAync();
        }

        // POST: ComboController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admnistrador")]
        public async Task<IActionResult> Create(ComboDTO dto, IFormFile? imageFile, int[] selectedProductos, int[] selectedCantidades)
        {
            MemoryStream target = new MemoryStream();

            if (dto.Imagen == null)
            {
                if (imageFile != null)
                {
                    imageFile.OpenReadStream().CopyTo(target);
                    dto.Imagen = target.ToArray();
                    ModelState.Remove("Imagen");
                }
                else
                {
                    var rutaImagenDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "nophoto.jpg");
                    if (System.IO.File.Exists(rutaImagenDefault))
                    {
                        byte[] imagenDefault = System.IO.File.ReadAllBytes(rutaImagenDefault);
                        dto.Imagen = imagenDefault;
                        ModelState.Remove("Imagen");
                    }
                }
            }

            CargarProductosSeleccionados(dto, selectedProductos, selectedCantidades);

            if (dto.Productos == null || !dto.Productos.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto para el combo");
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                    "Crear Combo",
                    "Errores: " + errors.ToString(),
                    Util.SweetAlertMessageType.error);

                await CargarListasAsync();
                return View(dto);
            }

            var idCombo = await _serviceCombo.AddAsync(dto);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Crear Combo",
                "Combo creado " + idCombo.ToString(),
                Util.SweetAlertMessageType.success);
            return RedirectToAction("Index");
        }

        // GET: ComboController/Create
        [Authorize(Roles = "Admnistrador")]
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();
            return View(new ComboDTO());
        }

        // POST: ComboController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComboDTO dto, IFormFile? imageFile, int[] selectedProductos, int[] selectedCantidades)
        {
            if (imageFile != null)
            {
                MemoryStream target = new MemoryStream();
                imageFile.OpenReadStream().CopyTo(target);
                dto.Imagen = target.ToArray();
                ModelState.Remove("Imagen");
            }

            CargarProductosSeleccionados(dto, selectedProductos, selectedCantidades);

            if (dto.Productos == null || !dto.Productos.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto para el combo");
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.ErrorMessage = errors;
                await CargarListasAsync();
                return View(dto);
            }
            else
            {
                await _serviceCombo.UpdateAsync(dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                    "Editar Combo",
                    "Combo actualizado correctamente",
                    Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            }
        }

        // GET: ComboController/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _serviceCombo.FindByIdAsync(id);
            await CargarListasAsync();
            return View(@object);
        }



    }
}
