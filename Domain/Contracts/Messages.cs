using Domain.Entities;

namespace Domain.Contracts;

public record CreateMeetingCommand(Guid MeetingId, PeriodDateTime MeetingPeriod, string Mode, IEnumerable<Guid> Participants, string Description);

public record LocationCreatedForMeeting(Guid LocationId, Guid MeetingId);

public record DataForLocation(Guid meetingId, string description);
