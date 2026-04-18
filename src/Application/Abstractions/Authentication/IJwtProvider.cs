
using Domain.Entities;

namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(User user);
}
