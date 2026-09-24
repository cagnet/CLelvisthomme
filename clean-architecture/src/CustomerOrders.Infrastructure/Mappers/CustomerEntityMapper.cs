using CustomerOrders.Core.Domain;
using CustomerOrders.Infrastructure.Persistence;

namespace CustomerOrders.Infrastructure.Mappers;

// Entity -> Domain only, exposed one-way. CustomerEntity never leaks outside
// this project's repositories.
internal static class CustomerEntityMapper
{
    public static Customer ToDomain(this CustomerEntity entity) =>
        new(entity.Id, entity.Name, entity.FirstName, entity.Email, entity.Address, entity.IsActive);
}
