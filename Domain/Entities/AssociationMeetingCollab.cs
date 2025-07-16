using Domain.Interfaces;

namespace Domain.Entities;

public class AssociationMeetingCollab : IAssociationMeetingCollab
{
    public Guid Id { get; private set; }

    public Guid CollabId { get; private set; }

    public Guid MeetingId { get; private set; }

    public AssociationMeetingCollab(Guid id, Guid collabId, Guid meetingId)
    {
        Id = id;
        CollabId = collabId;
        MeetingId = meetingId;
    }

}
