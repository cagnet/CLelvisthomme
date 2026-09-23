using System.Collections.Concurrent;
using CustomerOrders.Business.Entities;

namespace CustomerOrders.Data.InMemory;

public sealed class InMemoryDatabase
{
    private int _customerId;
    private int _orderId;
    internal ConcurrentDictionary<int, Customer> Customers { get; } = new();
    internal ConcurrentDictionary<int, Order> Orders { get; } = new();
    internal int NextCustomerId() => Interlocked.Increment(ref _customerId);
    internal int NextOrderId() => Interlocked.Increment(ref _orderId);
}
