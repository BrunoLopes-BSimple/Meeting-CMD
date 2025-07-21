using Domain.Interfaces;

namespace Application.IPublisher;

public interface IMessagePublisher
{
    public Task PublishMeetingCreated(IMeeting meeting, IEnumerable<Guid> collabIds);
}
