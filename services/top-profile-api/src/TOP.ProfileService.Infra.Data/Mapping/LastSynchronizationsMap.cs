using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Infra.Data.Mapping
{
    public class LastSynchronizationsMap : IEntityTypeConfiguration<LastSynchronization>
    {
        public void Configure(EntityTypeBuilder<LastSynchronization> builder)
        {
            builder.HasNoKey();
        }
    }
}
