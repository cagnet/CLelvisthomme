namespace CustomerOrders.Business.Entities;

public sealed record Order(int Id, int CustomerId, decimal Amount, DateTime CreatedAt);
