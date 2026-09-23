using CustomerOrders.Business.Repositories;
using CustomerOrders.Business.Services;
using CustomerOrders.Data.InMemory;
using CustomerOrders.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Customer Orders API", Version = "v1" });
});
builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Orders API v1");
    options.RoutePrefix = "swagger";
});
app.MapControllers();
app.Run();
public partial class Program;
