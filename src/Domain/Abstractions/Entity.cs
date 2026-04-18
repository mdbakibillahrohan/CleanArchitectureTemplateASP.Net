namespace Domain.Abstractions;

/// <summary>
/// Base entity with strongly typed ID, domain events, and audit fields.
/// </summary>
public class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
