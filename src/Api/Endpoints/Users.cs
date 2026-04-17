using Api.Extensions;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.GetUsers;
using Carter;
using MediatR;

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

        // ── Auth (anonymous) ───────────────────────────────────────────

        group.MapPost("register", async (CreateUserCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Created($"api/users/{result.Value}", result.Value)
                : result.ToProblemDetails();
        })
        .WithName("RegisterUser")
        .AllowAnonymous();

        group.MapPost("login", async (LoginCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Ok(new { Token = result.Value })
                : result.ToProblemDetails();
        })
        .WithName("LoginUser")
        .AllowAnonymous();

        // ── Protected endpoints ────────────────────────────────────────

        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetUserByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetUserById")
        .RequireAuthorization();

        group.MapGet("", async (int pageNumber, int pageSize, ISender sender) =>
        {
            var query = new GetUsersQuery(pageNumber, pageSize);
            var result = await sender.Send(query);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetUsers")
        .RequireAuthorization();

        group.MapPut("{id:guid}", async (Guid id, UpdateUserRequest request, ISender sender) =>
        {
            var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("UpdateUser")
        .RequireAuthorization();

        group.MapDelete("{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteUserCommand(id));
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("DeleteUser")
        .RequireAuthorization("AdminOnly");
    }
}

// Request model (not the command, keeps route param separate from body)
public sealed record UpdateUserRequest(string FirstName, string LastName, string Email);
