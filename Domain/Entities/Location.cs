using Domain.Interfaces;

namespace Domain.Entities;

public class Location : ILocation
{
    public Guid Id { get; private set; }
    public Location(Guid id)
    {
        Id = id;
    }

}
