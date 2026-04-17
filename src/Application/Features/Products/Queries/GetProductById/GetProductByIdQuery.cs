using Domain.Abstractions;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<Result<ProductDetailResponse>>;

public sealed record ProductDetailResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int Stock,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
