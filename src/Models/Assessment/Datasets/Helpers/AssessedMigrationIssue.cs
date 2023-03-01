using System.Collections.Generic;

using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Models
{
    public class AssessedMigrationIssue
    {
        public string IssueId { get; set; }
        public IssueCategories IssueCategory { get; set; }
        public List<string> IssueDescriptionList { get; set; } = new List<string>();
        public List<ImpactedObjectInfo> ImpactedObjects { get; set; } = new List<ImpactedObjectInfo>();
    }
}