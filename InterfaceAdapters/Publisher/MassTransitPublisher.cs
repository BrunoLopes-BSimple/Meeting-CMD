using Application.IPublisher;
using Domain.Interfaces;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Publisher;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishMeetingCreated(IMeeting meeting, IEnumerable<Guid> collabIds)
    {
        var eventMessage = new MeetingCreatedMessage(meeting.Id, meeting.Period, meeting.Mode, meeting.LocationId, collabIds);
        await _publishEndpoint.Publish(eventMessage);
    }
}
