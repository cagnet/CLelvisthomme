using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using CustomerOrders.Data.InMemory;

namespace CustomerOrders.Data.Repositories;

public sealed class CustomerRepository(InMemoryDatabase database) : ICustomerRepository
{
    public IReadOnlyCollection<Customer> GetAll() => database.Customers.Values.OrderBy(x => x.Id).ToArray();
    public Customer? GetById(int id) => database.Customers.GetValueOrDefault(id);
    public Customer Add(string name, bool isActive)
    {
        var customer = new Customer(database.NextCustomerId(), name, isActive);
        database.Customers[customer.Id] = customer;
        return customer;
    }
    public void Update(Customer customer) => database.Customers[customer.Id] = customer;
    public bool Delete(int id) => database.Customers.TryRemove(id, out _);
}
