using DBInjectionWithMultiProviders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.EntityConfigurations.MSSQL
{
    public class EFMigrationsHistoryConfig : IEntityTypeConfiguration<EFMigrationsHistory>
    {
        public void Configure(EntityTypeBuilder<EFMigrationsHistory> builder)
        {
            builder.ToTable("__EFMigrationsHistory");
            builder.HasKey(e => e.MigrationId);
        }
    }
}
