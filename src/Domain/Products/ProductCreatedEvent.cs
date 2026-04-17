using Domain.Abstractions;

namespace Domain.Products;

/// <summary>
/// Domain event raised when a product is created.
/// </summary>
public sealed record ProductCreatedEvent(ProductId ProductId, string ProductName) : IDomainEvent;
