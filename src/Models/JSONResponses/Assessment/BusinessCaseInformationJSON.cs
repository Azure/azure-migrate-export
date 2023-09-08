using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    class BusinessCaseInformationJSON
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("properties")]
        public BusinessCaseInformationProperty Properties { get; set; }
    }

    public class BusinessCaseInformationProperty
    {
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
