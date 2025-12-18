using System.Text.RegularExpressions;

namespace StockFlow.Domain.ValueObjects;

public partial record Sku
{
    public const string Pattern = @"^[A-Z]{4}-\d{4}$";

    public string Value { get; }
    private Sku(string value) => Value = value;

    [GeneratedRegex(Pattern)]
    private static partial Regex SkuRegex();

    public static Sku Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be empty.", nameof(value));

        var normalized = value.ToUpper().Trim();

        return !SkuRegex().IsMatch(normalized)
            ? throw new ArgumentException($"Invalid SKU format. Expected pattern: {Pattern}", nameof(value))
            : new Sku(normalized);
    }

    public override string ToString() => Value;
}
