using Application.DTO;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.IService;

public interface IMeetingService
{
    public Task<Result<CreatedMeetingDTO>> Create(CreateMeetingDTO dto);
    Task<IMeeting?> AddMeetingReferenceAsync(MeetingReference reference);
    public Task<Result<EditedMeetingDTO>> EditMeeting(EditMeetingDTO dto);
}
