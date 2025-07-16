using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests.MeetingServiceTests;

public class MeetingServiceCreateTests : MeetingServiceTestBase
{
    [Fact]
    public async Task Create_ShouldReturnSuccessResult_WhenValidDataProvided()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        var locationId = Guid.NewGuid();
        var collab1 = Guid.NewGuid();
        var collab2 = Guid.NewGuid();
        var collabList = new List<Guid> { collab1, collab2 };
        var mode = "Online";
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };

        var createDto = new CreateMeetingDTO(period, mode, locationId, collabList);

        var meetingDouble = new Mock<IMeeting>();
        meetingDouble.Setup(m => m.Id).Returns(meetingId);
        meetingDouble.Setup(m => m.Period).Returns(period);
        meetingDouble.Setup(m => m.Mode).Returns(mode);
        meetingDouble.Setup(m => m.LocationId).Returns(locationId);


        var association1Double = new Mock<IAssociationMeetingCollab>();
        var association2Double = new Mock<IAssociationMeetingCollab>();


        _meetingFactoryDouble.Setup(f => f.Create(period, mode, locationId)).Returns(meetingDouble.Object);

        _associationFactoryDouble.Setup(f => f.Create(meetingDouble.Object, collab1)).ReturnsAsync(association1Double.Object);

        _associationFactoryDouble.Setup(f => f.Create(meetingDouble.Object, collab2)).ReturnsAsync(association2Double.Object);

        // Act
        var result = await MeetingService.Create(createDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(period, result.Value.MeetingPeriod);
        Assert.Equal(mode, result.Value.Mode);
        Assert.Equal(locationId, result.Value.LocationId);
        Assert.Equal(collabList, result.Value.Participants);

        _meetingRepositoryDouble.Verify(m => m.Add(meetingDouble.Object), Times.Once);
        _associationRepositoryDouble.Verify(a => a.AddRange(It.Is<List<IAssociationMeetingCollab>>(l => l.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldReturnFailureResult_WhenExceptionIsThrown()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var collab1 = Guid.NewGuid();
        var collabList = new List<Guid> { collab1 };
        var mode = "Online";
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };

        var createDto = new CreateMeetingDTO(period, mode, locationId, collabList);

        var meetingDouble = new Mock<IMeeting>();
        meetingDouble.Setup(m => m.Period).Returns(period);
        meetingDouble.Setup(m => m.Mode).Returns(mode);
        meetingDouble.Setup(m => m.LocationId).Returns(locationId);

        _meetingFactoryDouble.Setup(f => f.Create(period, mode, locationId)).Returns(meetingDouble.Object);

        _associationFactoryDouble
            .Setup(f => f.Create(meetingDouble.Object, collab1))
            .ThrowsAsync(new Exception("Error creating association"));

        // Act
        var result = await MeetingService.Create(createDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Contains("Error creating association", result.Error.Message);
    }

}
