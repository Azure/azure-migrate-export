using System.Collections.Generic;

namespace Azure.Migrate.Export.Common
{
    public class AvsAssessmentConstants
    {
        public static readonly Dictionary<string, List<string>> RegionToAvsNodeTypeMap = new Dictionary<string, List<string>>
        {
            { "eastus", new List<string> { "AV36P" } },
            { "eastus2", new List<string> { "AV36P", "AV52" } },
            { "southcentralus", new List<string> { "AV52", "AV36", "AV36P" } },
            { "westus2", new List<string> { "AV36", "AV36P" } },
            { "australiaeast", new List<string> { "AV36", "AV36P" } },
            { "southeastasia", new List<string> { "AV36", "AV36P" } },
            { "northeurope", new List<string> { "AV36" } },
            { "swedencentral", new List<string> { "AV36" } },
            { "uksouth", new List<string> { "AV36P", "AV52" } },
            { "westeurope", new List<string> { "AV36P", "AV52" } },
            { "centralus", new List<string> { "AV36", "AV36P" } },
            { "southafricanorth", new List<string> { "AV36" } },
            { "eastasia", new List<string> { "AV36", "AV36P" } },
            { "japaneast", new List<string> { "AV36" } },
            { "canadacentral", new List<string> { "AV36", "AV36P" } },
            { "francecentral", new List<string> { "AV36" } },
            { "germanywestcentral", new List<string> { "AV36P" } },                           
            { "brazilsouth", new List<string> { "AV36" } },            
            { "australiasoutheast", new List<string> { "AV36" } },
            { "japanwest", new List<string> { "AV36" } },
            { "ukwest", new List<string> { "AV36" } }
        };
        public static string VCpuOversubscription = "4:1";
        public static readonly string MemoryOvercommit = "100%";
        public static double DedupeCompression = 1.5;
    }
}