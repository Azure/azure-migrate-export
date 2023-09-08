using System.Collections.Generic;
using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class BusinessCasePaaSSummaryJSON
    {
        [JsonProperty("onPremises")]
        public BusinessCaseOnPremisesPaaSSummary OnPremisesPaaSSummary { get; set; }
    }

    public class BusinessCaseOnPremisesPaaSSummary
    {
        [JsonProperty("totalOnPremisesPaasCost")]
        public BusinessCaseCostDetailsJSON TotalOnPremisesPaaSCost { get; set; }

        [JsonProperty("onPremisesPaasLicensingCost")]
        public BusinessCaseOnPremisesPaaSLicensingCost OnPremisesPaaSLicensingCost { get; set; }
    }

    public class BusinessCaseOnPremisesPaaSLicensingCost
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("totalCost")]
        public double TotalCost { get; set; }

        [JsonProperty("decomissionServerCost")]
        public double DecomissionServerCost { get; set; }
    }
}
