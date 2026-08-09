using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anzuelo.Application.Services
{
    public class ServicePedido : IServicePedido
    {
        private const decimal PORCENTAJE_IMPUESTO = 0.13m;
        private const decimal COSTO_ENVIO_DOMICILIO = 1500m;

        private const int ID_ESTADO_INICIAL = 1;
        private const int ID_ENTREGA_DOMICILIO = 1;
        private const int ID_PAGO_EFECTIVO = 1;
        private const int ID_PAGO_TARJETA_CREDITO = 2;
        private const int ID_PAGO_TARJETA_DEBITO = 3;
        private const int ID_ESTADO_PEDIDO_ESTACION_PENDIENTE = 1;
        private const int ID_ESTADO_PEDIDO_ESTACION_PROCESO = 2;
        private const int ID_ESTADO_PEDIDO_ESTACION_FINALIZADO = 3;

        private const string ROL_CLIENTE = "Cliente";

        private readonly IRepositoryPedido _repository;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryCombo _repositoryCombo;
        private readonly IRepositoryEstadoPedido _repositoryEstadoPedido;
        private readonly IRepositoryTipoEntrega _repositoryTipoEntrega;
        private readonly IRepositoryDireccion _repositoryDireccion;
        private readonly IRepositoryMetodoPago _repositoryMetodoPago;
        private readonly IRepositoryPreparacionEstacion _repositoryPreparacionEstacion;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicePedido> _logger;

        public ServicePedido(
            IRepositoryPedido repository,
            IRepositoryProducto repositoryProducto,
            IRepositoryCombo repositoryCombo,
            IRepositoryEstadoPedido repositoryEstadoPedido,
            IRepositoryTipoEntrega repositoryTipoEntrega,
            IRepositoryDireccion repositoryDireccion,
            IRepositoryMetodoPago repositoryMetodoPago,
            IRepositoryPreparacionEstacion repositoryPreparacionEstacion,
            IMapper mapper,
            ILogger<ServicePedido> logger)
        {
            _repository = repository;
            _repositoryProducto = repositoryProducto;
            _repositoryCombo = repositoryCombo;
            _repositoryEstadoPedido = repositoryEstadoPedido;
            _repositoryTipoEntrega = repositoryTipoEntrega;
            _repositoryDireccion = repositoryDireccion;
            _repositoryMetodoPago = repositoryMetodoPago;
            _repositoryPreparacionEstacion = repositoryPreparacionEstacion;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PedidoDTO> FindByIdAsync(int id)
        {
            var entidad = await _repository.FindByIdAsync(id);
            var dto = _mapper.Map<PedidoDTO>(entidad);

            AsignarNombresUsuarios(entidad, dto);

            foreach (var linea in dto.Detalles)
            {
                linea.TotalEstaciones = linea.Estaciones.Count;
                linea.EstacionesCompletadas = linea.Estaciones.Count(e => e.IdEstadoPedidoEstacion == ID_ESTADO_PEDIDO_ESTACION_FINALIZADO);

                foreach (var estacion in linea.Estaciones)
                {
                    estacion.ClaseEstado = estacion.IdEstadoPedidoEstacion switch
                    {
                        ID_ESTADO_PEDIDO_ESTACION_PENDIENTE => "pendiente",
                        ID_ESTADO_PEDIDO_ESTACION_PROCESO => "proceso",
                        ID_ESTADO_PEDIDO_ESTACION_FINALIZADO => "completado",
                        _ => "pendiente"
                    };
                }
            }

            return dto;
        }

        public async Task<bool> PerteneceAlUsuarioAsync(int idPedido, int idUsuario)
        {
            return await _repository.PerteneceAlUsuarioAsync(idPedido, idUsuario);
        }

        public async Task<int> AddAsync(PedidoDTO dto, int idUsuarioLogueado, string rolUsuarioLogueado)
        {
            int idUsuarioCliente;
            int? idUsuarioEncargado = null;
            var esCliente = rolUsuarioLogueado.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase);

            if (esCliente)
            {
                idUsuarioCliente = idUsuarioLogueado;
            }
            else
            {
                if (dto.IdUsuarioCliente <= 0)
                    throw new InvalidOperationException("Debe seleccionar el cliente para el pedido.");

                idUsuarioCliente = dto.IdUsuarioCliente;
                idUsuarioEncargado = idUsuarioLogueado;
            }

            var tipoEntrega = await _repositoryTipoEntrega.FindByIdAsync(dto.IdTipoEntrega ?? 0)
                ?? throw new InvalidOperationException("El método de entrega seleccionado no es válido.");

            var esDomicilio = tipoEntrega.IdTipoEntrega == ID_ENTREGA_DOMICILIO;
            decimal costoEnvio = 0m;

            if (esDomicilio)
            {
                if (!dto.IdDireccion.HasValue)
                    throw new InvalidOperationException("Debe indicar la dirección de entrega.");

                var direccion = await _repositoryDireccion.FindByIdAsync(dto.IdDireccion.Value)
                    ?? throw new InvalidOperationException("La dirección seleccionada no es válida.");

                if (direccion.IdUsuario != idUsuarioCliente)
                    throw new InvalidOperationException("La dirección no pertenece al cliente indicado.");

                costoEnvio = COSTO_ENVIO_DOMICILIO;
            }
            else
            {
                dto.IdDireccion = null;
            }

            var estado = await _repositoryEstadoPedido.FindByIdAsync(ID_ESTADO_INICIAL)
                ?? throw new InvalidOperationException("No se encontró el estado inicial configurado en el sistema.");

            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new InvalidOperationException("Debe agregar al menos un producto o combo al pedido.");

            decimal subtotalPedido = 0m;
            decimal impuestoPedido = 0m;

            foreach (var linea in dto.Detalles)
            {
                if (linea.Cantidad <= 0)
                    throw new InvalidOperationException("La cantidad de cada línea debe ser mayor a cero.");

                var esProducto = linea.IdProducto.HasValue;
                var esCombo = linea.IdCombo.HasValue;

                if (esProducto == esCombo)
                    throw new InvalidOperationException("Cada línea del pedido debe ser un producto o un combo, no ambos ni ninguno.");

                decimal precioUnitario;
                var estacionesLinea = new List<PedidoEstacionDTO>();
                var fechaInicioLinea = DateTime.Now;

                if (esProducto)
                {
                    var producto = await _repositoryProducto.FindByIdAsync(linea.IdProducto!.Value)
                        ?? throw new InvalidOperationException($"El producto con id {linea.IdProducto} no existe.");
                    precioUnitario = producto.Precio;

                    var pasos = await _repositoryPreparacionEstacion.ListByProductoAsync(linea.IdProducto.Value);
                    AgregarPasosPlanificados(estacionesLinea, pasos, linea.IdProducto.Value, ref fechaInicioLinea);
                }
                else
                {
                    var combo = await _repositoryCombo.FindByIdAsync(linea.IdCombo!.Value)
                        ?? throw new InvalidOperationException($"El combo con id {linea.IdCombo} no existe.");
                    precioUnitario = combo.PrecioTotal;

                    var productosCombo = await _repositoryCombo.ListProductosAsync(linea.IdCombo.Value);
                    foreach (var cp in productosCombo)
                    {
                        var pasos = await _repositoryPreparacionEstacion.ListByProductoAsync(cp.IdProducto);
                        AgregarPasosPlanificados(estacionesLinea, pasos,cp.IdProducto, ref fechaInicioLinea);
                    }
                }

                var subtotalLinea = precioUnitario * linea.Cantidad;
                var impuestoLinea = Math.Round(subtotalLinea * PORCENTAJE_IMPUESTO, 2);
                linea.PrecioUnitario = precioUnitario;
                linea.Subtotal = subtotalLinea;
                linea.Impuesto = impuestoLinea;
                linea.Observaciones ??= string.Empty;
                linea.Estaciones = estacionesLinea;

                subtotalPedido += subtotalLinea;
                impuestoPedido += impuestoLinea;
            }

            var totalPedido = subtotalPedido + impuestoPedido + costoEnvio;
            if (dto.Pago == null)
                throw new InvalidOperationException("Debe indicar la información de pago.");

            var metodoPago = await _repositoryMetodoPago.FindByIdAsync(dto.Pago.IdMetodoPago)
                ?? throw new InvalidOperationException("El método de pago seleccionado no es válido.");

            var esEfectivo = metodoPago.IdMetodoPago == ID_PAGO_EFECTIVO;
            var esTarjeta = metodoPago.IdMetodoPago == ID_PAGO_TARJETA_CREDITO || metodoPago.IdMetodoPago == ID_PAGO_TARJETA_DEBITO;

            dto.Pago.Monto = totalPedido;

            if (esEfectivo)
            {
                if (!dto.Pago.MontoRecibido.HasValue || dto.Pago.MontoRecibido.Value < totalPedido)
                    throw new InvalidOperationException("El monto recibido en efectivo debe ser mayor o igual al total del pedido.");

                dto.Pago.Vuelto = dto.Pago.MontoRecibido.Value - totalPedido;
                dto.Pago.Ultimos4Tarjeta = string.Empty;
            }
            else if (esTarjeta)
            {
                if (string.IsNullOrWhiteSpace(dto.Pago.Ultimos4Tarjeta) || dto.Pago.Ultimos4Tarjeta.Length != 4)
                    throw new InvalidOperationException("Debe indicar los últimos 4 dígitos de la tarjeta.");

                dto.Pago.MontoRecibido = totalPedido;
                dto.Pago.Vuelto = 0m;
            }
            else
            {
                throw new InvalidOperationException("El método de pago seleccionado no es válido.");
            }

            dto.IdEstadoPedido = estado.IdEstadoPedido;
            dto.IdTipoEntrega = tipoEntrega.IdTipoEntrega;
            dto.Subtotal = subtotalPedido;
            dto.Impuesto = impuestoPedido;
            dto.CostoEnvio = costoEnvio;
            dto.Total = totalPedido;

            var pedido = _mapper.Map<Pedido>(dto);

            _logger.LogInformation("Registrando pedido para el cliente {IdCliente}, total {Total}", idUsuarioCliente, totalPedido);

            return await _repository.AddAsync(pedido, idUsuarioCliente, idUsuarioEncargado);
        }

        public async Task<ICollection<PedidoDTO>> ListHistorialAsync(int idUsuarioLogueado, string rolUsuarioLogueado, DateTime? fecha, int? idEstadoPedido)
        {
            var esCliente = rolUsuarioLogueado.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase);

            ICollection<Pedido> pedidos;

            if (esCliente)
            {
                pedidos = await _repository.ListByClienteAsync(idUsuarioLogueado);
            }
            else
            {
                pedidos = await _repository.ListAsync(fecha, idEstadoPedido);
            }

            var listaDto = _mapper.Map<ICollection<PedidoDTO>>(pedidos);

            foreach (var (entidad, dto) in pedidos.Zip(listaDto))
            {
                AsignarNombresUsuarios(entidad, dto);
            }

            return listaDto;
        }

        private static void AsignarNombresUsuarios(Pedido entidad, PedidoDTO dto)
        {
            var usuarioCliente = entidad.IdUsuario
                .FirstOrDefault(u => u.IdRolNavigation.NombreRol.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase));

            var usuarioEncargado = entidad.IdUsuario
                .FirstOrDefault(u => !u.IdRolNavigation.NombreRol.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase));

            dto.NombreCliente = usuarioCliente != null ? $"{usuarioCliente.Nombre} {usuarioCliente.Apellido1}" : string.Empty;
            dto.CedulaCliente = usuarioCliente?.Cedula ?? string.Empty;
            dto.NombreEncargado = usuarioEncargado != null ? $"{usuarioEncargado.Nombre} {usuarioEncargado.Apellido1}" : string.Empty;
        }

        private static void AgregarPasosPlanificados(List<PedidoEstacionDTO> destino, ICollection<PreparacionEstacion> pasos, int idProducto, ref DateTime fechaInicio)
        {
            foreach (var paso in pasos.OrderBy(p => p.NumeroOrden))
            {
                var fechaFin = fechaInicio.AddMinutes(paso.TiempoEstimadoMinutos);

                destino.Add(new PedidoEstacionDTO
                {
                    IdProducto = idProducto,
                    IdEstacionCocina = paso.IdEstacionCocina,
                    IdEstadoPedidoEstacion = ID_ESTADO_PEDIDO_ESTACION_PENDIENTE,
                    OrdenProceso = paso.NumeroOrden,
                    TiempoEstimadoMinutos = paso.TiempoEstimadoMinutos,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                });

                fechaInicio = fechaFin;
            }
        }
    }
}