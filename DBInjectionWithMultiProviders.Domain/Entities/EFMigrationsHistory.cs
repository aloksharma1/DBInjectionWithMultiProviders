using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBInjectionWithMultiProviders.Domain.Entities
{
    public class EFMigrationsHistory
    {
        [Key]
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}
