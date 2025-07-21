using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.IRepository;

public interface IMeetingWithoutLocationRepository : IGenericRepositoryEF<IMeetingWithouLocation, MeetingWithoutLocation, IMeetingWithouLocationVisitor>
{

}
