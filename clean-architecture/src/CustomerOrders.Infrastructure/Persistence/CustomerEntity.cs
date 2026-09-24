namespace CustomerOrders.Infrastructure.Persistence;

// EF Core mapped entity, kept separate from Core.Domain.Customer — this project
// is the only place where an EF Core dependency is allowed to exist.
public sealed class CustomerEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public required bool IsActive { get; set; }
}
