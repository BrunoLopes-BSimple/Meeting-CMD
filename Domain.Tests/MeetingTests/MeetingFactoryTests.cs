using Domain.Entities;
using Domain.Factories.MeetingFactory;

namespace Domain.Tests.MeetingTests;

public class MeetingFactoryTests
{
    private readonly MeetingFactory _factory = new();

    [Theory]
    [InlineData("Online")]
    [InlineData("Hybrid")]
    [InlineData("OnSite")]
    public void Create_ShouldReturnMeeting_WhenValidMode(string mode)
    {
        // Arrange
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };
        var locationId = Guid.NewGuid();

        // Act
        var meeting = _factory.Create(period, mode, locationId);

        // Assert
        Assert.NotNull(meeting);
        Assert.Equal(mode, meeting.Mode);
        Assert.Equal(locationId, meeting.LocationId);
        Assert.Equal(period, meeting.Period);
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenModeIsNullOrWhitespace()
    {
        // Arrange
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };
        var locationId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _factory.Create(period, null!, locationId));
        Assert.Throws<ArgumentException>(() => _factory.Create(period, "", locationId));
        Assert.Throws<ArgumentException>(() => _factory.Create(period, "   ", locationId));
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenModeIsInvalid()
    {
        // Arrange
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };
        var locationId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _factory.Create(period, "InvalidMode", locationId));
        Assert.Equal("Mode defined does not exist.", ex.Message);
    }
}
