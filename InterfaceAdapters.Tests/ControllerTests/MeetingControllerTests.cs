
using System.Net;
using System.Net.Http.Json;
using Application.DTO;
using Domain.Entities;

namespace InterfaceAdapters.Tests.ControllerTests;

public class MeetingControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public MeetingControllerTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_WhenSucceeds_ShouldReturnOkWithCreatedMeeting()
    {
        // arrange
        var period = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };
        var mode = "Online";
        var locationId = Guid.NewGuid();
        var collabId1 = Guid.NewGuid();
        var collabId2 = Guid.NewGuid();
        var collabIds = new List<Guid> { collabId1, collabId2 };

        var payload = new CreateMeetingDTO(period, mode, locationId, collabIds);

        // act
        var response = await PostAsync("/api/meeting", payload);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseDto = await response.Content.ReadFromJsonAsync<CreatedMeetingDTO>();
        Assert.NotNull(responseDto);
        Assert.Equal(period, responseDto.MeetingPeriod);
        Assert.Equal(mode, responseDto.Mode);
        Assert.NotEqual(Guid.Empty, responseDto.MeetingId);
    }

    [Fact]
    public async Task Update_WhenSucceeds_ShouldReturnOkWithUpdatedMeeting()
    {
        // Arrange 
        var originalPeriod = new PeriodDateTime { _initDate = DateTime.UtcNow, _finalDate = DateTime.UtcNow.AddHours(1) };
        var originalMode = "Online";
        var originalLocationId = Guid.NewGuid();
        var originalCollabIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var createPayload = new CreateMeetingDTO(originalPeriod, originalMode, originalLocationId, originalCollabIds);
        var createResponse = await PostAsync("/api/meeting", createPayload);
        var createdMeeting = await createResponse.Content.ReadFromJsonAsync<CreatedMeetingDTO>();
        Assert.NotNull(createdMeeting); 

        // Arrange 
        var updatedPeriod = new PeriodDateTime { _initDate = DateTime.UtcNow.AddDays(1), _finalDate = DateTime.UtcNow.AddDays(1).AddHours(2) };
        var updatedMode = "Hybrid";
        var updatedLocationId = Guid.NewGuid();
        var updatedCollabIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var editPayload = new EditMeetingDTO(createdMeeting.MeetingId, updatedPeriod, updatedMode, updatedLocationId, updatedCollabIds);

        // Act
        var updateResponse = await PutAsync("/api/meeting", editPayload);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var responseDto = await updateResponse.Content.ReadFromJsonAsync<EditedMeetingDTO>();
        Assert.NotNull(responseDto);
        Assert.Equal(createdMeeting.MeetingId, responseDto.MeetingId);
        Assert.Equal(updatedMode, responseDto.Mode);
        Assert.Equal(updatedLocationId, responseDto.LocationId);
        Assert.Equal(updatedCollabIds, responseDto.CollaboratorIds);
    }

}
