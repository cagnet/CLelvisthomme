namespace CustomerOrders.Api.Contracts;

// Every field optional — a PATCH only overwrites what the caller sends.
public sealed record UpdateCustomerRequest(string? Name, string? FirstName, string? Email, string? Address, bool? IsActive);
