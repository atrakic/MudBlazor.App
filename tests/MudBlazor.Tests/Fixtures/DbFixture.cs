using Microsoft.EntityFrameworkCore;

using app.Data;
using app.Models;

namespace MudBlazor.Tests.Fixtures;

public class DbFixture : IDisposable
{

    public int MAX_RECORDS = 3;

    public ApplicationDbContext db { get; private set; }

    public DbFixture()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(DbFixture) + DateTime.Now.ToFileTimeUtc())
            .Options;

        db = new ApplicationDbContext(dbOptions);
        db.Database.EnsureCreated();

        SeedData();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
        db.Dispose();
        GC.SuppressFinalize(this);
    }

    public void SeedData()
    {
        var faker = new Faker("en");
        var products = new List<Product>();
        for (int i = 0; i < MAX_RECORDS; i++)

        {
            var product = new Product
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = faker.Random.Decimal(1, 100)
            };
            products.Add(product);
        }
        db.Products.AddRange(products);

        var customers = new List<Customer>();
        for (int i = 0; i < MAX_RECORDS; i++)
        {
            var customer = new Customer
            {
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Email = faker.Internet.Email(),
                Phone = faker.Phone.PhoneNumber()
            };
            customers.Add(customer);
        }
        db.Customers.AddRange(customers);

        var orders = new List<Order>();
        for (int i = 0; i < MAX_RECORDS; i++)
        {
            var order = new Order
            {
                OrderPlaced = faker.Date.Past(),
                OrderFulfilled = faker.Date.Recent(),
                CustomerId = faker.Random.Int(1, MAX_RECORDS)
            };
            orders.Add(order);
        }
        db.Orders.AddRange(orders);

        var orderDetails = new List<OrderDetail>();
        for (int i = 0; i < MAX_RECORDS; i++)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = faker.Random.Int(1, MAX_RECORDS),
                ProductId = faker.Random.Int(1, MAX_RECORDS),
                Quantity = faker.Random.Int(1, 10)
            };
            orderDetails.Add(orderDetail);
        }
        db.SaveChanges();
    }
}
