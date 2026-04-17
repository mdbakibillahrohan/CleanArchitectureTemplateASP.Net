using Domain.Abstractions;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email) : IRequest<Result>;
