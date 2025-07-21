using Application.ISender;
using Domain.Contracts;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Sender;

public class MassTransitSender : IMessageSender
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitSender(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task SendMeetingCreationCommandAsync(CreateMeetingCommand message)
    {
        var endpoit = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:meeting-creation-saga"));
        await endpoit.Send(message);
    }

    public async Task SendDataForMeetingCommandAsync(CreateMeetingCommand message)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:data-for-location"));
        await endpoint.Send(new DataForLocation(message.MeetingId, message.Description));
    }
}
