using Application.DTO;
using Application.IService;
using Domain.Entities;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class MeetingCreatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallAddMeetingReferenceAsync_WithCorrectData()
    {
        // arrange
        var serviceDouble = new Mock<IMeetingService>();
        var consumer = new MeetingCreatedConsumer(serviceDouble.Object);

        var collabIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };


        var message = new MeetingCreatedMessage(Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddDays(1)), It.IsAny<string>(), Guid.NewGuid(), collabIds);

        var context = Mock.Of<ConsumeContext<MeetingCreatedMessage>>(m => m.Message == message);
        var reference = new MeetingReference(message.Id, message.Period, message.Mode, message.LocationId);

        // act
        await consumer.Consume(context);

        // assert
        serviceDouble.Verify(s => s.AddMeetingReferenceAsync(reference, collabIds), Times.Once);
    }
}
