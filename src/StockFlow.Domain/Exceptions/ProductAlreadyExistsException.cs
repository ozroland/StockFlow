namespace StockFlow.Domain.Exceptions;

public class ProductAlreadyExistsException(string sku)
    : DomainException("Product Already Exists", $"A product with SKU '{sku}' already exists.")
{
    public string Sku { get; } = sku;
}
