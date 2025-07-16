using System.IO.Pipelines;
using Application.DTO;
using Application.IPublisher;
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
    private readonly IMessagePublisher _publisher;

    public MeetingService(IMeetingFactory meetingFactory, IAssociationMCFactory associationFactory, IMeetingRepository meetingRepository, IAssociationMCRepository associationRepository, LocationContext context, IMessagePublisher publisher)
    {
        _meetingFactory = meetingFactory;
        _associationFactory = associationFactory;
        _meetingRepository = meetingRepository;
        _associationRepository = associationRepository;
        _context = context;
        _publisher = publisher;
    }

    public async Task<IMeeting?> AddMeetingReferenceAsync(MeetingReference reference)
    {
        var meetingAlreadyExists = await _meetingRepository.AlreadyExistsAsync(reference.Id);

        if (meetingAlreadyExists) return null;

        var newMeeting = _meetingFactory.Create(reference.Id, reference.Period, reference.Mode, reference.LocationId);
        return await _meetingRepository.AddAsync(newMeeting);  
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

            var meeting = _meetingRepository.Add(newMeeting);
            _associationRepository.AddRange(associationsToCreate);
            var saved = await _context.SaveChangesAsync();

            var result = new CreatedMeetingDTO(newMeeting.Id, newMeeting.Period, newMeeting.Mode, dto.CollaboratorIds, dto.LocationId);

            await _publisher.PublishMeetingCreated(meeting);
            return Result<CreatedMeetingDTO>.Success(result);
        }
        catch (Exception e)
        {
            return Result<CreatedMeetingDTO>.Failure(Error.BadRequest(e.Message));
        }
    }
}
