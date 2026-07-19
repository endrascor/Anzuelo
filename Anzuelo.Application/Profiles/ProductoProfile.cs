using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzuelo.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {

            CreateMap<Producto, ProductoDTO>()

                .ForMember(
                    dest => dest.NombreCategoria,
                    opt => opt.MapFrom(src =>
                        src.IdCategoriaProductoNavigation != null
                            ? src.IdCategoriaProductoNavigation.Descripcion
                            : string.Empty))

                .ForMember(
                    dest => dest.NombreEstado,
                    opt => opt.MapFrom(src =>
                        src.IdEstadoProductoNavigation != null
                            ? src.IdEstadoProductoNavigation.Descripcion
                            : string.Empty))

                .ForMember(
                    dest => dest.Imagen,
                    opt => opt.MapFrom(src =>
                        src.ImagenProducto
                            .Select(i => i.Imagen)
                            .FirstOrDefault()))

                .ForMember(
                    dest => dest.Imagenes,
                    opt => opt.MapFrom(src =>
                        src.ImagenProducto))

                .ForMember(
                    dest => dest.Ingredientes,
                    opt => opt.MapFrom(src =>
                        src.ProductoIngrediente));

            CreateMap<ProductoDTO, Producto>()

                .ForMember(
                    dest => dest.IdProducto,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdCategoriaProductoNavigation,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.IdEstadoProductoNavigation,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.ImagenProducto,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.ProductoIngrediente,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.ComboProducto,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.MenuProducto,
                    opt => opt.Ignore());

            CreateMap<ProductoIngrediente, IngredienteDTO>()
                .ForMember(
                    dest => dest.IdIngrediente,
                    opt => opt.MapFrom(src =>
                        src.IdIngrediente))

                .ForMember(
                    dest => dest.NombreIngrediente,
                    opt => opt.MapFrom(src =>
                        src.IdIngredienteNavigation != null
                            ? src.IdIngredienteNavigation.NombreIngrediente
                            : string.Empty))

                .ForMember(
                    dest => dest.Cantidad,
                    opt => opt.MapFrom(src =>
                        src.Cantidad));

            CreateMap<ImagenProducto, ImagenDTO>()
                .ForMember(
                    dest => dest.IdImagenProducto,
                    opt => opt.MapFrom(src =>
                        src.IdImagenProducto))

                .ForMember(
                    dest => dest.Imagen,
                    opt => opt.MapFrom(src =>
                        src.Imagen));
        }
    }
}
