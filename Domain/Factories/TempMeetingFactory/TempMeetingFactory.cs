using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factories.TempMeetingFactory;

public class TempMeetingFactory : ITempMeetingFactory
{
    public TempMeetingFactory()
    {
    }

    public IMeetingWithouLocation Create(IMeetingWithouLocationVisitor visitor)
    {
        return new MeetingWithoutLocation(visitor.Id, visitor.Period, visitor.Mode, visitor.Participants, visitor.Description);
    }
}
