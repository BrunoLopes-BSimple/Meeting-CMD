using Domain.Entities;
using Moq;

namespace Domain.Tests.MeetingTests;

public class MeetingConstructorTests
{
    [Fact]
    public void Constructor_ShouldCreateMeeting()
    {
        new Meeting(Guid.NewGuid(), It.IsAny<PeriodDateTime>(), It.IsAny<string>(), Guid.NewGuid());
    }
}
