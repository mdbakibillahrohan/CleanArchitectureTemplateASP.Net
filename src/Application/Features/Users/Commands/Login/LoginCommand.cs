using Domain.Abstractions;
using MediatR;

namespace Application.Features.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
