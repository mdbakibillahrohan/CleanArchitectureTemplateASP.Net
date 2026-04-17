using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.Products.AnyAsync(p => p.Name == request.Name, cancellationToken))
        {
            return Result.Failure<Guid>(ProductErrors.NameAlreadyExists);
        }

        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Currency,
            request.Stock);

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id.Value;
    }
}
