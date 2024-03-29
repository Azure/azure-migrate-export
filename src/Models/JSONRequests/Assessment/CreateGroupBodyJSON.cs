using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class CreateGroupBodyJSON
    {
        [JsonProperty("properties")]
        public CreateGroupProperty Properties { get; set; } = new CreateGroupProperty();

        [JsonProperty("eTag")]
        public string ETag { get; set; } = "*";
    }

    public class CreateGroupProperty
    {
        [JsonProperty("groupType")]
        public string GroupType { get; set; } = "Default";
    }
}