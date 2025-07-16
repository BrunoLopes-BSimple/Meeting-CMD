using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories.AssociationFactory;

public interface IAssociationMCFactory
{
    public Task<IAssociationMeetingCollab> Create(IMeeting newMeeting, Guid collabId ); 
}
