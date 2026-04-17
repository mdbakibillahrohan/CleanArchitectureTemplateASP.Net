using Application.Abstractions.Data;
using Application.Abstractions.Pagination;
using Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersHandler
    : IRequestHandler<GetUsersQuery, Result<PagedResult<UserResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result<PagedResult<UserResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(u => u.FirstName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserResponse(
                u.Id.Value,
                u.FirstName,
                u.LastName,
                u.Email))
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<UserResponse>(
            users, request.PageNumber, request.PageSize, totalCount);

        return pagedResult;
    }
}
