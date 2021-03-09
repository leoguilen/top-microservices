using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Infra.Data.Mapping
{
    public class UserProfileDetailsMap : IEntityTypeConfiguration<UserProfileDetails>
    {
        public void Configure(EntityTypeBuilder<UserProfileDetails> builder)
        {
            builder.HasKey(prop => prop.UserId);

            builder.OwnsOne(prop => prop.Address);
            builder.Property(prop => prop.Bio)
                .HasMaxLength(255);
        }
    }
}
