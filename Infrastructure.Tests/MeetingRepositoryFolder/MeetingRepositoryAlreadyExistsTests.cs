using Domain.Entities;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.MeetingRepositoryFolder;

public class MeetingRepositoryAlreadyExistsTests : RepositoryTestBase
{
    [Fact]
    public async Task AlreadyExists_WhenMeetingExists_ShouldReturnTrue()
    {
        // arrange
        var meetingId = Guid.NewGuid();
        var meetingData = new MeetingDataModel
        {
            Id = meetingId,
            Period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) },
            Mode = "Online",
            LocationId = Guid.NewGuid()
        };


        context.Meetings.Add(meetingData);
        await context.SaveChangesAsync();

        var repository = new MeetingRepository(context, _mapper.Object);

        // act
        var result = await repository.AlreadyExistsAsync(meetingId);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task AlreadyExists_WhenMeetingDoesNotExist_ShouldReturnFalse()
    {
        // arrange
        var nonExistentId = Guid.NewGuid();
        var repository = new MeetingRepository(context, _mapper.Object);

        // act
        var result = await repository.AlreadyExistsAsync(nonExistentId);

        // assert
        Assert.False(result);
    }
}
