using CustomerOrders.Api.Contracts;
using CustomerOrders.Api.Mappers;
using CustomerOrders.Core.UseCases.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrders.Api.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrdersController(
    IGetOrderUseCase getOrder,
    IUpdateOrderUseCase updateOrder,
    IDeleteOrderUseCase deleteOrder) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> ReadAll() =>
        Ok(getOrder.GetAll().Select(o => o.ToDto()));

    [HttpGet("{id:int}")]
    public ActionResult<OrderDto> ReadOne(int id) =>
        getOrder.GetById(id) is { } order ? Ok(order.ToDto()) : NotFound();

    [HttpPatch("{id:int}")]
    public ActionResult<OrderDto> Update(int id, UpdateOrderRequest request)
    {
        if (request.Amount is <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Amount must be greater than zero.");
            return ValidationProblem(ModelState);
        }

        var order = updateOrder.Update(id, request.Amount);
        return order is null ? NotFound() : Ok(order.ToDto());
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) =>
        deleteOrder.Delete(id) ? NoContent() : NotFound();
}
