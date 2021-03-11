using DBInjectionWithMultiProviders.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace DBInjectionWithMultiProviders.Domain
{
    public class BaseEntity : IBaseEntity<long>
    {
        [Key]
        public long Id { get ; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
