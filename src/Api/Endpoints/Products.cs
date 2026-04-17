using Api.Extensions;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetProductById;
using Application.Features.Products.Queries.GetProducts;
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

        group.MapGet("", async (
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            ISender sender) =>
        {
            var query = new GetProductsQuery(pageNumber, pageSize, searchTerm, sortBy, sortDescending);
            var result = await sender.Send(query);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetProducts")
        .AllowAnonymous();

        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetProductById")
        .AllowAnonymous();

        group.MapPost("", async (CreateProductCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Created($"api/products/{result.Value}", result.Value)
                : result.ToProblemDetails();
        })
        .WithName("CreateProduct")
        .RequireAuthorization();

        group.MapPut("{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
        {
            var command = new UpdateProductCommand(
                id, request.Name, request.Description, request.Price, request.Currency, request.Stock);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("UpdateProduct")
        .RequireAuthorization();

        group.MapDelete("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("DeleteProduct")
        .RequireAuthorization("AdminOnly");
    }
}

public sealed record UpdateProductRequest(
    string Name, string Description, decimal Price, string Currency, int Stock);
