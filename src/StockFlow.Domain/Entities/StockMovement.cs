using StockFlow.Domain.Common;
using StockFlow.Domain.Enums;

namespace StockFlow.Domain.Entities;

public class StockMovement : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public string? Note { get; private set; }
    public Guid UserId { get; private set; }

    private StockMovement() { }

    public StockMovement(Guid productId, Guid warehouseId, StockMovementType type, int quantity, Guid userId, string? note = null)
    {
        if (quantity == 0)
            throw new ArgumentException("Quantity cannot be zero.");

        ProductId = productId;
        WarehouseId = warehouseId;
        Type = type;
        Quantity = quantity;
        UserId = userId;
        Note = note;
    }
}