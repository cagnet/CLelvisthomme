using CustomerOrders.Business.Entities;

namespace CustomerOrders.Business.Repositories;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(int id);
    Customer Add(string name, string firstName, string email, string address, bool isActive);
    void Update(Customer customer);
    bool Delete(int id);
}
