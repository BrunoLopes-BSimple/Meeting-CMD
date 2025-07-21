using Application.DTO;
using Application.IPublisher;
using Application.ISender;
using Application.IService;
using Application.Services;
using Domain.Entities;
using Domain.Factories.AssociationFactory;
using Domain.Factories.CollaboratorFactory;
using Domain.Factories.LocationFactory;
using Domain.Factories.MeetingFactory;
using Domain.Factories.TempMeetingFactory;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using InterfaceAdapters;
using InterfaceAdapters.Activities;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Publisher;
using InterfaceAdapters.Saga;
using InterfaceAdapters.Sender;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<LocationContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<IMeetingTempService, MeetingTempService>();


// Repositories
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAssociationMCRepository, AssociationMCRepository>();
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IMeetingWithoutLocationRepository, MeetingWithoutLocationRepository>();


// Factories
builder.Services.AddScoped<ILocationFactory, LocationFactory>();
builder.Services.AddScoped<IAssociationMCFactory, AssociationMCFactory>();
builder.Services.AddScoped<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddScoped<IMeetingFactory, MeetingFactory>();
builder.Services.AddScoped<ITempMeetingFactory, TempMeetingFactory>();



// Mappers
builder.Services.AddTransient<LocationDataModelConverter>();
builder.Services.AddTransient<MeetingDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<AssociationMCDataModelConverter>();
builder.Services.AddTransient<MeetingWithoutLocationDataModelConverter>();

// activities
builder.Services.AddScoped<CreateTempMeetingActivity>();
builder.Services.AddScoped<FinalizeMeetingActivity>();


// publisher
builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();

// sender
builder.Services.AddTransient<IMessageSender, MassTransitSender>();



builder.Services.AddAutoMapper(cfg =>
{
    // DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    // DTO
    cfg.CreateMap<Location, LocationDTO>();
});

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LocationCreatedConsumer>();
    x.AddConsumer<CollaboratorCreatedConsumer>();
    x.AddConsumer<MeetingCreatedConsumer>();

    x.AddSagaStateMachine<MeetingCreationSagaStateMachine, MeetingCreationSagaState>()
        .InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"meeting-cmd-{instance}", e =>
        {
            e.ConfigureConsumers(context);
        });

        cfg.ReceiveEndpoint($"collaborators-cmd-{instance}", e =>
        {
            e.ConfigureConsumer<CollaboratorCreatedConsumer>(context);

        });
        cfg.ReceiveEndpoint("meeting-creation-saga", e =>
        {
            e.ConfigureSaga<MeetingCreationSagaState>(context);
        });
    });


});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

    if (!env.IsEnvironment("Testing"))
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LocationContext>();
        dbContext.Database.Migrate();
    }
}

app.Run();

public partial class Program { }
