using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DBInjectionWithMultiProviders.Abstractions
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class DBProvider
    {
        [JsonPropertyName("ProviderName")]
        public string ProviderName { get; set; }

        [JsonPropertyName("ConnectionString")]
        public string ConnectionString { get; set; }

        [JsonPropertyName("InUse")]
        public bool InUse { get; set; }

        [JsonPropertyName("DevModeDefault")]
        public bool DevModeDefault { get; set; }
    }

    public class DatabaseProviders
    {
        [JsonPropertyName("DBProviders")]
        public List<DBProvider> DBProviders { get; set; }
    }
    public enum DatabaseProvider
    {
        MSSQL,
        SQLITE
    }
    public interface IDatabaseProvider
    {
    }
}
