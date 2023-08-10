using System;
using System.Collections.Generic;
using System.Linq;
namespace Azure.Migrate.Export.Models
{
    public class Financial_Summary
    {
        public string MigrationStrategy { get; set; }
        public string Workload { get; set; }
        public int SourceCount { get; set; }
        public int TargetCount { get; set; }
        public double StorageCost { get; set; }
        public double ComputeCost { get; set; }
        public double TotalAnnualCost { get; set; }
    }
    public class WebApp_PaaS_FS
    {
        public Dictionary<string, int> EnvironmentToUniqueIdMap { get; set; }
        public Dictionary<string, int> PlanToAppCountMap { get; set; }
    }
        public class SQL_MI_PaaS_FS
    {
        public int DevMachinesCountTarget { get; set; }
        public int ProdMachinesCountTarget { get; set; }
        public int DevUniqueMachinesSource { get; set; }
        public int ProdUniqueMachinesSource { get; set; }

    }

    public class WebApp_IaaS_FS
    {
        public int DevMachinesCountTarget { get; set; }
        public int ProdMachinesCountTarget { get; set; }
        public int DevUniqueMachinesSource { get; set; }
        public int ProdUniqueMachinesSource { get; set; }
        public Dictionary<string, HashSet<string>> EnvironmentToUniqueIdMap { get; set; }
        public Dictionary<string, int> EnvironmentCountDictionary { get; set; }
    }
        public class SQL_IaaS_FS
    {
        public int DevMachinesCountTarget { get; set; }
        public int ProdMachinesCountTarget { get; set; }
        public int DevUniqueMachinesSource { get; set; }
        public int ProdUniqueMachinesSource { get; set; }
        public Dictionary<string, HashSet<string>> EnvironmentToUniqueIdMap { get; set; }
        public Dictionary<string, int> EnvironmentCountDictionary { get; set; }
    }
    public class VMServer_IaaS_FS
    {
        public int DevMachinesCountTarget { get; set; }
        public int ProdMachinesCountTarget { get; set; }
        public int DevUniqueMachinesSource { get; set; }
        public int ProdUniqueMachinesSource { get; set; }
        public Dictionary<string, HashSet<string>> EnvironmentToUniqueIdMap { get; set; }
        public Dictionary<string, int> EnvironmentCountDictionary { get; set; }
    }

    public class Management_FS
    {
        public int SourceCount { get; set; }
        public int TargetCount { get; set; }
        public double StorageCost { get; set; } = 0;
        public double BackupComputeCost { get; set; }
        public double RecoveryComputeCost { get; set; }
        public double BackupTotalAnnualCost { get; set; }
        public double RecoveryTotalAnnualCost { get; set; }

    }
}
