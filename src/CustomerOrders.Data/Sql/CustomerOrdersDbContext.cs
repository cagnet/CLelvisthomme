using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Data.Sql;

public sealed class CustomerOrdersDbContext(DbContextOptions<CustomerOrdersDbContext> options) : DbContext(options)
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    // Fluent API only — no attributes on the entities, so CustomerEntity/OrderEntity
    // stay plain classes with zero dependency on EF Core beyond this file.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.FirstName).HasMaxLength(100);
            entity.Property(c => c.Email).HasMaxLength(254);
            entity.Property(c => c.Address).HasMaxLength(254);
            entity.Property(c => c.IsActive).IsRequired();
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.HasIndex(o => o.CustomerId).HasDatabaseName("IX_Order_CustomerId");
            entity.HasOne<CustomerEntity>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .HasConstraintName("FK_Order_Customer");
            // The "active customer" business rule stays in the application layer:
            // a FK/CHECK constraint cannot reliably express a condition on Customer.IsActive.
        });
    }
}
