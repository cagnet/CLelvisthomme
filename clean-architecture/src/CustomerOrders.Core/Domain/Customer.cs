namespace CustomerOrders.Core.Domain;

// Only Name is required — FirstName/Email/Address stay optional because the
// only validation requirement in the spec is on the customer's name.
public sealed record Customer(int Id, string Name, string? FirstName, string? Email, string? Address, bool IsActive);
