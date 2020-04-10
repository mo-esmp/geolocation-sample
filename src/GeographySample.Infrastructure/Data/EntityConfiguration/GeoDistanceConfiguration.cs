using GeographySample.Core.Distance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeographySample.Infrastructure.Data.EntityConfiguration
{
    internal class GeoDistanceConfiguration : IEntityTypeConfiguration<GeoDistanceEntity>
    {
        public void Configure(EntityTypeBuilder<GeoDistanceEntity> builder)
        {
            builder.HasOne(ld => ld.User).WithMany().HasForeignKey(ld => ld.UserId);
        }
    }
}