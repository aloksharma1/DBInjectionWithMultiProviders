using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Extensions;
using DBInjectionWithMultiProviders.Persistence;
using DBInjectionWithMultiProviders.Plugin1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBInjectionWithMultiProviders.Plugin1.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddPluginDbContext(this IServiceCollection serviceCollection,
             IConfiguration Configuration, IConfigurationRoot configRoot)
        {
            switch (Configuration.GetSection("CurrentDBProvider").Value.GetEnumValue<DatabaseProvider>())
            {
                case DatabaseProvider.MSSQL:
                    serviceCollection.AddDbContext<PluginDBContext>(options =>
                   options.UseSqlServer(Configuration.GetSection("DBProviders:0:ConnectionString").Value
                , b => b.MigrationsAssembly(typeof(PluginDBContext).Assembly.FullName)));
                    break;

                case DatabaseProvider.SQLITE:
                    serviceCollection.AddDbContext<PluginDBContext>(options =>
                   options.UseSqlite(Configuration.GetSection("DBProviders:1:ConnectionString").Value
                , b => b.MigrationsAssembly(typeof(PluginDBContext).Assembly.FullName)));
                    break;
                default:
                    break;
            }
            serviceCollection.AddScopedServices();
        }
        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<PluginDBContext>());
            serviceCollection.Add(new ServiceDescriptor(typeof(IRepository<>), typeof(Repository<>), ServiceLifetime.Scoped));
        }
    }
}
