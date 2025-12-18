namespace StockFlow.Domain.Exceptions;

public class InsufficientStockException(Guid productId, int currentStock, int requestedQuantity)
    : DomainException("Insufficient Stock",
        $"The product {productId} has insufficient stock. Current: {currentStock}, Requested deduction: {requestedQuantity}")
{
    public Guid ProductId { get; } = productId;
    public int CurrentStock { get; } = currentStock;
    public int RequestedQuantity { get; } = requestedQuantity;
}
