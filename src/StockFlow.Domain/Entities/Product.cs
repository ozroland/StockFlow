using StockFlow.Domain.Common;
using StockFlow.Domain.Events;
using StockFlow.Domain.Exceptions;
using StockFlow.Domain.ValueObjects;

namespace StockFlow.Domain.Entities;

public class Product(Sku sku, string name, Money price, string? description = null) : BaseEntity
{
    private const int LowStockThreshold = 10;

    public Sku Sku { get; private set; } = sku;
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public Money Price { get; private set; } = price;
    public int StockLevel { get; private set; }

    private Product() : this(null!, string.Empty, null!) { }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative.");

        Price = newPrice;
        LastModifiedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");

        Name = name;
        Description = description;
        LastModifiedAt = DateTimeOffset.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        StockLevel += quantity;
        LastModifiedAt = DateTimeOffset.UtcNow;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        int newLevel = StockLevel - quantity;

        if (newLevel < 0)
            throw new InsufficientStockException(Id, StockLevel, quantity);

        StockLevel = newLevel;
        LastModifiedAt = DateTimeOffset.UtcNow;

        if (StockLevel <= LowStockThreshold)
        {
            AddDomainEvent(new ProductLowStockEvent(Id, StockLevel));
        }
    }
}
