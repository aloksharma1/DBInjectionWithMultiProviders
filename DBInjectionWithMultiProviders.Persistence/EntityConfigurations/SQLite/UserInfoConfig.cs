using DBInjectionWithMultiProviders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.EntityConfigurations.SQLite
{
    public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo");
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.IsActive).HasConversion<int>();
            builder.Property(e => e.DateCreated).HasConversion<long>();
            builder.Property(e => e.DateModified).HasConversion<long>();
        }
    }
}
