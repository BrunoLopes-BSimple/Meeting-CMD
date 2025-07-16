using Domain.Entities;
using Domain.Factories.AssociationFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Domain.Tests.AssociationMeetingCollaboratorTests;

public class AssociationMCFactoryTests
{
    private readonly Mock<IAssociationMCRepository> _associationRepoDouble;
    private readonly Mock<IMeetingRepository> _meetingRepoDouble;
    private readonly AssociationMCFactory _factory;

    public AssociationMCFactoryTests()
    {
        _associationRepoDouble = new Mock<IAssociationMCRepository>();
        _meetingRepoDouble = new Mock<IMeetingRepository>();
        _factory = new AssociationMCFactory(_associationRepoDouble.Object, _meetingRepoDouble.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnNewAssociation_WhenNoExistingAssociations()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var meeting = new Mock<IMeeting>();
        meeting.Setup(m => m.Id).Returns(Guid.NewGuid());
        meeting.Setup(m => m.Period).Returns(new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) });

        _associationRepoDouble.Setup(r => r.GetByCollabIdAsync(collabId)).ReturnsAsync(new List<IAssociationMeetingCollab>());

        // Act
        var result = await _factory.Create(meeting.Object, collabId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(collabId, result.CollabId);
        Assert.Equal(meeting.Object.Id, result.MeetingId);
    }

    [Fact]
    public async Task Create_ShouldThrowInvalidOperationException_WhenScheduleConflict()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var newMeetingPeriod = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };

        var newMeeting = new Mock<IMeeting>();
        newMeeting.Setup(m => m.Id).Returns(Guid.NewGuid());
        newMeeting.Setup(m => m.Period).Returns(newMeetingPeriod);

        var existingMeetingId = Guid.NewGuid();
        var existingAssociation = new Mock<IAssociationMeetingCollab>();
        existingAssociation.Setup(a => a.MeetingId).Returns(existingMeetingId);

        _associationRepoDouble.Setup(r => r.GetByCollabIdAsync(collabId)).ReturnsAsync(new List<IAssociationMeetingCollab> { existingAssociation.Object });

        var conflictingPeriod = new PeriodDateTime { _initDate = newMeetingPeriod._initDate.AddMinutes(-10), _finalDate = newMeetingPeriod._finalDate.AddMinutes(10) };

        var existingMeeting = new Mock<IMeeting>();
        existingMeeting.Setup(m => m.Id).Returns(existingMeetingId);
        existingMeeting.Setup(m => m.Period).Returns(conflictingPeriod);

        _meetingRepoDouble.Setup(r => r.GetMeetingsByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<IMeeting> { existingMeeting.Object });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _factory.Create(newMeeting.Object, collabId));

        Assert.Contains("has a schedule conflict", exception.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnNewAssociation_WhenNoScheduleConflict()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var newMeetingPeriod = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };

        var newMeeting = new Mock<IMeeting>();
        newMeeting.Setup(m => m.Id).Returns(Guid.NewGuid());
        newMeeting.Setup(m => m.Period).Returns(newMeetingPeriod);

        var existingMeetingId = Guid.NewGuid();
        var existingAssociation = new Mock<IAssociationMeetingCollab>();
        existingAssociation.Setup(a => a.MeetingId).Returns(existingMeetingId);

        _associationRepoDouble.Setup(r => r.GetByCollabIdAsync(collabId)).ReturnsAsync(new List<IAssociationMeetingCollab> { existingAssociation.Object });

        var nonConflictingPeriod = new PeriodDateTime { _initDate = newMeetingPeriod._finalDate.AddMinutes(1), _finalDate = newMeetingPeriod._finalDate.AddHours(1) };

        var existingMeeting = new Mock<IMeeting>();
        existingMeeting.Setup(m => m.Id).Returns(existingMeetingId);
        existingMeeting.Setup(m => m.Period).Returns(nonConflictingPeriod);

        _meetingRepoDouble.Setup(r => r.GetMeetingsByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<IMeeting> { existingMeeting.Object });

        // Act
        var result = await _factory.Create(newMeeting.Object, collabId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(collabId, result.CollabId);
        Assert.Equal(newMeeting.Object.Id, result.MeetingId);
    }
}

