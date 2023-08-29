using Newtonsoft.Json;
using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Models
{
    public class ProductSupportState
    {
        [JsonProperty("supportStatus")]
        public SupportabilityStatus SupportStatus { get; set; }
    }
}