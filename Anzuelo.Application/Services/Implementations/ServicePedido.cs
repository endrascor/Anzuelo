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

        private const int ID_ESTADO_PENDIENTE_PAGO = 1;
        private const int ID_ENTREGA_DOMICILIO = 1;
        private const int ID_PAGO_EFECTIVO = 1;
        private const int ID_PAGO_TARJETA_CREDITO = 2;
        private const int ID_PAGO_TARJETA_DEBITO = 3;

        private const string ROL_CLIENTE = "Cliente";

        private readonly IRepositoryPedido _repository;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryCombo _repositoryCombo;
        private readonly IRepositoryEstadoPedido _repositoryEstadoPedido;
        private readonly IRepositoryTipoEntrega _repositoryTipoEntrega;
        private readonly IRepositoryDireccion _repositoryDireccion;
        private readonly IRepositoryMetodoPago _repositoryMetodoPago;
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
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PedidoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<PedidoDTO>(@object);
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

            var estado = await _repositoryEstadoPedido.FindByIdAsync(ID_ESTADO_PENDIENTE_PAGO)
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

                if (esProducto)
                {
                    var producto = await _repositoryProducto.FindByIdAsync(linea.IdProducto!.Value)
                        ?? throw new InvalidOperationException($"El producto con id {linea.IdProducto} no existe.");
                    precioUnitario = producto.Precio;
                }
                else
                {
                    var combo = await _repositoryCombo.FindByIdAsync(linea.IdCombo!.Value)
                        ?? throw new InvalidOperationException($"El combo con id {linea.IdCombo} no existe.");
                    precioUnitario = combo.PrecioTotal;
                }

                var subtotalLinea = precioUnitario * linea.Cantidad;
                var impuestoLinea = Math.Round(subtotalLinea * PORCENTAJE_IMPUESTO, 2);
                linea.PrecioUnitario = precioUnitario;
                linea.Subtotal = subtotalLinea;
                linea.Impuesto = impuestoLinea;
                linea.Observaciones ??= string.Empty;

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
    }
}