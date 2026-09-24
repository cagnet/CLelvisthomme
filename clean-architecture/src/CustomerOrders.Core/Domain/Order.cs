namespace CustomerOrders.Core.Domain;

public sealed record Order(int Id, int CustomerId, decimal Amount, DateTime CreatedAt);
