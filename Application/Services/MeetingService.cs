using Application.DTO;
using Application.IPublisher;
using Application.ISender;
using Application.IService;
using Domain.Contracts;
using Domain.Entities;
using Domain.Factories.AssociationFactory;
using Domain.Factories.MeetingFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.DataModels;

namespace Application.Services;

public class MeetingService : IMeetingService
{
    private readonly IMeetingFactory _meetingFactory;
    private readonly IAssociationMCFactory _associationFactory;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IAssociationMCRepository _associationRepository;
    private readonly LocationContext _context;
    private readonly IMessagePublisher _publisher;
    private readonly IMessageSender _sender;

    public MeetingService(IMeetingFactory meetingFactory, IAssociationMCFactory associationFactory, IMeetingRepository meetingRepository, IAssociationMCRepository associationRepository, LocationContext context, IMessagePublisher publisher, IMessageSender sender)
    {
        _meetingFactory = meetingFactory;
        _associationFactory = associationFactory;
        _meetingRepository = meetingRepository;
        _associationRepository = associationRepository;
        _context = context;
        _publisher = publisher;
        _sender = sender;
    }

    public async Task<IMeeting?> AddMeetingReferenceAsync(MeetingReference reference, IEnumerable<Guid> collabIds)
    {
        var meetingAlreadyExists = await _meetingRepository.AlreadyExistsAsync(reference.Id);
        if (meetingAlreadyExists) return null;

        var newMeeting = _meetingFactory.Create(reference.Id, reference.Period, reference.Mode, reference.LocationId);
        var associationsToCreate = new List<IAssociationMeetingCollab>();

        foreach (var collabId in collabIds)
        {
            var newAssociation = await _associationFactory.Create(newMeeting, collabId);
            associationsToCreate.Add(newAssociation);
        }

        _meetingRepository.Add(newMeeting);
        _associationRepository.AddRange(associationsToCreate);
        await _context.SaveChangesAsync();

        return newMeeting;
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

            await _publisher.PublishMeetingCreated(meeting, dto.CollaboratorIds);
            return Result<CreatedMeetingDTO>.Success(result);
        }
        catch (Exception e)
        {
            return Result<CreatedMeetingDTO>.Failure(Error.BadRequest(e.Message));
        }
    }

    public async Task<Result<EditedMeetingDTO>> EditMeeting(EditMeetingDTO dto)
    {
        var meeting = await _meetingRepository.GetByIdAsync(dto.MeetingId);
        if (meeting == null) return Result<EditedMeetingDTO>.Failure(Error.NotFound("Meeting Not Found."));

        var validModes = new[] { "Online", "Hybrid", "OnSite" };
        if (!validModes.Contains(dto.Mode)) return Result<EditedMeetingDTO>.Failure(Error.InternalServerError("Mode does not exist"));

        meeting.UpdatePeriod(dto.Period);
        meeting.UpdateMode(dto.Mode);
        meeting.UpdateLocationId(dto.LocationId);

        await _meetingRepository.UpdateMeeting(meeting);

        await _associationRepository.RemoveCollaboratorsFromMeeting(dto.MeetingId);

        var associationsToCreate = new List<IAssociationMeetingCollab>();
        foreach (var collabId in dto.CollaboratorIds)
        {
            var newAssociation = await _associationFactory.Create(meeting, collabId);
            associationsToCreate.Add(newAssociation);
        }

        _associationRepository.AddRange(associationsToCreate);

        await _context.SaveChangesAsync();

        var collabIds = associationsToCreate.Select(a => a.CollabId).ToList();

        var result = new EditedMeetingDTO(meeting.Id, meeting.Period, meeting.Mode, meeting.LocationId, collabIds);

        return Result<EditedMeetingDTO>.Success(result);
    }

    public async Task<bool> CreateMeetingWithoutLocation(CreateMeetingWithoutLocationDTO dto)
    {
        try
        {
            var startMessage = new CreateMeetingCommand(Guid.NewGuid(), dto.MeetingPeriod, dto.Mode, dto.Participants, dto.Description);

            await _sender.SendMeetingCreationCommandAsync(startMessage);
            return true;
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error creating temp meeting {e}");
        }
    }

    // todo finalize
    public async Task FinalizeAsync(Guid meetingId, PeriodDateTime period, string mode, IEnumerable<Guid> participants ,Guid locationId, string description)
    {
        try
        {
            var newMeeting = _meetingFactory.Create(meetingId, period, mode, locationId);
            var associationsToCreate = new List<IAssociationMeetingCollab>();

            foreach (var collabId in participants)
            {
                var newAssociation = await _associationFactory.Create(newMeeting, collabId);
                associationsToCreate.Add(newAssociation);
            }

            var meeting = _meetingRepository.Add(newMeeting);
            _associationRepository.AddRange(associationsToCreate);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error creating meeting : {e}");
        }
    }
}
