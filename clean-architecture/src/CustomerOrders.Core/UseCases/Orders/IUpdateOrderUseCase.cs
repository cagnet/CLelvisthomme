using CustomerOrders.Core.Domain;

namespace CustomerOrders.Core.UseCases.Orders;

public interface IUpdateOrderUseCase
{
    Order? Update(int id, decimal? amount);
}
