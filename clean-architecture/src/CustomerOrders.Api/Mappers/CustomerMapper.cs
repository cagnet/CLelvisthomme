using CustomerOrders.Api.Contracts;
using CustomerOrders.Core.Domain;

namespace CustomerOrders.Api.Mappers;

internal static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer customer) =>
        new(customer.Id, customer.Name, customer.FirstName, customer.Email, customer.Address, customer.IsActive);
}
