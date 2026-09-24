using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Ports;
using CustomerOrders.Infrastructure.Mappers;
using CustomerOrders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Infrastructure.Repositories;

public sealed class EfCustomerRepository(CustomerOrdersDbContext db) : ICustomerRepository
{
    public IReadOnlyCollection<Customer> GetAll() =>
        db.Customers.AsNoTracking().OrderBy(c => c.Id).Select(e => e.ToDomain()).ToArray();

    public Customer? GetById(int id) =>
        db.Customers.AsNoTracking().FirstOrDefault(c => c.Id == id)?.ToDomain();

    public Customer Add(string name, string? firstName, string? email, string? address, bool isActive)
    {
        var entity = new CustomerEntity
        {
            Name = name,
            FirstName = firstName,
            Email = email,
            Address = address,
            IsActive = isActive
        };
        db.Customers.Add(entity);
        db.SaveChanges();
        return entity.ToDomain();
    }

    public void Update(Customer customer)
    {
        var entity = db.Customers.First(c => c.Id == customer.Id);
        entity.Name = customer.Name;
        entity.FirstName = customer.FirstName;
        entity.Email = customer.Email;
        entity.Address = customer.Address;
        entity.IsActive = customer.IsActive;
        db.SaveChanges();
    }

    public bool Delete(int id)
    {
        var entity = db.Customers.FirstOrDefault(c => c.Id == id);
        if (entity is null) return false;
        db.Customers.Remove(entity);
        db.SaveChanges();
        return true;
    }
}
