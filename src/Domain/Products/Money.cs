namespace Domain.Products;

/// <summary>
/// Value Object representing monetary value.
/// Demonstrates Value Object pattern - immutable, equality by value.
/// </summary>
public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency = "USD") => new(0, currency);

    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        return new Money(amount, currency.ToUpperInvariant());
    }
}
