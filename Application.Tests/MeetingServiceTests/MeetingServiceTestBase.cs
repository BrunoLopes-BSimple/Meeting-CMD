using Application.Services;
using Domain.Factories.AssociationFactory;
using Domain.Factories.MeetingFactory;
using Domain.IRepository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.MeetingServiceTests;

public abstract class MeetingServiceTestBase
{
    protected readonly Mock<IMeetingFactory> _meetingFactoryDouble;
    protected readonly Mock<IAssociationMCFactory> _associationFactoryDouble;
    protected readonly Mock<IMeetingRepository> _meetingRepositoryDouble;
    protected readonly Mock<IAssociationMCRepository> _associationRepositoryDouble;
    protected readonly LocationContext _context;

    protected readonly MeetingService MeetingService;

    protected MeetingServiceTestBase()
    {
        var options = new DbContextOptionsBuilder<LocationContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _context = new LocationContext(options);

        _meetingFactoryDouble = new Mock<IMeetingFactory>();
        _associationFactoryDouble = new Mock<IAssociationMCFactory>();
        _meetingRepositoryDouble = new Mock<IMeetingRepository>();
        _associationRepositoryDouble = new Mock<IAssociationMCRepository>();

        MeetingService = new MeetingService(_meetingFactoryDouble.Object,
            _associationFactoryDouble.Object,
            _meetingRepositoryDouble.Object,
            _associationRepositoryDouble.Object,
            _context);
    }
}
