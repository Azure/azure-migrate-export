using System.Collections.Generic;

namespace Azure.Migrate.Export.Common
{
    public class OpportunityReportConstants
    {
        public const string OpportunityReportDirectory = @".\Opportunity-Report";
        public const string OpportunityReportName = @"AzureMigrate_Assessment_Opportunity_Report.xlsx";
        public const string BackSlash = @"\";
        public const string OpportunityReportPath = OpportunityReportDirectory + BackSlash + OpportunityReportName;

        public const string AVS_IaaS_Rehost_Perf_TabName = "AVS_IaaS_Rehost_Perf";
        public static readonly List<string> AVS_IaaS_Rehost_Perf_Columns = new List<string>
        {
            "Machine Name",
            "Azure VMWare Solution Readiness",
            "Azure VMWare Solution Readiness - Warnings",
            "Operating System",
            "Operating System Version",
            "Operating System Architecture",
            "Boot Type",
            "Cores",
            "Memory (in MB)",
            "Storage (in GB)",
            "Storage Utilization (in GB)",
            "Disk Read (in OPS)",
            "Disk Write (in OPS)",
            "Disk Read (in MBPS)",
            "Disk Write (in MBPS)",
            "Network Adapters",
            "IP Addresses",
            "MAC Addresses",
            "Network in (in MBPS)",
            "Network out (in MBPS)",
            "Disk Names",
            "Group Name",
            "Machine ID"
        };

        public const string AVS_Summary_TabName = "AVS_Summary";
        public static readonly List<string> AVS_Summary_Columns = new List<string>
        {
            "Subscription ID",
            "Resource Group",
            "Project Name",
            "Group Name",
            "Assessment Name",
            "Sizing Criterion",
            "Assessment Type",
            "Created on",
            "Total Machines Assessed",
            "Machines Ready",
            "Machines Ready with Conditions",
            "Machines not Ready",
            "Machines Readiness Unknown",
            "Recommended Number of Nodes",
            "Node Type",
            "Monthly Total Cost Estimate",
            "Predicted CPU Utilization (in %)",
            "Predicted Memory Utilization (in %)",
            "Predicted Storage Utilization (in %)",
            "Number of CPU Cores - Available",
            "Memory - Available (in TB)",
            "Storage - Available (in TB)",
            "Number of CPU Cores - Used",
            "Memory - Used (in TB)",
            "Storage - Used (in TB)",
            "Number of CPU Cores - Free",
            "Memory - Free (in TB)",
            "Storage - Free (in TB)",
            "Confidence Rating"
        };

        public const string SQL_MI_Issues_and_Warnings_TabName = "SQL_MI_Issues_and_Warnings";
        public static readonly List<string> SQL_MI_Issues_and_Warnings_Columns = new List<string>
        {
            "Machine Name",
            "SQL Instance",
            "Migration Readiness Target",
            "Category",
            "Title",
            "Impacted Object Type",
            "Impacted Object Name",
            "User Databases",
            "Machine ID"
        };

        public const string SQL_MI_Opportunity_TabName = "SQL_MI_Opportunity";
        public static readonly List<string> SQL_MI_Opportunity_Columns = new List<string>
        {
            "Machine Name",
            "SQL Instance",
            "Environment",
            "Azure SQL MI Readiness",
            "Azure SQL MI Readiness - Warnings",
            "Azure SQL MI Readiness - Issues",
            "Recommended Deployment Type",
            "Azure SQL MI Configuration",
            "Monthly Compute Cost Estimate (Pay as you go)",
            "Monthly Compute Cost Estimate (Pay as you go + RI)",
            "Monthly Compute Cost Estimate (Pay as you go + AHUB)",
            "Monthly Compute Cost Estimate (Pay as you go + AHUB + RI)",
            "Monthly Storage Cost Estimate",
            "User Databases",
            "SQL Edition",
            "SQL Version",
            "Total DB Size (in MB)",
            "Largest DB Size (in MB)",
            "vCores Allocated",
            "CPU Utilization (in %)",
            "Memory Utilization (in MB)",
            "Number of Disks",
            "Disk Read (in OPS)",
            "Disk Write (in OPS)",
            "Disk Read (in MBPS)",
            "Disk Write (in MBPS)",
            "Confidence Rating (in %)",
            "Azure SQL MI Configuration - Target Service Tier",
            "Azure SQL MI Configuration - Target Compute Tier",
            "Azure SQL MI Configuration - Target Hardware Type",
            "Azure SQL MI Configuration - Target vCores",
            "Azure SQL MI Configuration - Target Storage (in GB)",
            "Group Name",
            "Machine ID"
        };

