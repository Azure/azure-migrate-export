using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.Migrate.Export.Models
{
    public class VMwareHostsJSON
    {
        [JsonProperty("value")]
        public List<VMwareHostMachinesValue> Values { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    public class VMwareHostMachinesValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}