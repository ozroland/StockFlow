namespace StockFlow.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency = "EUR") => new(0, currency);
}
