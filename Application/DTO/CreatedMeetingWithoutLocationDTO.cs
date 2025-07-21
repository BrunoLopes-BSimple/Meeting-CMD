using Domain.Entities;

namespace Application.DTO;


public record CreatedMeetingWithoutLocationDTO(Guid id, PeriodDateTime MeetingPeriod, string Mode, IEnumerable<Guid> Participants, string Description);

