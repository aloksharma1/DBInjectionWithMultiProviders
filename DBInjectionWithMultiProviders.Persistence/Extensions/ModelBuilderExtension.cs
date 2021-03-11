using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBInjectionWithMultiProviders.Persistence.Extensions
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder ApplyAllConfigurationsFromNamespace(
        this ModelBuilder modelBuilder, Assembly assembly, string ns)
        {
            var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Single(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
            && m.GetParameters().Count() == 1
            && m.GetParameters().Single().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
            foreach (var type in assembly.GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters
                    && c.Namespace == ns
                ))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
            return modelBuilder;
        }
        public static bool CheckIfColumnOnTableExists(this DbContext dbContext, string table, string column)
        {
            using (dbContext)
            {
                var result = dbContext.Database.ExecuteSqlRaw($@"SELECT Count(*)
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{table}'
            AND COLUMN_NAME = '{column}'");
                return result == 1;
            }
        }
        public static bool CheckIfColumnOnTableExists(string connectionString, string table, string column)
        {
            var optionBuilder = new DbContextOptionsBuilder<DbContext>();
            optionBuilder.UseSqlServer(connectionString);
            var context = new DbContext(optionBuilder.Options);
            using (context)
            {
                var result = context.Database.ExecuteSqlRaw($@"SELECT Count(*)
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{table}'
            AND COLUMN_NAME = '{column}'");
                return result == 1;
            }
        }
    }
}
