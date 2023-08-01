using Newtonsoft.Json;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Models
{
    public class AzureSQLRecommendedAssessedEntitiesJSON
    {
        [JsonProperty("value")]
        public List<AzureSQLRecommendedAssessedEntityValue> Values { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    public class AzureSQLRecommendedAssessedEntityValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public AzureSQLRecommendedAssessedEntityProperty Properties { get; set; }
    }

    public class AzureSQLRecommendedAssessedEntityProperty
    {
        [JsonProperty("recommendedAzureSqlTargetType")]
        public AzureSQLTargetType RecommendedAzureSqlTargetType { get; set; }

        [JsonProperty("recommendedSuitability")]
        public AzureSQLCloudSuitability RecommendedSuitability { get; set; }

        [JsonProperty("assessedSqlEntityArmId")]
        public string AssessedSqlEntityArmId { get; set; }
    }
}