using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == new UserId(request.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        // Check if new email is taken by another user
        var emailTaken = await _context.Users
            .AnyAsync(u => u.Email == request.Email && u.Id != user.Id, cancellationToken);

        if (emailTaken)
        {
            return Result.Failure(UserErrors.EmailAlreadyExists);
        }

        user.Update(request.FirstName, request.LastName, request.Email);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
