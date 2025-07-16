using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factories.LocationFactory;

public interface ILocationFactory
{
    public ILocation Create(Guid id);
}
