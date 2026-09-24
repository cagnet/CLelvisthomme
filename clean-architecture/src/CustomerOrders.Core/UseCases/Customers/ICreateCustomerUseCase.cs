using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Customers;

public interface ICreateCustomerUseCase
{
    Customer Create(string name, string? firstName, string? email, string? address, bool isActive);
}
