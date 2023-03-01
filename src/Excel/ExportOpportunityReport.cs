using ClosedXML.Excel;
using System.Collections.Generic;

using Azure.Migrate.Export.Models;
using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Excel
{
    public class ExportOpportunityReport
    {
        private readonly List<AVS_Summary> AVS_Summary_List;
        private readonly List<AVS_IaaS_Rehost_Perf> AVS_IaaS_Rehost_Perf_List;
        private readonly List<SQL_MI_Issues_and_Warnings> SQL_MI_Issues_and_Warnings_List;
        private readonly List<SQL_MI_Opportunity> SQL_MI_Opportunity_List;
        private readonly List<WebApp_Opportunity> WebApp_Opportunity_List;
        private readonly List<VM_Opportunity_Perf> VM_Opportunity_Perf_List;
        private readonly List<VM_Opportunity_AsOnPrem> VM_Opportunity_AsOnPrem_List;

        XLWorkbook OpportunityWb;

        public ExportOpportunityReport
            (
                List<AVS_Summary> avs_Summary_List,
                List<AVS_IaaS_Rehost_Perf> avs_IaaS_Rehost_Perf_List,
                List<SQL_MI_Issues_and_Warnings> sql_MI_Issues_and_Warnings_List,
                List<SQL_MI_Opportunity> sql_MI_Opportunity_List,
                List<WebApp_Opportunity> webApp_Opportunity_List,
                List<VM_Opportunity_Perf> vm_Opportunity_Perf_List,
                List<VM_Opportunity_AsOnPrem> vm_Opportunity_AsOnPrem_List
            )
        {
            AVS_Summary_List = avs_Summary_List;
            AVS_IaaS_Rehost_Perf_List = avs_IaaS_Rehost_Perf_List;
            SQL_MI_Issues_and_Warnings_List = sql_MI_Issues_and_Warnings_List;
            SQL_MI_Opportunity_List = sql_MI_Opportunity_List;
            WebApp_Opportunity_List = webApp_Opportunity_List;
            VM_Opportunity_Perf_List = vm_Opportunity_Perf_List;
            VM_Opportunity_AsOnPrem_List = vm_Opportunity_AsOnPrem_List;

            OpportunityWb = new XLWorkbook();
        }

        public void GenerateOpportunityReportExcel()
        {
            Generate_SQL_MI_Opportunity_Worksheet();
            Generate_SQL_MI_Issues_and_Warnings_Worksheet();
            Generate_WebApp_Opportunity_Worksheet();
            Generate_VM_Opportunity_Perf_Worksheet();
            Generate_VM_Opportunity_AsOnPrem_Worksheet();
            Generate_AVS_Summary_Worksheet();
            Generate_AVS_IaaS_Server_Rehost_Perf_Worksheet();

            OpportunityWb.SaveAs(OpportunityReportConstants.OpportunityReportPath);
        }

        private void Generate_SQL_MI_Opportunity_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.SQL_MI_Opportunity_TabName, 1);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.SQL_MI_Opportunity_Columns);

            if (SQL_MI_Opportunity_List != null && SQL_MI_Opportunity_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_MI_Opportunity_List);
        }

        private void Generate_SQL_MI_Issues_and_Warnings_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.SQL_MI_Issues_and_Warnings_TabName, 2);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.SQL_MI_Issues_and_Warnings_Columns);

            if (SQL_MI_Issues_and_Warnings_List != null && SQL_MI_Issues_and_Warnings_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(SQL_MI_Issues_and_Warnings_List);
        }

        private void Generate_WebApp_Opportunity_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.WebApp_Opportunity_TabName, 3);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.WebApp_Opportunity_Columns);

            if (WebApp_Opportunity_List != null && WebApp_Opportunity_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(WebApp_Opportunity_List);
        }

        private void Generate_VM_Opportunity_Perf_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.VM_Opportunity_Perf_TabName, 4);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.VM_Opportunity_Perf_Columns);

            if (VM_Opportunity_Perf_List != null && VM_Opportunity_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_Opportunity_Perf_List);
        }

        private void Generate_VM_Opportunity_AsOnPrem_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.VM_Opportunity_AsOnPrem_TabName, 5);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.VM_Opportunity_AsOnPrem_Columns);

            if (VM_Opportunity_Perf_List != null && VM_Opportunity_AsOnPrem_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(VM_Opportunity_AsOnPrem_List);
        }

        private void Generate_AVS_Summary_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.AVS_Summary_TabName, 6);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.AVS_Summary_Columns);

            if (AVS_Summary_List != null && AVS_Summary_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(AVS_Summary_List);
        }

        private void Generate_AVS_IaaS_Server_Rehost_Perf_Worksheet()
        {
            var dataWs = OpportunityWb.Worksheets.Add(OpportunityReportConstants.AVS_IaaS_Rehost_Perf_TabName, 7);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, OpportunityReportConstants.AVS_IaaS_Rehost_Perf_Columns);

            if (AVS_IaaS_Rehost_Perf_List != null && AVS_IaaS_Rehost_Perf_List.Count > 0)
                dataWs.Cell(2, 1).InsertData(AVS_IaaS_Rehost_Perf_List);
        }
    }
}