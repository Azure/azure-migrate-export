using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class AzureVMWareSolutionAssessmentSettingsJSON
    {
        [JsonProperty("properties")]
        public AzureVMWareSolutionAssessmentSettingsProperty Properties { get; set; } = new AzureVMWareSolutionAssessmentSettingsProperty();
    }

    public class AzureVMWareSolutionAssessmentSettingsProperty
    {
        [JsonProperty("sizingCriterion")]
        public string SizingCriterion { get; set; } = "PerformanceBased";

        [JsonProperty("azureHybridUseBenefit")]
        public string AzureHybridUseBenefit { get; set; } = "No";

        [JsonProperty("reservedInstance")]
        public string ReservedInstance { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; } = "AV36";

        [JsonProperty("failuresToTolerateAndRaidLevel")]
        public string FailuresToTolerateAndRaidLevel { get; set; } = "Ftt1Raid1";

        [JsonProperty("vcpuOversubscription")]
        public string VcpuOversubscription { get; set; } = "4:1";

        [JsonProperty("memOvercommit")]
        public string MemOverCommit { get; set; } = "1";

        [JsonProperty("dedupeCompression")]
        public string DedupeCompression { get; set; } = "1.5";

        [JsonProperty("isStretchClusterEnabled")]
        public string IsStretchClusterEnabled { get; set; } = "No";

        [JsonProperty("percentile")]
        public string Percentile { get; set; } = "Percentile95";

        [JsonProperty("timeRange")]
        public string TimeRange { get; set; }

        [JsonProperty("scalingFactor")]
        public int ScalingFactor { get; set; } = 1;

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("azureOfferCode")]
        public string AzureOfferCode { get; set; } = "MSAZR0003P";

        [JsonProperty("discountPercentage")]
        public int DiscountPercentage { get; set; } = 0;

        [JsonProperty("azureLocation")]
        public string AzureLocation { get; set; }
    }
}