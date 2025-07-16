using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.IRepository;

public interface ICollaboratorRepository : IGenericRepositoryEF<ICollaborator, Collaborator, ICollaboratorVisitor>
{
    public Task<bool> AlreadyExistsAsync(Guid collbId);
}
