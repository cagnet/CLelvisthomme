using CustomerOrders.Business.Repositories;
using CustomerOrders.Business.Services;
using CustomerOrders.Data.InMemory;
using CustomerOrders.Data.Repositories;
using CustomerOrders.Data.Sql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Customer Orders API", Version = "v1" });
});

// Persistence backend switch: "InMemory" (default, no external dependency) or "Sql" (SQL Server via EF Core).
var persistence = builder.Configuration["Persistence"] ?? "InMemory";
if (persistence.Equals("Sql", StringComparison.OrdinalIgnoreCase))
{
    var connectionString = builder.Configuration.GetConnectionString("CustomerOrders")
        ?? throw new InvalidOperationException("Missing ConnectionStrings:CustomerOrders when Persistence=Sql.");
    builder.Services.AddDbContext<CustomerOrdersDbContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddScoped<ICustomerRepository, CustomerSqlRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderSqlRepository>();
}
else
{
    builder.Services.AddSingleton<InMemoryDatabase>();
    builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
    builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
}

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
var app = builder.Build();

if (persistence.Equals("Sql", StringComparison.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<CustomerOrdersDbContext>().Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Orders API v1");
    options.RoutePrefix = "swagger";
});
app.MapControllers();
app.Run();
public partial class Program;
