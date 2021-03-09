using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Infra.Data.Mapping
{
    public class UserProfileMap : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(prop => prop.Id);
            
            builder
                .Property(prop => prop.FirstName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder
                .Property(prop => prop.LastName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder
                .Property(prop => prop.UserName)
                .HasMaxLength(150)
                .IsRequired(true);

            builder
                .Property(prop => prop.Email)
                .HasMaxLength(200)
                .IsRequired(true);

            builder
                .Property(prop => prop.BirthDate)
                .IsRequired(false);

            builder
                .Property(prop => prop.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired(false);

            builder.Property(prop => prop.AcademicLevel)
                .HasConversion<int>()
                .IsRequired(false);
        }
    }
}
