using Domain.Visitor;

namespace Infrastructure.DataModels;

public class CollaboratorDataModel : ICollaboratorVisitor
{
    public Guid Id { get; set; }

    public CollaboratorDataModel(Guid id)
    {
        Id = id;
    }

    public CollaboratorDataModel() { }
}
