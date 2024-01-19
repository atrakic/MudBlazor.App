using Microsoft.EntityFrameworkCore;

using app.Data;

namespace app.Models;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context is null)
        {
            throw new System.ArgumentNullException(nameof(context));
        }

        if (context.Customers.Any())
        {
            return; // DB has been seeded
        }

        Console.WriteLine("Seeding the database.");

        context.Customers.AddRange(
            new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "foo@bar.com"
            }
        );
        context.SaveChanges();

        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Product 1",
                Price = 10.00m
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Price = 20.00m
            },
            new Product
            {
                Id = 3,
                Name = "Product 3",
                Price = 30.00m
            }
        );

        /**
        context.Orders.AddRange(
            new Order
            {
                CustomerId = 1,
                OrderPlaced = DateTime.UtcNow, // (0x80131904): Operand type clash: datetime2 is incompatible with text
            }
        );
        */

        context.SaveChanges();
    }
}
