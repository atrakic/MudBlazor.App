using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MudBlazor.App.Tests.IntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BasicTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/products")]
        [InlineData("/counter")]
        [InlineData("/weather")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType?.ToString());
        }
    }
}
