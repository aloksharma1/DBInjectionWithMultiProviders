using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Extensions;
using DBInjectionWithMultiProviders.Persistence.Extensions;
using DBInjectionWithMultiProviders.Plugin1.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Models
{
    public class EntityContextProvider : IPluginDbContext<ModelBuilder>
    {
        public void Setup(ModelBuilder modelBuilder,DatabaseProvider Provider)
        {
            //modelBuilder.Entity<Security>().ToTable("Security");
            switch (Provider)
            {
                case DatabaseProvider.MSSQL:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.MSSQL");
                    break;

                case DatabaseProvider.SQLITE:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.SQLite");
                    break;
            }
        }
    }
}
