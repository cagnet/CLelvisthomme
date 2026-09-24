namespace CustomerOrders.Core.UseCases.Orders;

public interface IDeleteOrderUseCase
{
    bool Delete(int id);
}
