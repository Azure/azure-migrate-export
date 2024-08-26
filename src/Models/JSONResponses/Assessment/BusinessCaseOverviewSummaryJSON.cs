using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseOverviewSummaryJSON
    {
        [JsonProperty("properties")]
        public BusinessCaseOverviewSummaryProperties Properties { get; set; }        
    }

    public class BusinessCaseOverviewSummaryProperties
    {
        [JsonProperty("totalAzureAvsCost")]
        public double TotalAzureAvsCost { get; set; }
        
        [JsonProperty("yearOnYearEstimates")]
        public BusinessCaseYOYCostDetailsJSON YearOnYearEstimates { get; set; }
    }
}