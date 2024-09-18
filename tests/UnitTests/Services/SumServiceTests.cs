using app.Services;

namespace UnitTests.Services;

public class SumServiceTests
{
    [Fact]
    public void SumService_ReturnsCorrectSum()
    {
        // Arrange
        var calculator = Substitute.For<ISumService>();
        calculator.CalculateSum(2, 3).Returns(5);
        var sumService = new SumService();

        // Act
        var result = sumService.CalculateSum(2, 3);

        // Assert
        Assert.Equal(5, result);
    }
}
