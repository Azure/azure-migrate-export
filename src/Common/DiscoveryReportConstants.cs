using System.Collections.Generic;

namespace Azure.Migrate.Export.Common
{
    public class DiscoveryReportConstants
    {
        public const string DiscoveryReportDirectory = @".\Discovery-Report";
        public const string DiscoveryReportName = @"AzureMigrate_Discovery_Report.xlsx";
        public const string BackSlash = @"\";
        public const string DiscoveryReportPath = DiscoveryReportDirectory + BackSlash + DiscoveryReportName;
        public const string PropertiesTabName = "Properties";
        public const string Discovery_Report_TabName = "Discovery_Report";
        public const string vCenterHost_Report_TabName = "vCenter_Host_Report";

        public static readonly List<string> PropertiesList = new List<string>
        {
            "Tenant ID",
            "Subscription",
            "Resource Group",
            "Azure Migrate Project Name",
            "Discovery Site Name",
            "Workflow",
            "Source Appliances"
        };

        public static readonly List<string> DiscoveryReportColumns = new List<string>
        {
            "Machine Name",
            "Environment",
            "Software Inventory Count",
            "SQL Discovery Server Count",
            "Is SQL Service Present",
            "Web Applications Count",
            "Operating System",
            "Cores",
            "Memory (in MB)",
            "Total Disks",
            "IP Addresses",
            "MAC Addresses",
            "Total Network Adapters",
            "Boot Type",
            "Power Stats",
            "Support Status",
            "First Discovery Time",
            "Last Updated Time",
            "Machine ID"
        };

        public static readonly List<string> VCenterHostReportColumns = new List<string>
        {
            "vCenters",
            "Hosts"
        };
    }
}