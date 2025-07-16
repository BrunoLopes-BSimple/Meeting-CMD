using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollaboratorRepository : GenericRepositoryEF<ICollaborator, Collaborator, CollaboratorDataModel>, ICollaboratorRepository
{
    private readonly IMapper _mapper;

    public CollaboratorRepository(LocationContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<bool> AlreadyExistsAsync(Guid collbId)
    {
        return await _context.Set<CollaboratorDataModel>().AnyAsync(c => c.Id == collbId);
    }
}
