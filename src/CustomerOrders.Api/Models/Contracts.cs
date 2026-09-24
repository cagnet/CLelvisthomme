public sealed record CreateCustomer(string Name, string FirstName, string Email, string Address, bool IsActive = true);
public sealed record UpdateCustomer(string? Name, string? FirstName, string? Email, string? Address, bool? IsActive);
public sealed record CreateOrder(decimal Amount);
public sealed record UpdateOrder(decimal? Amount);
public sealed record Problem(string Code, string Detail);
