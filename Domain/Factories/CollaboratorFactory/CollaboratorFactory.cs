using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factories.CollaboratorFactory;

public class CollaboratorFactory : ICollaboratorFactory
{
    public ICollaborator Create(Guid id)
    {
        return new Collaborator(id);
    }
}
