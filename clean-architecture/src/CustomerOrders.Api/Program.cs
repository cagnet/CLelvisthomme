using CustomerOrders.Api.Middleware;
using CustomerOrders.Core.Ports;
using CustomerOrders.Core.Services;
using CustomerOrders.Core.UseCases.Customers;
using CustomerOrders.Core.UseCases.Orders;
using CustomerOrders.Infrastructure.Persistence;
using CustomerOrders.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Customer Orders API — Clean Architecture", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("CustomerOrders")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:CustomerOrders.");
builder.Services.AddDbContext<CustomerOrdersDbContext>(options => options.UseSqlServer(connectionString));

// Ports -> Infrastructure adapters.
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// UseCases -> Core services. Both services below implement several fine-grained
// interfaces each, so every UseCase is wired to the same instance per resource.
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IGetCustomerUseCase>(sp => sp.GetRequiredService<CustomerService>());
builder.Services.AddScoped<ICreateCustomerUseCase>(sp => sp.GetRequiredService<CustomerService>());
builder.Services.AddScoped<IUpdateCustomerUseCase>(sp => sp.GetRequiredService<CustomerService>());
builder.Services.AddScoped<IDeleteCustomerUseCase>(sp => sp.GetRequiredService<CustomerService>());

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IGetOrderUseCase>(sp => sp.GetRequiredService<OrderService>());
builder.Services.AddScoped<ICreateOrderUseCase>(sp => sp.GetRequiredService<OrderService>());
builder.Services.AddScoped<IUpdateOrderUseCase>(sp => sp.GetRequiredService<OrderService>());
builder.Services.AddScoped<IDeleteOrderUseCase>(sp => sp.GetRequiredService<OrderService>());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CustomerOrdersDbContext>().Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Orders API v1");
    options.RoutePrefix = "swagger";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();

public partial class Program;
