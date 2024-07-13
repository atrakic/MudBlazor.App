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

    public void SeedData()
    {
        var faker = new Faker("en");

        var products = new List<Product>();
        var customers = new List<Customer>();
        var orders = new List<Order>();
        var orderDetails = new List<OrderDetail>();

        for (int i = 0; i < MAX_RECORDS; i++)
        {
            var product = new Product
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = faker.Random.Decimal(1, 100)
            };
            products.Add(product);

            var customer = new Customer
            {
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Email = faker.Internet.Email(),
                Phone = faker.Phone.PhoneNumber()
            };
            customers.Add(customer);

            var order = new Order
            {
                OrderPlaced = faker.Date.Past(),
                OrderFulfilled = faker.Date.Recent(),
                CustomerId = faker.Random.Int(1, MAX_RECORDS)
            };
            orders.Add(order);

            var orderDetail = new OrderDetail
            {
                OrderId = faker.Random.Int(1, MAX_RECORDS),
                ProductId = faker.Random.Int(1, MAX_RECORDS),
                Quantity = faker.Random.Int(1, 10)
            };
            orderDetails.Add(orderDetail);
        }
        db.Products.AddRange(products);
        db.Customers.AddRange(customers);
        db.Orders.AddRange(orders);
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
