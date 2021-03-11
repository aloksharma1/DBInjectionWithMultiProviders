using DBInjectionWithMultiProviders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.EntityConfigurations.MSSQL
{
    public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo");
            builder.HasKey(e => e.Id).Metadata.IsPrimaryKey();
            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.FullName).HasColumnType("nvarchar(50)");
            builder.Property(e => e.Email).HasColumnType("nvarchar(550)");
            builder.Property(e => e.Password).HasColumnType("nvarchar(20)");
            builder.Property(e => e.IsActive).HasColumnType("bit");
        }
    }
}
