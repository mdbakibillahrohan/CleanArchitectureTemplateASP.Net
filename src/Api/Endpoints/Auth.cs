using Carter;

namespace Api.Endpoints;

public sealed class Auth:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");
        
    }
}