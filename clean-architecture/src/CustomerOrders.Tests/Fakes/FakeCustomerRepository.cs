using CustomerOrders.Core.Domain;
using CustomerOrders.Core.Ports;

namespace CustomerOrders.Tests.Fakes;

// In-memory test double for ICustomerRepository — no EF Core, no database,
// just enough behavior to exercise CustomerService's business rules.
public sealed class FakeCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<int, Customer> _customers = new();
    private int _nextId;

    public IReadOnlyCollection<Customer> GetAll() => _customers.Values.ToArray();

    public Customer? GetById(int id) => _customers.GetValueOrDefault(id);

    public Customer Add(string name, string? firstName, string? email, string? address, bool isActive)
    {
        var customer = new Customer(++_nextId, name, firstName, email, address, isActive);
        _customers[customer.Id] = customer;
        return customer;
    }

    public void Update(Customer customer) => _customers[customer.Id] = customer;

    public bool Delete(int id) => _customers.Remove(id);
}
