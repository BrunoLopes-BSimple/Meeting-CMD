using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class LocationContextFactory : IDesignTimeDbContextFactory<LocationContext>
    {
        public LocationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocationContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=avaliacao-meeting-cmd-2;Username=postgres;Password=postgres");

            return new LocationContext(optionsBuilder.Options);
        }
    }
}