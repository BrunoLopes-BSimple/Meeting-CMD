using Domain.Entities;

namespace Application.DTO;

public record CreateMeetingWithoutLocationDTO(PeriodDateTime MeetingPeriod, string Mode, IEnumerable<Guid> Participants, string Description);

