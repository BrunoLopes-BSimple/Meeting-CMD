using Domain.Entities;

namespace Domain.Messages;

public record MeetingCreatedMessage(Guid Id, PeriodDateTime Period, string Mode, Guid LocationId);
