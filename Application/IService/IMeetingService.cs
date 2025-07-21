using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.IService;

public interface IMeetingService
{
    public Task<Result<CreatedMeetingDTO>> Create(CreateMeetingDTO dto);
    Task<IMeeting?> AddMeetingReferenceAsync(MeetingReference reference, IEnumerable<Guid> collabIds);
    public Task<Result<EditedMeetingDTO>> EditMeeting(EditMeetingDTO dto);
    public Task<bool> CreateMeetingWithoutLocation(CreateMeetingWithoutLocationDTO dto);
    public Task FinalizeAsync(Guid meetingId, PeriodDateTime period, string mode, IEnumerable<Guid> participants, Guid LocationId, string description);
}
