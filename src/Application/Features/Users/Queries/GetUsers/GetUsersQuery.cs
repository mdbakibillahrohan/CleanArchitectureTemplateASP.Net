using Application.Abstractions.Pagination;
using Domain.Abstractions;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    int PageNumber,
    int PageSize) : IRequest<Result<PagedResult<UserResponse>>>;
