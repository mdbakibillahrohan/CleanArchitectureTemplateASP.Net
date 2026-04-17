using Domain.Abstractions;

namespace Domain.Products;

/// <summary>
/// Product aggregate root demonstrating rich domain model.
/// Uses Value Object (Money), Domain Events, and encapsulated state changes.
/// </summary>
public sealed class Product : Entity<ProductId>
{
    private Product(
        ProductId id,
        string name,
        string description,
        Money price,
        int stock,
        ProductStatus status)
        : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        Status = status;
    }

    private Product() { } // EF Core

    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public Money Price { get; private set; } = default!;
    public int Stock { get; private set; }
    public ProductStatus Status { get; private set; }

    public static Product Create(
        string name,
        string description,
        decimal price,
        string currency,
        int stock)
    {
        var product = new Product(
            ProductId.New(),
            name,
            description,
            Money.Create(price, currency),
            stock,
            stock > 0 ? ProductStatus.Active : ProductStatus.OutOfStock);

        product.RaiseDomainEvent(new ProductCreatedEvent(product.Id, product.Name));

        return product;
    }

    public void Update(string name, string description, decimal price, string currency, int stock)
    {
        Name = name;
        Description = description;
        Price = Money.Create(price, currency);
        Stock = stock;
        Status = stock > 0 ? ProductStatus.Active : ProductStatus.OutOfStock;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = ProductStatus.Inactive;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
