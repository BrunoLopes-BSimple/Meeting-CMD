using Application.ISender;
using Application.IService;
using Domain.Contracts;
using InterfaceAdapters.Saga;
using MassTransit;

namespace InterfaceAdapters.Activities;

public class CreateTempMeetingActivity : IStateMachineActivity<MeetingCreationSagaState, CreateMeetingCommand>
{
    private readonly IMessageSender _messageSender;
    private readonly IMeetingTempService _meetingTempService;

    public CreateTempMeetingActivity(IMessageSender messageSender, IMeetingTempService meetingTempService)
    {
        _messageSender = messageSender;
        _meetingTempService = meetingTempService;
    }

    public async Task Execute(BehaviorContext<MeetingCreationSagaState, CreateMeetingCommand> context, IBehavior<MeetingCreationSagaState, CreateMeetingCommand> next)
    {
        Console.WriteLine("Entrou na primeira activity");
        var msg = context.Message;

        context.Saga.MeetingId = msg.MeetingId;
        context.Saga.Period = msg.MeetingPeriod;
        context.Saga.Mode = msg.Mode;
        context.Saga.Description = msg.Description;
        context.Saga.ParticipantIds = msg.Participants;

        var tempMeeting = await _meetingTempService.Create(msg.MeetingId, msg.MeetingPeriod, msg.Mode, msg.Participants, msg.Description);

        var message = new CreateMeetingCommand(tempMeeting.id, tempMeeting.MeetingPeriod, tempMeeting.Mode, tempMeeting.Participants, tempMeeting.Description);

        await _messageSender.SendDataForMeetingCommandAsync(message);

        await next.Execute(context);
    }

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public Task Faulted<TException>(BehaviorExceptionContext<MeetingCreationSagaState, CreateMeetingCommand, TException> context, IBehavior<MeetingCreationSagaState, CreateMeetingCommand> next) where TException : Exception => next.Faulted(context);

    public void Probe(ProbeContext context) => context.CreateScope("create-temp-meeting");
}
