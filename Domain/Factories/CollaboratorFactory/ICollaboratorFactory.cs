using Domain.Interfaces;

namespace Domain.Factories.CollaboratorFactory;

public interface ICollaboratorFactory
{
    public ICollaborator Create(Guid id);
}
