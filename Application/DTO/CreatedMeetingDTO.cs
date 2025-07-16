using Domain.Entities;

namespace Application.DTO
{
    public record CreatedMeetingDTO(Guid MeetingId, PeriodDateTime MeetingPeriod, string Mode, IEnumerable<Guid> Participants, Guid LocationId);
}