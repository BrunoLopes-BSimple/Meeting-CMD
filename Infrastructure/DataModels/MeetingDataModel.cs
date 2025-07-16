using Domain.Entities;
using Domain.Visitor;

namespace Infrastructure.DataModels;

public class MeetingDataModel : IMeetingVisitor
{
    public Guid Id { get; set; }

    public PeriodDateTime Period { get; set; }

    public string Mode { get; set; }
    public Guid LocationId { get; set; }

    public MeetingDataModel() { }

    public MeetingDataModel(Guid id, PeriodDateTime period, string mode, Guid locationId)
    {
        Id = id;
        Period = period;
        Mode = mode;
        LocationId = locationId;
    }
}
