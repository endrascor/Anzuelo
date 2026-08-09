using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;

namespace Anzuelo.Application.Services
{
    public class ServicePedidoEstacion :
        IServicePedidoEstacion
    {
        private readonly IRepositoryPedidoEstacion
            _repository;

        private readonly IRepositoryPedido
            _repositoryPedido;

        private readonly IRepositoryEstadoPedido
            _repositoryEstadoPedido;

        private readonly IMapper _mapper;

        public ServicePedidoEstacion(
            IRepositoryPedidoEstacion repository,
            IRepositoryPedido repositoryPedido,
            IRepositoryEstadoPedido repositoryEstadoPedido,
            IMapper mapper)
        {
            _repository = repository;
            _repositoryPedido = repositoryPedido;
            _repositoryEstadoPedido =
                repositoryEstadoPedido;
            _mapper = mapper;
        }

        public async Task<ICollection<PedidoEstacionDTO>>
            ListAsync()
        {
            var list =
                await _repository.ListAsync();

            var collection =
                _mapper.Map<List<PedidoEstacionDTO>>(
                    list);

            var productos =
                collection.GroupBy(x => new
                {
                    x.IdDetallePedido,
                    x.IdProducto
                });

            foreach (var producto in productos)
            {
                var etapas =
                    producto
                        .OrderBy(x =>
                            x.OrdenProceso)
                        .ToList();

                foreach (var etapa in etapas)
                {
                    etapa.PuedeFinalizar =
                        EsEnProceso(
                            etapa
                                .NombreEstadoPedidoEstacion);

                    if (!EsPendiente(
                            etapa
                                .NombreEstadoPedidoEstacion))
                    {
                        etapa.PuedeIniciar = false;
                        continue;
                    }

                    var etapasAnteriores =
                        etapas.Where(x =>
                            x.OrdenProceso <
                            etapa.OrdenProceso);

                    etapa.PuedeIniciar =
                        etapasAnteriores.All(x =>
                            EsCompletado(
                                x.NombreEstadoPedidoEstacion));
                }
            }

            return collection;
        }

        public async Task<PedidoEstacionDTO?>
            FindByIdAsync(int id)
        {
            var pedidoEstacion =
                await _repository.FindByIdAsync(id);

            if (pedidoEstacion == null)
                return null;

            return _mapper.Map<PedidoEstacionDTO>(
                pedidoEstacion);
        }

        public async Task<bool>
            IniciarAsync(int id)
        {
            var pedidoEstacion =
                await _repository.FindByIdAsync(id);

            if (pedidoEstacion == null)
                return false;

            if (!EsPendiente(
                    pedidoEstacion
                        .IdEstadoPedidoEstacionNavigation
                        .Descripcion))
            {
                return false;
            }

            var proceso =
                await _repository
                    .ListProcesoProductoAsync(
                        pedidoEstacion.IdDetallePedido,
                        pedidoEstacion.IdProducto);

            var anteriores =
                proceso.Where(x =>
                    x.OrdenProceso <
                    pedidoEstacion.OrdenProceso);

            var puedeIniciar =
                anteriores.All(x =>
                    EsCompletado(
                        x.IdEstadoPedidoEstacionNavigation
                            .Descripcion));

            if (!puedeIniciar)
                return false;

            var idEstadoProceso =
                await _repository
                    .FindEstadoIdAsync("proceso");

            if (!idEstadoProceso.HasValue)
                return false;

            var estadoPedidoPreparacion =
                await _repositoryEstadoPedido
                    .FindByDescripcionAsync("prepar");

            if (estadoPedidoPreparacion == null)
                return false;

            pedidoEstacion.IdEstadoPedidoEstacion =
                idEstadoProceso.Value;

            pedidoEstacion.FechaInicio =
                DateTime.Now;

            await _repository.UpdateAsync(
                pedidoEstacion);

            var idPedido =
                pedidoEstacion
                    .IdDetallePedidoNavigation
                    .IdPedido;

            var pedido =
                await _repositoryPedido
                    .FindByIdAsync(idPedido);

            if (pedido == null)
                return false;

            pedido.IdEstadoPedido =
                estadoPedidoPreparacion.IdEstadoPedido;

            await _repositoryPedido.UpdateAsync(
                pedido);

            return true;
        }

        public async Task<bool>
            FinalizarAsync(int id)
        {
            var pedidoEstacion =
                await _repository.FindByIdAsync(id);

            if (pedidoEstacion == null)
                return false;

            if (!EsEnProceso(
                    pedidoEstacion
                        .IdEstadoPedidoEstacionNavigation
                        .Descripcion))
            {
                return false;
            }

            var idEstadoFinalizado =
                await _repository
                    .FindEstadoIdAsync("finaliz");

            if (!idEstadoFinalizado.HasValue)
                return false;

            pedidoEstacion.IdEstadoPedidoEstacion =
                idEstadoFinalizado.Value;

            pedidoEstacion.FechaFin =
                DateTime.Now;

            await _repository.UpdateAsync(
                pedidoEstacion);

            var idPedido =
                pedidoEstacion
                    .IdDetallePedidoNavigation
                    .IdPedido;

            var todasFinalizadas =
                await _repository
                    .TodasFinalizadasPorPedidoAsync(
                        idPedido,
                        idEstadoFinalizado.Value);

            if (!todasFinalizadas)
                return true;

            var estadoPedidoEntregado =
                await _repositoryEstadoPedido
                    .FindByDescripcionAsync("entreg");

            if (estadoPedidoEntregado == null)
                return false;

            var pedido =
                await _repositoryPedido
                    .FindByIdAsync(idPedido);

            if (pedido == null)
                return false;

            pedido.IdEstadoPedido =
                estadoPedidoEntregado.IdEstadoPedido;

            await _repositoryPedido.UpdateAsync(
                pedido);

            return true;
        }

        private bool EsPendiente(
            string? estado)
        {
            return !string.IsNullOrWhiteSpace(estado)
                && estado
                    .ToLower()
                    .Contains("pendiente");
        }

        private bool EsEnProceso(
            string? estado)
        {
            return !string.IsNullOrWhiteSpace(estado)
                && estado
                    .ToLower()
                    .Contains("proceso");
        }

        private bool EsCompletado(
            string? estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return false;

            estado = estado.ToLower();

            return estado.Contains("complet")
                || estado.Contains("finaliz")
                || estado.Contains("terminad");
        }
    }
}