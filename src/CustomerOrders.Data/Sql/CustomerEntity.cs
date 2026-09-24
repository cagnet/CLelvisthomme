namespace CustomerOrders.Data.Sql;
public class CustomerEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public required bool IsActive  { get; set; }
}
