using Application.DTO;
using Domain.Entities;

namespace Application.IService;

    public interface IMeetingTempService
    {
    public Task<CreatedMeetingWithoutLocationDTO> Create(Guid id, PeriodDateTime period, string mode, IEnumerable<Guid> participants, string description);
    }
