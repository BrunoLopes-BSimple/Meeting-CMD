using Domain.Entities;

namespace Domain.Tests.CollaboratorTests;

public class CollaboratorConstructorTests
{
    [Fact]
    public void Constructor_ShouldCreateCollaborator()
    {
        new Collaborator(Guid.NewGuid());
    }
}
