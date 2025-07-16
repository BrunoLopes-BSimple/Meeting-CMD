using Infrastructure.DataModels;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryAlreadyExistsTests :RepositoryTestBase
{
    [Fact]
    public async Task AlreadyExists_WhenCollaboratorExists_ShouldReturnTrue()
    {
        // arrange
        var collaboratorId = Guid.NewGuid();
        var collaboratorData = new CollaboratorDataModel { Id = collaboratorId };

        context.Collaborators.Add(collaboratorData);
        await context.SaveChangesAsync();

        var repository = new CollaboratorRepository(context, _mapper.Object);

        // act
        var result = await repository.AlreadyExistsAsync(collaboratorId);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task AlreadyExists_WhenCollaboratorDoesNotExist_ShouldReturnFalse()
    {
        // arrange
        var nonExistentId = Guid.NewGuid();
        var repository = new CollaboratorRepository(context, _mapper.Object);

        // act
        var result = await repository.AlreadyExistsAsync(nonExistentId);

        // assert
        Assert.False(result);
    }
}
