namespace StockFlow.Application.Features.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal PriceAmount,
    string PriceCurrency,
    int StockLevel
);
