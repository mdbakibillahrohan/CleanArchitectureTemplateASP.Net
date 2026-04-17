using MediatR;

namespace Domain.Abstractions;

/// <summary>
/// Marker interface for domain events dispatched via MediatR.
/// </summary>
public interface IDomainEvent : INotification;
