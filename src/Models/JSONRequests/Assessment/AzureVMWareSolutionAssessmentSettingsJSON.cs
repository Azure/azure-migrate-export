using System.Collections.Generic;
using Newtonsoft.Json;

using Azure.Migrate.Export.Common;

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

        [JsonProperty("nodeTypes")]
        public List<string> NodeTypes { get; set; }

        [JsonProperty("failuresToTolerateAndRaidLevelList")]
        public List<string> FailuresToTolerateAndRaidLevelList { get; set; } = new List<string> { "Ftt1Raid1", "Ftt1Raid5", "Ftt2Raid1", "Ftt2Raid6", "Ftt3Raid1" };

        [JsonProperty("vcpuOversubscription")]
        public string VcpuOversubscription { get; set; } = AvsAssessmentConstants.VCpuOversubscription;

        [JsonProperty("memOvercommit")]
        public string MemOverCommit { get; set; } = "1";

        [JsonProperty("dedupeCompression")]
        public string DedupeCompression { get; set; } = AvsAssessmentConstants.DedupeCompression.ToString();

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

        [JsonProperty("externalStorageTypes")]
        public List<string> ExternalStorageTypes { get; set; } = new List<string> { "AnfStandard", "AnfPremium", "AnfUltra" };
    }
}