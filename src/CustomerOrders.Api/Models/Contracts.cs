public sealed record CreateCustomer(string Name, bool IsActive = true);
public sealed record UpdateCustomer(string? Name, bool? IsActive);
public sealed record CreateOrder(decimal Amount);
public sealed record UpdateOrder(decimal? Amount);
public sealed record Problem(string Code, string Detail);
