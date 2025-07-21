using AutoMapper;
using Domain.Entities;
using Infrastructure.DataModels;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<Location, LocationDataModel>();
        CreateMap<LocationDataModel, Location>().ConvertUsing<LocationDataModelConverter>();

        CreateMap<Meeting, MeetingDataModel>();
        CreateMap<MeetingDataModel, Meeting>().ConvertUsing<MeetingDataModelConverter>();

        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>().ConvertUsing<CollaboratorDataModelConverter>();

        CreateMap<AssociationMeetingCollab, AssociationMCDataModel>();
        CreateMap<AssociationMCDataModel, AssociationMeetingCollab>().ConvertUsing<AssociationMCDataModelConverter>();

        CreateMap<MeetingWithoutLocation, MeetingWithouLocationDataModel>();
        CreateMap<MeetingWithouLocationDataModel, MeetingWithoutLocation>().ConvertUsing<MeetingWithoutLocationDataModelConverter>();

    }
}
