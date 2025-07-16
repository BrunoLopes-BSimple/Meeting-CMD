using Domain.Interfaces;

namespace Application.IService;

public interface ICollaboratorService
{
    public Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId);
}
