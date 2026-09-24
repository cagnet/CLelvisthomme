using CustomerOrders.Api.Contracts;
using CustomerOrders.Core.Domain;

namespace CustomerOrders.Api.Mappers;

internal static class OrderMapper
{
    public static OrderDto ToDto(this Order order) =>
        new(order.Id, order.CustomerId, order.Amount, order.CreatedAt);
}
