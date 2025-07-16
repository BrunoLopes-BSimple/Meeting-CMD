using Domain.Entities;
using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factories.LocationFactory;

public class LocationFactory : ILocationFactory
{
    public LocationFactory(){}

    public ILocation Create(Guid id)
    {
        return new Location(id);
    }
}
