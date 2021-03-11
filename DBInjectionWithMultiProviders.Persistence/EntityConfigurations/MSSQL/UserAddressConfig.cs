using DBInjectionWithMultiProviders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.EntityConfigurations.MSSQL
{
    public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddress");
            builder.HasKey(e => e.Id).Metadata.IsPrimaryKey();
            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.Landmark).HasColumnType("nvarchar(50)");
            builder.Property(e => e.State).HasColumnType("nvarchar(50)");
            builder.Property(e => e.Country).HasColumnType("nvarchar(50)");
            builder.Property(e => e.ZipCode).HasColumnType("nvarchar(15)");
            builder.Property(e => e.IsActive).HasColumnType("bit");
        }
    }
}
