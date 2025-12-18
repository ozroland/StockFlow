using MediatR;

using StockFlow.Application.Common.Interfaces;

namespace StockFlow.Application.Features.Products.Commands.UpdateProductStock;

public enum StockChangeType
{
    Increase,
    Decrease 
}

public record UpdateProductStockRequest(
    int Quantity,
    StockChangeType ChangeType
);

public record UpdateProductStockCommand(
    Guid ProductId,
    int Quantity,
    StockChangeType ChangeType
) : IRequest;

public class UpdateProductStockCommandHandler(IProductRepository repository) : IRequestHandler<UpdateProductStockCommand>
{
    public async Task Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

        switch (request.ChangeType)
        {
            case StockChangeType.Increase:
                product.AddStock(request.Quantity);
                break;
            case StockChangeType.Decrease:
                product.RemoveStock(request.Quantity);
                break;
        }

        await repository.UpdateAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
