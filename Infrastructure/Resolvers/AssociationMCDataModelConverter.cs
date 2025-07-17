using AutoMapper;
using Domain.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Resolvers;

public class AssociationMCDataModelConverter : ITypeConverter<AssociationMCDataModel, AssociationMeetingCollab>
{
    public AssociationMeetingCollab Convert(AssociationMCDataModel source, AssociationMeetingCollab destination, ResolutionContext context)
    {
        return new AssociationMeetingCollab(source.Id, source.CollabId, source.MeetingId);
    }
}
