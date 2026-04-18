using Api.Extensions;
using Carter;
using MediatR;

namespace Api.Endpoints;

/// <summary>
/// Product endpoints using Carter module.
/// Demonstrates: full CRUD, filtering, sorting, pagination, role-based auth.
/// </summary>
public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products").WithTags("Products");

        
    }
}

