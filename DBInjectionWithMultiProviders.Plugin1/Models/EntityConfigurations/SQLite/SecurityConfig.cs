using DBInjectionWithMultiProviders.Domain.Entities;
using DBInjectionWithMultiProviders.Plugin1.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.SQLite
{
    public class SecurityConfig : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("Security").HasKey(e => e.Id).Metadata.IsPrimaryKey();
            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.loginTime).HasConversion<long>();
            builder.Property(e => e.logoutTime).HasConversion<long>();
            //https://stackoverflow.com/questions/20886049/ef-code-first-foreign-key-without-navigation-property
            builder.HasOne<UserInfo>().WithMany().HasForeignKey(e => e.LoginUserId);
            builder.Property(e => e.IsActive).HasConversion<int>();
            builder.Property(e => e.DateCreated).HasConversion<long>();            
            builder.Property(e => e.DateModified).HasConversion<long>();
        }
    }
}
