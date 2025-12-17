namespace StockFlow.Domain.ValueObjects;

public record Sku
{
    private const int _defaultLength = 8;
    public string Value { get; }

    private Sku(string value) => Value = value;

    public static Sku? Create(string value)
    {
        return string.IsNullOrWhiteSpace(value) || value.Length < _defaultLength ? null : new Sku(value.ToUpper());
    }

    public override string ToString() => Value;
}
