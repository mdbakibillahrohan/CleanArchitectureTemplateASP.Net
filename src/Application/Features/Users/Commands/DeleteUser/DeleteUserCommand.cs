using Domain.Abstractions;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : IRequest<Result>;