        public const string VM_Opportunity_AsOnPrem_TabName = "VM_Opportunity_As-is";
        public static readonly List<string> VM_Opportunity_AsOnPrem_Columns = new List<string>
        {
            "Machine Name",
            "Environment",
            "Azure VM Readiness",
            "Recommended VM Size",
            "Monthly Compute Cost Estimate",
            "Monthly Storage Cost Estimate",
            "Operating System",
            "VM Host",
            "Boot Type",
            "Cores",
            "Memory (in MB)",
            "Storage (in GB)",
            "Network Adapters",
            "IP Addresses",
            "MAC Addresses",
            "Disk Names",
            "Azure Disk Readiness",
            "Recommended Disk SKUs",
            "Standard HDD Disks",
            "Standard SSD Disks",
            "Premium Disks",
            "Ultra Disks",
            "Monthly Storage Cost for Standard HDD Disks",
            "Monthly Storage Cost for Standard SSD Disks",
            "Monthly Storage Cost for Premium Disks",
            "Monthly Storage Cost for Ultra Disks",
            "Group Name",
            "Machine ID"
        };

        public const string VM_Opportunity_Perf_TabName = "VM_Opportunity_Perf";
        public static readonly List<string> VM_Opportunity_Perf_Columns = new List<string>
        {
            "Machine Name",
            "Environment",
            "Azure VM Readiness",
            "Azure VM Readiness - Warnings",
            "Recommended VM Size",
            "Monthly Compute Cost Estimate (Pay as you go)",
            "Monthly Compute Cost Estimate (Pay as you go + RI)",
            "Monthly Compute Cost Estimate (Pay as you go + AHUB)",
            "Monthly Compute Cost Estimate (Pay as you go + AHUB + RI)",
            "Monthly Compute Cost Estimate (Pay as you go + ASP)",
            "Monthly Storage Cost Estimate",
            "Operating System",
            "VM Host",
            "Boot Type",
            "Cores",
            "Memory (in MB)",
            "CPU Utilization (in %)",
            "Memory Utilization (in %)",
            "Storage (in GB)",
            "Network Adapters",
            "IP Addresses",
            "MAC Addresses",
            "Disk Names",
            "Azure Disk Readiness",
            "Recommended Disk SKUs",
            "Standard HDD Disks",
            "Standard SSD Disks",
            "Premium Disks",
            "Ultra Disks",
            "Monthly Storage Cost for Standard HDD Disks",
            "Monthly Storage Cost for Standard SSD Disks",
            "Monthly Storage Cost for Premium Disks",
            "Monthly Storage Cost for Ultra Disks",
            "Monthly Azure Site Recovery Cost Estimate",
            "Monthly Azure Backup Cost Estimate",
            "Group Name",
            "Machine ID"
        };

        public const string WebApp_Opportunity_TabName = "WebApp_Opportunity";
        public static readonly List<string> WebApp_Opportunity_Columns = new List<string>
        {
            "Machine Name",
            "Web Application Name",
            "Environment",
            "Azure App Service Readiness",
            "Azure App Service Readiness - Issues",
            "Azure Recommended Target",
            "Group Name",
            "Machine ID",
        };
    }
}