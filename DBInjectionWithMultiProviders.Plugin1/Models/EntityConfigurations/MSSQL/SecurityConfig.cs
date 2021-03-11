using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Domain.Entities;
using DBInjectionWithMultiProviders.Persistence;
using DBInjectionWithMultiProviders.Persistence.Extensions;
using DBInjectionWithMultiProviders.Plugin1.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.MSSQL
{
    public class SecurityConfig : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("Security").HasKey(e => e.Id).Metadata.IsPrimaryKey();
            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.LoginIp).HasColumnType("nvarchar(20)").IsRequired(false);
            //https://stackoverflow.com/questions/20886049/ef-code-first-foreign-key-without-navigation-property
            builder.HasOne<UserInfo>().WithMany().HasForeignKey(e => e.LoginUserId).IsRequired(false);

            //additional properties that comes with 2nd,3rd,...... nth migration
            if (MigrationHelper.MigrationExists("20210308061104_addedTimeSlotsSql"))
            {
                builder.Property(e => e.loginTime).HasColumnType("datetime").IsRequired(false);
                builder.Property(e => e.logoutTime).HasColumnType("datetime").IsRequired(false);
            }
            else
            {
                builder.Ignore(e => e.loginTime);
                builder.Ignore(e => e.logoutTime);
            }
        }
    }
}
