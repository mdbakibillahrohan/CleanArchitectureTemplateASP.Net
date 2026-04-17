using Application.Abstractions.Data;
using Application.Abstractions.Pagination;
using Domain.Abstractions;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsHandler
    : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result<PagedResult<ProductResponse>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Product> query = _context.Products.AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p =>
                p.Name.Contains(request.SearchTerm) ||
                p.Description.Contains(request.SearchTerm));
        }

        // Sorting
        Expression<Func<Product, object>> sortExpression = request.SortBy?.ToLowerInvariant() switch
        {
            "name" => p => p.Name,
            "price" => p => p.Price.Amount,
            "stock" => p => p.Stock,
            "status" => p => p.Status,
            _ => p => p.CreatedAtUtc
        };

        query = request.SortDescending
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductResponse(
                p.Id.Value,
                p.Name,
                p.Description,
                p.Price.Amount,
                p.Price.Currency,
                p.Stock,
                p.Status.ToString()))
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductResponse>(
            products, request.PageNumber, request.PageSize, totalCount);
    }
}
