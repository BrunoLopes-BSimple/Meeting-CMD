using Domain.Visitor;

namespace Infrastructure.DataModels;

public class AssociationMCDataModel : IAssociationMCVisitor
{
    public Guid Id { get; set; }

    public Guid CollabId { get; set; }

    public Guid MeetingId { get; set; }

    public AssociationMCDataModel(Guid id, Guid collabId, Guid meetingId)
    {
        Id = id;
        CollabId = collabId;
        MeetingId = meetingId;
    }

    public AssociationMCDataModel() { }
}
