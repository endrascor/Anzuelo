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
                .ForMember(dest => dest.NombreCategoria,
                    opt => opt.MapFrom(src =>
                        src.IdCategoriaProductoNavigation.Descripcion))

                .ForMember(dest => dest.NombreEstado,
                    opt => opt.MapFrom(src =>
                        src.IdEstadoProductoNavigation.Descripcion))

                .ForMember(dest => dest.Imagen,
                    opt => opt.MapFrom(src =>
                        src.ImagenProducto
                            .Select(i => i.Imagen)
                            .FirstOrDefault()))

                .ForMember(dest => dest.Imagenes,
                    opt => opt.MapFrom(src => src.ImagenProducto))

                .ForMember(dest => dest.Ingredientes,
                    opt => opt.MapFrom(src => src.ProductoIngrediente));

            CreateMap<ProductoIngrediente, IngredienteDTO>()
    .ForMember(dest => dest.IdIngrediente,
        opt => opt.MapFrom(src => src.IdIngrediente))
    .ForMember(dest => dest.NombreIngrediente,
        opt => opt.MapFrom(src => src.IdIngredienteNavigation.NombreIngrediente))
    .ForMember(dest => dest.Cantidad,
        opt => opt.MapFrom(src => src.Cantidad));

            CreateMap<ImagenProducto, ImagenDTO>()
    .ForMember(dest => dest.IdImagenProducto,
        opt => opt.MapFrom(src => src.IdImagenProducto))
    .ForMember(dest => dest.Imagen,
        opt => opt.MapFrom(src => src.Imagen));

        }
    }
}
