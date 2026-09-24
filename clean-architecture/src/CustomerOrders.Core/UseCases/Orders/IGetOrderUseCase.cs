using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Orders;

public interface IGetOrderUseCase
{
    IReadOnlyCollection<Order> GetAll();
    Order? GetById(int id);
}
