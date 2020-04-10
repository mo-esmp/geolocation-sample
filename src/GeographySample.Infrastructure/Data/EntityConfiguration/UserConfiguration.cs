using GeographySample.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeographySample.Infrastructure.Data.EntityConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(p => p.FirstName).IsUnicode().HasMaxLength(50).IsRequired();
            builder.Property(p => p.LastName).IsUnicode().HasMaxLength(50).IsRequired();
        }
    }
}