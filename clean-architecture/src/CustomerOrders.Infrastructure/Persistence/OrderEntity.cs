namespace CustomerOrders.Infrastructure.Persistence;

public sealed class OrderEntity
{
    public int Id { get; set; }
    public required int CustomerId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
}
