using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseCostDetailsJSON
    {
        [JsonProperty("storageCost")]
        public double? StorageCost { get; set; }

        [JsonProperty("computeCost")]
        public double? ComputeCost { get; set; }

        [JsonProperty("itLaborCost")]
        public double? ITLaborCost { get; set; }

        [JsonProperty("networkCost")]
        public double? NetworkCost { get; set; }

        [JsonProperty("ahubSavings")]
        public double? AHUBSavings { get; set; }

        [JsonProperty("esuSavings")]
        public double? ESUSavings { get; set; }

        [JsonProperty("securityCost")]
        public double? SecurityCost { get; set; }
    }
}
