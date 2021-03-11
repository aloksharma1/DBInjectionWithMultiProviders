using DBInjectionWithMultiProviders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.EntityConfigurations.SQLite
{
    public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddress");
            builder.Property(e => e.Id).ValueGeneratedNever().IsRequired();
            builder.Property(e => e.IsActive).HasConversion<int>();
            builder.Property(e => e.DateCreated).HasConversion<long>();
            builder.Property(e => e.DateModified).HasConversion<long>();
        }
    }
}
