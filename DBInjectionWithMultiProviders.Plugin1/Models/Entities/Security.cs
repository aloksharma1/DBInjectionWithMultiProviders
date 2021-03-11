using DBInjectionWithMultiProviders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1.Models.Entities
{
    public class Security : BaseEntity
    {
        public string LoginIp { get; set; }
        public DateTime? LoginDate { get; set; } = DateTime.UtcNow;
        public long? LoginUserId { get; set; }
        public DateTime? loginTime { get; set; }
        public DateTime? logoutTime { get; set; }
    }
}
