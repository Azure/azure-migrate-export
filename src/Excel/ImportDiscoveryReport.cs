using ClosedXML.Excel;
using System;
using System.IO;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Excel
{
    public class ImportDiscoveryReport
    {
        private UserInput UserInputObj;
        private List<DiscoveryData> DiscoveredData;
        public ImportDiscoveryReport(UserInput userInputObj, List<DiscoveryData> discoveredData)
        {
            UserInputObj = userInputObj;
            DiscoveredData = discoveredData;
        }

        public void ImportDiscoveryData()
        {
            ValidateDiscoveryReportPresence();

            using (var fileStream = new FileStream(DiscoveryReportConstants.DiscoveryReportPath, FileMode.Open, FileAccess.Read)) // only read the data
            {
                using (var discoveryWb = new XLWorkbook(fileStream))
                {
                    ValidateDiscoveryReport(discoveryWb);

                    if (DiscoveredData == null)
                        DiscoveredData = new List<DiscoveryData>();

                    LoadExcelData(discoveryWb);
                }
            }
        }

        private void ValidateDiscoveryReportPresence()
        {
            if (!Directory.Exists(DiscoveryReportConstants.DiscoveryReportDirectory))
                throw new Exception($"Discovery report directory {DiscoveryReportConstants.DiscoveryReportDirectory} not found.");
            if (!File.Exists(DiscoveryReportConstants.DiscoveryReportPath))
                throw new Exception($"Discovery report file {DiscoveryReportConstants.DiscoveryReportPath} not found");

            UserInputObj.LoggerObj.LogInformation("Validated the presence of Discovery Report");
        }

        private void ValidateDiscoveryReport(XLWorkbook discoveryWb)
        {
            var discoveryDataSheet = discoveryWb.Worksheet(2);

            var headerRow = discoveryDataSheet.Row(1);

            UserInputObj.LoggerObj.LogInformation("Validating columns of discovery report");

            var columns = DiscoveryReportConstants.DiscoveryReportColumns;

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                if (string.IsNullOrEmpty(column))
                    throw new Exception("Encountered empty column name from app settings");

                var cell = headerRow.Cell(i + 1);

                string sheetColumn = cell.GetValue<string>();

                if (string.IsNullOrEmpty(sheetColumn))
                    throw new Exception($"Expected column {column}, but received empty column name");

                if (!column.Equals(sheetColumn))
                    throw new Exception($"Expected column {column}, but encountered column {sheetColumn}");
            }

            UserInputObj.LoggerObj.LogInformation("Validated columns in discovery report excel");
        }

        private void LoadExcelData(XLWorkbook discoveryWb)
        {
            UserInputObj.LoggerObj.LogInformation("Loading data from discovery report");

            var discoveryDataSheet = discoveryWb.Worksheet(2);

            int i = 2;
            var row = discoveryDataSheet.Row(i);

            while (!row.IsEmpty())
            {
                i += 1;
                DiscoveryData obj = new DiscoveryData();

                obj.MachineName = row.Cell(1).GetValue<string>();
                obj.EnvironmentType = row.Cell(2).GetValue<string>();
                obj.SoftwareInventory = row.Cell(3).GetValue<int>();
                obj.SqlDiscoveryServerCount = row.Cell(4).GetValue<int>();
                obj.IsSqlServicePresent = row.Cell(5).GetValue<bool>();
                obj.WebAppCount = row.Cell(6).GetValue<int>();
                obj.OperatingSystem = row.Cell(7).GetValue<string>();
                obj.Cores = row.Cell(8).GetValue<int>();
                obj.MemoryInMB = row.Cell(9).GetValue<double>();
                obj.TotalDisks = row.Cell(10).GetValue<int>();
                obj.IpAddress = row.Cell(11).GetValue<string>();
                obj.MacAddress = row.Cell(12).GetValue<string>();
                obj.TotalNetworkAdapters = row.Cell(13).GetValue<int>();
                obj.BootType = row.Cell(14).GetValue<string>();
                obj.PowerStatus = row.Cell(15).GetValue<string>();
                obj.SupportStatus = row.Cell(16).GetValue<string>();
                obj.FirstDiscoveryTime = row.Cell(17).GetValue<string>();
                obj.LastUpdatedTime = row.Cell(18).GetValue<string>();
                obj.MachineId = row.Cell(19).GetValue<string>();

                DiscoveredData.Add(obj);

                row = discoveryDataSheet.Row(i);
            }

            UserInputObj.LoggerObj.LogInformation($"Updated discovery data model with {DiscoveredData.Count} machines");
        }
    }
}