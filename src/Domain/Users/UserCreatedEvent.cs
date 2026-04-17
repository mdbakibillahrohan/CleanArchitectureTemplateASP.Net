using Domain.Abstractions;

namespace Domain.Users;

/// <summary>
/// Domain event raised when a new user is created.
/// Demonstrates domain event pattern dispatched via MediatR.
/// </summary>
public sealed record UserCreatedEvent(UserId UserId) : IDomainEvent;
