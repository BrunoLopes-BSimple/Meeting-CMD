using Domain.Entities;

namespace Domain.Tests.LocationTests;

public class LocationConstructorTests
{
    [Fact]
    public void Location_WhenCreatingLocationWithValidData_ThenCreatesLocation()
    {
        // arrange
        var id = Guid.NewGuid();

        // act

        // assert
        new Location(id);
    }
}
