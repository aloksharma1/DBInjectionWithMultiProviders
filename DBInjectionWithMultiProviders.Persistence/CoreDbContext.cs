using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Extensions;
using DBInjectionWithMultiProviders.Persistence.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DBInjectionWithMultiProviders.Persistence
{
    public class CoreDbContext : DbContext, IApplicationDbContext
    {
        protected readonly IConfiguration Configuration;

        public IConfigurationRoot ConfigRoot { get; }
        //plain ctor
        //used with unit testing or 
        //in case where db provider is not configured on startup.cs
        public CoreDbContext()
        {

        }
        //public CoreDbContext(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //}
        public CoreDbContext([NotNull] DbContextOptions<CoreDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            //In Entity framework Core (on version 2.2.4) you can use the following code in your DbContext to create tables in your database if they don't exist:
            //try
            //{
            //    var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
            //    _ = await databaseCreator.CreateTablesAsync();
            //}
            //catch (Exception)
            //{
            //    //A SqlException will be thrown if tables already exist. So simply ignore it.
            //}
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
            var Provider = Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>();
            switch (Provider)
            {
                case DatabaseProvider.MSSQL:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Persistence.EntityConfigurations.MSSQL");
                    break;

                case DatabaseProvider.SQLITE:
                    modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "DBInjectionWithMultiProviders.Persistence.EntityConfigurations.SQLite");
                    break;
            }
            if (Configuration.GetSection("IsPlugin")?.Value.ToLower() != "true")
            {
                foreach (var item in ReflectionHelper.GetAssemblyByInterface<IPluginDbContext<ModelBuilder>>())
                {
                    var method = item.GetMethod("Setup", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    if (method.IsStatic)
                    {
                        method.Invoke(null, new Object[] { modelBuilder, Provider });
                    }
                    else
                    {
                        method.Invoke(Activator.CreateInstance(item), new Object[] { modelBuilder, Provider });
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        //Ref https://github.com/dotnet/efcore/issues/10169
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
            base.Dispose();
        }
    }
}
