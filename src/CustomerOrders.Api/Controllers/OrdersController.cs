using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

[ApiController]
[Route("orders")]
public sealed class OrdersController(OrderService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Order>> ReadAll() => Ok(service.ReadAll());

    [HttpGet("{id:int}")]
    public ActionResult<Order> ReadOne(int id) => service.ReadOne(id) is { } order ? Ok(order) : NotFound();

    [HttpPost("/customers/{customerId:int}/orders")]
    public ActionResult<Order> Create(int customerId, CreateOrder request)
    {
        if (request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Amount must be greater than zero.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var order = service.Create(customerId, request.Amount);
            return order is null ? NotFound() : CreatedAtAction(nameof(ReadOne), new { id = order.Id }, order);
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new Problem(exception.Code, exception.Message));
        }
    }

    [HttpPatch("{id:int}")]
    public ActionResult<Order> Update(int id, UpdateOrder request)
    {
        if (request.Amount is <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Amount must be greater than zero.");
            return ValidationProblem(ModelState);
        }

        var order = service.Update(id, request.Amount);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) => service.Delete(id) ? NoContent() : NotFound();
}
