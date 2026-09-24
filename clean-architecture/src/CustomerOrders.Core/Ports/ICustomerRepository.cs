using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.Ports;

// Output port — what the domain needs from persistence, without knowing how
// it is implemented. Infrastructure provides the concrete adapter.
public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(int id);
    Customer Add(string name, string? firstName, string? email, string? address, bool isActive);
    void Update(Customer customer);
    bool Delete(int id);
}
