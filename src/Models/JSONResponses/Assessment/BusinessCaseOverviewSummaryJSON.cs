using Newtonsoft.Json;

namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseOverviewSummaryJSON
    {
        [JsonProperty("yearOnYearEstimates")]
        public BusinessCaseYOYCostDetailsJSON YearOnYearEstimates { get; set; }
    }
}
