using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DBInjectionWithMultiProviders.Domain.Entities;

namespace DBInjectionWithMultiProviders.Persistence.Extensions
{
    public static class MigrationHelper
    {
        public static bool MigrationExists(string migrationName)
        {
            if (!string.IsNullOrEmpty(migrationName))
            {
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                
                // Create design-time services
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddEntityFrameworkDesignTimeServices();
                var serviceProvider = serviceCollection.BuildServiceProvider();
                //IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../MyCookingMaster.API/appsettings.json").Build();
                var builder = new DbContextOptionsBuilder<CoreDbContext>();
                //var connectionString = configuration.GetSection("DBProviders:0:ConnectionString").Value;
                //builder.UseSqlServer(connectionString);
                using var db= new CoreDbContext(builder.Options, configuration);
                return db.Set<EFMigrationsHistory>().AnyAsync(efh => efh.MigrationId == migrationName).Result;
            }
            return false;
        }
    }
}
