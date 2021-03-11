using DBInjectionWithMultiProviders.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1
{
    public class Module : IPlugin
    {
        public string GetName()
        {
            return "Plugin1";
        }
    }
}
