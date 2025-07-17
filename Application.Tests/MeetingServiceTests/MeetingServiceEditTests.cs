using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests.MeetingServiceTests;

public class MeetingServiceEditTests : MeetingServiceTestBase
{
    [Fact]
    public async Task EditMeeting_ShouldReturnSuccessResult_WhenValidDataProvided()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        var locationId = Guid.NewGuid();
        var collab1 = Guid.NewGuid();
        var collab2 = Guid.NewGuid();
        var collabList = new List<Guid> { collab1, collab2 };
        var mode = "Online";
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };

        var dto = new EditMeetingDTO(meetingId, period, mode, locationId, collabList);

        var meetingDouble = new Mock<IMeeting>();
        meetingDouble.Setup(m => m.Id).Returns(meetingId);
        meetingDouble.Setup(m => m.Period).Returns(period);
        meetingDouble.Setup(m => m.Mode).Returns(mode);
        meetingDouble.Setup(m => m.LocationId).Returns(locationId);

        _meetingRepositoryDouble.Setup(r => r.GetByIdAsync(meetingId)).ReturnsAsync(meetingDouble.Object);
        _associationRepositoryDouble.Setup(r => r.RemoveCollaboratorsFromMeeting(meetingId)).Returns(Task.CompletedTask);

        var association1 = new Mock<IAssociationMeetingCollab>();
        var association2 = new Mock<IAssociationMeetingCollab>();
        association1.Setup(a => a.CollabId).Returns(collab1);
        association2.Setup(a => a.CollabId).Returns(collab2);

        _associationFactoryDouble.Setup(f => f.Create(meetingDouble.Object, collab1)).ReturnsAsync(association1.Object);
        _associationFactoryDouble.Setup(f => f.Create(meetingDouble.Object, collab2)).ReturnsAsync(association2.Object);

        // Act
        var result = await MeetingService.EditMeeting(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(meetingId, result.Value.MeetingId);
        Assert.Equal(mode, result.Value.Mode);
        Assert.Equal(locationId, result.Value.LocationId);
        Assert.Equal(period, result.Value.Period);
        Assert.Equal(collabList, result.Value.CollaboratorIds);

        _meetingRepositoryDouble.Verify(r => r.UpdateMeeting(meetingDouble.Object), Times.Once);
        _associationRepositoryDouble.Verify(r => r.RemoveCollaboratorsFromMeeting(meetingId), Times.Once);
        _associationRepositoryDouble.Verify(r => r.AddRange(It.Is<List<IAssociationMeetingCollab>>(l => l.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task EditMeeting_ShouldReturnFailure_WhenMeetingNotFound()
    {
        // Arrange
        var dto = new EditMeetingDTO(Guid.NewGuid(), new PeriodDateTime(), "Online", Guid.NewGuid(), new List<Guid>());
        
        _meetingRepositoryDouble.Setup(r => r.GetByIdAsync(dto.MeetingId)).ReturnsAsync((IMeeting)null);

        // Act
        var result = await MeetingService.EditMeeting(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Meeting Not Found.", result.Error.Message);
    }

    [Fact]
    public async Task EditMeeting_ShouldReturnFailure_WhenModeIsInvalid()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        var meetingDouble = new Mock<IMeeting>();
        _meetingRepositoryDouble.Setup(r => r.GetByIdAsync(meetingId)).ReturnsAsync(meetingDouble.Object);

        var dto = new EditMeetingDTO(meetingId, new PeriodDateTime(), "InvalidMode", Guid.NewGuid(), new List<Guid>());

        // Act
        var result = await MeetingService.EditMeeting(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Mode does not exist", result.Error.Message);
    }

    [Fact]
    public async Task EditMeeting_ShouldReturnFailure_WhenAssociationCreationFails()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var period = new PeriodDateTime();
        var locationId = Guid.NewGuid();

        var dto = new EditMeetingDTO(meetingId, period, "Online", locationId, new List<Guid> { collabId });

        var meetingDouble = new Mock<IMeeting>();
        _meetingRepositoryDouble.Setup(r => r.GetByIdAsync(meetingId)).ReturnsAsync(meetingDouble.Object);
        _associationRepositoryDouble.Setup(r => r.RemoveCollaboratorsFromMeeting(meetingId)).Returns(Task.CompletedTask);

        _associationFactoryDouble.Setup(f => f.Create(meetingDouble.Object, collabId)).ThrowsAsync(new Exception("Error creating association"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => MeetingService.EditMeeting(dto));
    }

}
