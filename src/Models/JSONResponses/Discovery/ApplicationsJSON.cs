using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.Migrate.Export.Models
{
    public class ApplicationsJSON
    {
        [JsonProperty("properties")]
        public ApplicationsProperty Properties { get; set; }
    }

    public class ApplicationsProperty
    {
        [JsonProperty("appsAndRoles")]
        public AppsAndRolesInfo AppsAndRoles { get; set; }
    }

    public class AppsAndRolesInfo
    {
        [JsonProperty("applications")]
        public List<ApplicationsInfo> Applications { get; set; }
    }

    public class ApplicationsInfo
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}