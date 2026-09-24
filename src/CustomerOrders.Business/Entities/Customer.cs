namespace CustomerOrders.Business.Entities;

public sealed record Customer(int Id, string Name, string? FirstName, string? Email, string? Address, bool IsActive);
