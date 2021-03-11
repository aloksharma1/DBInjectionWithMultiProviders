using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DBInjectionWithMultiProviders.Persistence.Extensions
{

    /// <summary>
    /// https://stackoverflow.com/questions/25928353/making-addorupdate-change-only-some-properties
    /// source : https://gist.github.com/twilly86/eb6b61a22b66b4b33717aff84a31a060
    /// Alternative to db migration AddOrUpdate, will only update specific fields, instead of overwriting everyting
    /// Usuage:
    /// context.Upsert(
    ///  p => p.UserName,   
    ///  p => new { p.Active, p.FullName, p.Email
    ///},
    /// new User
    ///    {
    ///        Active = true,
    ///        FullName = "My user name",
    ///       UserName = "ThisUser",
    ///        Email = "my@email",
    ///    }
    ///);
    /// </summary>
    public static class SeedExtension
    {
        public static void UpsertSeed<T>(this DbContext db, Expression<Func<T, object>> identifierExpression, Expression<Func<T, object>> updatingExpression, params T[] entities) where T : class
        {
            if (updatingExpression == null)
                throw new Exception($"updatingExpression cannot be null");


            var identifyingProperties = GetProperties<T>(identifierExpression).ToList();
            Debug.Assert(identifyingProperties.Count != 0);

            var updatingProperties = GetProperties<T>(updatingExpression).Where(pi => IsModifiedable(pi.PropertyType)).ToList();
            Debug.Assert(updatingProperties.Count != 0);

            // get all records at once, generally we should onyl be using this when seeding so seeding records should be fairly limited
            var records = db.Set<T>().ToList();

            var added = new List<T>();

            var parameter = Expression.Parameter(typeof(T));
            foreach (var entity in entities)
            {
                var matches = identifyingProperties.Select(pi => Expression.Equal(Expression.Property(parameter, pi.Name), Expression.Convert(Expression.Constant(pi.GetValue(entity, null)), Expression.Property(parameter, pi.Name).Type)));

                var matchExpression = matches.Aggregate<BinaryExpression, Expression>(null, (agg, v) => (agg == null) ? v : Expression.AndAlso(agg, v));

                var predicate = Expression.Lambda<Func<T, bool>>(matchExpression, new[] { parameter });
                var existing = records.AsQueryable().SingleOrDefault(predicate);
                if (existing == null)
                {
                    // New.
                    added.Add(entity);
                    continue;
                }

                // Update.

                foreach (var prop in updatingProperties)
                {
                    var oldValue = prop.GetValue(existing, null);
                    var newValue = prop.GetValue(entity, null);
                    if (Equals(oldValue, newValue)) continue;

                    db.Entry(existing).Property(prop.Name).IsModified = true;
                    prop.SetValue(existing, newValue);
                }
            }

            if (added.Any())
                db.Set<T>().AddRange(added);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/25928353/making-addorupdate-change-only-some-properties
        /// Alternative to db migration AddOrUpdate, will only add items
        /// </summary>
        public static void AddDontUpdateSeed<T>(this DbContext db, Expression<Func<T, object>> identifierExpression, params T[] entities) where T : class
        {
            var identifyingProperties = GetProperties<T>(identifierExpression).ToList();
            Debug.Assert(identifyingProperties.Count != 0);

            // get all records at once, generally we should onyl be using this when seeding so seeding records should be fairly limited
            var records = db.Set<T>().ToList();

            var added = new List<T>();

            var parameter = Expression.Parameter(typeof(T));
            foreach (var entity in entities)
            {
                var matches = identifyingProperties.Select(pi => Expression.Equal(Expression.Property(parameter, pi.Name), Expression.Convert(Expression.Constant(pi.GetValue(entity, null)), Expression.Property(parameter, pi.Name).Type)));

                var matchExpression = matches.Aggregate<BinaryExpression, Expression>(null, (agg, v) => (agg == null) ? v : Expression.AndAlso(agg, v));

                var predicate = Expression.Lambda<Func<T, bool>>(matchExpression, new[] { parameter });
                var existing = records.AsQueryable().SingleOrDefault(predicate);

                if (existing == null)
                {
                    // New.
                    added.Add(entity);
                }
            }

            if (added.Any())
                db.Set<T>().AddRange(added);
        }



        private static bool IsModifiedable(Type type)
        {
            return type.IsPrimitive || type.IsValueType || type == typeof(string);
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>(Expression<Func<T, object>> exp) where T : class
        {
            Debug.Assert(exp != null);
            Debug.Assert(exp.Body != null);
            Debug.Assert(exp.Parameters.Count == 1);

            var type = typeof(T);
            var properties = new List<PropertyInfo>();

            if (exp.Body.NodeType == ExpressionType.MemberAccess)
            {
                if (exp.Body is MemberExpression memExp && memExp.Member != null)
                    properties.Add(type.GetProperty(memExp.Member.Name));
            }
            else if (exp.Body.NodeType == ExpressionType.Convert)
            {
                if (exp.Body is UnaryExpression unaryExp)
                {
                    if (unaryExp.Operand is MemberExpression propExp && propExp.Member != null)
                        properties.Add(type.GetProperty(propExp.Member.Name));
                }
            }
            else if (exp.Body.NodeType == ExpressionType.New)
            {
                if (exp.Body is NewExpression newExp)
                    properties.AddRange(newExp.Members.Select(x => type.GetProperty(x.Name)));
            }

            return properties.OfType<PropertyInfo>();
        }
    }
}
