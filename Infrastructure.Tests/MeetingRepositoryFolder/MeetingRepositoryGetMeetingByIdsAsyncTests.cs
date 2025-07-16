using Domain.Entities;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.MeetingRepositoryFolder;

public class MeetingRepositoryGetMeetingByIdsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task GetMeetingsByIdsAsync_ShouldReturnMeetings_WhenExist()
    {
        // Arrange
        var meetingId = Guid.NewGuid();

        var meetingData = new MeetingDataModel
        {
            Id = meetingId,
            Period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) },
            Mode = "Online",
            LocationId = Guid.NewGuid()
        };

        context.Set<MeetingDataModel>().Add(meetingData);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<IEnumerable<Meeting>>(It.IsAny<IEnumerable<MeetingDataModel>>()))
            .Returns((IEnumerable<MeetingDataModel> dm) => dm.Select(d => new Meeting(d.Id, d.Period, d.Mode, d.LocationId)));

        var repo = new MeetingRepository(context, _mapper.Object);

        // Act
        var result = await repo.GetMeetingsByIdsAsync(new[] { meetingId });

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, m => m.Id == meetingId);
    }

    [Fact]
    public async Task GetMeetingsByIdsAsync_ShouldReturnEmpty_WhenNoneFound()
    {
        // Arrange
        var repo = new MeetingRepository(context, _mapper.Object);

        _mapper.Setup(m => m.Map<IEnumerable<Meeting>>(It.IsAny<IEnumerable<MeetingDataModel>>()))
            .Returns(Enumerable.Empty<Meeting>());

        // Act
        var result = await repo.GetMeetingsByIdsAsync(new[] { Guid.NewGuid() });

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
