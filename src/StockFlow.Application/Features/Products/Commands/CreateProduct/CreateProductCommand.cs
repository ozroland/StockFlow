using MediatR;

using StockFlow.Application.Common.Interfaces;
using StockFlow.Domain.Entities;
using StockFlow.Domain.ValueObjects;

namespace StockFlow.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Sku,
    string Name,
    decimal PriceAmount,
    string PriceCurrency,
    string? Description
) : IRequest<Guid>;

public class CreateProductCommandHandler(IProductRepository repository) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var skuResult = Sku.Create(request.Sku) ?? throw new ArgumentException("Invalid SKU format!");
        var existingProduct = await repository.GetBySkuAsync(skuResult, cancellationToken);
        if (existingProduct is not null)
        {
            throw new InvalidOperationException($"Product with SKU '{request.Sku}' already exists.");
        }

        var price = new Money(request.PriceAmount, request.PriceCurrency);
        var product = new Product(skuResult, request.Name, price, request.Description);

        await repository.AddAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}