using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<Cart, CartDto>();
        CreateMap<OrderItems, OrderDetailDto>()
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(src => src.Product));
        CreateMap<ProductFormRequestDto, ProductRequestDto>();
        CreateMap<ProductRequestDto,Product>();
        CreateMap<ProductImageRequestDto, ProductImage>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.Image.FileName)))
            .ForMember(dest => dest.FileSizeInBytes, opt => opt.MapFrom(src => src.Image.Length))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); ;
        CreateMap<Address, AddressDto>();
        CreateMap<Product, ProductResponseDto>();
        CreateMap<ProductImage, ProductImageResponseDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath));
    }
}
