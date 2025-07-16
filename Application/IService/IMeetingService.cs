using Application.DTO;
using Domain.Entities;

namespace Application.IService;

public interface IMeetingService
{
    public Task<Result<CreatedMeetingDTO>> Create(CreateMeetingDTO dto);
}
