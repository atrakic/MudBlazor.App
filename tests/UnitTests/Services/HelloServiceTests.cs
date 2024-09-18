using app.Services;

namespace UnitTests.Services;

public class HelloServiceTests
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
