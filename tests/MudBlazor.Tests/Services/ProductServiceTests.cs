using Microsoft.Extensions.Configuration;

using app.Services;
using app.Models;

using MudBlazor.Tests.Fixtures;

namespace MudBlazor.Tests.Services;


public class ProductServiceTests : IClassFixture<DbFixture>
{
    DbFixture fixture;

    public ProductServiceTests(DbFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void GetDbRecords_Test()
    {
        Assert.NotNull(fixture.db);
        Assert.NotEmpty(fixture.db.Products);
        fixture.db.Products.Should().NotBeEmpty();
        fixture.db.Products.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsProducts()
    {
        // Arrange
        var expectedProducts = fixture.db.Products;
        var productService = new ProductService(fixture.db);

        // Act
        var actualProducts = await productService.GetProductsAsync();

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsAddedProduct()
    {
        // Arrange
        var expectedProduct = fixture.db.Products.First();
        var productService = new ProductService(fixture.db);

        // Act
        var actualProduct = await productService.GetProductAsync(expectedProduct.Id);

        // Assert
        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task AddProductAsync_ReturnsAddedProduct()
    {
        // Arrange
        var productToAdd = new Product { Name = "New Product" };
        var addedProduct = new Product { Id = 4, Name = "New Product", Description = null, Price = 0};
        var productService = new ProductService(fixture.db);

        // Act
        var actualProduct = await productService.AddProductAsync(productToAdd);

        // Assert
        actualProduct.Should().BeEquivalentTo(addedProduct);
    }
}
