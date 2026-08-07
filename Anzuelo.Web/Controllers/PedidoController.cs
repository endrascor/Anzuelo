using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Anzuelo.Web.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private const string TEMPDATA_CARRITO = "CarritoPedido";
        private const string ROL_CLIENTE = "Cliente";
        private const decimal PORCENTAJE_IMPUESTO = 0.13m;

        private readonly IServicePedido _servicePedido;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCombo _serviceCombo;
        private readonly IServiceTipoEntrega _serviceTipoEntrega;
        private readonly IServiceMetodoPago _serviceMetodoPago;
        private readonly IServiceDireccion _serviceDireccion;
        private readonly IServiceUsuario _serviceUsuario;

        public PedidoController(
            IServicePedido servicePedido,
            IServiceProducto serviceProducto,
            IServiceCombo serviceCombo,
            IServiceTipoEntrega serviceTipoEntrega,
            IServiceMetodoPago serviceMetodoPago,
            IServiceDireccion serviceDireccion,
            IServiceUsuario serviceUsuario)
        {
            _servicePedido = servicePedido;
            _serviceProducto = serviceProducto;
            _serviceCombo = serviceCombo;
            _serviceTipoEntrega = serviceTipoEntrega;
            _serviceMetodoPago = serviceMetodoPago;
            _serviceDireccion = serviceDireccion;
            _serviceUsuario = serviceUsuario;
        }

        private int? IdUsuarioSesion
        {
            get
            {
                var valor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(valor, out var id) ? id : null;
            }
        }

        private string? RolSesion => User.FindFirst(ClaimTypes.Role)?.Value;

        public async Task<ActionResult> Create()
        {
            await CargarListasAsync();

            TempData[TEMPDATA_CARRITO] = null;
            TempData.Keep(TEMPDATA_CARRITO);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoDTO dto)
        {
            var carrito = ObtenerCarrito();

            if (!carrito.Any())
                return BadRequest("Debe agregar al menos un producto o combo al pedido.");

            try
            {
                dto.Detalles = carrito;

                var idPedido = await _servicePedido.AddAsync(dto, IdUsuarioSesion!.Value, RolSesion!);

                TempData[TEMPDATA_CARRITO] = null;
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                    "Registrar Pedido",
                    "Pedido registrado con éxito. ID: " + idPedido.ToString(),
                    Util.SweetAlertMessageType.success);

                return Json(new { success = true, redirectUrl = Url.Action(nameof(Details), new { id = idPedido }) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarLinea(int? idProducto, int? idCombo, int cantidad, string? observaciones)
        {
            if (cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            if ((idProducto.HasValue && idCombo.HasValue) || (!idProducto.HasValue && !idCombo.HasValue))
                return BadRequest("Debe seleccionar un producto o un combo, no ambos.");

            var carrito = ObtenerCarrito();

            var existente = idProducto.HasValue
                ? carrito.FirstOrDefault(d => d.IdProducto == idProducto && d.Observaciones == observaciones)
                : carrito.FirstOrDefault(d => d.IdCombo == idCombo && d.Observaciones == observaciones);

            if (existente != null)
            {
                existente.Cantidad += cantidad;
                existente.Subtotal = existente.PrecioUnitario * existente.Cantidad;
                existente.Impuesto = Math.Round(existente.Subtotal * PORCENTAJE_IMPUESTO, 2);
            }
            else
            {
                string nombre;
                decimal precio;

                if (idProducto.HasValue)
                {
                    var producto = await _serviceProducto.FindByIdAsync(idProducto.Value);
                    if (producto == null)
                        return BadRequest("El producto no existe.");
                    nombre = producto.Nombre;
                    precio = producto.Precio;
                }
                else
                {
                    var combo = await _serviceCombo.FindByIdAsync(idCombo!.Value);
                    if (combo == null)
                        return BadRequest("El combo no existe.");
                    nombre = combo.Nombre;
                    precio = combo.PrecioTotal;
                }

                var subtotal = precio * cantidad;

                carrito.Add(new DetallePedidoDTO
                {
                    IdProducto = idProducto,
                    IdCombo = idCombo,
                    Cantidad = cantidad,
                    Observaciones = observaciones,
                    Nombre = nombre,
                    PrecioUnitario = precio,
                    Subtotal = subtotal,
                    Impuesto = Math.Round(subtotal * PORCENTAJE_IMPUESTO, 2)
                });
            }

            GuardarCarrito(carrito);
            return PartialView("_DetallePedido", carrito);
        }

        [HttpPost]
        public IActionResult ActualizarCantidad(int index, int cantidad)
        {
            var carrito = ObtenerCarrito();

            if (index < 0 || index >= carrito.Count)
                return BadRequest("Línea no válida.");

            if (cantidad <= 0)
            {
                carrito.RemoveAt(index);
            }
            else
            {
                carrito[index].Cantidad = cantidad;
                carrito[index].Subtotal = carrito[index].PrecioUnitario * cantidad;
                carrito[index].Impuesto = Math.Round(carrito[index].Subtotal * PORCENTAJE_IMPUESTO, 2);
            }

            GuardarCarrito(carrito);
            return PartialView("_DetallePedido", carrito);
        }

        [HttpPost]
        public IActionResult EliminarLinea(int index)
        {
            var carrito = ObtenerCarrito();

            if (index < 0 || index >= carrito.Count)
                return BadRequest("Línea no válida.");

            carrito.RemoveAt(index);
            GuardarCarrito(carrito);
            return PartialView("_DetallePedido", carrito);
        }

        public IActionResult ObtenerDetalle()
        {
            var carrito = ObtenerCarrito();
            return PartialView("_DetallePedido", carrito);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var pedido = await _servicePedido.FindByIdAsync(id);
            if (pedido == null)
                return NotFound("El pedido solicitado no existe.");

            return View(pedido);
        }

        private async Task CargarListasAsync()
        {
            ViewBag.ListTipoEntrega = await _serviceTipoEntrega.ListAsync();
            ViewBag.ListMetodoPago = await _serviceMetodoPago.ListAsync();
            ViewBag.ListProductos = await _serviceProducto.ListAync();
            ViewBag.ListCombos = await _serviceCombo.ListAync();

            if (RolSesion != ROL_CLIENTE)
            {
                ViewBag.ListClientes = await _serviceUsuario.ListByRolAsync(ROL_CLIENTE);
            }
            else
            {
                ViewBag.ListDirecciones = await _serviceDireccion.ListByUsuarioAsync(IdUsuarioSesion!.Value);
                ViewBag.ClienteSesion = await _serviceUsuario.FindByIdAsync(IdUsuarioSesion!.Value);
            }
        }

        private List<DetallePedidoDTO> ObtenerCarrito()
        {
            var json = TempData[TEMPDATA_CARRITO] as string;

            var carrito = string.IsNullOrEmpty(json)
                ? new List<DetallePedidoDTO>()
                : JsonSerializer.Deserialize<List<DetallePedidoDTO>>(json)!;

            TempData[TEMPDATA_CARRITO] = json;
            TempData.Keep(TEMPDATA_CARRITO);

            return carrito;
        }

        private void GuardarCarrito(List<DetallePedidoDTO> carrito)
        {
            TempData[TEMPDATA_CARRITO] = JsonSerializer.Serialize(carrito);
            TempData.Keep(TEMPDATA_CARRITO);
        }

        [HttpPost]
        public IActionResult ActualizarObservaciones(int index, string observaciones)
        {
            var carrito = ObtenerCarrito();

            if (index < 0 || index >= carrito.Count)
                return BadRequest("Línea no válida.");

            carrito[index].Observaciones = observaciones;

            GuardarCarrito(carrito);
            return PartialView("_DetallePedido", carrito);
        }

        [AllowAnonymous]
        public IActionResult ObtenerCantidadCarrito()
        {
            var carrito = ObtenerCarrito();
            return Json(new { cantidad = carrito.Sum(d => d.Cantidad) });
        }

        [HttpPost]
        public async Task<IActionResult> CrearDireccion(string canton, string provincia, string distrito, string detalle, int? idCliente)
        {
            int idUsuarioDestino;

            if (idCliente.HasValue)
            {
                if (RolSesion == ROL_CLIENTE)
                    return BadRequest("No tiene permisos para registrar direcciones de otros clientes.");

                idUsuarioDestino = idCliente.Value;
            }
            else
            {
                if (IdUsuarioSesion == null)
                    return BadRequest("No hay una sesión activa.");

                idUsuarioDestino = IdUsuarioSesion.Value;
            }

            if (string.IsNullOrWhiteSpace(canton) || string.IsNullOrWhiteSpace(provincia) ||
                string.IsNullOrWhiteSpace(distrito) || string.IsNullOrWhiteSpace(detalle))
            {
                return BadRequest("Todos los campos de la dirección son obligatorios.");
            }

            var dto = new DireccionDTO
            {
                Canton = canton,
                Provincia = provincia,
                Distrito = distrito,
                Detalle = detalle,
                IdUsuario = idUsuarioDestino
            };

            var idDireccion = await _serviceDireccion.AddAsync(dto);
            dto.IdDireccion = idDireccion;

            return PartialView("_SelectDireccion", dto);
        }

        public async Task<IActionResult> ObtenerDireccionesCliente(int idCliente)
        {
            var direcciones = await _serviceDireccion.ListByUsuarioAsync(idCliente);
            ViewBag.IdClienteActual = idCliente;
            return PartialView("_SelectDireccionCliente", direcciones.ToList());
        }
    }
}