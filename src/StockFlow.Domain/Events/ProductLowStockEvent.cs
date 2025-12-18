using StockFlow.Domain.Common;

namespace StockFlow.Domain.Events;

public record ProductLowStockEvent(Guid ProductId, int CurrentStock) : IDomainEvent;
