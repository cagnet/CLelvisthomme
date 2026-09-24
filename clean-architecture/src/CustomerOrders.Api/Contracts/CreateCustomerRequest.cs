namespace CustomerOrders.Api.Contracts;

// Name is validated explicitly in CustomersController.Create (ModelState),
// matching the existing N-Tier project's approach — not a DataAnnotations
// attribute, which ASP.NET Core cannot combine with a record's primary
// constructor parameters here.
public sealed record CreateCustomerRequest(
    string Name,
    string? FirstName,
    string? Email,
    string? Address,
    bool IsActive = true);
