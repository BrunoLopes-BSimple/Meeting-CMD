using Domain.Entities;

namespace Application.DTO;

public record CreateMeetingWithoutLocation(Guid MeetingId, PeriodDateTime MeetingPeriod, string Mode, IEnumerable<Guid> Participants, string Description);

