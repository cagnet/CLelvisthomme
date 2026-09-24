namespace CustomerOrders.Api.Contracts;

// Single response shape, reused by GET, POST and PATCH — Customer has one
// stable representation, unlike a case such as a single-exposure token that
// would justify a per-endpoint response DTO.
public sealed record CustomerDto(int Id, string Name, string? FirstName, string? Email, string? Address, bool IsActive);
