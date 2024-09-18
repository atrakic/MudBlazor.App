using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using app.Core.Model;

namespace app.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString = default!;
    public static bool IsSqlServer = true;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Debug.WriteLine($"{ContextId} context created.");
    }
    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Custom table configurations due to: "SQLite Error 1: 'near "MAX": syntax error'"
        modelBuilder.Entity<Customer>(entity =>
        {
            if (!IsSqlServer)
            {
                entity.ToTable("Customers");
                foreach (var property in entity.Metadata.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetColumnType("text");
                    }
                }
            }
        });
    }
}
