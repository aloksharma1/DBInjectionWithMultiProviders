using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Abstractions
{
    public interface IPluginDbContext<T>
    {
        public void Setup(T modelBuilder,DatabaseProvider Provider);
    }
}
