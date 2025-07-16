using Domain.Visitor;

namespace Infrastructure.DataModels;

public class LocationDataModel : ILocationVisitor
{
    public Guid Id { get; set; }

    public LocationDataModel(Guid id, string description)
    {
        Id = id;
    }

    public LocationDataModel() { }
}
