using Newtonsoft.Json;
using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Models
{
    public class ProductSupportState
    {
        [JsonProperty("supportStatus")]
        [JsonConverter(typeof(SupportabilityStatusEnumConverter))]
        public SupportabilityStatus SupportStatus { get; set; }
    }
}