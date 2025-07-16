using Application.DTO;
using Application.IPublisher;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using Domain.Factories.LocationFactory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepo;
    private readonly ILocationFactory _locationFactory;

    public LocationService(ILocationRepository repository, ILocationFactory factory)
    {
        _locationRepo = repository;
        _locationFactory = factory;
    }

    public async Task<ILocation?> AddLocationReferenceAsync(LocationReference reference)
    {
        var locationAlreadyExists = await _locationRepo.AlreadyExists(reference.Id);
        if (locationAlreadyExists) return null;

        var newLocation = _locationFactory.Create(reference.Id);
        return await _locationRepo.AddAsync(newLocation);
    }
}
