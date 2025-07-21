using Domain.Entities;

namespace Domain.Interfaces;

public interface IMeetingWithouLocation
{
    public Guid Id { get; }
    public PeriodDateTime Period { get; }
    public string Mode { get; }
    public IEnumerable<Guid> Participants { get; }
    public string Description { get; }
}
