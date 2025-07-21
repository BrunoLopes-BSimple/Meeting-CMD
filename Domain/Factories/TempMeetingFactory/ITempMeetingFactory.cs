using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factories.TempMeetingFactory;

public interface ITempMeetingFactory
{
    public IMeetingWithouLocation Create(IMeetingWithouLocationVisitor visitor);
}
