using Microsoft.Extensions.Configuration;

using app.Services;
using app.Models;

using MudBlazor.Tests.Fixtures;

namespace MudBlazor.Tests.Services;


//using TimeEdit_Skemaoensker.Models_KPSKEMAPP;

//using TimeEditSkemaoensker.IntegrationTests.Fixtures;

//namespace TimeEditSkemaoensker.IntegrationTests;



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
        fixture.db.Products.Should().HaveCountGreaterThan(50);
    }

    [Fact]
    public void GetProductsAsync_ReturnsProducts()
    {
        // Arrange
        var expectedProducts = fixture.db.Products;

        var productService = new ProductService(fixture.db);

        // Act
        var actualProducts = productService.GetProductsAsync().Result;

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public void GetProductAsync_ReturnsAddedProduct()
    {
        // Arrange
        var expectedProduct = fixture.db.Products.First();

        var productService = new ProductService(fixture.db);

        // Act
        var actualProduct = productService.GetProductAsync(expectedProduct.Id).Result;

        // Assert
        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public void AddProductAsync_ReturnsAddedProduct()
    {
        // Arrange
        var productToAdd = new Product { Name = "New Product" };
        var addedProduct = new Product { Id = 101, Name = "New Product" };

        var productService = new ProductService(fixture.db);

        // Act
        var actualProduct = productService.AddProductAsync(productToAdd).Result;

        // Assert
        actualProduct.Should().BeEquivalentTo(addedProduct);
    }
}
