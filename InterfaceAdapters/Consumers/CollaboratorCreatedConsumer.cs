using Application.IService;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class CollaboratorCreatedConsumer : IConsumer<CollaboratorCreatedMessage>
{
    private readonly ICollaboratorService _collabService;

    public CollaboratorCreatedConsumer(ICollaboratorService collabService)
    {
        _collabService = collabService;
    }

    public async Task Consume(ConsumeContext<CollaboratorCreatedMessage> context)
    {
        await _collabService.AddCollaboratorReferenceAsync(context.Message.Id);
    }
}
