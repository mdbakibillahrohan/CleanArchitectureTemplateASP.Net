using Application.Abstractions.Pagination;
using Domain.Abstractions;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts;

/// <summary>
/// Query demonstrating pagination, filtering, and sorting.
/// </summary>
public sealed record GetProductsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    string? SortBy,
    bool SortDescending = false) : IRequest<Result<PagedResult<ProductResponse>>>;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int Stock,
    string Status);
