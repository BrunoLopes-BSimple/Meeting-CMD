using Domain.Interfaces;

namespace Domain.Entities;

public class MeetingWithoutLocation : IMeetingWithouLocation
{
    public Guid Id { get; private set; }
    public PeriodDateTime Period { get; private set; }
    public string Mode { get; private set; }
    public IEnumerable<Guid> Participants { get; private set; }
    public string Description { get; private set; }

    public MeetingWithoutLocation(Guid id, PeriodDateTime period, string mode, IEnumerable<Guid> participants, string description)
    {
        Id = id;
        Period = period;
        Mode = mode;
        Participants = participants;
        Description = description;
    }
}
