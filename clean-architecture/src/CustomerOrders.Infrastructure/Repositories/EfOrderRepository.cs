using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Ports;
using CustomerOrders.Infrastructure.Mappers;
using CustomerOrders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Infrastructure.Repositories;

public sealed class EfOrderRepository(CustomerOrdersDbContext db) : IOrderRepository
{
    public IReadOnlyCollection<Order> GetAll() =>
        db.Orders.AsNoTracking().OrderBy(o => o.Id).Select(e => e.ToDomain()).ToArray();

    public IReadOnlyCollection<Order> GetByCustomerId(int customerId) =>
        db.Orders.AsNoTracking().Where(o => o.CustomerId == customerId).OrderBy(o => o.Id).Select(e => e.ToDomain()).ToArray();

    public Order? GetById(int id) =>
        db.Orders.AsNoTracking().FirstOrDefault(o => o.Id == id)?.ToDomain();

    public Order Add(int customerId, decimal amount, DateTime createdAt)
    {
        var entity = new OrderEntity { CustomerId = customerId, Amount = amount, CreatedAt = createdAt };
        db.Orders.Add(entity);
        db.SaveChanges();
        return entity.ToDomain();
    }

    public void Update(Order order)
    {
        var entity = db.Orders.First(o => o.Id == order.Id);
        entity.Amount = order.Amount;
        db.SaveChanges();
    }

    public bool Delete(int id)
    {
        var entity = db.Orders.FirstOrDefault(o => o.Id == id);
        if (entity is null) return false;
        db.Orders.Remove(entity);
        db.SaveChanges();
        return true;
    }

    public bool ExistsForCustomer(int customerId) =>
        db.Orders.AsNoTracking().Any(o => o.CustomerId == customerId);
}
