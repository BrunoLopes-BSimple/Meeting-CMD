using Domain.Contracts;
using InterfaceAdapters.Activities;
using MassTransit;

namespace InterfaceAdapters.Saga;

public class MeetingCreationSagaStateMachine : MassTransitStateMachine<MeetingCreationSagaState>
{
    public State PendingLocationCreation { get; private set; } = null!;

    public Event<CreateMeetingCommand> MeetingCommandReceived { get; private set; } = null!;
    public Event<LocationCreatedForMeeting> LocationCreated { get; private set; } = null!;

    public MeetingCreationSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => MeetingCommandReceived, x =>
        {
            x.CorrelateBy((context, saga) => saga.Message.MeetingId == context.MeetingId);
            x.SelectId(context => Guid.NewGuid());
            x.InsertOnInitial = true;
        });

        Event(() => LocationCreated, x => x.CorrelateBy((context, saga) => saga.Message.MeetingId == context.MeetingId));

        Initially(
            When(MeetingCommandReceived)
            .Activity(x => x.OfType<CreateTempMeetingActivity>())
            .TransitionTo(PendingLocationCreation)
        );

        During(PendingLocationCreation,
            When(LocationCreated)
            .Activity(x => x.OfType<FinalizeMeetingActivity>())
            .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
