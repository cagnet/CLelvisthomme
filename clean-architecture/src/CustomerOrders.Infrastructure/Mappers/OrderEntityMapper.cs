using CustomerOrders.Core.Domain;
using CustomerOrders.Infrastructure.Persistence;

namespace CustomerOrders.Infrastructure.Mappers;

internal static class OrderEntityMapper
{
    public static Order ToDomain(this OrderEntity entity) =>
        new(entity.Id, entity.CustomerId, entity.Amount, entity.CreatedAt);
}
