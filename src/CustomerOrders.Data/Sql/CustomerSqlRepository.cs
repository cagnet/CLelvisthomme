using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Data.Sql;

public sealed class CustomerSqlRepository(CustomerOrdersDbContext db) : ICustomerRepository
{
    public IReadOnlyCollection<Customer> GetAll() =>
        db.Customers.AsNoTracking().OrderBy(c => c.Id).Select(ToDomain).ToArray();

    public Customer? GetById(int id) =>
        db.Customers.AsNoTracking().FirstOrDefault(c => c.Id == id) is { } entity ? ToDomain(entity) : null;

    public Customer Add(string name, string firstName, string email, string address, bool isActive)
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
        return ToDomain(entity);
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

    private static Customer ToDomain(CustomerEntity entity) =>
        new(entity.Id, entity.Name, entity.FirstName, entity.Email, entity.Address, entity.IsActive);
}
