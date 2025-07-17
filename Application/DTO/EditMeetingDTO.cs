using Domain.Entities;

namespace Application.DTO;

public record EditMeetingDTO(Guid MeetingId, PeriodDateTime Period, string Mode, Guid LocationId, IEnumerable<Guid> CollaboratorIds);