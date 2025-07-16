using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.IRepository;

public interface IMeetingRepository : IGenericRepositoryEF<IMeeting, Meeting, IMeetingVisitor>
{
    public Task<IEnumerable<IMeeting>> GetMeetingsByIdsAsync(IEnumerable<Guid> meetingIds);
    public Task<IMeeting?> GetByIdAsync(Guid id);
}
