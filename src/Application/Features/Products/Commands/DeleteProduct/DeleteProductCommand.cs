using Domain.Abstractions;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : IRequest<Result>;
