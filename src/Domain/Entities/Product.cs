using Domain.Abstractions;

namespace Domain.Entities;

public class Product:Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}