namespace CustomerOrders.Api.Contracts;

public sealed record OrderDto(int Id, int CustomerId, decimal Amount, DateTime CreatedAt);
