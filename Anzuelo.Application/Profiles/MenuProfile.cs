using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;

namespace Anzuelo.Application.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            /*
             * Tablas intermedias utilizadas en Edit.
             */

            CreateMap<MenuProducto, MenuProductoDTO>()
                .ForMember(
                    destino => destino.IdProducto,
                    opcion => opcion.MapFrom(
                        origen => origen.IdProducto))

                .ForMember(
                    destino => destino.NombreProducto,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdProductoNavigation != null
                                ? origen.IdProductoNavigation.Nombre
                                : string.Empty))

                .ForMember(
                    destino => destino.Descuento,
                    opcion => opcion.MapFrom(
                        origen => origen.Descuento));

            CreateMap<MenuCombo, MenuComboDTO>()
                .ForMember(
                    destino => destino.IdCombo,
                    opcion => opcion.MapFrom(
                        origen => origen.IdCombo))

                .ForMember(
                    destino => destino.NombreCombo,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdComboNavigation != null
                                ? origen.IdComboNavigation.Nombre
                                : string.Empty))

                .ForMember(
                    destino => destino.Descuento,
                    opcion => opcion.MapFrom(
                        origen => origen.Descuento));

            /*
             * Menu hacia MenuDTO.
             */

            CreateMap<Menu, MenuDTO>()

                .ForMember(
                    destino => destino.NombreEstado,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdEstadoMenuNavigation != null
                                ? origen.IdEstadoMenuNavigation.Descripcion
                                : string.Empty))

                .ForMember(
                    destino => destino.FechaInicio,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation.FechaInicio))

                .ForMember(
                    destino => destino.FechaFinal,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation.FechaFinal))

                .ForMember(
                    destino => destino.HoraInicio,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation.HoraInicio))

                .ForMember(
                    destino => destino.HoraFinal,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation.HoraFinal))

                .ForMember(
                    destino => destino.DescripcionDisponibilidad,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation.Descripcion))

                .ForMember(
                    destino => destino.IdDisponibilidadDia,
                    opcion => opcion.MapFrom(
                        origen =>
                            origen.IdDisponibilidadNavigation
                                .IdDisponibilidadDia))

                /*
                 * Aquí se recuperan los descuentos.
                 */
                .ForMember(
                    destino => destino.Productos,
                    opcion => opcion.MapFrom(
                        origen => origen.MenuProducto))

                .ForMember(
                    destino => destino.Combos,
                    opcion => opcion.MapFrom(
                        origen => origen.MenuCombo))

                /*
                 * Conserva debajo tus mapeos actuales
                 * de ProductosPorCategoria y
                 * CombosPorCategoria.
                 */
                .ForMember(
                    destino => destino.ProductosPorCategoria,
                    opcion => opcion.MapFrom(origen =>
                        origen.MenuProducto
                            .GroupBy(relacion =>
                                relacion
                                    .IdProductoNavigation
                                    .IdCategoriaProductoNavigation
                                    .Descripcion
                                ?? "Sin categoría")
                            .Select(grupo =>
                                new MenuCategoriaProductoDTO
                                {
                                    Categoria = grupo.Key,

                                    Productos = grupo
                                        .Select(relacion =>
                                            new ProductoDTO
                                            {
                                                IdProducto =
                                                    relacion.IdProducto,

                                                Nombre =
                                                    relacion
                                                        .IdProductoNavigation
                                                        .Nombre,

                                                Descripcion =
                                                    relacion
                                                        .IdProductoNavigation
                                                        .Descripcion,

                                                Precio =
                                                    relacion
                                                        .IdProductoNavigation
                                                        .Precio,

                                                NombreCategoria =
                                                    grupo.Key,

                                                Imagen =
                                                    relacion
                                                        .IdProductoNavigation
                                                        .ImagenProducto
                                                        .Select(imagen =>
                                                            imagen.Imagen)
                                                        .FirstOrDefault()
                                            })
                                        .ToList()
                                })
                            .ToList()))

                .ForMember(
                    destino => destino.CombosPorCategoria,
                    opcion => opcion.MapFrom(origen =>
                        origen.MenuCombo
                            .GroupBy(relacion =>
                                relacion
                                    .IdComboNavigation
                                    .IdCategoriaComboNavigation
                                    .Descripcion
                                ?? "Sin categoría")
                            .Select(grupo =>
                                new MenuCategoriaComboDTO
                                {
                                    Categoria = grupo.Key,

                                    Combos = grupo
                                        .Select(relacion =>
                                            new ComboDTO
                                            {
                                                IdCombo =
                                                    relacion.IdCombo,

                                                Nombre =
                                                    relacion
                                                        .IdComboNavigation
                                                        .Nombre,

                                                Descripcion =
                                                    relacion
                                                        .IdComboNavigation
                                                        .Descripcion,

                                                PrecioTotal =
                                                    relacion
                                                        .IdComboNavigation
                                                        .PrecioTotal,

                                                NombreCategoria =
                                                    grupo.Key,

                                                Productos =
                                                    relacion
                                                        .IdComboNavigation
                                                        .ComboProducto
                                                        .Select(comboProducto =>
                                                            new ComboProductoDTO
                                                            {
                                                                IdProducto =
                                                                    comboProducto
                                                                        .IdProducto,

                                                                NombreProducto =
                                                                    comboProducto
                                                                        .IdProductoNavigation
                                                                        .Nombre,

                                                                Cantidad =
                                                                    comboProducto
                                                                        .Cantidad,

                                                                Imagen =
                                                                    comboProducto
                                                                        .IdProductoNavigation
                                                                        .ImagenProducto
                                                                        .Select(imagen =>
                                                                            imagen.Imagen)
                                                                        .FirstOrDefault()
                                                            })
                                                        .ToList()
                                            })
                                        .ToList()
                                })
                            .ToList()));

            /*
             * MenuDTO hacia Menu.
             */

            CreateMap<MenuDTO, Menu>()
                .ForMember(
                    destino => destino.IdMenu,
                    opcion => opcion.Ignore())

                .ForMember(
                    destino => destino.IdDisponibilidadNavigation,
                    opcion => opcion.Ignore())

                .ForMember(
                    destino => destino.IdEstadoMenuNavigation,
                    opcion => opcion.Ignore())

                .ForMember(
                    destino => destino.MenuProducto,
                    opcion => opcion.Ignore())

                .ForMember(
                    destino => destino.MenuCombo,
                    opcion => opcion.Ignore());
        }
    }
}