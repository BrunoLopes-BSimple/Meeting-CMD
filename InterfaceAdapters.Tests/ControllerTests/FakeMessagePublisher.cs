using Application.IPublisher;
using Domain.Interfaces;

namespace InterfaceAdapters.Tests.ControllerTests;

public class FakeMessagePublisher : IMessagePublisher
{
    public Task PublishMeetingCreated(IMeeting meeting)
    {
        return Task.CompletedTask;
    }
}