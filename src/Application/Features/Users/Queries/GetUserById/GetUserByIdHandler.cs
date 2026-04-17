using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler
    : IRequestHandler<GetUserByIdQuery, Result<UserDetailResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result<UserDetailResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == new UserId(request.UserId))
            .Select(u => new UserDetailResponse(
                u.Id.Value,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role.ToString(),
                u.CreatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserDetailResponse>(UserErrors.NotFound);
        }

        return user;
    }
}
