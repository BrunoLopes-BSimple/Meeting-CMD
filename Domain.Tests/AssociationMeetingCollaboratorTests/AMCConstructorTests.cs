using Domain.Entities;

namespace Domain.Tests.AssociationMeetingCollaboratorTests;

public class AMCConstructorTests
{
    [Fact]
    public void Constructor_ShouldCreateAssociation()
    {
        new AssociationMeetingCollab(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }
}
