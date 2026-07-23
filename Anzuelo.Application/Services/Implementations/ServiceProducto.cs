using Anzuelo.Application.DTOs;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceProducto> _logger;

        public ServiceProducto(
            IRepositoryProducto repository,
            IMapper mapper,
            ILogger<ServiceProducto> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ProductoDTO>> ListAync()
        {
            var list = await _repository.ListAsync();

            return _mapper.Map<ICollection<ProductoDTO>>(list);
        }

        public async Task<ProductoDTO> FindByIdAsync(int id)
        {
            var producto = await _repository.FindByIdAsync(id);

            return _mapper.Map<ProductoDTO>(producto);
        }

        public async Task<int> AddAsync(
    ProductoDTO dto,
    ICollection<byte[]> imagenes)
        {
            var producto = _mapper.Map<Producto>(dto);

            // Crear las relaciones producto-ingrediente
            producto.ProductoIngrediente = dto.Ingredientes
                .Where(x =>
                    x.IdIngrediente > 0 &&
                    x.Cantidad > 0)
                .Select(x => new ProductoIngrediente
                {
                    IdIngrediente = x.IdIngrediente,
                    Cantidad = x.Cantidad
                })
                .ToList();

            // Crear las entidades ImagenProducto
            producto.ImagenProducto = imagenes
                .Where(x => x != null && x.Length > 0)
                .Select(x => new ImagenProducto
                {
                    Imagen = x
                })
                .ToList();

            return await _repository.AddAsync(producto);
        }

        public async Task UpdateAsync(
            int id,
            ProductoDTO dto,
            ICollection<byte[]> nuevasImagenes)
        {
            var producto =
                _mapper.Map<Producto>(dto);

            producto.IdProducto = id;

            producto.ProductoIngrediente =
                CrearIngredientes(
                    dto.Ingredientes);

            /*
             * Se agregan solamente los IDs de las
             * imágenes que el usuario conservó.
             */
            producto.ImagenProducto =
                dto.Imagenes
                    .Where(x =>
                        x.IdImagenProducto > 0)
                    .Select(x =>
                        new ImagenProducto
                        {
                            IdProducto = id,

                            IdImagenProducto =
                                x.IdImagenProducto,

                            Imagen =
                                Array.Empty<byte>()
                        })
                    .ToList();

            /*
             * Se agregan las imágenes nuevas.
             */
            foreach (var imagenNueva
                in CrearImagenes(nuevasImagenes))
            {
                producto.ImagenProducto.Add(
                    imagenNueva);
            }

            await _repository.UpdateAsync(
                producto);
        }

        private static ICollection<
            ProductoIngrediente>
            CrearIngredientes(
                ICollection<IngredienteDTO>?
                    ingredientes)
        {
            if (ingredientes == null)
            {
                return new List<
                    ProductoIngrediente>();
            }

            return ingredientes
                .Where(x =>
                    x.IdIngrediente > 0 &&
                    x.Cantidad > 0)
                .GroupBy(x =>
                    x.IdIngrediente)
                .Select(grupo =>
                    new ProductoIngrediente
                    {
                        IdIngrediente =
                            grupo.Key,

                        Cantidad =
                            grupo.Sum(x =>
                                x.Cantidad)
                    })
                .ToList();
        }

        private static ICollection<
            ImagenProducto>
            CrearImagenes(
                ICollection<byte[]>? imagenes)
        {
            if (imagenes == null)
            {
                return new List<
                    ImagenProducto>();
            }

            return imagenes
                .Where(x =>
                    x != null &&
                    x.Length > 0)
                .Select(x =>
                    new ImagenProducto
                    {
                        Imagen = x
                    })
                .ToList();
        }
        public async Task<bool> ExisteNombreAsync(
    string nombre,
    int? idProductoExcluir = null)
        {
            return await _repository
                .ExisteNombreAsync(
                    nombre,
                    idProductoExcluir);
        }
    }
}