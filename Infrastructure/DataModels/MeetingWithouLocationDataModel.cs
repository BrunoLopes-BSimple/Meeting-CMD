using Domain.Entities;
using Domain.Visitor;

namespace Infrastructure.DataModels;

public class MeetingWithouLocationDataModel : IMeetingWithouLocationVisitor
{
    public Guid Id { get; set; }
    public PeriodDateTime Period { get; set; }
    public string Mode { get; set; }
    public IEnumerable<Guid> Participants { get; set; }
    public string Description { get; set; }

    public MeetingWithouLocationDataModel() { }

    public MeetingWithouLocationDataModel(Guid id, PeriodDateTime period, string mode, IEnumerable<Guid> participants, string description)
    {
        Id = id;
        Period = period;
        Mode = mode;
        Participants = participants;
        Description = description;
    }
}
