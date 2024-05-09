using Microsoft.EntityFrameworkCore;

using app.Data;
using app.Models;

namespace MudBlazor.Tests.Fixtures;

public class DbFixture : IDisposable
{
    public ApplicationDbContext db { get; private set; }
    private int MAX_RECORDS = 100;

    public DbFixture()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TEST" + DateTime.Now.ToFileTimeUtc())
            .Options;

        db = new ApplicationDbContext(dbOptions);
        db.Database.EnsureCreated();
        SeedData();
    }

    public void SeedData()
    {
        var products = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
            .Generate(MAX_RECORDS);
        db.Products.AddRange(products);

        var customers = new Faker<Customer>()
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .Generate(MAX_RECORDS);
        db.Customers.AddRange(customers);

        var orders = new Faker<Order>()
            .RuleFor(o => o.OrderPlaced, f => f.Date.Past())
            .RuleFor(o => o.OrderFulfilled, f => f.Date.Future())
            .RuleFor(o => o.CustomerId, f => f.Random.Number(1, 100))
            .Generate(MAX_RECORDS);
        db.Orders.AddRange(orders);

        var orderDetails = new Faker<OrderDetail>()
            .RuleFor(od => od.Quantity, f => f.Random.Number(1, 10))
            .RuleFor(od => od.ProductId, f => f.Random.Number(1, 100))
            .RuleFor(od => od.OrderId, f => f.Random.Number(1, 100))
            .Generate(100);
        db.OrderDetails.AddRange(orderDetails);

        db.SaveChanges();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
        db.Dispose();
        GC.SuppressFinalize(this);
    }
}
