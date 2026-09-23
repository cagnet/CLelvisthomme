using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

[ApiController]
[Route("customers")]
public sealed class CustomersController(CustomerService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> ReadAll() => Ok(service.ReadAll());

    [HttpGet("{id:int}")]
    public ActionResult<Customer> ReadOne(int id) => service.ReadOne(id) is { } customer ? Ok(customer) : NotFound();

    [HttpGet("{id:int}/orders")]
    public ActionResult<IEnumerable<Order>> ReadOrders(int id) => service.ReadOrders(id) is { } orders ? Ok(orders) : NotFound();

    [HttpPost]
    public ActionResult<Customer> Create(CreateCustomer request)
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
