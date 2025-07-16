namespace Domain.Interfaces;

public interface IAssociationMeetingCollab
{
    public Guid Id { get; }
    public Guid CollabId { get; }
    public Guid MeetingId { get; }
}
