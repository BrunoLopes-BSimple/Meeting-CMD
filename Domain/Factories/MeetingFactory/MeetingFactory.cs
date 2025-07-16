using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories.MeetingFactory;

public class MeetingFactory : IMeetingFactory
{
    public IMeeting Create(PeriodDateTime period, string mode, Guid locationId)
    {
        if (string.IsNullOrWhiteSpace(mode))
        {
            throw new ArgumentException("No mode was defined.");
        }

        var validModes = new[] { "Online", "Hybrid", "OnSite" };
        if (validModes.Contains(mode))
        {
            return new Meeting(Guid.NewGuid(), period, mode, locationId);
        }

        throw new ArgumentException("Mode defined does not exist.");
    }
}
