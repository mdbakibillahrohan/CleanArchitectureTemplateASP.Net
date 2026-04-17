using Domain.Abstractions;

namespace Domain.Users;

public sealed class User : Entity<UserId>
{
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        Role role)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    private User() { } // EF Core

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public Role Role { get; private set; }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        Role role = Role.User)
    {
        var user = new User(UserId.New(), firstName, lastName, email, passwordHash, role);

        user.RaiseDomainEvent(new UserCreatedEvent(user.Id));

        return user;
    }

    public void Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}

public enum Role
{
    User = 1,
    Admin = 2
}
