using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Excel;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Assessment.Processor
{
    public class ProcessDatasets
    {
        private readonly Dictionary<string, string> AssessmentIdToDiscoveryIdLookup;
        // TODO: trim if not required
        // Independent
        //private readonly Dictionary<string, List<AssessmentSiteMachine>> AzureVM;
        //private readonly Dictionary<string, List<AssessmentSiteMachine>> AzureSql;
        //private readonly Dictionary<string, List<AssessmentSiteMachine>> AzureWebApp;
        private HashSet<string> AzureWebApp_IaaS;
        //List<AssessmentSiteMachine> AzureVMWareSolution;

        // Dependent
        private readonly HashSet<string> SqlServicesVM;
        private readonly HashSet<string> GeneralVM;

        // Datasets
        private readonly Dictionary<string, AzureVMPerformanceBasedDataset> AzureVMPerformanceBasedMachinesData;
        private readonly Dictionary<string, AzureVMAsOnPremDataset> AzureVMAsOnPremMachinesData;
        private readonly Dictionary<AssessmentInformation, AVSAssessmentPropertiesDataset> AVSAssessmentsData;
        private readonly Dictionary<string, AVSAssessedMachinesDataset> AVSAssessedMachinesData;
        private readonly Dictionary<string, AzureWebAppDataset> AzureWebAppData;
        private readonly Dictionary<string, AzureSQLInstanceDataset> AzureSQLInstancesData;
        private readonly Dictionary<string, AzureSQLMachineDataset> AzureSQLMachinesData;

        private UserInput UserInputObj;

        public ProcessDatasets
            (
                Dictionary<string, string> assessmentIdToDiscoveryIdLookup,

                HashSet<string> azureWebApp_IaaS,
                HashSet<string> sqlServicesVM,
                HashSet<string> generalVM,

                Dictionary<string, AzureVMPerformanceBasedDataset> azureVMPerformanceBasedMachinesData,
                Dictionary<string, AzureVMAsOnPremDataset> azureVMAsOnPremMachinesData,
                Dictionary<AssessmentInformation, AVSAssessmentPropertiesDataset> avsAssessmentsData,
                Dictionary<string, AVSAssessedMachinesDataset> avsAssessedMachinesData,
                Dictionary<string, AzureWebAppDataset> azureWebAppData,
                Dictionary<string, AzureSQLInstanceDataset> azureSQLInstancesData,
                Dictionary<string, AzureSQLMachineDataset> azureSQLMachinesData,

                UserInput userInputObj
            )
        {
            AssessmentIdToDiscoveryIdLookup = assessmentIdToDiscoveryIdLookup;

            AzureWebApp_IaaS = azureWebApp_IaaS;
            SqlServicesVM = sqlServicesVM;
            GeneralVM = generalVM;

            AzureVMPerformanceBasedMachinesData = azureVMPerformanceBasedMachinesData;
            AzureVMAsOnPremMachinesData = azureVMAsOnPremMachinesData;
            AVSAssessmentsData = avsAssessmentsData;
            AVSAssessedMachinesData = avsAssessedMachinesData;
            AzureWebAppData = azureWebAppData;
            AzureSQLInstancesData = azureSQLInstancesData;
            AzureSQLMachinesData = azureSQLMachinesData;

            UserInputObj = userInputObj;
        }

        public void InititateProcessing()
        {
            if (UserInputObj == null)
                throw new Exception("Received null user input object for processing datasets.");

            UserInputObj.LoggerObj.LogInformation("Processing datasets to generate excel models");

            // Core report models
            CoreProperties corePropertiesObj = new CoreProperties();
            List<All_VM_IaaS_Server_Rehost_Perf> All_VM_IaaS_Server_Rehost_Perf_List = new List<All_VM_IaaS_Server_Rehost_Perf>();
            List<SQL_All_Instances> SQL_All_Instances_List = new List<SQL_All_Instances>();
            List<SQL_MI_PaaS> SQL_MI_PaaS_List = new List<SQL_MI_PaaS>();
            List<SQL_IaaS_Instance_Rehost_Perf> SQL_IaaS_Instance_Rehost_Perf_List = new List<SQL_IaaS_Instance_Rehost_Perf>();
            List<SQL_IaaS_Server_Rehost_Perf> SQL_IaaS_Server_Rehost_Perf_List = new List<SQL_IaaS_Server_Rehost_Perf>();
            List<SQL_IaaS_Server_Rehost_AsOnPrem> SQL_IaaS_Server_Rehost_AsOnPrem_List = new List<SQL_IaaS_Server_Rehost_AsOnPrem>();
            List<WebApp_PaaS> WebApp_PaaS_List = new List<WebApp_PaaS>();
            List<WebApp_IaaS_Server_Rehost_Perf> WebApp_IaaS_Server_Rehost_Perf_List = new List<WebApp_IaaS_Server_Rehost_Perf>();
            List<WebApp_IaaS_Server_Rehost_AsOnPrem> WebApp_IaaS_Server_Rehost_AsOnPrem_List = new List<WebApp_IaaS_Server_Rehost_AsOnPrem>();
            List<VM_SS_IaaS_Server_Rehost_Perf> VM_SS_IaaS_Server_Rehost_Perf_List = new List<VM_SS_IaaS_Server_Rehost_Perf>();
            List<VM_SS_IaaS_Server_Rehost_AsOnPrem> VM_SS_IaaS_Server_Rehost_AsOnPrem_List = new List<VM_SS_IaaS_Server_Rehost_AsOnPrem>();
            List<VM_IaaS_Server_Rehost_Perf> VM_IaaS_Server_Rehost_Perf_List = new List<VM_IaaS_Server_Rehost_Perf>();
            List<VM_IaaS_Server_Rehost_AsOnPrem> VM_IaaS_Server_Rehost_AsOnPrem_List = new List<VM_IaaS_Server_Rehost_AsOnPrem>();

            // Opportunity report models
            List<AVS_Summary> AVS_Summary_List = new List<AVS_Summary>();
            List<AVS_IaaS_Rehost_Perf> AVS_IaaS_Rehost_Perf_List = new List<AVS_IaaS_Rehost_Perf>();
            List<SQL_MI_Issues_and_Warnings> SQL_MI_Issues_and_Warnings_List = new List<SQL_MI_Issues_and_Warnings>();
            List<SQL_MI_Opportunity> SQL_MI_Opportunity_List = new List<SQL_MI_Opportunity>();
            List<WebApp_Opportunity> WebApp_Opportunity_List = new List<WebApp_Opportunity>();
            List<VM_Opportunity_Perf> VM_Opportunity_Perf_List = new List<VM_Opportunity_Perf>();
            List<VM_Opportunity_AsOnPrem> VM_Opportunity_AsOnPrem_List = new List<VM_Opportunity_AsOnPrem>();

            // Clash report models
            List<Clash_Report> Clash_Report_List = new List<Clash_Report>();

            // Dependent lists
            HashSet<string> AzureSQL_IaaS_Instance = new HashSet<string>(); // Also the list for SQL_MI_Opportunity
            HashSet<string> AzureSQL_IaaS_Server = new HashSet<string>();
            HashSet<string> AzureVM_Opportunity_Perf = new HashSet<string>();
            HashSet<string> AzureVM_Opportunity_AsOnPrem = new HashSet<string>();
            HashSet<string> AzureWebApp_Opportunity = new HashSet<string>();
            if (AzureWebApp_IaaS == null)
                AzureWebApp_IaaS = new HashSet<string>();

            // Core report tabs
            CreateCorePropertiesModel(corePropertiesObj);
            Process_All_VM_IaaS_Server_Rehost_Perf_Model(All_VM_IaaS_Server_Rehost_Perf_List);
            Process_SQL_All_Instances_Model(SQL_All_Instances_List); // should be the first SQL core report model to be processed.
            Process_SQL_MI_PaaS_Model(SQL_MI_PaaS_List, AzureSQL_IaaS_Instance, AzureSQL_IaaS_Server);
            Process_SQL_IaaS_Instance_Rehost_Perf_Model(SQL_IaaS_Instance_Rehost_Perf_List, AzureSQL_IaaS_Instance, AzureSQL_IaaS_Server);
            Process_SQL_IaaS_Server_Rehost_Perf_Model(SQL_IaaS_Server_Rehost_Perf_List, AzureSQL_IaaS_Server, AzureVM_Opportunity_Perf);
            Process_SQL_IaaS_Server_Rehost_AsOnPrem_Model(SQL_IaaS_Server_Rehost_AsOnPrem_List, AzureSQL_IaaS_Server, AzureVM_Opportunity_AsOnPrem);
            Process_WebApp_PaaS_Model(WebApp_PaaS_List, AzureWebApp_IaaS, AzureWebApp_Opportunity);
            Process_WebApp_IaaS_Server_Rehost_Perf_Model(WebApp_IaaS_Server_Rehost_Perf_List, AzureWebApp_IaaS, AzureVM_Opportunity_Perf);
            Process_WebApp_IaaS_Server_Rehost_AsOnPrem_Model(WebApp_IaaS_Server_Rehost_AsOnPrem_List, AzureWebApp_IaaS, AzureVM_Opportunity_AsOnPrem);
            Process_VM_SS_IaaS_Server_Rehost_Perf_Model(VM_SS_IaaS_Server_Rehost_Perf_List, AzureVM_Opportunity_Perf, AzureSQL_IaaS_Server);
            Process_VM_SS_IaaS_Server_Rehost_AsOnPrem_Model(VM_SS_IaaS_Server_Rehost_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem, AzureSQL_IaaS_Server);
            Process_VM_IaaS_Server_Rehost_Perf_Model(VM_IaaS_Server_Rehost_Perf_List, AzureVM_Opportunity_Perf);
            Process_VM_IaaS_Server_Rehost_AsOnPrem_Model(VM_IaaS_Server_Rehost_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem);

            // Opportunity report tabs
            Process_AVS_Summary_Model(AVS_Summary_List);
            Process_AVS_IaaS_Rehost_Perf_Model(AVS_IaaS_Rehost_Perf_List);
            Process_SQL_MI_Issues_and_Warnings_Model(SQL_MI_Issues_and_Warnings_List);
            Process_SQL_MI_Opportunity_Model(SQL_MI_Opportunity_List, AzureSQL_IaaS_Instance);
            Process_WebApp_Opportunity_Model(WebApp_Opportunity_List, AzureWebApp_Opportunity);
            Process_VM_Opportunity_Perf_Model(VM_Opportunity_Perf_List, AzureVM_Opportunity_Perf);
            Process_VM_Opportunity_AsOnPrem_Model(VM_Opportunity_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem);

            // Generate the clash report
            UserInputObj.LoggerObj.LogInformation("Generating excel model for Clash_Report");
            foreach (var kvp in AzureVMPerformanceBasedMachinesData)
            {
                Clash_Report obj = new Clash_Report();
                
                obj.MachineName = kvp.Value.DisplayName;
                obj.Environment = kvp.Value.Environment;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(kvp.Value.OperatingSystem);
                obj.BootType = UtilityFunctions.GetStringValue(kvp.Value.BootType);

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(kvp.Value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IPAddresses = macIpKvp.Value;

                obj.VMHost = UtilityFunctions.GetStringValue(kvp.Value.DatacenterManagementServerName);
                obj.MachineId = kvp.Value.DatacenterMachineArmId;
                
                int count = 0;
                foreach (var value in VM_IaaS_Server_Rehost_Perf_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.VM_IaaS_Server_Rehost_Perf_Clash = count;

                count = 0;
                foreach (var value in SQL_IaaS_Instance_Rehost_Perf_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.SQL_IaaS_Instance_Rehost_Perf_Clash = count;

                count = 0;
                foreach (var value in SQL_MI_PaaS_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.SQL_MI_PaaS_Clash = count;

                count = 0;
                foreach (var value in SQL_IaaS_Server_Rehost_Perf_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.SQL_IaaS_Server_Rehost_Perf_Clash = count;

                count = 0;
                foreach (var value in WebApp_PaaS_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.WebApp_PaaS_Clash = count;

                count = 0;
                foreach (var value in WebApp_IaaS_Server_Rehost_Perf_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.WebApp_IaaS_Server_Rehost_Perf_Clash = count;

                count = 0;
                foreach (var value in VM_SS_IaaS_Server_Rehost_Perf_List)
                    if (kvp.Key.Equals(value.MachineId))
                        count += 1;
                obj.VM_SS_IaaS_Server_Rehost_Perf_Clash = count;

                Clash_Report_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated Clash_Report excel model with data of {Clash_Report_List.Count} machines");

            UserInputObj.LoggerObj.LogInformation(90 - UserInputObj.LoggerObj.GetCurrentProgress(), "Completed job for creating excel models");

            UserInputObj.LoggerObj.LogInformation("Generating core report excel sheet");
            ExportCoreReport exportCoreReportObj = new ExportCoreReport
                (
                    corePropertiesObj,
                    All_VM_IaaS_Server_Rehost_Perf_List,
                    SQL_All_Instances_List,
                    SQL_MI_PaaS_List,
                    SQL_IaaS_Instance_Rehost_Perf_List,
                    SQL_IaaS_Server_Rehost_Perf_List,
                    SQL_IaaS_Server_Rehost_AsOnPrem_List,
                    WebApp_PaaS_List,
                    WebApp_IaaS_Server_Rehost_Perf_List,
                    WebApp_IaaS_Server_Rehost_AsOnPrem_List,
                    VM_SS_IaaS_Server_Rehost_Perf_List,
                    VM_SS_IaaS_Server_Rehost_AsOnPrem_List,
                    VM_IaaS_Server_Rehost_Perf_List,
                    VM_IaaS_Server_Rehost_AsOnPrem_List
                );
            exportCoreReportObj.GenerateCoreReportExcel();
            UserInputObj.LoggerObj.LogInformation(93 - UserInputObj.LoggerObj.GetCurrentProgress(), "Generated core report excel sheet");

            UserInputObj.LoggerObj.LogInformation("Generating opportunity report excel sheet");
            ExportOpportunityReport exportOpportunityReportObj = new ExportOpportunityReport
                (
                    AVS_Summary_List,
                    AVS_IaaS_Rehost_Perf_List,
                    SQL_MI_Issues_and_Warnings_List,
                    SQL_MI_Opportunity_List,
                    WebApp_Opportunity_List,
                    VM_Opportunity_Perf_List,
                    VM_Opportunity_AsOnPrem_List
                );
            exportOpportunityReportObj.GenerateOpportunityReportExcel();
            UserInputObj.LoggerObj.LogInformation(96 - UserInputObj.LoggerObj.GetCurrentProgress(), "Generated opportunity report excel sheet");

            UserInputObj.LoggerObj.LogInformation("Generating clash report excel sheet");
            ExportClashReport exportClashReportObj = new ExportClashReport(Clash_Report_List);
            exportClashReportObj.GenerateClashReportExcel();
            UserInputObj.LoggerObj.LogInformation(100 - UserInputObj.LoggerObj.GetCurrentProgress(), "Generated clash report excel sheet");
        }

        private void Process_All_VM_IaaS_Server_Rehost_Perf_Model(List<All_VM_IaaS_Server_Rehost_Perf> All_VM_IaaS_Server_Rehost_Perf_List)
        {
            
            bool isSuccessful = false;
            isSuccessful = Create_All_VM_IaaS_Server_Rehost_Perf_Model(All_VM_IaaS_Server_Rehost_Perf_List);
            
            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered an issue while processing All_VM_IaaS_Server_Rehost_Perf excel model");
        }

        private bool Create_All_VM_IaaS_Server_Rehost_Perf_Model(List<All_VM_IaaS_Server_Rehost_Perf> All_VM_IaaS_Server_Rehost_Perf_List)
        {
            if (AzureVMPerformanceBasedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for All_VM_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is null");
                return false;
            }
            if (AzureVMPerformanceBasedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for All_VM_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for All_VM_IaaS_Server_Rehost_Perf");

            if (All_VM_IaaS_Server_Rehost_Perf_List == null)
                All_VM_IaaS_Server_Rehost_Perf_List = new List<All_VM_IaaS_Server_Rehost_Perf>();

            foreach (var VMPerfDataKvp in AzureVMPerformanceBasedMachinesData)
            {
                if (VMPerfDataKvp.Value.Suitability != Suitabilities.ConditionallySuitable && VMPerfDataKvp.Value.Suitability != Suitabilities.Suitable)
                    continue;

                All_VM_IaaS_Server_Rehost_Perf obj = new All_VM_IaaS_Server_Rehost_Perf();

                obj.MachineName = VMPerfDataKvp.Value.DisplayName;
                obj.Environment = VMPerfDataKvp.Value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(VMPerfDataKvp.Value.Suitability);
                obj.AzureVMReadiness_Warnings = VMPerfDataKvp.Value.SuitabilityExplanation;
                obj.RecommendedVMSize = VMPerfDataKvp.Value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = VMPerfDataKvp.Value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = VMPerfDataKvp.Value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(VMPerfDataKvp.Value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(VMPerfDataKvp.Value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(VMPerfDataKvp.Value.BootType);
                obj.Cores = VMPerfDataKvp.Value.NumberOfCores;
                obj.MemoryInMB = VMPerfDataKvp.Value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = VMPerfDataKvp.Value.PercentageOfCoresUtilization;
                obj.MemoryUtilizationPercentage = VMPerfDataKvp.Value.PercentageOfMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(VMPerfDataKvp.Value.Disks);
                obj.NetworkAdapters = VMPerfDataKvp.Value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(VMPerfDataKvp.Value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(VMPerfDataKvp.Value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(VMPerfDataKvp.Value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(VMPerfDataKvp.Value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(VMPerfDataKvp.Value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = VMPerfDataKvp.Value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = VMPerfDataKvp.Value.AzureBackupMonthlyCostEstimate;
                obj.GroupName = VMPerfDataKvp.Value.GroupName;
                obj.MachineId = VMPerfDataKvp.Value.DatacenterMachineArmId;

                All_VM_IaaS_Server_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated All_VM_IaaS_Server_Rehost_Perf excel model with data of {All_VM_IaaS_Server_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void CreateCorePropertiesModel(CoreProperties coreProperties)
        {
            UserInputObj.LoggerObj.LogInformation("Creating excel model for assessment properties");
            coreProperties.Workflow = UserInputObj.WorkflowObj.IsExpressWorkflow ? "Express" : "Custom - Assessment";
            coreProperties.TenantId = UserInputObj.TenantId;
            coreProperties.Subscription = UserInputObj.Subscription.Value;
            coreProperties.ResourceGroupName = UserInputObj.ResourceGroupName.Value;
            coreProperties.TargetRegion = UserInputObj.TargetRegion.Value;
            coreProperties.Currency = UserInputObj.Currency.Key;
            coreProperties.AzureMigrateProjectName = UserInputObj.AzureMigrateProjectName.Value;
            coreProperties.AssessmentSiteName = UserInputObj.AssessmentProjectName;
            coreProperties.AssessmentDuration = UserInputObj.AssessmentDuration;
            coreProperties.OptimizationPreference = UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value;
            coreProperties.AssessSQLServices = UserInputObj.PreferredOptimizationObj.AssessSqlServicesSeparately ? "Yes" : "No";
        }

        private void Process_AVS_Summary_Model(List<AVS_Summary> AVS_Summary_List)
        {
            if (AVSAssessmentsData == null)
                return;
            if (AVSAssessmentsData.Count <= 0)
                return;

            bool isAVSSummarySuccessful = false;
            isAVSSummarySuccessful = Create_AVS_Summary_Model(AVS_Summary_List);

            if (!isAVSSummarySuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating AVS_Summary excel model");
        }

        private bool Create_AVS_Summary_Model(List<AVS_Summary> AVS_Summary_List)
        {
            if (AVSAssessmentsData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for AVS_Summary as AVS assessments dataset is null");
                return false;
            }

            if (AVSAssessmentsData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for AVS_Summary as AVS assessments dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for AVS_Summary ");

            if (AVS_Summary_List == null)
                AVS_Summary_List = new List<AVS_Summary>();

            foreach (var avsAssessmentData in AVSAssessmentsData)
            {
                AVS_Summary obj = new AVS_Summary();

                obj.SubscriptionId = avsAssessmentData.Value.SubscriptionId;
                obj.ResourceGroup = avsAssessmentData.Value.ResourceGroup;
                obj.ProjectName = avsAssessmentData.Value.AssessmentProjectName;
                obj.GroupName = avsAssessmentData.Value.GroupName;
                obj.AssessmentName = avsAssessmentData.Value.AssessmentName;
                obj.SizingCriterion = avsAssessmentData.Value.SizingCriterion;
                obj.AssessmentType = avsAssessmentData.Value.AssessmentType;
                obj.CreatedOn = avsAssessmentData.Value.CreatedOn;
                obj.TotalMachinesAssessed = avsAssessmentData.Value.TotalMachinesAssessd;
                obj.MachinesReady = avsAssessmentData.Value.MachinesReady;
                obj.MachinesReadyWithConditions = avsAssessmentData.Value.MachinesReadyWithConditions;
                obj.MachinesNotReady = avsAssessmentData.Value.MachinesNotReady;
                obj.MachinesReadinessUnknown = avsAssessmentData.Value.MachinesReadinessUnknown;
                obj.RecommendedNumberOfNodes = avsAssessmentData.Value.RecommendedNumberOfNodes;
                obj.NodeType = avsAssessmentData.Value.NodeType;
                obj.MonthlyTotalCostEstimate = avsAssessmentData.Value.TotalMonthlyCostEstimate;
                obj.PredictedCpuUtilizationPercentage = avsAssessmentData.Value.PredictedCpuUtilizationPercentage;
                obj.PredictedMemoryUtilizationPercentage = avsAssessmentData.Value.PredictedMemoryUtilizationPercentage;
                obj.PredictedStorageUtilizationPercentage = avsAssessmentData.Value.PredictedStorageUtilizationPercentage;
                obj.NumberOfCpuCoresAvailable = (int) Math.Floor(avsAssessmentData.Value.NumberOfCpuCoresAvailable);
                obj.MemoryInTBAvailable = avsAssessmentData.Value.MemoryInTBAvailable;
                obj.StorageInTBAvailable = avsAssessmentData.Value.StorageInTBAvailable;
                obj.NumberOfCpuCoresUsed = (int) Math.Floor(avsAssessmentData.Value.NumberOfCpuCoresUsed);
                obj.MemoryInTBUsed = avsAssessmentData.Value.MemoryInTBUsed;
                obj.StorageInTBUsed = avsAssessmentData.Value.StorageInTBUsed;
                obj.NumberOfCpuCoresFree = (int)Math.Floor(avsAssessmentData.Value.NumberOfCpuCoresFree);
                obj.MemoryInTBFree = avsAssessmentData.Value.MemoryInTBFree;
                obj.StorageInTBFree = avsAssessmentData.Value.StorageInTBFree;
                obj.ConfidenceRating = avsAssessmentData.Value.ConfidenceRating;

                AVS_Summary_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated AVS_Summary excel model with data of {AVS_Summary_List.Count} assessments");
            return true;
        }

        private void Process_AVS_IaaS_Rehost_Perf_Model(List<AVS_IaaS_Rehost_Perf> AVS_IaaS_Rehost_Perf_List)
        {
            if (AVSAssessedMachinesData == null)
                return;
            if (AVSAssessedMachinesData.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_AVS_IaaS_Rehost_Perf_Model(AVS_IaaS_Rehost_Perf_List);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating AVS_IaaS_Rehost_Perf excel model");
        }

        private bool Create_AVS_IaaS_Rehost_Perf_Model(List<AVS_IaaS_Rehost_Perf> AVS_IaaS_Rehost_Perf_List)
        {
            if (AVSAssessedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating AVS_IaaS_Rehost_Perf excel model as AVS assessed machines dataset is null");
                return false;
            }
            if (AVSAssessedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating AVS_IaaS_Rehost_Perf excel model as AVS assessed machines dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for AVS_IaaS_Rehost_Perf");
            
            if (AVS_IaaS_Rehost_Perf_List == null)
                AVS_IaaS_Rehost_Perf_List = new List<AVS_IaaS_Rehost_Perf>();
            
            foreach (var avsAssessedMachine in AVSAssessedMachinesData)
            {
                AVS_IaaS_Rehost_Perf obj = new AVS_IaaS_Rehost_Perf();

                obj.MachineName = avsAssessedMachine.Value.DisplayName;
                obj.AzureVMWareSolutionReadiness = new EnumDescriptionHelper().GetEnumDescription(avsAssessedMachine.Value.Suitability);
                obj.AzureVMWareSolutionReadiness_Warnings = avsAssessedMachine.Value.SuitabilityExplanation;
                obj.OperatingSystem = avsAssessedMachine.Value.OperatingSystemName;
                obj.OperatingSystemVersion = avsAssessedMachine.Value.OperatingSystemVersion;
                obj.OperatingSystemArchitecture = avsAssessedMachine.Value.OperatingSystemArchitecture;
                obj.BootType = avsAssessedMachine.Value.BootType;
                obj.Cores = avsAssessedMachine.Value.NumberOfCores;
                obj.MemoryInMB = avsAssessedMachine.Value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(avsAssessedMachine.Value.Disks);
                obj.StorageInUseInGB = avsAssessedMachine.Value.StorageInUseGB;
                obj.DiskReadInOPS = UtilityFunctions.GetDiskReadInOPS(avsAssessedMachine.Value.Disks);
                obj.DiskWriteInOPS = UtilityFunctions.GetDiskWriteInOPS(avsAssessedMachine.Value.Disks);
                obj.DiskReadInMBPS = UtilityFunctions.GetDiskReadInMBPS(avsAssessedMachine.Value.Disks);
                obj.DiskWriteInMBPS = UtilityFunctions.GetDiskWriteInMBPS(avsAssessedMachine.Value.Disks);
                obj.NetworkAdapters = avsAssessedMachine.Value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(avsAssessedMachine.Value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.NetworkInMBPS = UtilityFunctions.GetNetworkInMBPS(avsAssessedMachine.Value.NetworkAdapterList);
                obj.NetworkOutMBPS = UtilityFunctions.GetNetworkOutMBPS(avsAssessedMachine.Value.NetworkAdapterList);
                obj.DiskNames = UtilityFunctions.GetDiskNames(avsAssessedMachine.Value.Disks);
                obj.GroupName = avsAssessedMachine.Value.GroupName;
                obj.MachineId = avsAssessedMachine.Value.DatacenterMachineArmId;

                AVS_IaaS_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated AVS_IaaS_Rehost_Perf excel model with data of {AVS_IaaS_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void Process_SQL_MI_Issues_and_Warnings_Model(List<SQL_MI_Issues_and_Warnings> SQL_MI_Issues_and_Warnings_List)
        {
            if (AzureSQLInstancesData == null)
                return;
            if (AzureSQLInstancesData.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_SQL_MI_Issues_and_Warnings_Model(SQL_MI_Issues_and_Warnings_List);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_MI_Issues_and_Warnings excel model");
        }

        private bool Create_SQL_MI_Issues_and_Warnings_Model(List<SQL_MI_Issues_and_Warnings> SQL_MI_Issues_and_Warnings_List)
        {
            if (AzureSQLInstancesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_Issues_and_Warnings excel model as Azure SQL instances dataset is null");
                return false;
            }
            if (AzureSQLInstancesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_Issues_and_Warnings excel model as Azure SQL instances dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for SQL_MI_Issue_and_Warnings");

            if (SQL_MI_Issues_and_Warnings_List == null)
                SQL_MI_Issues_and_Warnings_List = new List<SQL_MI_Issues_and_Warnings>();
            
            foreach (var azureSqlInstance in AzureSQLInstancesData)
            {
                foreach (var migrationIssue in azureSqlInstance.Value.AzureSQLMIMigrationIssues)
                {
                    if (migrationIssue.IssueCategory != IssueCategories.Warning && migrationIssue.IssueCategory != IssueCategories.Issue)
                        continue;

                    foreach (var impactedObject in migrationIssue.ImpactedObjects)
                    {
                        SQL_MI_Issues_and_Warnings obj = new SQL_MI_Issues_and_Warnings();

                        obj.MachineName =  azureSqlInstance.Value.MachineName;
                        obj.SQLInstance = azureSqlInstance.Value.InstanceName;
                        obj.Category = migrationIssue.IssueCategory.ToString();
                        obj.Title = migrationIssue.IssueId;
                        obj.ImpactedObjectType = impactedObject.ObjectType;
                        obj.ImpactedObjectName = impactedObject.ObjectName;
                        obj.UserDatabases = azureSqlInstance.Value.DatabaseSummaryNumberOfUserDatabases;
                        obj.MachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(azureSqlInstance.Value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[azureSqlInstance.Value.MachineArmId] : "";
                
                        SQL_MI_Issues_and_Warnings_List.Add(obj);
                    }
                }
            }

            UserInputObj.LoggerObj.LogInformation($"Updated SQL_MI_Issues_and_Warnings excel model with {SQL_MI_Issues_and_Warnings_List.Count} datapoints");
            return true;
        }

        private void Process_SQL_All_Instances_Model(List<SQL_All_Instances> SQL_All_Instances_List)
        {
            if (AzureSQLInstancesData == null)
                return;
            if (AzureSQLInstancesData.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;

            bool isSuccessful = false;
            isSuccessful = Create_SQL_All_Instances_Model(SQL_All_Instances_List);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_All_Instances excel model");
        }

        private bool Create_SQL_All_Instances_Model(List<SQL_All_Instances> SQL_All_Instances_List)
        {
            if (AzureSQLInstancesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_All_Instances excel model as Azure SQL instances dataset is null");
                return false;
            }
            if (AzureSQLInstancesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_All_Instances excel model as Azure SQL instances dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for SQL_All_Instances");

            if (SQL_All_Instances_List == null)
                SQL_All_Instances_List = new List<SQL_All_Instances>();

            foreach (var azureSqlInstance in AzureSQLInstancesData)
            {
                SQL_All_Instances obj = new SQL_All_Instances();

                obj.MachineName = azureSqlInstance.Value.MachineName;
                obj.SQLInstance = azureSqlInstance.Value.InstanceName;
                obj.Environment = azureSqlInstance.Value.Environment;
                obj.AzureSQLMIReadiness = new EnumDescriptionHelper().GetEnumDescription(azureSqlInstance.Value.AzureSQLMISuitability);
                obj.AzureSQLMIReadiness_Warnings = UtilityFunctions.GetMigrationIssueWarnings(azureSqlInstance.Value.AzureSQLMIMigrationIssues);
                obj.RecommendedDeploymentType = azureSqlInstance.Value.RecommendedAzureSqlTargetType.ToString();
                obj.AzureSQLMIConfiguration = UtilityFunctions.GetSQLMIConfiguration(azureSqlInstance.Value);
                obj.MonthlyComputeCostEstimate = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_AHUB_RI3year;
                obj.MonthlyStorageCostEstimate = azureSqlInstance.Value.AzureSQLMIMonthlyStorageCost;
                obj.UserDatabases = azureSqlInstance.Value.DatabaseSummaryNumberOfUserDatabases;
                obj.SQLEdition = azureSqlInstance.Value.SQLEdition;
                obj.SQLVersion = azureSqlInstance.Value.SQLVersion;
                obj.TotalDBSizeInMB = azureSqlInstance.Value.DatabaseSummaryTotalDatabaseSizeInMB;
                obj.LargestDBSizeInMB = azureSqlInstance.Value.DatabaseSumaryLargestDatabaseSizeInMB;
                obj.VCoresAllocated = azureSqlInstance.Value.NumberOfCoresAllocated;
                obj.CpuUtilizationInPercentage = azureSqlInstance.Value.PercentageCoresUtilization;
                obj.MemoryInUseInMB = azureSqlInstance.Value.MemoryInUseMB;
                obj.NumberOfDisks = azureSqlInstance.Value.LogicalDisks.Count;
                obj.DiskReadInOPS = UtilityFunctions.GetDiskReadInOPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskWriteInOPS = UtilityFunctions.GetDiskWriteInOPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskReadInMBPS = UtilityFunctions.GetDiskReadInMBPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskWriteInMBPS = UtilityFunctions.GetDiskWriteInMBPS(azureSqlInstance.Value.LogicalDisks);
                obj.AzureSQLMIConfigurationTargetServiceTier = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuServiceTier);
                obj.AzureSQLMIConfigurationTargetComputeTier = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuComputeTier);
                obj.AzureSQLMIConfigurationTargetHardwareType = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuHardwareGeneration);
                obj.AzureSQLMIConfigurationTargetCores = azureSqlInstance.Value.AzureSQLMISkuCores;
                obj.AzureSQLMIConfigurationTargetStorageInGB = Math.Round(azureSqlInstance.Value.AzureSQLMISkuStorageMaxSizeInMB / 1024.0);
                obj.GroupName = azureSqlInstance.Value.GroupName;
                obj.MachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(azureSqlInstance.Value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[azureSqlInstance.Value.MachineArmId] : "";

                SQL_All_Instances_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated SQL_All_Instances excel model with data of {SQL_All_Instances_List.Count} instances");
            return true;
        }

        private void Process_SQL_MI_PaaS_Model(List<SQL_MI_PaaS> SQL_MI_PaaS_List, HashSet<string> AzureSQL_IaaS_Instance, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (AzureSQLInstancesData == null)
                return;
            if (AzureSQLInstancesData.Count <= 0)
                return;
            
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
            {
                if (AzureSQL_IaaS_Server == null)
                    AzureSQL_IaaS_Server = new HashSet<string>();

                foreach (var kvp in AzureSQLMachinesData)
                    if (!AzureSQL_IaaS_Server.Contains(kvp.Key))
                        AzureSQL_IaaS_Server.Add(kvp.Key);
                
                return;
            }

            bool isSuccessful = false;
            isSuccessful = Create_SQL_MI_PaaS_Model(SQL_MI_PaaS_List, AzureSQL_IaaS_Instance);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_MI_PaaS excel model");
        }

        private bool Create_SQL_MI_PaaS_Model(List<SQL_MI_PaaS> SQL_MI_PaaS_List, HashSet<string> AzureSQL_IaaS_Instance)
        {
            if (AzureSQLInstancesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_PaaS excel model as Azure SQL instances dataset is null");
                return false;
            }
            if (AzureSQLInstancesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_PaaS excel model as Azure SQL instances dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for SQL_MI_PaaS");

            if (SQL_MI_PaaS_List == null)
                SQL_MI_PaaS_List = new List<SQL_MI_PaaS>();
            if (AzureSQL_IaaS_Instance == null)
                AzureSQL_IaaS_Instance = new HashSet<string>();
            
            foreach (var azureSqlInstance in AzureSQLInstancesData)
            {
                if (azureSqlInstance.Value.AzureSQLMISuitability != Suitabilities.Suitable)
                {
                    if (!AzureSQL_IaaS_Instance.Contains(azureSqlInstance.Value.SQLInstanceSDSArmId))
                        AzureSQL_IaaS_Instance.Add(azureSqlInstance.Value.SQLInstanceSDSArmId);
                    continue;
                }

                SQL_MI_PaaS obj = new SQL_MI_PaaS();

                obj.MachineName = azureSqlInstance.Value.MachineName;
                obj.SQLInstance = azureSqlInstance.Value.InstanceName;
                obj.Environment = azureSqlInstance.Value.Environment;
                obj.AzureSQLMIReadiness = new EnumDescriptionHelper().GetEnumDescription(azureSqlInstance.Value.AzureSQLMISuitability);
                obj.AzureSQLMIReadiness_Warnings = UtilityFunctions.GetMigrationIssueWarnings(azureSqlInstance.Value.AzureSQLMIMigrationIssues);
                obj.RecommendedDeploymentType = AzureSQLTargetType.AzureSqlManagedInstance.ToString(); // azureSqlInstance.Value.RecommendedAzureSqlTargetType.ToString();
                obj.AzureSQLMIConfiguration = UtilityFunctions.GetSQLMIConfiguration(azureSqlInstance.Value);
                obj.MonthlyComputeCostEstimate = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = azureSqlInstance.Value.AzureSQLMIMonthlyComputeCost_AHUB_RI3year;
                obj.MonthlyStorageCostEstimate = azureSqlInstance.Value.AzureSQLMIMonthlyStorageCost;
                obj.UserDatabases = azureSqlInstance.Value.DatabaseSummaryNumberOfUserDatabases;
                obj.SQLEdition = azureSqlInstance.Value.SQLEdition;
                obj.SQLVersion = azureSqlInstance.Value.SQLVersion;
                obj.TotalDBSizeInMB = azureSqlInstance.Value.DatabaseSummaryTotalDatabaseSizeInMB;
                obj.LargestDBSizeInMB = azureSqlInstance.Value.DatabaseSumaryLargestDatabaseSizeInMB;
                obj.VCoresAllocated = azureSqlInstance.Value.NumberOfCoresAllocated;
                obj.CpuUtilizationInPercentage = azureSqlInstance.Value.PercentageCoresUtilization;
                obj.MemoryInUseInMB = azureSqlInstance.Value.MemoryInUseMB;
                obj.NumberOfDisks = azureSqlInstance.Value.LogicalDisks.Count;
                obj.DiskReadInOPS = UtilityFunctions.GetDiskReadInOPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskWriteInOPS = UtilityFunctions.GetDiskWriteInOPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskReadInMBPS = UtilityFunctions.GetDiskReadInMBPS(azureSqlInstance.Value.LogicalDisks);
                obj.DiskWriteInMBPS = UtilityFunctions.GetDiskWriteInMBPS(azureSqlInstance.Value.LogicalDisks);
                obj.AzureSQLMIConfigurationTargetServiceTier = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuServiceTier);
                obj.AzureSQLMIConfigurationTargetComputeTier = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuComputeTier);
                obj.AzureSQLMIConfigurationTargetHardwareType = UtilityFunctions.GetStringValue(azureSqlInstance.Value.AzureSQLMISkuHardwareGeneration);
                obj.AzureSQLMIConfigurationTargetCores = azureSqlInstance.Value.AzureSQLMISkuCores;
                obj.AzureSQLMIConfigurationTargetStorageInGB = Math.Round(azureSqlInstance.Value.AzureSQLMISkuStorageMaxSizeInMB / 1024.0);
                obj.GroupName = azureSqlInstance.Value.GroupName;
                obj.MachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(azureSqlInstance.Value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[azureSqlInstance.Value.MachineArmId] : "";

                SQL_MI_PaaS_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated SQL_MI_PaaS excel model with data of {SQL_MI_PaaS_List.Count} instances");
            UserInputObj.LoggerObj.LogInformation($"Number of instances that will be evaluated for Azure SQL instance rehost: {AzureSQL_IaaS_Instance.Count}");
            return true;
        }

        private void Process_SQL_IaaS_Instance_Rehost_Perf_Model(List<SQL_IaaS_Instance_Rehost_Perf> SQL_IaaS_Instance_Rehost_Perf_List, HashSet<String> AzureSQL_IaaS_Instance, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (AzureSQL_IaaS_Instance == null)
                return;
            if (AzureSQL_IaaS_Instance.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_SQL_IaaS_Instance_Rehost_Perf_Model(SQL_IaaS_Instance_Rehost_Perf_List, AzureSQL_IaaS_Instance, AzureSQL_IaaS_Server);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_IaaS_Instance_Rehost_Perf excel model");
        }

        private bool Create_SQL_IaaS_Instance_Rehost_Perf_Model(List<SQL_IaaS_Instance_Rehost_Perf> SQL_IaaS_Instance_Rehost_Perf_List, HashSet<String> AzureSQL_IaaS_Instance, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (AzureSQLInstancesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Instance_Rehost_Perf excel model as Azure SQL instances dataset is null");
                return false;
            }
            if (AzureSQLInstancesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Instance_Rehost_Perf excel model as Azure SQL instances dataset is empty");
                return false;
            }

            if (SQL_IaaS_Instance_Rehost_Perf_List == null)
                SQL_IaaS_Instance_Rehost_Perf_List = new List<SQL_IaaS_Instance_Rehost_Perf>();
            if (AzureSQL_IaaS_Server == null)
                AzureSQL_IaaS_Server = new HashSet<string>();

            foreach (string sqlInstanceId in AzureSQL_IaaS_Instance)
            {
                if (!AzureSQLInstancesData.ContainsKey(sqlInstanceId))
                {
                    UserInputObj.LoggerObj.LogWarning($"SQL Instance ID {sqlInstanceId} does not exist in Azure SQL instances dataset, skipping");
                    continue;
                }

                var value = AzureSQLInstancesData[sqlInstanceId];
                string discoveredMachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[value.MachineArmId] : "";

                if (value.AzureSQLVMSuitability != Suitabilities.Suitable && value.AzureSQLVMSuitability != Suitabilities.ConditionallySuitable)
                {
                    if (string.IsNullOrEmpty(discoveredMachineId))
                        continue;

                    if (!AzureSQL_IaaS_Server.Contains(discoveredMachineId))
                        AzureSQL_IaaS_Server.Add(discoveredMachineId);
                    
                    continue;
                }

                SQL_IaaS_Instance_Rehost_Perf obj = new SQL_IaaS_Instance_Rehost_Perf();

                obj.MachineName = value.MachineName;
                obj.SQLInstance = value.InstanceName;
                obj.Environment = value.Environment;
                obj.SQLServerOnAzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.AzureSQLVMSuitability);
                obj.SQLServerOnAzureVMReadiness_Warnings = UtilityFunctions.GetMigrationIssueWarnings(value.AzureSQLVMMigrationIssues);
                obj.SQLServerOnAzureVMConfiguration = value.AzureSQLVMSkuName;
                obj.MonthlyComputeCostEstimate = value.AzureSQLVMMonthlyComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = value.AzureSQLVMMonthlyComputeCost_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.AzureSQLVMMonthlyComputeCost_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.AzureSQLVMMonthlyComputeCost_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.AzureSQLVMMonthlyComputeCost_ASP3year;
                obj.MonthlyStorageCostEstimate = value.AzureSQLVMMonthlyStorageCost;
                obj.SQLServerONAzureVMManagedDiskConfiguration = UtilityFunctions.GetRecommendedDiskSKUs(value.AzureSQLVMLogDisks) +
                                                                 UtilityFunctions.GetRecommendedDiskSKUs(value.AzureSQLVMDataDisks);
                obj.UserDatabases = value.DatabaseSummaryNumberOfUserDatabases;
                obj.RecommendedDeploymentType = AzureSQLTargetType.AzureSqlVirtualMachine.ToString(); // value.RecommendedAzureSqlTargetType.ToString();
                
                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Standard) + UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMLogDisks, RecommendedDiskTypes.StandardSSD) + UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMDataDisks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Premium) + UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Ultra) + UtilityFunctions.GetDiskTypeCount(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Standard) + UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks =  UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMLogDisks, RecommendedDiskTypes.StandardSSD) + UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMDataDisks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Premium) + UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMLogDisks, RecommendedDiskTypes.Ultra) + UtilityFunctions.GetDiskTypeStorageCost(value.AzureSQLVMDataDisks, RecommendedDiskTypes.Ultra);

                obj.SQLEdition = value.SQLEdition;
                obj.SQLVersion = value.SQLVersion;
                obj.TotalDBSizeInMB = value.DatabaseSummaryTotalDatabaseSizeInMB;
                obj.LargestDBSizeInMB = value.DatabaseSumaryLargestDatabaseSizeInMB;
                obj.VCoresAllocated = value.NumberOfCoresAllocated;
                obj.CpuUtilizationPercentage = value.PercentageCoresUtilization;
                obj.MemoryInUseInMB = value.MemoryInUseMB;
                obj.NumberOfDisks = value.LogicalDisks.Count;
                obj.DiskReadInOPS = UtilityFunctions.GetDiskReadInOPS(value.LogicalDisks);
                obj.DiskWriteInOPS = UtilityFunctions.GetDiskWriteInOPS(value.LogicalDisks);
                obj.DiskReadInMBPS = UtilityFunctions.GetDiskReadInMBPS(value.LogicalDisks);
                obj.DiskWriteInMBPS = UtilityFunctions.GetDiskWriteInMBPS(value.LogicalDisks);
                obj.ConfidenceRatingInPercentage = value.ConfidenceRatingInPercentage;
                obj.SQLServerOnAzureVMConfigurationTargetCores = value.AzureSQLVMCores;
                obj.MonthlyAzureSiteRecoveryCostEstimate = value.MonthlyAzureSiteRecoveryCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.MonthlyAzureBackupCostEstimate;
                obj.GroupName = value.GroupName;
                obj.MachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[value.MachineArmId] : "";

                SQL_IaaS_Instance_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated SQL_IaaS_Instance_Rehost_Perf excel model with data of {SQL_IaaS_Instance_Rehost_Perf_List.Count} instances");
            UserInputObj.LoggerObj.LogInformation($"Number of servers that will be evaluated for Azure SQL server rehost: {AzureSQL_IaaS_Server.Count}");
            return true;
        }

        private void Process_SQL_IaaS_Server_Rehost_Perf_Model(List<SQL_IaaS_Server_Rehost_Perf> SQL_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureSQL_IaaS_Server, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureSQL_IaaS_Server == null)
                return;
            if (AzureSQL_IaaS_Server.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_SQL_IaaS_Server_Rehost_Perf_Model(SQL_IaaS_Server_Rehost_Perf_List, AzureSQL_IaaS_Server, AzureVM_Opportunity_Perf);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_IaaS_Server_Rehost_Perf excel model");
        }

        private bool Create_SQL_IaaS_Server_Rehost_Perf_Model(List<SQL_IaaS_Server_Rehost_Perf> SQL_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureSQL_IaaS_Server, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureSQLMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Server_Rehost_Perf excel model as Azure SQL machines dataset is null");
                return false;
            }
            if (AzureSQLMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Server_Rehost_Perf excel model as Azure SQL machines dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for SQL_IaaS_Server_Rehost_Perf");

            if (SQL_IaaS_Server_Rehost_Perf_List == null)
                SQL_IaaS_Server_Rehost_Perf_List = new List<SQL_IaaS_Server_Rehost_Perf>();
            if (AzureVM_Opportunity_Perf == null)
                AzureVM_Opportunity_Perf = new HashSet<string>();

            foreach (var discoveredMachineId in AzureSQL_IaaS_Server)
            {
                if (!AzureSQLMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID {discoveredMachineId} does not exist in Azure SQL machines dataset, skipping");
                    continue;
                }

                var value = AzureSQLMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_Perf.Contains(discoveredMachineId))
                        AzureVM_Opportunity_Perf.Add(discoveredMachineId);
                    continue;
                }
                
                SQL_IaaS_Server_Rehost_Perf obj = new SQL_IaaS_Server_Rehost_Perf();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureVMReadiness_Warnings = value.SuitabilityExplanation;
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = value.MonthlyComputeCostEstimate_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.MonthlyComputeCostEstimate_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.MonthlyComputeCostEstimate_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.MonthlyComputeCostEstimate_ASP3year;
                obj.MonthlyStorageCostEstimate = value.MonthlyStorageCost;
                obj.OperatingSystem = value.OperatingSystemName;
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = value.BootType;
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = value.PercentageCoresUtilization;
                obj.MemoryUtilizationPercentage = value.PercentageMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.AzureBackupMonthlyCostEstimate;

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                SQL_IaaS_Server_Rehost_Perf_List.Add(obj);
            }
            
            UserInputObj.LoggerObj.LogInformation($"Updated SQL_IaaS_Server_Rehost_Perf excel model with data of {SQL_IaaS_Server_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void Process_SQL_IaaS_Server_Rehost_AsOnPrem_Model(List<SQL_IaaS_Server_Rehost_AsOnPrem> SQL_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureSQL_IaaS_Server, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureSQL_IaaS_Server == null)
                return;
            if (AzureSQL_IaaS_Server.Count <= 0)
                return;

            bool isSuccessful = false;
            isSuccessful = Create_SQL_IaaS_Server_Rehost_AsOnPrem_Model(SQL_IaaS_Server_Rehost_AsOnPrem_List, AzureSQL_IaaS_Server, AzureVM_Opportunity_AsOnPrem);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating SQL_IaaS_Server_Rehost_AsOnPrem excel model");
        }

        private bool Create_SQL_IaaS_Server_Rehost_AsOnPrem_Model(List<SQL_IaaS_Server_Rehost_AsOnPrem> SQL_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureSQL_IaaS_Server, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Server_Rehost_AsOnPrem excel model as Azure VM AsOnPrem dataset is null");
                return false;
            }
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_IaaS_Server_Rehost_AsonPrem excel model as Azure VM AsOnPrem dataset is null");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model fro SQL_IaaS_Server_Rehost_AsOnPrem");

            if (SQL_IaaS_Server_Rehost_AsOnPrem_List == null)
                SQL_IaaS_Server_Rehost_AsOnPrem_List = new List<SQL_IaaS_Server_Rehost_AsOnPrem>();
            if (AzureVM_Opportunity_AsOnPrem == null)
                AzureVM_Opportunity_AsOnPrem = new HashSet<string>();
            
            foreach (string discoveredMachineId in AzureSQL_IaaS_Server)
            {
                if (!AzureVMAsOnPremMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machind ID {discoveredMachineId} does not exist in Azure VM AsOnPrem dataset, skipping");
                    continue;
                }

                var value = AzureVMAsOnPremMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_AsOnPrem.Contains(discoveredMachineId))
                        AzureVM_Opportunity_AsOnPrem.Add(discoveredMachineId);
                    continue;
                }
                SQL_IaaS_Server_Rehost_AsOnPrem obj = new SQL_IaaS_Server_Rehost_AsOnPrem();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = value.OperatingSystem;
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                SQL_IaaS_Server_Rehost_AsOnPrem_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated SQL_IaaS_Server_Rehost_AsOnPrem excel model with data of {SQL_IaaS_Server_Rehost_AsOnPrem_List.Count} machines");
            return true;
        }

        private void Process_SQL_MI_Opportunity_Model(List<SQL_MI_Opportunity> SQL_MI_Opportunity_List, HashSet<string> AzureSQL_IaaS_Instance)
        {
            if (AzureSQL_IaaS_Instance == null)
                return;
            if (AzureSQL_IaaS_Instance.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_SQL_MI_Opportunity_Model(SQL_MI_Opportunity_List, AzureSQL_IaaS_Instance);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for SQL_MI_Opportunity");
        }

        private bool Create_SQL_MI_Opportunity_Model(List<SQL_MI_Opportunity> SQL_MI_Opportunity_List, HashSet<string> AzureSQL_IaaS_Instance)
        {
            if (AzureSQLInstancesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_Opportunity excel model as Azure SQL instances dataset is null");
                return false;
            }
            if (AzureSQLInstancesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating SQL_MI_Opportuniey excel model as Azure SQL instances dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for SQL_MI_Opportunity");

            if (SQL_MI_Opportunity_List == null)
                SQL_MI_Opportunity_List = new List<SQL_MI_Opportunity>();
            
            foreach (var instanceArmId in AzureSQL_IaaS_Instance)
            {
                if (!AzureSQLInstancesData.ContainsKey(instanceArmId))
                {
                    UserInputObj.LoggerObj.LogWarning($"SQL Instance ID: {instanceArmId} does not exist Azure SQL instances dataset, skipping");
                    continue;
                }

                var value = AzureSQLInstancesData[instanceArmId];

                SQL_MI_Opportunity obj = new SQL_MI_Opportunity();

                obj.MachineName = value.MachineName;
                obj.SQLInstance = value.InstanceName;
                obj.Environment = value.Environment;
                obj.AzureSQLMIReadiness = new EnumDescriptionHelper().GetEnumDescription(value.AzureSQLMISuitability);
                obj.AzureSQLMIReadiness_Warnings = UtilityFunctions.GetMigrationIssueWarnings(value.AzureSQLMIMigrationIssues);
                obj.AzureSQLMIReadiness_Issues = UtilityFunctions.GetMigrationIssueByType(value.AzureSQLMIMigrationIssues, IssueCategories.Issue);
                obj.RecommendedDeploymentType = value.RecommendedAzureSqlTargetType.ToString();
                obj.AzureSQLMIConfiguration = UtilityFunctions.GetSQLMIConfiguration(value);
                obj.MonthlyComputeCostEstimate = value.AzureSQLMIMonthlyComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = value.AzureSQLMIMonthlyComputeCost_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.AzureSQLMIMonthlyComputeCost_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.AzureSQLMIMonthlyComputeCost_AHUB_RI3year;
                obj.MonthlyStorageCostEstimate = value.AzureSQLMIMonthlyStorageCost;
                obj.UserDatabases = value.DatabaseSummaryNumberOfUserDatabases;
                obj.SQLEdition = value.SQLEdition;
                obj.SQLVersion = value.SQLVersion;
                obj.TotalDBSizeInMB = value.DatabaseSummaryTotalDatabaseSizeInMB;
                obj.LargestDBSizeInMB = value.DatabaseSumaryLargestDatabaseSizeInMB;
                obj.VCoresAllocated = value.NumberOfCoresAllocated;
                obj.CpuUtilizationInPercentage = value.PercentageCoresUtilization;
                obj.MemoryInUseInMB = value.MemoryInUseMB;
                obj.NumberOfDisks = value.LogicalDisks.Count;
                obj.DiskReadInOPS = UtilityFunctions.GetDiskReadInOPS(value.LogicalDisks);
                obj.DiskWriteInOPS = UtilityFunctions.GetDiskWriteInOPS(value.LogicalDisks);
                obj.DiskReadInMBPS = UtilityFunctions.GetDiskReadInMBPS(value.LogicalDisks);
                obj.DiskWriteInMBPS = UtilityFunctions.GetDiskWriteInMBPS(value.LogicalDisks);
                obj.ConfidenceRatingInPercentage = value.ConfidenceRatingInPercentage;
                obj.AzureSQLMIConfigurationTargetServiceTier = UtilityFunctions.GetStringValue(value.AzureSQLMISkuServiceTier);
                obj.AzureSQLMIConfigurationTargetComputeTier = UtilityFunctions.GetStringValue(value.AzureSQLMISkuComputeTier);
                obj.AzureSQLMIConfigurationTargetHardwareType = UtilityFunctions.GetStringValue(value.AzureSQLMISkuHardwareGeneration);
                obj.AzureSQLMIConfigurationTargetCores = value.AzureSQLMISkuCores;
                obj.AzureSQLMIConfigurationTargetStorageInGB = Math.Round(value.AzureSQLMISkuStorageMaxSizeInMB / 1024.0);
                obj.GroupName = value.GroupName;
                obj.MachineId = AssessmentIdToDiscoveryIdLookup.ContainsKey(value.MachineArmId) ? AssessmentIdToDiscoveryIdLookup[value.MachineArmId] : "";

                SQL_MI_Opportunity_List.Add(obj);
            }
            
            UserInputObj.LoggerObj.LogInformation($"Updated SQL_MI_Opportunity excel model with data of {SQL_MI_Opportunity_List.Count} instances");
            return true;
        }

        private void Process_WebApp_PaaS_Model (List<WebApp_PaaS> WebApp_PaaS_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureWebApp_Opportunity)
        {
            if (AzureWebAppData == null)
                return;
            if (AzureWebAppData.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_WebApp_PaaS_Model(WebApp_PaaS_List, AzureWebApp_IaaS, AzureWebApp_Opportunity);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning($"Encountered issue while generating WebApp_PaaS excel model");
        }

        private bool Create_WebApp_PaaS_Model(List<WebApp_PaaS> WebApp_PaaS_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureWebApp_Opportunity)
        {
            if (AzureWebAppData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for WebApp_PaaS as Azure WebApp dataset is null");
                return false;
            }
            if (AzureWebAppData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning($"Not creating excel model for WebApp_PaaS as Azure WebApp dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model fro WebApp_PaaS");

            if (WebApp_PaaS_List == null)
                WebApp_PaaS_List = new List<WebApp_PaaS>();
            if (AzureWebApp_IaaS == null)
                AzureWebApp_IaaS = new HashSet<string>();
            if (AzureWebApp_Opportunity == null)
                AzureWebApp_Opportunity = new HashSet<string>();
            

            foreach (var webApp in AzureWebAppData)
            {
                if (webApp.Value.Suitability != Suitabilities.Suitable && webApp.Value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureWebApp_Opportunity.Contains(webApp.Key))
                        AzureWebApp_Opportunity.Add(webApp.Key);
                    if (!AzureWebApp_IaaS.Contains(webApp.Value.DiscoveredMachineId))
                        AzureWebApp_IaaS.Add(webApp.Value.DiscoveredMachineId);
                    
                    continue;
                }

                WebApp_PaaS obj = new WebApp_PaaS();

                obj.MachineName = webApp.Value.MachineName;
                obj.WebAppName = webApp.Value.WebAppName;
                obj.Environment = webApp.Value.Environment;
                obj.AzureAppServiceReadiness = new EnumDescriptionHelper().GetEnumDescription(webApp.Value.Suitability);
                obj.AzureAppServiceReadiness_Warnings = UtilityFunctions.GetMigrationIssueWarnings(webApp.Value.MigrationIssues);
                obj.AppServicePlanName = webApp.Value.AppServicePlanName;
                obj.RecommendedSKU = webApp.Value.WebAppSkuName;
                obj.MonthlyComputeCostEstimate = webApp.Value.EstimatedComputeCost;
                obj.MonthlyComputeCostEstimate_RI3year = webApp.Value.EstimatedComputeCost_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = webApp.Value.EstimatedComputeCost_ASP3year;
                obj.AzureRecommendedTarget = webApp.Value.AzureRecommendedTarget;
                obj.GroupName = webApp.Value.GroupName;
                obj.MachineId = webApp.Value.DiscoveredMachineId;

                WebApp_PaaS_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated WebApp_PaaS excel model with data of {WebApp_PaaS_List.Count} web applications");
            return true;
        }

        private void Process_WebApp_IaaS_Server_Rehost_Perf_Model(List<WebApp_IaaS_Server_Rehost_Perf> WebApp_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureWebApp_IaaS == null)
                return;
            if (AzureWebApp_IaaS.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_WebApp_IaaS_Server_Rehost_Perf_Model(WebApp_IaaS_Server_Rehost_Perf_List, AzureWebApp_IaaS, AzureVM_Opportunity_Perf);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for WebApp_IaaS_Server_Rehost_Perf");
        }

        private bool Create_WebApp_IaaS_Server_Rehost_Perf_Model(List<WebApp_IaaS_Server_Rehost_Perf> WebApp_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureVMPerformanceBasedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for WebApp_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is null");
                return false;
            }
            if (AzureVMPerformanceBasedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for WebApp_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for WebApp_IaaS_Server_Rehost_Perf");

            if (WebApp_IaaS_Server_Rehost_Perf_List == null)
                WebApp_IaaS_Server_Rehost_Perf_List = new List<WebApp_IaaS_Server_Rehost_Perf>();
            if (AzureVM_Opportunity_Perf == null)
                AzureVM_Opportunity_Perf = new HashSet<string>();
            
            foreach (var discoveredMachineId in AzureWebApp_IaaS)
            {
                if (!AzureVMPerformanceBasedMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} was not found in Azure VM Perf dataset, skipping");
                    continue;
                }

                var value = AzureVMPerformanceBasedMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_Perf.Contains(discoveredMachineId))
                        AzureVM_Opportunity_Perf.Add(discoveredMachineId);
                    
                    continue;
                }

                WebApp_IaaS_Server_Rehost_Perf obj = new WebApp_IaaS_Server_Rehost_Perf();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureVMReadiness_Warnings = value.SuitabilityExplanation;
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyComputeCostEstimate_RI3year = value.MonthlyComputeCostEstimate_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.MonthlyComputeCostEstimate_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.MonthlyComputeCostEstimate_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.MonthlyComputeCostEstimate_ASP3year;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = value.PercentageOfCoresUtilization;
                obj.MemoryUtilizationPercentage = value.PercentageOfMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.AzureBackupMonthlyCostEstimate;

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                WebApp_IaaS_Server_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated excel model for WebApp_IaaS_Server_Rehost_Perf with data of {WebApp_IaaS_Server_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void Process_WebApp_IaaS_Server_Rehost_AsOnPrem_Model(List<WebApp_IaaS_Server_Rehost_AsOnPrem> WebApp_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureWebApp_IaaS == null)
                return;
            if (AzureWebApp_IaaS.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_WebApp_IaaS_Server_Rehost_AsOnPrem_Model(WebApp_IaaS_Server_Rehost_AsOnPrem_List, AzureWebApp_IaaS, AzureVM_Opportunity_AsOnPrem);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for WebApp_IaaS_Server_Rehost_AsOnPrem");
        }

        private bool Create_WebApp_IaaS_Server_Rehost_AsOnPrem_Model(List<WebApp_IaaS_Server_Rehost_AsOnPrem> WebApp_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureWebApp_IaaS, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for WebApp_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is null");
                return false;
            }
            if (AzureVMAsOnPremMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for WebApp_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for WebApp_IaaS_Server_Rehost_AsOnPrem");

            if (WebApp_IaaS_Server_Rehost_AsOnPrem_List == null)
                WebApp_IaaS_Server_Rehost_AsOnPrem_List = new List<WebApp_IaaS_Server_Rehost_AsOnPrem>();
            if (AzureVM_Opportunity_AsOnPrem == null)
                AzureVM_Opportunity_AsOnPrem = new HashSet<string>();
            
            foreach (var discoveredMachineId in AzureWebApp_IaaS)
            {
                if (!AzureVMAsOnPremMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} was not found in Azure VM AsOnPrem dataset, skipping");
                    continue;
                }

                var value = AzureVMAsOnPremMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_AsOnPrem.Contains(discoveredMachineId))
                        AzureVM_Opportunity_AsOnPrem.Add(discoveredMachineId);
                    
                    continue;
                }

                WebApp_IaaS_Server_Rehost_AsOnPrem obj = new WebApp_IaaS_Server_Rehost_AsOnPrem();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                WebApp_IaaS_Server_Rehost_AsOnPrem_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated WebApp_IaaS_Server_Rehost_AsOnPrem excel model with data of {WebApp_IaaS_Server_Rehost_AsOnPrem_List.Count} machines");
            return true;
        }

        private void Process_WebApp_Opportunity_Model(List<WebApp_Opportunity> WebApp_Opportunity_List, HashSet<string> WebApp_Opportunity)
        {
            if (WebApp_Opportunity == null)
                return;
            if (WebApp_Opportunity.Count <= 0)
                return;
            if (UserInputObj.PreferredOptimizationObj.OptimizationPreference.Value.Equals("Migrate to all IaaS"))
                return;

            bool isSuccessful = false;
            isSuccessful = Create_WebApp_Opportunity_Model(WebApp_Opportunity_List, WebApp_Opportunity);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for WebApp_Opportunity");
        }

        private bool Create_WebApp_Opportunity_Model(List<WebApp_Opportunity> WebApp_Opportunity_List, HashSet<string> WebApp_Opportunity)
        {
            if (AzureWebAppData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for WebApp_Opportunity as Azure WebApp dataset is null");
                return false;
            }
            if (AzureWebAppData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning($"Not creating excel model for WebApp_Opportunity as Azure WebApp dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for WebApp_Opportunity");

            if (WebApp_Opportunity_List == null)
                WebApp_Opportunity_List = new List<WebApp_Opportunity>();
            
            foreach (var webAppId in WebApp_Opportunity)
            {
                if (!AzureWebAppData.ContainsKey(webAppId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Web App ID: {webAppId} is not present in Azure Web App dataset, skipping");
                    continue;
                }

                var value = AzureWebAppData[webAppId];

                WebApp_Opportunity obj = new WebApp_Opportunity();

                obj.MachineName = value.MachineName;
                obj.WebAppName = value.WebAppName;
                obj.Environment = value.Environment;
                obj.AzureAppServiceReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureAppServiceReadiness_Issues = UtilityFunctions.GetMigrationIssueByType(value.MigrationIssues, IssueCategories.Issue);
                obj.AzureRecommendedTarget = value.AzureRecommendedTarget;
                obj.GroupName = value.GroupName;
                obj.MachineId = value.DiscoveredMachineId;

                WebApp_Opportunity_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Update WebApp_Opportunity model with data of {WebApp_Opportunity_List.Count} web applications");
            return true;
        }

        private void Process_VM_SS_IaaS_Server_Rehost_Perf_Model(List<VM_SS_IaaS_Server_Rehost_Perf> VM_SS_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureVM_Opportunity_Perf, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (SqlServicesVM == null)
                return;
            if (SqlServicesVM.Count <= 0)
                return;
            if (!UserInputObj.PreferredOptimizationObj.AssessSqlServicesSeparately)
                return;
            
            if (AzureSQL_IaaS_Server == null)
                AzureSQL_IaaS_Server = new HashSet<string>();
            
            bool isSuccessful = false;
            isSuccessful = Create_VM_SS_IaaS_Server_Rehost_Perf_Model(VM_SS_IaaS_Server_Rehost_Perf_List, AzureVM_Opportunity_Perf, AzureSQL_IaaS_Server);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_SS_IaaS_Server_Rehost_Perf");
        }

        private bool Create_VM_SS_IaaS_Server_Rehost_Perf_Model(List<VM_SS_IaaS_Server_Rehost_Perf> VM_SS_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureVM_Opportunity_Perf, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (AzureVMPerformanceBasedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_SS_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is null");
                return false;
            }
            if (AzureVMPerformanceBasedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_SS_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_SS_IaaS_Server_Rehost_Perf");

            if (VM_SS_IaaS_Server_Rehost_Perf_List == null)
                VM_SS_IaaS_Server_Rehost_Perf_List = new List<VM_SS_IaaS_Server_Rehost_Perf>();
            if (AzureVM_Opportunity_Perf == null)
                AzureVM_Opportunity_Perf = new HashSet<string>();
            
            foreach (var discoveredMachineId in SqlServicesVM)
            {
                if (AzureSQL_IaaS_Server.Contains(discoveredMachineId))
                    continue;

                if (!AzureVMPerformanceBasedMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM Perf dataset, skipping");
                    continue;
                }

                var value = AzureVMPerformanceBasedMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_Perf.Contains(discoveredMachineId))
                        AzureVM_Opportunity_Perf.Add(discoveredMachineId);
                    continue;
                }

                VM_SS_IaaS_Server_Rehost_Perf obj = new VM_SS_IaaS_Server_Rehost_Perf();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureVMReadiness_Warnings = value.SuitabilityExplanation;
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyComputeCostEstimate_RI3year = value.MonthlyComputeCostEstimate_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.MonthlyComputeCostEstimate_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.MonthlyComputeCostEstimate_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.MonthlyComputeCostEstimate_ASP3year;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = value.PercentageOfCoresUtilization;
                obj.MemoryUtilizationPercentage = value.PercentageOfMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.AzureBackupMonthlyCostEstimate;

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_SS_IaaS_Server_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_SS_IaaS_Server_Rehost_Perf excel model with data of {VM_SS_IaaS_Server_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void Process_VM_SS_IaaS_Server_Rehost_AsOnPrem_Model(List<VM_SS_IaaS_Server_Rehost_AsOnPrem> VM_SS_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (SqlServicesVM == null)
                return;
            if (SqlServicesVM.Count <= 0)
                return;
            if (!UserInputObj.PreferredOptimizationObj.AssessSqlServicesSeparately)
                return;

            if (AzureSQL_IaaS_Server == null)
                AzureSQL_IaaS_Server = new HashSet<string>();
            
            bool isSuccessful = false;
            isSuccessful = Create_VM_SS_IaaS_Server_Rehost_AsOnPrem_Model(VM_SS_IaaS_Server_Rehost_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem, AzureSQL_IaaS_Server);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_SS_IaaS_Server_Rehost_AsOnPrem");
        }

        private bool Create_VM_SS_IaaS_Server_Rehost_AsOnPrem_Model(List<VM_SS_IaaS_Server_Rehost_AsOnPrem> VM_SS_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem, HashSet<string> AzureSQL_IaaS_Server)
        {
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_SS_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is null");
                return false;
            }
            if (AzureVMAsOnPremMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_SS_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_SS_IaaS_Server_Rehost_AsOnPrem");

            if (VM_SS_IaaS_Server_Rehost_AsOnPrem_List == null)
                VM_SS_IaaS_Server_Rehost_AsOnPrem_List = new List<VM_SS_IaaS_Server_Rehost_AsOnPrem>();
            if (AzureVM_Opportunity_AsOnPrem == null)
                AzureVM_Opportunity_AsOnPrem = new HashSet<string>();
            
            foreach (var discoveredMachineId in SqlServicesVM)
            {
                if (AzureSQL_IaaS_Server.Contains(discoveredMachineId))
                    continue;

                if (!AzureVMAsOnPremMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM AsOnPrem dataset, skipping");
                    continue;
                }

                var value = AzureVMAsOnPremMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_AsOnPrem.Contains(discoveredMachineId))
                        AzureVM_Opportunity_AsOnPrem.Add(discoveredMachineId);
                    continue;
                }

                VM_SS_IaaS_Server_Rehost_AsOnPrem obj = new VM_SS_IaaS_Server_Rehost_AsOnPrem();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_SS_IaaS_Server_Rehost_AsOnPrem_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_SS_IaaS_Server_Rehost_AsOnPrem excel model with data of {VM_SS_IaaS_Server_Rehost_AsOnPrem_List.Count} machines");
            return true;
        }

        private void Process_VM_IaaS_Server_Rehost_Perf_Model(List<VM_IaaS_Server_Rehost_Perf> VM_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (GeneralVM == null)
                return;
            if (GeneralVM.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_VM_IaaS_Server_Rehost_Perf_Model(VM_IaaS_Server_Rehost_Perf_List, AzureVM_Opportunity_Perf);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_IaaS_Server_Rehost_Perf");
        }

        private bool Create_VM_IaaS_Server_Rehost_Perf_Model(List<VM_IaaS_Server_Rehost_Perf> VM_IaaS_Server_Rehost_Perf_List, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureVMPerformanceBasedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is null");
                return false;
            }
            if (AzureVMPerformanceBasedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_IaaS_Server_Rehost_Perf as Azure VM Perf dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_IaaS_Server_Rehost_Perf");

            if (VM_IaaS_Server_Rehost_Perf_List == null)
                VM_IaaS_Server_Rehost_Perf_List = new List<VM_IaaS_Server_Rehost_Perf>();
            if (AzureVM_Opportunity_Perf == null)
                AzureVM_Opportunity_Perf = new HashSet<string>();
            
            foreach (var discoveredMachineId in GeneralVM)
            {
                if (!AzureVMPerformanceBasedMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM Perf dataset, skipping");
                    continue;
                }

                var value = AzureVMPerformanceBasedMachinesData[discoveredMachineId];
                
                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_Perf.Contains(discoveredMachineId))
                        AzureVM_Opportunity_Perf.Add(discoveredMachineId);
                    continue;
                }

                VM_IaaS_Server_Rehost_Perf obj = new VM_IaaS_Server_Rehost_Perf();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureVMReadiness_Warnings = value.SuitabilityExplanation;
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyComputeCostEstimate_RI3year = value.MonthlyComputeCostEstimate_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.MonthlyComputeCostEstimate_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.MonthlyComputeCostEstimate_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.MonthlyComputeCostEstimate_ASP3year;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = value.PercentageOfCoresUtilization;
                obj.MemoryUtilizationPercentage = value.PercentageOfMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.AzureBackupMonthlyCostEstimate;

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_IaaS_Server_Rehost_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_IaaS_Server_Rehost_Perf excel model with data of {VM_IaaS_Server_Rehost_Perf_List.Count} machines");
            return true;
        }

        private void Process_VM_IaaS_Server_Rehost_AsOnPrem_Model(List<VM_IaaS_Server_Rehost_AsOnPrem> VM_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (GeneralVM == null)
                return;
            if (GeneralVM.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_VM_IaaS_Server_Rehost_AsOnPrem_Model(VM_IaaS_Server_Rehost_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_IaaS_Server_Rehost_AsOnPrem");
        }

        private bool Create_VM_IaaS_Server_Rehost_AsOnPrem_Model(List<VM_IaaS_Server_Rehost_AsOnPrem> VM_IaaS_Server_Rehost_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is null");
                return false;
            }
            if (AzureVMAsOnPremMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_IaaS_Server_Rehost_AsOnPrem as Azure VM AsOnPrem dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_IaaS_Server_Rehost_AsOnPrem");

            if (VM_IaaS_Server_Rehost_AsOnPrem_List == null)
                VM_IaaS_Server_Rehost_AsOnPrem_List = new List<VM_IaaS_Server_Rehost_AsOnPrem>();
            if (AzureVM_Opportunity_AsOnPrem == null)
                AzureVM_Opportunity_AsOnPrem = new HashSet<string>();
            
            foreach (var discoveredMachineId in GeneralVM)
            {
                if (!AzureVMAsOnPremMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM AsOnPrem dataset, skipping");
                    continue;
                }

                var value = AzureVMAsOnPremMachinesData[discoveredMachineId];

                if (value.Suitability != Suitabilities.Suitable && value.Suitability != Suitabilities.ConditionallySuitable)
                {
                    if (!AzureVM_Opportunity_AsOnPrem.Contains(discoveredMachineId))
                        AzureVM_Opportunity_AsOnPrem.Add(discoveredMachineId);
                    continue;
                }

                VM_IaaS_Server_Rehost_AsOnPrem obj = new VM_IaaS_Server_Rehost_AsOnPrem();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_IaaS_Server_Rehost_AsOnPrem_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_IaaS_Server_Rehost_AsOnPrem excel model with data of {VM_IaaS_Server_Rehost_AsOnPrem_List.Count} machines");
            return true;
        }

        private void Process_VM_Opportunity_Perf_Model(List<VM_Opportunity_Perf> VM_Opportunity_Perf_List, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureVM_Opportunity_Perf == null)
                return;
            if (AzureVM_Opportunity_Perf.Count <= 0)
                return;
            
            bool isSuccessful = false;
            isSuccessful = Create_VM_Opportunity_Perf_Model(VM_Opportunity_Perf_List, AzureVM_Opportunity_Perf);
            
            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_Opportunity");
        }

        private bool Create_VM_Opportunity_Perf_Model(List<VM_Opportunity_Perf> VM_Opportunity_Perf_List, HashSet<string> AzureVM_Opportunity_Perf)
        {
            if (AzureVMPerformanceBasedMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_Opportunity_Perf as Azure VM Perf dataset is null");
                return false;
            }
            if (AzureVMPerformanceBasedMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not generating excel model for VM_Opportunity_Perf as Azure VM Perf dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_Opportunity_Perf");

            if (VM_Opportunity_Perf_List == null)
                VM_Opportunity_Perf_List = new List<VM_Opportunity_Perf>();
            
            foreach (var discoveredMachineId in AzureVM_Opportunity_Perf)
            {
                if (!AzureVMPerformanceBasedMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM Perf dataset, skipping");
                    continue;
                }

                var value = AzureVMPerformanceBasedMachinesData[discoveredMachineId];

                VM_Opportunity_Perf obj = new VM_Opportunity_Perf();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.AzureVMReadiness_Warnings = value.SuitabilityExplanation;
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyComputeCostEstimate_RI3year = value.MonthlyComputeCostEstimate_RI3year;
                obj.MonthlyComputeCostEstimate_AHUB = value.MonthlyComputeCostEstimate_AHUB;
                obj.MonthlyComputeCostEstimate_AHUB_RI3year = value.MonthlyComputeCostEstimate_AHUB_RI3year;
                obj.MonthlyComputeCostEstimate_ASP3year = value.MonthlyComputeCostEstimate_ASP3year;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.CpuUtilizationPercentage = value.PercentageOfCoresUtilization;
                obj.MemoryUtilizationPercentage = value.PercentageOfMemoryUtilization;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyAzureSiteRecoveryCostEstimate = value.AzureSiteRecoveryMonthlyCostEstimate;
                obj.MonthlyAzureBackupCostEstimate = value.AzureBackupMonthlyCostEstimate;

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_Opportunity_Perf_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_Opportunity_Perf excel model with data of {VM_Opportunity_Perf_List.Count} machines");
            return true;
        }

        private void Process_VM_Opportunity_AsOnPrem_Model(List<VM_Opportunity_AsOnPrem> VM_Opportunity_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureVM_Opportunity_AsOnPrem == null)
                return;
            if (AzureVM_Opportunity_AsOnPrem.Count <= 0)
                return;

            bool isSuccessful = false;
            isSuccessful = Create_VM_Opportunity_AsOnPrem_Model(VM_Opportunity_AsOnPrem_List, AzureVM_Opportunity_AsOnPrem);

            if (!isSuccessful)
                UserInputObj.LoggerObj.LogWarning("Encountered issue while generating excel model for VM_Opportunity_AsOnPrem");
        }

        private bool Create_VM_Opportunity_AsOnPrem_Model(List<VM_Opportunity_AsOnPrem> VM_Opportunity_AsOnPrem_List, HashSet<string> AzureVM_Opportunity_AsOnPrem)
        {
            if (AzureVMAsOnPremMachinesData == null)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_Opportunity_AsOnPrem as Azure VM AsOnPrem dataset is null");
                return false;
            }
            if (AzureVMAsOnPremMachinesData.Count <= 0)
            {
                UserInputObj.LoggerObj.LogWarning("Not creating excel model for VM_Opportunity_AsOnPrem as Azure VM AsOnPrem dataset is empty");
                return false;
            }

            UserInputObj.LoggerObj.LogInformation("Creating excel model for VM_Opportunity_AsOnPrem");

            if (VM_Opportunity_AsOnPrem_List == null)
                VM_Opportunity_AsOnPrem_List = new List<VM_Opportunity_AsOnPrem>();
            
            foreach (var discoveredMachineId in AzureVM_Opportunity_AsOnPrem)
            {
                if (!AzureVMAsOnPremMachinesData.ContainsKey(discoveredMachineId))
                {
                    UserInputObj.LoggerObj.LogWarning($"Discovered Machine ID: {discoveredMachineId} does not exist in Azure VM AsOnPrem dataset, skipping");
                    continue;
                }

                var value = AzureVMAsOnPremMachinesData[discoveredMachineId];

                VM_Opportunity_AsOnPrem obj = new VM_Opportunity_AsOnPrem();

                obj.MachineName = value.DisplayName;
                obj.Environment = value.Environment;
                obj.AzureVMReadiness = new EnumDescriptionHelper().GetEnumDescription(value.Suitability);
                obj.RecommendedVMSize = value.RecommendedVMSize;
                obj.MonthlyComputeCostEstimate = value.MonthlyComputeCostEstimate;
                obj.MonthlyStorageCostEstimate = value.StorageMonthlyCost;
                obj.OperatingSystem = UtilityFunctions.GetStringValue(value.OperatingSystem);
                obj.VMHost = UtilityFunctions.GetStringValue(value.DatacenterManagementServerName);
                obj.BootType = UtilityFunctions.GetStringValue(value.BootType);
                obj.Cores = value.NumberOfCores;
                obj.MemoryInMB = value.MegabytesOfMemory;
                obj.StorageInGB = UtilityFunctions.GetTotalStorage(value.Disks);
                obj.NetworkAdapters = value.NetworkAdapters;

                var macIpKvp = UtilityFunctions.ParseMacIpAddress(value.NetworkAdapterList);
                obj.MacAddresses = macIpKvp.Key;
                obj.IpAddresses = macIpKvp.Value;

                obj.DiskNames = UtilityFunctions.GetDiskNames(value.Disks);
                obj.AzureDiskReadiness = UtilityFunctions.GetDiskReadiness(value.Disks);
                obj.RecommendedDiskSKUs = UtilityFunctions.GetRecommendedDiskSKUs(value.Disks);

                obj.StandardHddDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Standard);
                obj.StandardSsdDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.PremiumDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Premium);
                obj.UltraDisks = UtilityFunctions.GetDiskTypeCount(value.Disks, RecommendedDiskTypes.Ultra);

                obj.MonthlyStorageCostForStandardHddDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Standard);
                obj.MonthlyStorageCostForStandardSsdDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.StandardSSD);
                obj.MonthlyStorageCostForPremiumDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Premium);
                obj.MonthlyStorageCostForUltraDisks = UtilityFunctions.GetDiskTypeStorageCost(value.Disks, RecommendedDiskTypes.Ultra);

                obj.GroupName = value.GroupName;
                obj.MachineId = value.DatacenterMachineArmId;

                VM_Opportunity_AsOnPrem_List.Add(obj);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated VM_Opportunity_AsOnPrem excel model with data of {VM_Opportunity_AsOnPrem_List.Count} machines");
            return true;
        }
    }
}