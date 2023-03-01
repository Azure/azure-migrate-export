using System.Collections.Generic;

namespace Azure.Migrate.Export.Common
{
    public class ClashReportConstants
    {
        public const string ClashReportDirectory = @".\Clash-Report";
        public const string ClashReportName = @"AzureMigrate_Assessment_Clash_Report.xlsx";
        public const string BackSlash = @"\";
        public const string ClashReportPath = ClashReportDirectory + BackSlash + ClashReportName;

        public const string Clash_Report_TabName = "Clash_Report";
        public static readonly List<string> Clash_Report_Columns = new List<string>
        {
            "Machine Name",
            "Environment",
            "Operating System",
            "Boot Type",
            "IP Addresses",
            "MAC Addresses",
            "VM_IaaS_Server_Rehost_Perf",
            "SQL_IaaS_Instance_Rehost_Perf",
            "SQL_MI_PaaS",
            "SQL_IaaS_Server_Rehost_Perf",
            "WebApp_PaaS",
            "WebApp_IaaS_Server_Rehost_Perf",
            "VM_SS_IaaS_Server_Rehost_Perf",
            "VM Host",
            "Machine ID"
        };
    }
}