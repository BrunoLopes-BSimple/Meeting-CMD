using Application.DTO;
using Application.IService;
using Domain.Entities;
using Domain.Factories.TempMeetingFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModels;

namespace Application.Services;

public class MeetingTempService : IMeetingTempService
{
    private readonly IMeetingWithoutLocationRepository _tempRepo;
    private readonly ITempMeetingFactory _factory;

    public MeetingTempService(IMeetingWithoutLocationRepository tempRepo, ITempMeetingFactory factory)
    {
        _tempRepo = tempRepo;
        _factory = factory;
    }

    public async Task<CreatedMeetingWithoutLocationDTO> Create(Guid id, PeriodDateTime period, string mode, IEnumerable<Guid> participants, string description)
    {
        IMeetingWithouLocation meetingTemp;
        try
        {
            var meetingDm = new MeetingWithouLocationDataModel(id, period, mode, participants, description);

            meetingTemp = _factory.Create(meetingDm);
            await _tempRepo.AddAsync(meetingTemp);

            var result = new CreatedMeetingWithoutLocationDTO(meetingTemp.Id, meetingTemp.Period, meetingTemp.Mode, meetingTemp.Participants, meetingTemp.Description);
            return result;
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error creating temp meeting : {e}");
        }
    }
}
