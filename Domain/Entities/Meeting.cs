using Domain.Interfaces;

namespace Domain.Entities;

public class Meeting : IMeeting
{
    public Guid Id { get; private set; }

    public PeriodDateTime Period { get; private set; }

    public string Mode { get; private set; }

    public Guid LocationId { get; private set; }

    public Meeting(Guid id, PeriodDateTime period, string mode, Guid locationId)
    {
        Id = id;
        Period = period;
        Mode = mode;
        LocationId = locationId;
    }

    public void UpdatePeriod(PeriodDateTime period)
    {
        this.Period = period;
    }

    public void UpdateMode(string mode)
    {
        this.Mode = mode;
    }

    public void UpdateLocationId(Guid locationId)
    {
        this.LocationId = locationId;
    }
}
