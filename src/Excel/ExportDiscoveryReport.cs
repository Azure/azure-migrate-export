using ClosedXML.Excel;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Models;

namespace  Azure.Migrate.Export.Excel
{
    public class ExportDiscoveryReport
    {
        private readonly List<DiscoveryData> DiscoveredData;
        private readonly DiscoveryProperties DiscoveryPropertiesData;
        XLWorkbook DiscoveryWb;

        public ExportDiscoveryReport(List<DiscoveryData> discoveredData, DiscoveryProperties discoveryPropertiesData)
        {
            DiscoveredData = discoveredData;
            DiscoveryPropertiesData = discoveryPropertiesData;
            DiscoveryWb = new XLWorkbook();
        }

        public void GenerateDiscoveryReportExcel()
        {
            GeneratePropertyWorksheet();
            GenerateDiscoveryReportWorksheet();

            DiscoveryWb.SaveAs(DiscoveryReportConstants.DiscoveryReportPath);
        }

        private void GeneratePropertyWorksheet()
        {
            var propertiesWs = DiscoveryWb.Worksheets.Add(DiscoveryReportConstants.PropertiesTabName, 1);

            var propertyHeaders = DiscoveryReportConstants.PropertiesList;

            for (int i = 0; i < propertyHeaders.Count; i++)
                propertiesWs.Cell(1, i + 1).Value = propertyHeaders[i];

            // Add values: important to add in the same order as above

            propertiesWs.Cell(2, 1).Value = DiscoveryPropertiesData.TenantId;
            propertiesWs.Cell(2, 2).Value = DiscoveryPropertiesData.Subscription;
            propertiesWs.Cell(2, 3).Value = DiscoveryPropertiesData.ResourceGroup;
            propertiesWs.Cell(2, 4).Value = DiscoveryPropertiesData.AzureMigrateProjectName;
            propertiesWs.Cell(2, 5).Value = DiscoveryPropertiesData.DiscoverySiteName;
            propertiesWs.Cell(2, 6).Value = DiscoveryPropertiesData.Workflow;
            propertiesWs.Cell(2, 7).Value = DiscoveryPropertiesData.SourceAppliances;
        }

        private void GenerateDiscoveryReportWorksheet()
        {
            var dataWs = DiscoveryWb.Worksheets.Add(DiscoveryReportConstants.Discovery_Report_TabName, 2);

            UtilityFunctions.AddColumnHeadersToWorksheet(dataWs, DiscoveryReportConstants.DiscoveryReportColumns);

            if (DiscoveredData != null && DiscoveredData.Count > 0)
                dataWs.Cell(2, 1).InsertData(DiscoveredData);
        }
    }
}