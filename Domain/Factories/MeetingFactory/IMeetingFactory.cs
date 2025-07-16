using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories.MeetingFactory;

public interface IMeetingFactory
{
    public IMeeting Create(PeriodDateTime period, string mode, Guid locationId);
    public IMeeting Create(Guid id, PeriodDateTime period, string mode, Guid locationId);
}
