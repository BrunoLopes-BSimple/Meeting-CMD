using Domain.Entities;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.MeetingRepositoryFolder;

public class MeetingRepository_GetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnMeeting_WhenExists()
    {
        // Arrange
        var meetingId = Guid.NewGuid();

        var meetingData = new MeetingDataModel
        {
            Id = meetingId,
            Period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) },
            Mode = "Hybrid",
            LocationId = Guid.NewGuid()
        };

        context.Set<MeetingDataModel>().Add(meetingData);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<Meeting>(It.IsAny<MeetingDataModel>()))
            .Returns((MeetingDataModel dm) => new Meeting(dm.Id, dm.Period, dm.Mode, dm.LocationId));

        var repo = new MeetingRepository(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(meetingId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(meetingId, result!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var repo = new MeetingRepository(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}
