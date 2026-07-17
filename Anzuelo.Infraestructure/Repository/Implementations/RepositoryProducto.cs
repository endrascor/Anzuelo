using Anzuelo.Infraestructure.Repository.Interfaces;
using Anzuelo.Infraestructure.Data;
using Anzuelo.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly AnzueloContext _context;
        public RepositoryProducto(AnzueloContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Producto>> ListAsync()
        {
            var collection = await _context.Set<Producto>()
                .Include(x => x.IdCategoriaProductoNavigation)
                .Include(x => x.IdEstadoProductoNavigation)
                .Include(x => x.IdPreparacionNavigation)
                .Include(x => x.ImagenProducto)
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            return collection;
        }

        public async Task<Producto> FindByIdAsync(int id)
        {
            var @Object = await _context.Set<Producto>()
                .Include(x => x.IdCategoriaProductoNavigation)
        .Include(x => x.IdEstadoProductoNavigation)
        .Include(x => x.IdPreparacionNavigation)

        .Include(x => x.ImagenProducto)

        .Include(x => x.ProductoIngrediente)
            .ThenInclude(pi => pi.IdIngredienteNavigation)

        .Include(x => x.ComboProducto)
            .ThenInclude(cp => cp.IdComboNavigation)

        .Include(x => x.MenuProducto)
            .ThenInclude(mp => mp.IdMenuNavigation)

        .FirstOrDefaultAsync(x => x.IdProducto == id);
            return @Object;
        }
        public async Task<int> AddAsync(Producto entity)
        {
            await _context.Set<Producto>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.IdProducto;
        }

        public async Task UpdateAsync(Producto entity)
        {
            var productoExistente =
                await _context.Set<Producto>()
                    .Include(x =>
                        x.ProductoIngrediente)
                    .Include(x =>
                        x.ImagenProducto)
                    .FirstOrDefaultAsync(x =>
                        x.IdProducto ==
                        entity.IdProducto);

            if (productoExistente == null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el producto con ID {entity.IdProducto}.");
            }

            productoExistente.Nombre =
                entity.Nombre;

            productoExistente.Descripcion =
                entity.Descripcion;

            productoExistente.Precio =
                entity.Precio;

            productoExistente.IdCategoriaProducto =
                entity.IdCategoriaProducto;

            productoExistente.IdEstadoProducto =
                entity.IdEstadoProducto;

            productoExistente.IdPreparacion =
                entity.IdPreparacion;

            ActualizarIngredientes(
                productoExistente,
                entity.ProductoIngrediente);

            ActualizarImagenes(
                productoExistente,
                entity.ImagenProducto);

            await _context.SaveChangesAsync();
        }

        private void ActualizarIngredientes(
            Producto productoExistente,
            ICollection<ProductoIngrediente>
                nuevosIngredientes)
        {
            var ingredientesNuevos =
                nuevosIngredientes
                    .Where(x =>
                        x.IdIngrediente > 0 &&
                        x.Cantidad > 0)
                    .GroupBy(x =>
                        x.IdIngrediente)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo.Sum(x =>
                            x.Cantidad));

            var relacionesAEliminar =
                productoExistente
                    .ProductoIngrediente
                    .Where(x =>
                        !ingredientesNuevos
                            .ContainsKey(
                                x.IdIngrediente))
                    .ToList();

            _context.Set<ProductoIngrediente>()
                .RemoveRange(relacionesAEliminar);

            foreach (var ingredienteNuevo
                in ingredientesNuevos)
            {
                var relacionExistente =
                    productoExistente
                        .ProductoIngrediente
                        .FirstOrDefault(x =>
                            x.IdIngrediente ==
                            ingredienteNuevo.Key);

                if (relacionExistente != null)
                {
                    relacionExistente.Cantidad =
                        ingredienteNuevo.Value;
                }
                else
                {
                    productoExistente
                        .ProductoIngrediente
                        .Add(
                            new ProductoIngrediente
                            {
                                IdProducto =
                                    productoExistente
                                        .IdProducto,

                                IdIngrediente =
                                    ingredienteNuevo.Key,

                                Cantidad =
                                    ingredienteNuevo.Value
                            });
                }
            }
        }

        private void ActualizarImagenes(
            Producto productoExistente,
            ICollection<ImagenProducto>
                imagenesRecibidas)
        {
            /*
             * IDs de las imágenes existentes que
             * el usuario decidió conservar.
             */
            var idsConservados =
                imagenesRecibidas
                    .Where(x =>
                        x.IdImagenProducto > 0)
                    .Select(x =>
                        x.IdImagenProducto)
                    .ToHashSet();

            /*
             * Eliminar las imágenes que ya no
             * fueron enviadas desde el Edit.
             */
            var imagenesAEliminar =
                productoExistente
                    .ImagenProducto
                    .Where(x =>
                        !idsConservados.Contains(
                            x.IdImagenProducto))
                    .ToList();

            _context.Set<ImagenProducto>()
                .RemoveRange(imagenesAEliminar);

            /*
             * Agregar las imágenes nuevas.
             */
            var imagenesNuevas =
                imagenesRecibidas
                    .Where(x =>
                        x.IdImagenProducto == 0 &&
                        x.Imagen != null &&
                        x.Imagen.Length > 0)
                    .ToList();

            foreach (var imagenNueva
                in imagenesNuevas)
            {
                productoExistente
                    .ImagenProducto
                    .Add(
                        new ImagenProducto
                        {
                            IdProducto =
                                productoExistente
                                    .IdProducto,

                            Imagen =
                                imagenNueva.Imagen
                        });
            }
        }
    }
}

