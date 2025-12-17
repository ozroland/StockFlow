using StockFlow.Domain.Entities;
using StockFlow.Domain.ValueObjects;

namespace StockFlow.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Product?> GetBySkuAsync(Sku sku, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
