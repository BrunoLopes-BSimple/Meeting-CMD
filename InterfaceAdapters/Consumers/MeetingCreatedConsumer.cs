using Application.DTO;
using Application.IService;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class MeetingCreatedConsumer : IConsumer<MeetingCreatedMessage>
{
    private readonly IMeetingService _meetingService;

    public MeetingCreatedConsumer(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task Consume(ConsumeContext<MeetingCreatedMessage> context)
    {
        var reference = new MeetingReference(context.Message.Id, context.Message.Period, context.Message.Mode, context.Message.LocationId);

        await _meetingService.AddMeetingReferenceAsync(reference);
    }
}