using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModels;

namespace Infrastructure.Repositories;

public class MeetingWithoutLocationRepository : GenericRepositoryEF<IMeetingWithouLocation, MeetingWithoutLocation, MeetingWithouLocationDataModel>, IMeetingWithoutLocationRepository
{
    private readonly IMapper _mapper;

    public MeetingWithoutLocationRepository(LocationContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }
}
