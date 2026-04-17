using Domain.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.EventHandlers;

/// <summary>
/// Handles the UserCreatedEvent domain event.
/// Demonstrates how domain events are dispatched and handled via MediatR notifications.
/// </summary>
public sealed class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event Handled: User {UserId} was created",
            notification.UserId.Value);

        // In a real app you could:
        // - Send a welcome email
        // - Publish an integration event
        // - Update a read model / cache

        return Task.CompletedTask;
    }
}
