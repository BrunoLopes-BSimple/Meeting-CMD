using AutoMapper;
using Domain.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Resolvers;

public class CollaboratorDataModelConverter : ITypeConverter<CollaboratorDataModel, Collaborator>
{
    public Collaborator Convert(CollaboratorDataModel source, Collaborator destination, ResolutionContext context)
    {
        return new Collaborator(source.Id);
    }
}
