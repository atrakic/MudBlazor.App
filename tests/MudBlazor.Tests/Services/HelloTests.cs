using app.Services;

namespace MudBlazor.Tests.Services;

public class HelloTests
{
    [Fact]
    public void HelloService_Test()
    {
        // Arrange
        var helloService = new HelloService();

        // Act
        var message = helloService.GetMessage();

        // Assert
        Assert.Equal("Hello from HelloService Service!", message);
    }
}
