using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class LocationContext : DbContext
{
    public virtual DbSet<LocationDataModel> Locations { get; set; }
    public virtual DbSet<AssociationMCDataModel> AssociationsMC { get; set; }
    public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
    public virtual DbSet<MeetingDataModel> Meetings { get; set; }
    public virtual DbSet<MeetingWithouLocationDataModel> TempMeeting { get; set; }

    public LocationContext(DbContextOptions<LocationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeetingDataModel>().OwnsOne(a => a.Period);
        modelBuilder.Entity<MeetingWithouLocationDataModel>().OwnsOne(a => a.Period);

        base.OnModelCreating(modelBuilder);
    }
}
