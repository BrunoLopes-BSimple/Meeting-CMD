using Domain.Entities;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationMCRepositoryTests;

public class AssociationMCRepositoryGetByCollabIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task GetByCollabIdAsync_ShouldReturnAssociations_WhenExist()
    {
        // Arrange
        var collabId = Guid.NewGuid();

        var associationData = new AssociationMCDataModel { Id = Guid.NewGuid(), CollabId = collabId, MeetingId = Guid.NewGuid() };
        context.Set<AssociationMCDataModel>().Add(associationData);
        await context.SaveChangesAsync();

        var expectedDomain = new AssociationMeetingCollab(associationData.Id, associationData.CollabId, associationData.MeetingId);
        _mapper.Setup(m => m.Map<IEnumerable<AssociationMeetingCollab>>(It.IsAny<IEnumerable<AssociationMCDataModel>>()))
            .Returns((IEnumerable<AssociationMCDataModel> dm) => dm.Select(d => new AssociationMeetingCollab(d.Id, d.CollabId, d.MeetingId)));

        var repo = new AssociationMCRepository(context, _mapper.Object);

        // Act
        var result = await repo.GetByCollabIdAsync(collabId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, a => Assert.Equal(collabId, a.CollabId));
    }

    [Fact]
    public async Task GetByCollabIdAsync_ShouldReturnEmpty_WhenNoneFound()
    {
        // Arrange
        var collabId = Guid.NewGuid();

        _mapper.Setup(m => m.Map<IEnumerable<AssociationMeetingCollab>>(It.IsAny<IEnumerable<AssociationMCDataModel>>()))
            .Returns(Enumerable.Empty<AssociationMeetingCollab>());

        var repo = new AssociationMCRepository(context, _mapper.Object);

        // Act
        var result = await repo.GetByCollabIdAsync(collabId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
