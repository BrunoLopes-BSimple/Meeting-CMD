using Domain.Entities;

namespace Application.DTO;

public record MeetingReference(Guid Id, PeriodDateTime Period, string Mode, Guid LocationId);
