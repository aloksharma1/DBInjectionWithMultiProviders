using DBInjectionWithMultiProviders.Domain.Entities;
using DBInjectionWithMultiProviders.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Data
{
    public class InitSeed
    {

        public static void Initialize(PluginDBContext context)
        {
            context.UpsertSeed(
            p => p.UserName,
            p => new
            {
                p.IsActive,
                p.FullName,
                p.Email
            },
            new UserInfo
            {
                IsActive = true,
                FullName = "My user name",
                UserName = "ThisUser",
                Email = "my@email",
            }
           );
        }
    }
}
