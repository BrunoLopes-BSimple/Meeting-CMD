using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.IRepository;

public interface IAssociationMCRepository : IGenericRepositoryEF<IAssociationMeetingCollab, AssociationMeetingCollab, IAssociationMCVisitor>
{
    Task<IEnumerable<IAssociationMeetingCollab>> GetByCollabIdAsync(Guid collabId);
    public Task RemoveCollaboratorsFromMeeting(Guid meetingId);
}
