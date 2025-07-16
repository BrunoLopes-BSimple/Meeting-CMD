using Application.IService;
using Domain.Entities;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallAddCollaboratorReferenceAsync_WithCorrectData()
    {
        // arrange
        var serviceDouble = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(serviceDouble.Object);

        var message = new CollaboratorCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

        var context = Mock.Of<ConsumeContext<CollaboratorCreatedMessage>>(c => c.Message == message);

        // act
        await consumer.Consume(context);

        // asset
        serviceDouble.Verify(s => s.AddCollaboratorReferenceAsync(message.Id), Times.Once);
    }
}
