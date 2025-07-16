using Domain.Entities;

namespace Application.DTO;

    public record CreateMeetingDTO(
        PeriodDateTime Period,
        string Mode,
        Guid LocationId,
        IEnumerable<Guid> CollaboratorIds
    );
