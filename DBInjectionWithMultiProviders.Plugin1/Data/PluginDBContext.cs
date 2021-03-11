using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Domain.Entities;
using DBInjectionWithMultiProviders.Extensions;
using DBInjectionWithMultiProviders.Persistence;
using DBInjectionWithMultiProviders.Persistence.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Data
{
    //used to generate separate plugin migrations
    public class PluginDBContext : DbContext, IApplicationDbContext
    {
        protected readonly IConfiguration Configuration;
        public PluginDBContext([NotNull] DbContextOptions<PluginDBContext> options, IConfiguration configuration) : base(options)
        {
            this.Configuration = configuration;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                switch (Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>())
                {
                    case DatabaseProvider.MSSQL:
                        optionsBuilder.UseSqlServer(Configuration.GetSection("DBProviders:0:ConnectionString").Value);
                        break;

                    case DatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(Configuration.GetSection("DBProviders:1:ConnectionString").Value);
                        break;
                    default:
                        break;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>();
            switch (Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>())
            {
                case DatabaseProvider.MSSQL:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(typeof(CoreDbContext).Assembly, "DBInjectionWithMultiProviders.Persistence.EntityConfigurations.MSSQL")
                        .ApplyAllConfigurationsFromNamespace(this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.MSSQL")
                        ;
                    break;
                case DatabaseProvider.SQLITE:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(typeof(CoreDbContext).Assembly, "DBInjectionWithMultiProviders.Persistence.EntityConfigurations.SQLite")
                        .ApplyAllConfigurationsFromNamespace(this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Plugin1.Models.EntityConfigurations.SQLite");
                    break;
                default:
                    break;
            }
            //ignore base entities which are not connected with this plugin entities
            modelBuilder.Ignore<UserAddress>();
            base.OnModelCreating(modelBuilder);
        }
        public override void Dispose()
        {
            switch (Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>())
            {
                case DatabaseProvider.MSSQL:
                    SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                    break;
                case DatabaseProvider.SQLITE:
                    break;
                default:
                    break;

                    //case DatabaseProvider.MySQL:
                    //    MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                    //    break;
            }
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
