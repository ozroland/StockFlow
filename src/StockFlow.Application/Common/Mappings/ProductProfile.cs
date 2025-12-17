using AutoMapper;

using StockFlow.Application.Features.Products.DTOs;
using StockFlow.Domain.Entities;

namespace StockFlow.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Value))
            .ForMember(dest => dest.PriceAmount, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency));
    }
}
