namespace Application.Features.Users.Queries.GetUsers;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
