using Domain.Entities;

namespace Domain.Interfaces;

public interface IMeeting
{
    public Guid Id { get; }
    public PeriodDateTime Period { get; }
    public string Mode { get; }
    public Guid LocationId { get; }

    public void UpdatePeriod(PeriodDateTime period);
    public void UpdateMode(string mode);
    public void UpdateLocationId(Guid locationId);
}
