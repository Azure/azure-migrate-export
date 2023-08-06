using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class AzureAssessmentCostComponent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
