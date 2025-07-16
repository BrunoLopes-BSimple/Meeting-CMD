
using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepository;

namespace Domain.Factories.AssociationFactory;

public class AssociationMCFactory : IAssociationMCFactory
{
    private readonly IAssociationMCRepository _associationRepo;
    private readonly IMeetingRepository _meetingRepo;

    public AssociationMCFactory(IAssociationMCRepository associationRepo, IMeetingRepository meetingRepo)
    {
        _associationRepo = associationRepo;
        _meetingRepo = meetingRepo;
    }

    public async Task<IAssociationMeetingCollab> Create(IMeeting newMeeting, Guid collabId)
    {
        // todas as reuniões existentes para este colaborador
        var existingAssociations = await _associationRepo.GetByCollabIdAsync(collabId);
        if (!existingAssociations.Any())
        {
            // se não tem reuniões cria a associação diretamente
            return new AssociationMeetingCollab(Guid.NewGuid(), collabId, newMeeting.Id);
        }

        var existingMeetingIds = existingAssociations.Select(a => a.MeetingId);
        var existingMeetings = await _meetingRepo.GetMeetingsByIdsAsync(existingMeetingIds);

        foreach (var existingMeeting in existingMeetings)
        {
            if (newMeeting.Period.Intersects(existingMeeting.Period))
            {
                // caso exista sobreposicao de horarios
                throw new InvalidOperationException(
                    $"Collaborator with ID '{collabId}' has a schedule conflict with meeting ID '{existingMeeting.Id}'."
                );
            }
        }

        return new AssociationMeetingCollab(Guid.NewGuid(), collabId, newMeeting.Id);
    }
}
