using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseCompareSummaryJSON
    {
        [JsonProperty("azurePaasCostDetails")]
        public BusinessCaseCostDetailsJSON AzurePaaSCostDetails { get; set; }

        [JsonProperty("azureIaasCostDetails")]
        public BusinessCaseCostDetailsJSON AzureIaaSCostDetails { get; set; }

        [JsonProperty("onPremisesPaasCostDetails")]
        public BusinessCaseCostDetailsJSON OnPremisesPaaSCostDetails { get; set; }

        [JsonProperty("onPremisesIaasCostDetails")]
        public BusinessCaseCostDetailsJSON OnPremisesIaaSCostDetails { get; set;  }
    }
}
