using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == new ProductId(request.ProductId), cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        var nameTaken = await _context.Products
            .AnyAsync(p => p.Name == request.Name && p.Id != product.Id, cancellationToken);

        if (nameTaken)
        {
            return Result.Failure(ProductErrors.NameAlreadyExists);
        }

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.Currency,
            request.Stock);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
