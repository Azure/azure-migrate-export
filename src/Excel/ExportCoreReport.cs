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
                List<VM_IaaS_Server_Rehost_AsOnPrem> vm_IaaS_Server_Rehost_AsOnPrem_List
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

            CoreWb = new XLWorkbook();
        }

        public void GenerateCoreReportExcel()
        {
            Generate_Properties_Worksheet();
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

        private void Generate_SQL_MI_PaaS_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_MI_PaaS_TabName, 2);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_MI_PaaS_Columns);

            if (SQL_MI_PaaS_List != null && SQL_MI_PaaS_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_MI_PaaS_List);
        }

        private void Generate_SQL_IaaS_Instance_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Instance_Rehost_Perf_TabName, 3);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Instance_Rehost_Perf_Columns);

            if (SQL_IaaS_Instance_Rehost_Perf_List != null && SQL_IaaS_Instance_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Instance_Rehost_Perf_List);
        }

        private void Generate_SQL_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Server_Rehost_Perf_TabName, 4);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Server_Rehost_Perf_Columns);

            if (SQL_IaaS_Server_Rehost_Perf_List != null && SQL_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_SQL_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_IaaS_Server_Rehost_AsOnPrem_TabName, 5);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (SQL_IaaS_Server_Rehost_AsOnPrem_List != null && SQL_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_WebApp_PaaS_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_PaaS_TabName, 6);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_PaaS_Columns);

            if (WebApp_PaaS_List != null && WebApp_PaaS_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_PaaS_List);
        }

        private void Generate_WebApp_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_IaaS_Server_Rehost_Perf_TabName, 7);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_IaaS_Server_Rehost_Perf_Columns);

            if (WebApp_IaaS_Server_Rehost_Perf_List != null && WebApp_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_WebApp_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.WebApp_IaaS_Server_Rehost_AsOnPrem_TabName, 8);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.WebApp_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (WebApp_IaaS_Server_Rehost_AsOnPrem_List != null && WebApp_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_VM_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_IaaS_Server_Rehost_Perf_TabName, 9);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_IaaS_Server_Rehost_Perf_Columns);

            if (VM_IaaS_Server_Rehost_Perf_List != null && VM_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_VM_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_IaaS_Server_Rehost_AsOnPrem_TabName, 10);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (VM_IaaS_Server_Rehost_AsOnPrem_List != null && VM_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_VM_SS_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_SS_IaaS_Server_Rehost_Perf_TabName, 11);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_SS_IaaS_Server_Rehost_Perf_Columns);

            if (VM_SS_IaaS_Server_Rehost_Perf_List != null && VM_SS_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_SS_IaaS_Server_Rehost_Perf_List);
        }

        private void Generate_VM_SS_IaaS_Server_Rehost_AsOnPrem_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.VM_SS_IaaS_Server_Rehost_AsOnPrem_TabName, 12);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.VM_SS_IaaS_Server_Rehost_AsOnPrem_Columns);

            if (VM_SS_IaaS_Server_Rehost_AsOnPrem_List != null && VM_SS_IaaS_Server_Rehost_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_SS_IaaS_Server_Rehost_AsOnPrem_List);
        }

        private void Generate_SQL_All_Instances_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.SQL_All_Instances_TabName, 13);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.SQL_All_Instances_Columns);

            if (SQL_All_Instances_List != null && SQL_All_Instances_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_All_Instances_List);
        }
        
        private void Generate_All_VM_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = CoreWb.Worksheets.Add(CoreReportConstants.All_VM_IaaS_Server_Rehost_Perf_TabName, 14);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, CoreReportConstants.All_VM_IaaS_Server_Rehost_Perf_Columns);

            if (All_VM_IaaS_Server_Rehost_Perf_List != null && All_VM_IaaS_Server_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(All_VM_IaaS_Server_Rehost_Perf_List);
        }
    }
}