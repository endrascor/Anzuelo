using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using X.PagedList.Extensions;

namespace Anzuelo.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto
            _serviceProducto;

        private readonly IServiceCategoriaProducto
            _serviceCategoriaProducto;

        private readonly IServiceEstadoProducto
            _serviceEstadoProducto;

        private readonly IServiceIngrediente
            _serviceIngrediente;

        public ProductoController(
            IServiceProducto serviceProducto,
            IServiceCategoriaProducto
                serviceCategoriaProducto,
            IServiceEstadoProducto
                serviceEstadoProducto,
            IServiceIngrediente
                serviceIngrediente)
        {
            _serviceProducto =
                serviceProducto;

            _serviceCategoriaProducto =
                serviceCategoriaProducto;

            _serviceEstadoProducto =
                serviceEstadoProducto;

            _serviceIngrediente =
                serviceIngrediente;
        }

        public async Task<ActionResult> Index()
        {
            var collection =
                await _serviceProducto
                    .ListAync();

            return View(collection);
        }

        public async Task<ActionResult>
            IndexAdmin(int? page)
        {
            var collection =
                await _serviceProducto
                    .ListAync();

            return View(
                collection.ToPagedList(
                    page ?? 1,
                    5));
        }

        public async Task<ActionResult>
            Details(int id)
        {
            var producto =
                await _serviceProducto
                    .FindByIdAsync(id);

            return View(producto);
        }

        private async Task CargarListasAsync(
            ProductoDTO? producto = null)
        {
            var categorias =
                await _serviceCategoriaProducto
                    .ListAync();

            var estados =
                await _serviceEstadoProducto
                    .ListAync();

            var ingredientes =
                await _serviceIngrediente
                    .ListAsync();

            ViewBag.ListCategorias =
                new SelectList(
                    categorias,
                    "IdCategoriaProducto",
                    "Descripcion",
                    producto?
                        .IdCategoriaProducto);

            ViewBag.ListEstados =
                new SelectList(
                    estados,
                    "IdEstadoProducto",
                    "Descripcion",
                    producto?
                        .IdEstadoProducto);

            ViewBag.ListIngredientes =
                ingredientes
                    .Select(x =>
                        new SelectListItem
                        {
                            Value =
                                x.IdIngrediente
                                    .ToString(),

                            Text =
                                x.NombreIngrediente
                        })
                    .ToList();
        }

        [HttpGet]
        public async Task<IActionResult>
            Create()
        {
            await CargarListasAsync();

            return View(
                new ProductoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create(
                ProductoDTO dto,
                List<IFormFile>? imageFiles)
        {
            var imagenes =
                await ConvertirImagenesAsync(
                    imageFiles);

            ValidarIngredientes(dto);

            if (imagenes.Count == 0)
            {
                ModelState.AddModelError(
                    "imageFiles",
                    "Debe seleccionar al menos una imagen.");
            }

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(dto);

                return View(dto);
            }

            await _serviceProducto.AddAsync(
                dto,
                imagenes);

            TempData["MensajeExito"] =
        "El producto se guardó correctamente.";

            return RedirectToAction(
                nameof(IndexAdmin));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var producto =
                await _serviceProducto.FindByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            await CargarListasAsync(producto);

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(
                int id,
                ProductoDTO dto,
                List<IFormFile>? imageFiles)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            dto.IdProducto = id;

            var nuevasImagenes =
                await ConvertirImagenesAsync(
                    imageFiles);

            ValidarIngredientes(dto);

            int cantidadImagenesExistentes =
                dto.Imagenes?
                    .Count(x =>
                        x.IdImagenProducto > 0)
                ?? 0;

            int totalImagenes =
                cantidadImagenesExistentes +
                nuevasImagenes.Count;

            if (totalImagenes == 0)
            {
                ModelState.AddModelError(
                    "imageFiles",
                    "El producto debe conservar o agregar al menos una imagen.");
            }

            if (!ModelState.IsValid)
            {
                await RestaurarImagenesParaVistaAsync(
                    id,
                    dto);

                await CargarListasAsync(dto);

                return View(dto);
            }

            await _serviceProducto.UpdateAsync(
                id,
                dto,
                nuevasImagenes);
            TempData["MensajeExito"] =
        "El producto se actualizo correctamente.";
            return RedirectToAction(
                nameof(IndexAdmin));
        }

        private void ValidarIngredientes(
            ProductoDTO dto)
        {
            bool tieneIngredienteValido =
                dto.Ingredientes != null &&
                dto.Ingredientes.Any(x =>
                    x.IdIngrediente > 0 &&
                    x.Cantidad > 0);

            if (!tieneIngredienteValido)
            {
                ModelState.AddModelError(
                    "Ingredientes",
                    "Debe seleccionar al menos un ingrediente.");
            }
        }

        private async Task
            RestaurarImagenesParaVistaAsync(
                int id,
                ProductoDTO dto)
        {
            var productoBaseDatos =
                await _serviceProducto
                    .FindByIdAsync(id);

            var idsConservados =
                dto.Imagenes?
                    .Where(x =>
                        x.IdImagenProducto > 0)
                    .Select(x =>
                        x.IdImagenProducto)
                    .ToHashSet()
                ?? new HashSet<int>();

            dto.Imagenes =
                productoBaseDatos.Imagenes
                    .Where(x =>
                        idsConservados.Contains(
                            x.IdImagenProducto))
                    .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            CrearIngredienteRapido(
                string nombreIngrediente)
        {
            if (string.IsNullOrWhiteSpace(
                nombreIngrediente))
            {
                return BadRequest(new
                {
                    success = false,

                    message =
                        "Debe ingresar el nombre del ingrediente."
                });
            }

            var ingrediente =
                await _serviceIngrediente
                    .AddAsync(
                        nombreIngrediente);

            return Json(new
            {
                success = true,

                id =
                    ingrediente.IdIngrediente,

                nombre =
                    ingrediente.NombreIngrediente
            });
        }

        private async Task<List<byte[]>>
            ConvertirImagenesAsync(
                ICollection<IFormFile>?
                    archivos)
        {
            var resultado =
                new List<byte[]>();

            if (archivos == null)
            {
                return resultado;
            }

            string[] tiposPermitidos =
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            const long tamañoMaximo =
                5 * 1024 * 1024;

            foreach (var archivo
                in archivos)
            {
                if (archivo.Length == 0)
                {
                    continue;
                }

                if (!tiposPermitidos.Contains(
                    archivo.ContentType))
                {
                    ModelState.AddModelError(
                        "imageFiles",
                        $"El archivo {archivo.FileName} no es una imagen válida.");

                    continue;
                }

                if (archivo.Length >
                    tamañoMaximo)
                {
                    ModelState.AddModelError(
                        "imageFiles",
                        $"La imagen {archivo.FileName} supera los 5 MB.");

                    continue;
                }

                using var memoria =
                    new MemoryStream();

                await archivo.CopyToAsync(
                    memoria);

                resultado.Add(
                    memoria.ToArray());
            }

            return resultado;
        }
    }
}