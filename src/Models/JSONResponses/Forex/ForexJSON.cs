using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.Migrate.Export.Models
{
    public class ForexJSON
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }
}
