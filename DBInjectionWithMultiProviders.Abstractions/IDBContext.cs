using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Abstractions
{
    public interface IDBContext<T>
    {
        public void Setup(T modelBuilder);
    }
}
