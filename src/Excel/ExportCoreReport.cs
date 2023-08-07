using ClosedXML.Excel;
using System.Collections.Generic;

using Azure.Migrate.Export.Models;
using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Excel
{
    public class ExportCoreReport
    {
        private readonly CoreProperties CorePropertiesObj;
        private readonly List<All_VM_IaaS_Server_Rehost_Perf> All_VM_IaaS_Server_Rehost_Perf_List;
        private readonly List<SQL_All_Instances> SQL_All_Instances_List;
        private readonly List<SQL_MI_PaaS> SQL_MI_PaaS_List;
        private readonly List<SQL_IaaS_Instance_Rehost_Perf> SQL_IaaS_Instance_Rehost_Perf_List;
        private readonly List<SQL_IaaS_Server_Rehost_Perf> SQL_IaaS_Server_Rehost_Perf_List;
        private readonly List<SQL_IaaS_Server_Rehost_AsOnPrem> SQL_IaaS_Server_Rehost_AsOnPrem_List;
        private readonly List<WebApp_PaaS> WebApp_PaaS_List;
        private readonly List<WebApp_IaaS_Server_Rehost_Perf> WebApp_IaaS_Server_Rehost_Perf_List;
        private readonly List<WebApp_IaaS_Server_Rehost_AsOnPrem> WebApp_IaaS_Server_Rehost_AsOnPrem_List;
        private readonly List<VM_SS_IaaS_Server_Rehost_Perf> VM_SS_IaaS_Server_Rehost_Perf_List;
        private readonly List<VM_SS_IaaS_Server_Rehost_AsOnPrem> VM_SS_IaaS_Server_Rehost_AsOnPrem_List;
        private readonly List<VM_IaaS_Server_Rehost_Perf> VM_IaaS_Server_Rehost_Perf_List;
        private readonly List<VM_IaaS_Server_Rehost_AsOnPrem> VM_IaaS_Server_Rehost_AsOnPrem_List;
        private readonly Business_Case Business_Case_Data;
        private readonly Cash_Flows Cash_Flows_Data;
        private readonly List<Decommissioned_Machines> Decommissioned_Machines_List;

        XLWorkbook CoreWb;

        public ExportCoreReport
            (
                CoreProperties corePropertiesObj,
                List<All_VM_IaaS_Server_Rehost_Perf> all_VM_IaaS_Server_Rehost_Perf_List,
                List<SQL_All_Instances> sql_All_Instances_List,
                List<SQL_MI_PaaS> sql_MI_PaaS_List,
                List<SQL_IaaS_Instance_Rehost_Perf> sql_IaaS_Instance_Rehost_Perf_List,
                List<SQL_IaaS_Server_Rehost_Perf> sql_IaaS_Server_Rehost_Perf_List,
                List<SQL_IaaS_Server_Rehost_AsOnPrem> sql_IaaS_Server_Rehost_AsOnPrem_List,
                List<WebApp_PaaS> webApp_PaaS_List,
                List<WebApp_IaaS_Server_Rehost_Perf> webApp_IaaS_Server_Rehost_Perf_List,
                List<WebApp_IaaS_Server_Rehost_AsOnPrem> webApp_IaaS_Server_Rehost_AsOnPrem_List,
                List<VM_SS_IaaS_Server_Rehost_Perf> vm_SS_IaaS_Server_Rehost_Perf_List,
                List<VM_SS_IaaS_Server_Rehost_AsOnPrem> vm_SS_IaaS_Server_Rehost_AsOnPrem_List,
                List<VM_IaaS_Server_Rehost_Perf> vm_IaaS_Server_Rehost_Perf_List,
                List<VM_IaaS_Server_Rehost_AsOnPrem> vm_IaaS_Server_Rehost_AsOnPrem_List,
                Business_Case business_Case_Data,
                Cash_Flows cash_Flows_Data,
                List<Decommissioned_Machines> decommissioned_Machines_List
            )
        {
            CorePropertiesObj = corePropertiesObj;
            All_VM_IaaS_Server_Rehost_Perf_List = all_VM_IaaS_Server_Rehost_Perf_List;
            SQL_All_Instances_List = sql_All_Instances_List;
            SQL_MI_PaaS_List = sql_MI_PaaS_List;
            SQL_IaaS_Instance_Rehost_Perf_List = sql_IaaS_Instance_Rehost_Perf_List;
            SQL_IaaS_Server_Rehost_Perf_List = sql_IaaS_Server_Rehost_Perf_List;
            SQL_IaaS_Server_Rehost_AsOnPrem_List = sql_IaaS_Server_Rehost_AsOnPrem_List;
            WebApp_PaaS_List = webApp_PaaS_List;
            WebApp_IaaS_Server_Rehost_Perf_List = webApp_IaaS_Server_Rehost_Perf_List;
            WebApp_IaaS_Server_Rehost_AsOnPrem_List = webApp_IaaS_Server_Rehost_AsOnPrem_List;
            VM_SS_IaaS_Server_Rehost_Perf_List = vm_SS_IaaS_Server_Rehost_Perf_List;
            VM_SS_IaaS_Server_Rehost_AsOnPrem_List = vm_SS_IaaS_Server_Rehost_AsOnPrem_List;
            VM_IaaS_Server_Rehost_Perf_List = vm_IaaS_Server_Rehost_Perf_List;
            VM_IaaS_Server_Rehost_AsOnPrem_List = vm_IaaS_Server_Rehost_AsOnPrem_List;
            Business_Case_Data = business_Case_Data;
            Cash_Flows_Data = cash_Flows_Data;
            Decommissioned_Machines_List = decommissioned_Machines_List;

            CoreWb = new XLWorkbook();
        }

        public void GenerateCoreReportExcel()
        {
            Generate_Properties_Worksheet();
            Generate_Business_Case_Worksheet();
            Generate_Cash_Flows_Worksheet();
            Generate_SQL_MI_PaaS_Worksheet();
            Generate_SQL_IaaS_Instance_Rehost_Perf_Worksheet();
            Generate_SQL_IaaS_Server_Rehost_Perf_Worksheet();
            Generate_SQL_IaaS_Server_Rehost_AsOnPrem_Worksheet();
            Generate_WebApp_PaaS_Worksheet();
            Generate_WebApp_IaaS_Server_Rehost_Perf_Worksheet();
            Generate_WebApp_IaaS_Server_Rehost_AsOnPrem_Worksheet();
            Generate_VM_IaaS_Server_Rehost_Perf_Worksheet();
            Generate_VM_IaaS_Server_Rehost_AsOnPrem_Worksheet();
            Generate_VM_SS_IaaS_Server_Rehost_Perf_Worksheet();
            Generate_VM_SS_IaaS_Server_Rehost_AsOnPrem_Worksheet();
            Generate_SQL_All_Instances_Worksheet();
            Generate_All_VM_IaaS_Server_Rehost_Perf_Worksheet();
            Generate_Decommissioned_Machines_Worksheet();
            
            CoreWb.SaveAs(CoreReportConstants.CoreReportPath);
        }

        private void Generate_Properties_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.PropertiesTabName, 1);
            var propertyHeaders = CoreReportConstants.PropertyList;

            for (int i = 0; i < propertyHeaders.Count; i++)
                dataWs.Cell(1, i + 1).Value = propertyHeaders[i];

            // Add values: important to add in the same order as above

            dataWs.Cell(2, 1).Value = CorePropertiesObj.TenantId;
            dataWs.Cell(2, 2).Value = CorePropertiesObj.Subscription;
            dataWs.Cell(2, 3).Value = CorePropertiesObj.ResourceGroupName;
            dataWs.Cell(2, 4).Value = CorePropertiesObj.AzureMigrateProjectName;
            dataWs.Cell(2, 5).Value = CorePropertiesObj.AssessmentSiteName;
            dataWs.Cell(2, 6).Value = CorePropertiesObj.Workflow;
            dataWs.Cell(2, 7).Value = CorePropertiesObj.TargetRegion;
            dataWs.Cell(2, 8).Value = CorePropertiesObj.Currency;
            dataWs.Cell(2, 9).Value = CorePropertiesObj.AssessmentDuration;
            dataWs.Cell(2, 10).Value = CorePropertiesObj.OptimizationPreference;
            dataWs.Cell(2, 11).Value = CorePropertiesObj.AssessSQLServices;
        }

        private void Generate_Business_Case_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.Business_Case_TabName, 2);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.Business_Case_Columns);
            for (int i = 0; i < CoreReportConstants.Business_Case_RowTypes.Count; i++)
                dataWs.Cell(i + 2, 1).Value = CoreReportConstants.Business_Case_RowTypes[i];

            if (Business_Case_Data == null)
                return;

            dataWs.Cell(2, 2).Value = Business_Case_Data.OnPremisesCost.ComputeCost;
            dataWs.Cell(3, 2).Value = Business_Case_Data.OnPremisesCost.LicenseCost;
            dataWs.Cell(4, 2).Value = Business_Case_Data.OnPremisesCost.EsuIaaSLicenseCost;
            dataWs.Cell(5, 2).Value = Business_Case_Data.OnPremisesCost.EsuPaaSLicenseCost;
            dataWs.Cell(6, 2).Value = Business_Case_Data.OnPremisesCost.StorageCost;
            dataWs.Cell(7, 2).Value = Business_Case_Data.OnPremisesCost.NetworkCost;
            dataWs.Cell(8, 2).Value = Business_Case_Data.OnPremisesCost.SecurityCost;
            dataWs.Cell(9, 2).Value = Business_Case_Data.OnPremisesCost.ITStaffCost;
            dataWs.Cell(10, 2).Value = Business_Case_Data.OnPremisesCost.FacilitiesCost;

            dataWs.Cell(2, 3).Value = Business_Case_Data.AzureIaaSCost.ComputeCost;
            dataWs.Cell(3, 3).Value = Business_Case_Data.AzureIaaSCost.LicenseCost;
            dataWs.Cell(4, 3).Value = Business_Case_Data.AzureIaaSCost.EsuIaaSLicenseCost;
            dataWs.Cell(5, 3).Value = Business_Case_Data.AzureIaaSCost.EsuPaaSLicenseCost;
            dataWs.Cell(6, 3).Value = Business_Case_Data.AzureIaaSCost.StorageCost;
            dataWs.Cell(7, 3).Value = Business_Case_Data.AzureIaaSCost.NetworkCost;
            dataWs.Cell(8, 3).Value = Business_Case_Data.AzureIaaSCost.SecurityCost;
            dataWs.Cell(9, 3).Value = Business_Case_Data.AzureIaaSCost.ITStaffCost;
            dataWs.Cell(10, 3).Value = Business_Case_Data.AzureIaaSCost.FacilitiesCost;

            dataWs.Cell(2, 4).Value = Business_Case_Data.AzurePaaSCost.ComputeCost;
            dataWs.Cell(3, 4).Value = Business_Case_Data.AzurePaaSCost.LicenseCost;
            dataWs.Cell(4, 4).Value = Business_Case_Data.AzurePaaSCost.EsuIaaSLicenseCost;
            dataWs.Cell(5, 4).Value = Business_Case_Data.AzurePaaSCost.EsuPaaSLicenseCost;
            dataWs.Cell(6, 4).Value = Business_Case_Data.AzurePaaSCost.StorageCost;
            dataWs.Cell(7, 4).Value = Business_Case_Data.AzurePaaSCost.NetworkCost;
            dataWs.Cell(8, 4).Value = Business_Case_Data.AzurePaaSCost.SecurityCost;
            dataWs.Cell(9, 4).Value = Business_Case_Data.AzurePaaSCost.ITStaffCost;
            dataWs.Cell(10, 4).Value = Business_Case_Data.AzurePaaSCost.FacilitiesCost;

            dataWs.Cell(2, 5).Value = Business_Case_Data.TotalAzureCost.ComputeCost;
            dataWs.Cell(3, 5).Value = Business_Case_Data.TotalAzureCost.LicenseCost;
            dataWs.Cell(4, 5).Value = Business_Case_Data.TotalAzureCost.EsuIaaSLicenseCost;
            dataWs.Cell(5, 5).Value = Business_Case_Data.TotalAzureCost.EsuPaaSLicenseCost;
            dataWs.Cell(6, 5).Value = Business_Case_Data.TotalAzureCost.StorageCost;
            dataWs.Cell(7, 5).Value = Business_Case_Data.TotalAzureCost.NetworkCost;
            dataWs.Cell(8, 5).Value = Business_Case_Data.TotalAzureCost.SecurityCost;
            dataWs.Cell(9, 5).Value = Business_Case_Data.TotalAzureCost.ITStaffCost;
            dataWs.Cell(10, 5).Value = Business_Case_Data.TotalAzureCost.FacilitiesCost;
        }

        private void Generate_Cash_Flows_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.Cash_Flows_TabName, 3);

            for (int i = 0; i < CoreReportConstants.Cash_Flows_Years.Count; i++)
                dataWs.Cell(1, i + 3).Value = CoreReportConstants.Cash_Flows_Years[i];
            
            for (int i = 0; i < CoreReportConstants.Cash_Flows_CloudComputingServiceTypes.Count; i++)
            {
                dataWs.Cell(3 * i + 2, 1).Value = CoreReportConstants.Cash_Flows_CloudComputingServiceTypes[i];
                dataWs.Cell(3 * i + 3, 1).Value = CoreReportConstants.Cash_Flows_CloudComputingServiceTypes[i];
                dataWs.Cell(3 * i + 4, 1).Value = CoreReportConstants.Cash_Flows_CloudComputingServiceTypes[i];
            }

            for (int i = 0; i < CoreReportConstants.Cash_Flows_Types.Count; i++)
            {
                dataWs.Cell(3 * 1 + i - 1, 2).Value = CoreReportConstants.Cash_Flows_Types[i];
                dataWs.Cell(3 * 2 + i - 1, 2).Value = CoreReportConstants.Cash_Flows_Types[i];
                dataWs.Cell(3 * 3 + i - 1, 2).Value = CoreReportConstants.Cash_Flows_Types[i];
            }

            // Total
            // Current state Cash Flow
            dataWs.Cell(2, 3).Value = Cash_Flows_Data.TotalYOYCosts.OnPremisesCostYOY.Year0;
            dataWs.Cell(2, 4).Value = Cash_Flows_Data.TotalYOYCosts.OnPremisesCostYOY.Year1;
            dataWs.Cell(2, 5).Value = Cash_Flows_Data.TotalYOYCosts.OnPremisesCostYOY.Year2;
            dataWs.Cell(2, 6).Value = Cash_Flows_Data.TotalYOYCosts.OnPremisesCostYOY.Year3;

            // Future state Cash Flow
            dataWs.Cell(3, 3).Value = Cash_Flows_Data.TotalYOYCosts.AzureCostYOY.Year0;
            dataWs.Cell(3, 4).Value = Cash_Flows_Data.TotalYOYCosts.AzureCostYOY.Year1;
            dataWs.Cell(3, 5).Value = Cash_Flows_Data.TotalYOYCosts.AzureCostYOY.Year2;
            dataWs.Cell(3, 6).Value = Cash_Flows_Data.TotalYOYCosts.AzureCostYOY.Year3;

            // Savings
            dataWs.Cell(4, 3).Value = Cash_Flows_Data.TotalYOYCosts.SavingsYOY.Year0;
            dataWs.Cell(4, 4).Value = Cash_Flows_Data.TotalYOYCosts.SavingsYOY.Year1;
            dataWs.Cell(4, 5).Value = Cash_Flows_Data.TotalYOYCosts.SavingsYOY.Year2;
            dataWs.Cell(4, 6).Value = Cash_Flows_Data.TotalYOYCosts.SavingsYOY.Year3;

            // IaaS
            // Current state Cash Flow
            dataWs.Cell(5, 3).Value = Cash_Flows_Data.IaaSYOYCosts.OnPremisesCostYOY.Year0;
            dataWs.Cell(5, 4).Value = Cash_Flows_Data.IaaSYOYCosts.OnPremisesCostYOY.Year1;
            dataWs.Cell(5, 5).Value = Cash_Flows_Data.IaaSYOYCosts.OnPremisesCostYOY.Year2;
            dataWs.Cell(5, 6).Value = Cash_Flows_Data.IaaSYOYCosts.OnPremisesCostYOY.Year3;

            // Future state Cash Flow
            dataWs.Cell(6, 3).Value = Cash_Flows_Data.IaaSYOYCosts.AzureCostYOY.Year0;
            dataWs.Cell(6, 4).Value = Cash_Flows_Data.IaaSYOYCosts.AzureCostYOY.Year1;
            dataWs.Cell(6, 5).Value = Cash_Flows_Data.IaaSYOYCosts.AzureCostYOY.Year2;
            dataWs.Cell(6, 6).Value = Cash_Flows_Data.IaaSYOYCosts.AzureCostYOY.Year3;

            // Savings
            dataWs.Cell(7, 3).Value = Cash_Flows_Data.IaaSYOYCosts.SavingsYOY.Year0;
            dataWs.Cell(7, 4).Value = Cash_Flows_Data.IaaSYOYCosts.SavingsYOY.Year1;
            dataWs.Cell(7, 5).Value = Cash_Flows_Data.IaaSYOYCosts.SavingsYOY.Year2;
            dataWs.Cell(7, 6).Value = Cash_Flows_Data.IaaSYOYCosts.SavingsYOY.Year3;

            // PaaS
            // Current state Cash Flow
            dataWs.Cell(8, 3).Value = Cash_Flows_Data.PaaSYOYCosts.OnPremisesCostYOY.Year0;
            dataWs.Cell(8, 4).Value = Cash_Flows_Data.PaaSYOYCosts.OnPremisesCostYOY.Year1;
            dataWs.Cell(8, 5).Value = Cash_Flows_Data.PaaSYOYCosts.OnPremisesCostYOY.Year2;
            dataWs.Cell(8, 6).Value = Cash_Flows_Data.PaaSYOYCosts.OnPremisesCostYOY.Year3;

            // Future state Cash Flow
            dataWs.Cell(9, 3).Value = Cash_Flows_Data.PaaSYOYCosts.AzureCostYOY.Year0;
            dataWs.Cell(9, 4).Value = Cash_Flows_Data.PaaSYOYCosts.AzureCostYOY.Year1;
            dataWs.Cell(9, 5).Value = Cash_Flows_Data.PaaSYOYCosts.AzureCostYOY.Year2;
            dataWs.Cell(9, 6).Value = Cash_Flows_Data.PaaSYOYCosts.AzureCostYOY.Year3;

            // Savings
            dataWs.Cell(10, 3).Value = Cash_Flows_Data.PaaSYOYCosts.SavingsYOY.Year0;
            dataWs.Cell(10, 4).Value = Cash_Flows_Data.PaaSYOYCosts.SavingsYOY.Year1;
            dataWs.Cell(10, 5).Value = Cash_Flows_Data.PaaSYOYCosts.SavingsYOY.Year2;
            dataWs.Cell(10, 6).Value = Cash_Flows_Data.PaaSYOYCosts.SavingsYOY.Year3;
        }

        private void Generate_SQL_MI_PaaS_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_MI_PaaS_TabName, 4);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_MI_PaaS_Columns);

            if (SQL_MI_PaaS_List != null && SQL_MI_PaaS_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_MI_PaaS_List);
        }

        private void Generate_SQL_IaaS_Instance_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Instance_Rehost_Perf_TabName, 5);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Instance_Rehost_Perf_Columns);

            if (SQL_IaaS_Instance_Rehost_Perf_List != null && SQL_IaaS_Instance_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Instance_Rehost_Perf_List);
        }

        private void Generate_SQL_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Server_Rehost_Perf_TabName, 6);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Server_Rehost_Perf_Columns);

            if (SQL_IaaS_Server_Rehost_Perf_List != null && SQL_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_SQL_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Server_Rehost_AsOnPrem_TabName, 7);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (SQL_IaaS_Server_Rehost_AsOnPrem_List != null && SQL_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_WebApp_PaaS_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_PaaS_TabName, 8);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_PaaS_Columns);

            if (WebApp_PaaS_List != null && WebApp_PaaS_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_PaaS_List);
        }

        private void Generate_WebApp_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_IaaS_Server_Rehost_Perf_TabName, 9);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_IaaS_Server_Rehost_Perf_Columns);

            if (WebApp_IaaS_Server_Rehost_Perf_List != null && WebApp_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_WebApp_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_IaaS_Server_Rehost_AsOnPrem_TabName, 10);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (WebApp_IaaS_Server_Rehost_AsOnPrem_List != null && WebApp_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_VM_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_IaaS_Server_Rehost_Perf_TabName, 11);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_IaaS_Server_Rehost_Perf_Columns);

            if (VM_IaaS_Server_Rehost_Perf_List != null && VM_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_VM_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_IaaS_Server_Rehost_AsOnPrem_TabName, 12);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (VM_IaaS_Server_Rehost_AsOnPrem_List != null && VM_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_VM_SS_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_SS_IaaS_Server_Rehost_Perf_TabName, 13);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_SS_IaaS_Server_Rehost_Perf_Columns);

            if (VM_SS_IaaS_Server_Rehost_Perf_List != null && VM_SS_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_SS_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_VM_SS_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_SS_IaaS_Server_Rehost_AsOnPrem_TabName, 14);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_SS_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (VM_SS_IaaS_Server_Rehost_AsOnPrem_List != null && VM_SS_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_SS_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_SQL_All_Instances_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_All_Instances_TabName, 15);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_All_Instances_Columns);

            if (SQL_All_Instances_List != null && SQL_All_Instances_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_All_Instances_List);
        }
        
        private void Generate_All_VM_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.All_VM_IaaS_Server_Rehost_Perf_TabName, 16);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.All_VM_IaaS_Server_Rehost_Perf_Columns);

            if (All_VM_IaaS_Server_Rehost_Perf_List != null && All_VM_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(All_VM_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_Decommissioned_Machines_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.Decommissioned_Machines_TabName, 17);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.Decommissioned_Machines_Columns);

            if (Decommissioned_Machines_List != null & Decommissioned_Machines_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(Decommissioned_Machines_List);
        }
    }
}