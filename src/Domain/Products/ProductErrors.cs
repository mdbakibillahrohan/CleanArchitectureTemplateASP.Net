using Domain.Abstractions;

namespace Domain.Products;

/// <summary>
/// Static error definitions for the Product aggregate.
/// </summary>
public static class ProductErrors
{
    public static readonly Error NotFound = new(
        "Product.NotFound", "The product was not found.", ErrorType.NotFound);

    public static readonly Error NameAlreadyExists = new(
        "Product.NameAlreadyExists", "A product with this name already exists.", ErrorType.Conflict);
}
