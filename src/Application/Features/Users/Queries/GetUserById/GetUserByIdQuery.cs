using Domain.Abstractions;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDetailResponse>>;

public sealed record UserDetailResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    DateTime CreatedAtUtc);
