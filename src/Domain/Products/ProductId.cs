namespace Domain.Products;

/// <summary>
/// Strongly typed ID for Product entity.
/// </summary>
public record ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.NewGuid());
}
