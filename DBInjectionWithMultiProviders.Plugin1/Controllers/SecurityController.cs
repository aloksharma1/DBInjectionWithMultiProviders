using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Plugin1.Data;
using DBInjectionWithMultiProviders.Plugin1.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Controllers
{
    public class SecurityController : Controller
    {

        public SecurityController()
        {

        }
        public IActionResult Index([FromServices] IRepository<Security> repository)
        {
            var data = repository.GetById<long>(1);
            return View(data);
        }
        [Route("~/ReApply/Plugin1")]
        public async Task<IActionResult> ReApplyMigrationsAsync([FromServices] PluginDBContext dbContext)
        {
            // Create design-time services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEntityFrameworkDesignTimeServices();
            serviceCollection.AddDbContextDesignTimeServices(dbContext);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var migrationsService = serviceProvider.GetService<IMigrator>();
            await migrationsService.MigrateAsync("20210308061104_addedTimeSlotsSql");
            dbContext.Dispose();
            //GC.Collect();
            return Content("Migration Succesfull");
        }
        [Route("~/RollBack/Plugin1")]
        public async Task<IActionResult> RollBackMigrationAsync([FromServices] PluginDBContext dbContext, [FromServices] IServiceCollection services)
        {
            var Services = services.BuildServiceProvider();
            //full rollback
            //0 to full rollback 1 to apply where 1 also means dbContext.Database.Migrate();
            //await dbContext.Database.GetService<IMigrator>().MigrateAsync("0");

            //partial rollback
            //https://github.com/dotnet/efcore/issues/23595
            //https://github.com/dotnet/efcore/issues/9339
            // Create design-time services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEntityFrameworkDesignTimeServices();
            serviceCollection.AddDbContextDesignTimeServices(dbContext);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var migrationsScaffolder = serviceProvider.GetService<IMigrationsScaffolder>();
            //var migration = migrationsScaffolder.ScaffoldMigration("20210308061104_addedTimeSlotsSql", "DBInjectionWithMultiProviders.Plugin1.Migrations.MSSQL");
            try
            {
                migrationsScaffolder.RemoveMigration(AppContext.BaseDirectory, "DBInjectionWithMultiProviders.Plugin1.Migrations.MSSQL.20210308061104_addedTimeSlotsSql", true, "");
            }
            catch (DirectoryNotFoundException)
            {
                //ignore this error as it will occur in a published project where nothing is available to write/remove on a source file
                //dispose context so we can rebuild model
                dbContext.Dispose();
                //GC.Collect();
            }
            //.RemoveMigration(AppDomain.CurrentDomain.RelativeSearchPath, "20210308061104_addedTimeSlotsSql",false,"en-US");
            return Content("Rollback successful");
        }
    }
}
