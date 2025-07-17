using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AssociationMCRepository : GenericRepositoryEF<IAssociationMeetingCollab, AssociationMeetingCollab, AssociationMCDataModel>, IAssociationMCRepository
{
    private readonly IMapper _mapper;

    public AssociationMCRepository(LocationContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<IAssociationMeetingCollab>> GetByCollabIdAsync(Guid collabId)
    {
        var associationsDM = await _context.Set<AssociationMCDataModel>().Where(a => a.CollabId == collabId).ToListAsync();

        var associations = _mapper.Map<IEnumerable<AssociationMeetingCollab>>(associationsDM);
        return associations;
    }

    public async Task RemoveCollaboratorsFromMeeting(Guid meetingId)
    {
        var associations = await _context.Set<AssociationMCDataModel>().Where(a => a.MeetingId == meetingId).ToListAsync();

        _context.Set<AssociationMCDataModel>().RemoveRange(associations);
    }
}
