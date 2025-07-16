using Domain.Entities;
using Domain.Interfaces;
using Domain.Messages;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.PublisherTests;

public class PublishMeetingTests
{
    [Fact]
    public async Task PublishMeetingCreatedAsync_ShouldPublishEventWithCorrectData()
    {
        // Arrange 
        var publishEndpointDouble = new Mock<IPublishEndpoint>();

        var publisher = new MassTransitPublisher(publishEndpointDouble.Object);

        var meetingDouble = new Mock<IMeeting>();
        var meetingId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));
        var mode = "Online";
        var locationId = Guid.NewGuid();

        meetingDouble.Setup(c => c.Id).Returns(meetingId);
        meetingDouble.Setup(c => c.Period).Returns(period);
        meetingDouble.Setup(c => c.Mode).Returns(mode);
        meetingDouble.Setup(c => c.LocationId).Returns(locationId);

        // Act 
        await publisher.PublishMeetingCreated(meetingDouble.Object);

        // Assert
        publishEndpointDouble.Verify(
            p => p.Publish(
                It.Is<MeetingCreatedMessage>(e =>
                    e.Id == meetingId &&
                    e.Period == period &&
                    e.Mode == mode &&
                    e.LocationId == locationId
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

}
