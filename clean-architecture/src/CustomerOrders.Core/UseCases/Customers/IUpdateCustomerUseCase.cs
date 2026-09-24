using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Customers;

public interface IUpdateCustomerUseCase
{
    Customer? Update(int id, string? name, string? firstName, string? email, string? address, bool? isActive);
}
