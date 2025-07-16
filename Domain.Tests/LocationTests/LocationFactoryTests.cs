using Domain.Factories.LocationFactory;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.LocationTests;

public class LocationFactoryTests
{
    [Fact]
    public void LocationFactory_WhenPassingValidId_ShouldCreateLocation()
    {
        // arrange
        var id = Guid.NewGuid();
        var factory = new LocationFactory();

        // act
        var result = factory.Create(id);

        // assert
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}
