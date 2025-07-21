using AutoMapper;
using Domain.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Resolvers;

public class MeetingWithoutLocationDataModelConverter : ITypeConverter<MeetingWithouLocationDataModel, MeetingWithoutLocation>
{
    public MeetingWithoutLocation Convert(MeetingWithouLocationDataModel source, MeetingWithoutLocation destination, ResolutionContext context)
    {
        return new MeetingWithoutLocation(source.Id, source.Period, source.Mode, source.Participants, source.Description);
    }
}
