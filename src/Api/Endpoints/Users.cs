
using Carter;

namespace Api.Endpoints;

/// <summary>
/// User endpoints using Carter module.
/// Demonstrates: CQRS, JWT auth, validation, pagination, Result pattern.
/// </summary>
public class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users").WithTags("Users");

        
    }
}
