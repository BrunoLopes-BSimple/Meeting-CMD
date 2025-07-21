using Application.IService;
using Domain.Contracts;
using InterfaceAdapters.Saga;
using MassTransit;

namespace InterfaceAdapters.Activities;

public class FinalizeMeetingActivity : IStateMachineActivity<MeetingCreationSagaState, LocationCreatedForMeeting>
{
    private readonly IMeetingService _meetingService;

    public FinalizeMeetingActivity(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task Execute(BehaviorContext<MeetingCreationSagaState, LocationCreatedForMeeting> context, IBehavior<MeetingCreationSagaState, LocationCreatedForMeeting> next)
    {
        Console.WriteLine("Entrou na segunda activity");
        await _meetingService.FinalizeAsync(context.Message.MeetingId, context.Saga.Period, context.Saga.Mode, context.Saga.ParticipantIds, context.Message.LocationId, context.Saga.Description);
    }

    public void Accept(StateMachineVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Task Faulted<TException>(BehaviorExceptionContext<MeetingCreationSagaState, LocationCreatedForMeeting, TException> context, IBehavior<MeetingCreationSagaState, LocationCreatedForMeeting> next) where TException : Exception
    {
        throw new NotImplementedException();
    }

    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }
}
