using Application.IService;
using Domain.Entities;
using Domain.Factories.CollaboratorFactory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly ICollaboratorFactory _collaboratorFactory;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
    }

    public async Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId)
    {
        var collabAlreadyExists = await _collaboratorRepository.AlreadyExistsAsync(collabId);

        if (collabAlreadyExists) return null;

        var newCollab = _collaboratorFactory.Create(collabId);
        return await _collaboratorRepository.AddAsync(newCollab);
    }
}
