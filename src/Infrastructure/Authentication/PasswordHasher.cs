using Application.Abstractions.Authentication;

namespace Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) => 
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
