using Domain.Entities;
using MassTransit;

namespace InterfaceAdapters.Saga;

public class MeetingCreationSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string? CurrentState { get; set; }

    public Guid MeetingId { get; set; }
    public PeriodDateTime Period { get; set; }
    public string Mode { get; set; }
    public IEnumerable<Guid> ParticipantIds { get; set; }
    public string Description { get; set; }
}
