using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class PreparacionController : Controller
    {
        private readonly IServicePreparacion _servicePreparacion;
        private readonly IServiceEstacionCocina _serviceEstacionCocina;
        private readonly IServiceProducto _serviceProducto;
        private readonly ILogger<PreparacionController> _logger;

        public PreparacionController(IServicePreparacion servicePreparacion, IServiceEstacionCocina serviceEstacionCocina, IServiceProducto serviceProducto, ILogger<PreparacionController> logger)
        {
            _servicePreparacion = servicePreparacion;
            _serviceEstacionCocina = serviceEstacionCocina;
            _serviceProducto = serviceProducto;
            _logger = logger;
        }


        // GET: PreparacionController
        public async Task<ActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _servicePreparacion.ListAync();
            return View(collection);
        }

        // GET: PreparacionController/Details/1
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _servicePreparacion.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Proceso de preparación no existente");
                }

                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CargarEstacionesSeleccionadas(PreparacionDTO dto, int[] ids, int[] tiempos)
        {
            if (dto.Estaciones == null)
            {
                dto.Estaciones = new List<PreparacionEstacionDTO>();
            }
            else
            {
                dto.Estaciones.Clear();
            }

            if (ids == null) return;
            for (int i = 0; i < ids.Length; i++)
            {
                dto.Estaciones.Add(new PreparacionEstacionDTO
                {
                    IdEstacionCocina = ids[i],
                    NumeroOrden = i + 1,
                    TiempoEstimadoMinutos = tiempos != null && tiempos.Length > i ? tiempos[i] : 1
                });
            }
        }

        private async Task CargarListasAsync()
        {
            ViewBag.ListProductos = await _serviceProducto.ListAync();
            ViewBag.ListEstaciones = await _serviceEstacionCocina.ListAync();
        }

        // GET: PreparacionController/IndexAdmin
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _servicePreparacion.ListAync();
            return View(collection.ToPagedList(page ?? 1, 5));
        }

        // POST: PreparacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admnistrador")]
        public async Task<IActionResult> Create(PreparacionDTO dto, int[] selectedEstaciones, int[] selectedTiempos)
        {
            CargarEstacionesSeleccionadas(dto, selectedEstaciones, selectedTiempos);

            if (dto.Estaciones == null || !dto.Estaciones.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos una estación al proceso");
            }
            else if (dto.Estaciones.Select(e => e.IdEstacionCocina).Distinct().Count() != dto.Estaciones.Count)
            {
                ModelState.AddModelError("", "No se puede repetir la misma estación en el proceso");
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                    "Crear Proceso de Preparación",
                    "Errores: " + errors.ToString(),
                    Util.SweetAlertMessageType.error);

                await CargarListasAsync();
                return View(dto);
            }

            var idPreparacion = await _servicePreparacion.AddAsync(dto);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Crear Proceso de Preparación",
                "Proceso creado " + idPreparacion.ToString(),
                Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }

        // GET: PreparacionController/Create
        //[Authorize(Roles = "Admnistrador")]
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();
            return View(new PreparacionDTO());
        }

        // POST: PreparacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PreparacionDTO dto, int[] selectedEstaciones, int[] selectedTiempos)
        {
            if (id != dto.IdPreparacion)
            {
                return BadRequest();
            }

            CargarEstacionesSeleccionadas(dto, selectedEstaciones, selectedTiempos);

            if (dto.Estaciones == null || !dto.Estaciones.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos una estación al proceso");
            }
            else if (dto.Estaciones.Select(e => e.IdEstacionCocina).Distinct().Count() != dto.Estaciones.Count)
            {
                ModelState.AddModelError("", "No se puede repetir la misma estación en el proceso");
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                    "Editar Proceso de Preparación",
                    "Errores: " + errors,
                    Util.SweetAlertMessageType.error);

                await CargarListasAsync();
                return View(dto);
            }
            else
            {
                await _servicePreparacion.UpdateAsync(dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                    "Editar Proceso de Preparación",
                    "Proceso actualizado correctamente",
                    Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            }
        }

        // GET: PreparacionController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _servicePreparacion.FindByIdAsync(id);
            await CargarListasAsync();
            return View(@object);
        }
    }
}