using Application.DTO;
using Application.IService;
using Domain.Entities;
using Domain.Factories.AssociationFactory;
using Domain.Factories.MeetingFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure;

namespace Application.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingFactory _meetingFactory;
    private readonly IAssociationMCFactory _associationFactory;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IAssociationMCRepository _associationRepository;
    private readonly LocationContext _context;

    public MeetingService(IMeetingFactory meetingFactory, IAssociationMCFactory associationFactory, IMeetingRepository meetingRepository, IAssociationMCRepository associationRepository, LocationContext context)
    {
        _meetingFactory = meetingFactory;
        _associationFactory = associationFactory;
        _meetingRepository = meetingRepository;
        _associationRepository = associationRepository;
        _context = context;
    }

    public async Task<Result<CreatedMeetingDTO>> Create(CreateMeetingDTO dto)
    {
        try
        {
            var newMeeting = _meetingFactory.Create(dto.Period, dto.Mode, dto.LocationId);
            var associationsToCreate = new List<IAssociationMeetingCollab>();

            foreach (var collabId in dto.CollaboratorIds)
            {
                var newAssociation = await _associationFactory.Create(newMeeting, collabId);
                associationsToCreate.Add(newAssociation);
            }

            _meetingRepository.Add(newMeeting);
            _associationRepository.AddRange(associationsToCreate);
            await _context.SaveChangesAsync();

            var result = new CreatedMeetingDTO(newMeeting.Id, newMeeting.Period, newMeeting.Mode, dto.CollaboratorIds, dto.LocationId);

            return Result<CreatedMeetingDTO>.Success(result);
        }
        catch (Exception e)
        {
            return Result<CreatedMeetingDTO>.Failure(Error.BadRequest(e.Message));

        }
    }
}
