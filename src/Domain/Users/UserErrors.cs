using Domain.Abstractions;

namespace Domain.Users;

/// <summary>
/// Static error definitions for the User aggregate.
/// </summary>
public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound", "The user was not found.", ErrorType.NotFound);

    public static readonly Error EmailAlreadyExists = new(
        "User.EmailAlreadyExists", "The email is already in use.", ErrorType.Conflict);

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials", "Invalid email or password.", ErrorType.UnAuthorized);
}
