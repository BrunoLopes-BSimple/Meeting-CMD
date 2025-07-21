using Domain.Contracts;

namespace Application.ISender;

public interface IMessageSender
{
    public Task SendMeetingCreationCommandAsync(CreateMeetingCommand message);
    public Task SendDataForMeetingCommandAsync(CreateMeetingCommand message);
}
