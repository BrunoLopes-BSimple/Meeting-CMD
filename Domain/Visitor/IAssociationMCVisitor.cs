namespace Domain.Visitor;

public interface IAssociationMCVisitor
{
    public Guid Id { get; }

    public Guid CollabId { get; }

    public Guid MeetingId { get; }
}
