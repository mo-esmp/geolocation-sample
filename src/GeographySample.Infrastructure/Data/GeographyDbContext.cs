using GeographySample.Core.Distance;
using GeographySample.Core.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeographySample.Infrastructure.Data
{
    public class GeographyDbContext : IdentityDbContext<UserEntity>
    {
        public GeographyDbContext(DbContextOptions<GeographyDbContext> options)
            : base(options)
        {
        }

        public DbSet<GeoDistanceEntity> GeoDistances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(GeographyDbContext).Assembly);
        }
    }
}