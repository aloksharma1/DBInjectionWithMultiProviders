using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBInjectionWithMultiProviders.Persistence.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection serviceCollection,
             IConfiguration Configuration, IConfigurationRoot configRoot)
        {
            switch (Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>())
            {
                case DatabaseProvider.MSSQL:
                    serviceCollection.AddDbContext<CoreDbContext>(options =>
                   options.UseSqlServer(Configuration.GetSection("DBProviders:0:ConnectionString").Value
                , b => b.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName)));
                    break;

                case DatabaseProvider.SQLITE:
                    serviceCollection.AddDbContext<CoreDbContext>(options =>
                   options.UseSqlite(Configuration.GetSection("DBProviders:1:ConnectionString").Value
                , b => b.MigrationsAssembly(typeof(CoreDbContext).Assembly.FullName)));
                    break;
                default:
                    break;
            }
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<CoreDbContext>());
        }
    }
}
