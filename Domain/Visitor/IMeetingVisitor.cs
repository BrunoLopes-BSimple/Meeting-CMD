using Domain.Entities;

namespace Domain.Visitor;

public interface IMeetingVisitor
{
    public Guid Id { get; }
    public PeriodDateTime Period { get; }
    public string Mode { get; }
    public Guid LocationId { get; }
}
