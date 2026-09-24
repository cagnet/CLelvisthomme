using CustomerOrders.Api.Contracts;
using CustomerOrders.Api.Mappers;
using CustomerOrders.Core.UseCases.Customers;
using CustomerOrders.Core.UseCases.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrders.Api.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomersController(
    IGetCustomerUseCase getCustomer,
    ICreateCustomerUseCase createCustomer,
    IUpdateCustomerUseCase updateCustomer,
    IDeleteCustomerUseCase deleteCustomer,
    ICreateOrderUseCase createOrder) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> ReadAll() =>
        Ok(getCustomer.GetAll().Select(c => c.ToDto()));

    [HttpGet("{id:int}")]
    public ActionResult<CustomerDto> ReadOne(int id) =>
        getCustomer.GetById(id) is { } customer ? Ok(customer.ToDto()) : NotFound();

    [HttpGet("{id:int}/orders")]
    public ActionResult<IEnumerable<OrderDto>> ReadOrders(int id) =>
        getCustomer.GetOrders(id) is { } orders ? Ok(orders.Select(o => o.ToDto())) : NotFound();

    [HttpPost]
    public ActionResult<CustomerDto> Create(CreateCustomerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var customer = createCustomer.Create(request.Name, request.FirstName, request.Email, request.Address, request.IsActive);
        return CreatedAtAction(nameof(ReadOne), new { id = customer.Id }, customer.ToDto());
    }

    // Order creation lives here (not in OrdersController): a sub-resource is
    // created through its parent — same REST convention adopted for task 1.
    [HttpPost("{customerId:int}/orders")]
    public ActionResult<OrderDto> CreateOrder(int customerId, CreateOrderRequest request)
    {
        if (request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Amount must be greater than zero.");
            return ValidationProblem(ModelState);
        }

        var order = createOrder.Create(customerId, request.Amount);
        return order is null
            ? NotFound()
            : CreatedAtAction(nameof(OrdersController.ReadOne), "Orders", new { id = order.Id }, order.ToDto());
    }

    [HttpPatch("{id:int}")]
    public ActionResult<CustomerDto> Update(int id, UpdateCustomerRequest request)
    {
        if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name cannot be empty.");
            return ValidationProblem(ModelState);
        }

        var customer = updateCustomer.Update(id, request.Name, request.FirstName, request.Email, request.Address, request.IsActive);
        return customer is null ? NotFound() : Ok(customer.ToDto());
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) =>
        deleteCustomer.Delete(id) ? NoContent() : NotFound();
}
