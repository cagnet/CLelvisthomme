using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

[ApiController]
[Route("customers")]
public sealed class CustomersController(CustomerService service, OrderService orderService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> ReadAll() => Ok(service.ReadAll());

    [HttpGet("{id:int}")]
    public ActionResult<Customer> ReadOne(int id) => service.ReadOne(id) is { } customer ? Ok(customer) : NotFound();

    [HttpGet("{id:int}/orders")]
    public ActionResult<IEnumerable<Order>> ReadOrders(int id) => service.ReadOrders(id) is { } orders ? Ok(orders) : NotFound();


    [HttpPost("{customerId:int}/orders")]
    public ActionResult<Order> CreateOrder(int customerId, CreateOrder request)
    {
        if (request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Amount must be greater than zero.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var order = orderService.Create(customerId, request.Amount);
            return order is null ? NotFound() : CreatedAtAction(nameof(OrdersController.ReadOne), "Orders", new { id = order.Id }, order);
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new Problem(exception.Code, exception.Message));
        }
    }


    [HttpPost]
    public ActionResult<Customer> CreateCustomer(CreateCustomer request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var customer = service.Create(request.Name, request.IsActive);
        return CreatedAtAction(nameof(ReadOne), new { id = customer.Id }, customer);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<Customer> Update(int id, UpdateCustomer request)
    {
        if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name cannot be empty.");
            return ValidationProblem(ModelState);
        }

        var customer = service.Update(id, request.Name, request.IsActive);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            return service.Delete(id) ? NoContent() : NotFound();
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new Problem(exception.Code, exception.Message));
        }
    }
}
