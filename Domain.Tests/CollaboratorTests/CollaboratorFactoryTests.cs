using Domain.Factories.CollaboratorFactory;

namespace Domain.Tests.CollaboratorTests;

    public class CollaboratorFactoryTests
    {
    [Fact]
    public void Create_ShouldReturnCollaborator_WithCorrectId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var factory = new CollaboratorFactory();

        // Act
        var collaborator = factory.Create(id);

        // Assert
        Assert.NotNull(collaborator);
        Assert.Equal(id, collaborator.Id);
    }
}
