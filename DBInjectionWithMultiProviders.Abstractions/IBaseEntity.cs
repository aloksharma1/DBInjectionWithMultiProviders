using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Abstractions
{
    public interface IBaseEntity<T>  where T : struct
    {
       public T Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
    }
}
