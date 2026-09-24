namespace CustomerOrders.Api.Contracts;

// Business-rule error shape (409 responses) — matches the existing N-Tier
// project's Problem(Code, Detail) contract for consistency across the API.
public sealed record ProblemDto(string Code, string Detail);
