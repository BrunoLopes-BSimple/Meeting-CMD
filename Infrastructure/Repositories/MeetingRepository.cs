using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MeetingRepository : GenericRepositoryEF<IMeeting, Meeting, MeetingDataModel>, IMeetingRepository
{
    private readonly IMapper _mapper;

    public MeetingRepository(LocationContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<IMeeting>> GetMeetingsByIdsAsync(IEnumerable<Guid> meetingIds)
    {
        var meetingsDM = await _context.Set<MeetingDataModel>().Where(m => meetingIds.Contains(m.Id)).ToListAsync();

        var meetings = _mapper.Map<IEnumerable<Meeting>>(meetingsDM);
        return meetings;
    }

    public async Task<IMeeting?> GetByIdAsync(Guid id)
    {
        var meetingDM = await this._context.Set<MeetingDataModel>()
                            .FirstOrDefaultAsync(m => m.Id == id);

        if (meetingDM == null)
            return null;

        var meeting = _mapper.Map<Meeting>(meetingDM);
        return meeting;
    }

    public async Task<bool> AlreadyExistsAsync(Guid meetingId)
    {
        return await _context.Set<MeetingDataModel>().AnyAsync(m => m.Id == meetingId);
    }
}
