using Domain.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Products.EventHandlers;

/// <summary>
/// Handles the ProductCreatedEvent domain event.
/// </summary>
public sealed class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event Handled: Product '{ProductName}' ({ProductId}) was created",
            notification.ProductName,
            notification.ProductId.Value);

        return Task.CompletedTask;
    }
}
