using ClosedXML.Excel;
using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Forex;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Common
{
    public static class UtilityFunctions
    {
        public static T FindFirstExceptionOfType<T>(Exception e)
            where T : Exception
        {
            if (e == null)
            {
                return null;
            }

            Stack<Exception> stack = new Stack<Exception>();
            stack.Push(e);

            while (stack.Count != 0)
            {
                var ex = stack.Pop();
                T retval = ex as T;

                if (retval != null)
                {
                    return retval;
                }

                if (ex.InnerException != null)
                {
                    stack.Push(ex.InnerException);
                }
            }

            return null;
        }

        public static Exception FindFirstExceptionOfType(Exception ex)
        {
            Exception result = ex;
            while (result.InnerException != null)
            {
                result = result.InnerException;
            }

            return result;
        }

        public static void InitiateCancellation(UserInput userInputObj)
        {
            userInputObj.LoggerObj.LogInformation("Initiating process termination upon user request");
            userInputObj.CancellationContext.Token.ThrowIfCancellationRequested();
        }

        public static string PrependErrorLogType()
        {
            return LoggerConstants.ErrorLogTypePrefix + LoggerConstants.LogTypeMessageSeparator;
        }

        public static bool IsAssessmentCompleted(KeyValuePair<AssessmentInformation, AssessmentPollResponse> assessmentInfo)
        {
            return (assessmentInfo.Value == AssessmentPollResponse.Completed ||
                    assessmentInfo.Value == AssessmentPollResponse.OutDated);
        }

        public static double GetAzureBackupMonthlyCostEstimate(List<AssessedDisk> disks)
        {
            double exchangeRate = ForexData.GetExchangeRate();
            double totalDiskStorage = 0;
            foreach (var disk in disks)
                totalDiskStorage += disk.GigabytesProvisioned;
            
            double storageCost = totalDiskStorage * ((3.38 * 0.0224) * exchangeRate);
            double backupCost = exchangeRate;

            if (totalDiskStorage <= 50)
                backupCost *= 5;
            else if (totalDiskStorage > 50 && totalDiskStorage <= 500)
                backupCost *= 10;
            else if (totalDiskStorage > 500)
                backupCost *= Math.Ceiling(totalDiskStorage / 500) * 10;

            return backupCost + storageCost;
        }

        public static double GetAzureSiteRecoveryMonthlyCostEstimate()
        {
            double exchangeRate = ForexData.GetExchangeRate();
            return 25.0 * exchangeRate;
        }

        public static string GetConfidenceRatingInStars(double confidenceRatingInPercentage)
        {
            string result = "";

            if (confidenceRatingInPercentage <= 20)
                result = "1 Star";
            if (confidenceRatingInPercentage > 20 && confidenceRatingInPercentage <= 40)
                result = "2 Stars";
            if (confidenceRatingInPercentage > 40 && confidenceRatingInPercentage <= 60)
                result = "3 Stars";
            if (confidenceRatingInPercentage > 60 && confidenceRatingInPercentage <= 80)
                result = "4 Stars";
            if (confidenceRatingInPercentage > 80)
                result = "5 Stars";

            return result;
        }

        public static string GetStringValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return value;
        }

        public static double GetTotalStorage(List<AssessedDisk> disks)
        {
            double total = 0;

            foreach (var disk in disks)
                total += disk.GigabytesProvisioned;

            return total;
        }

        public static KeyValuePair<string, string> ParseMacIpAddress(List<AssessedNetworkAdapter> networkAdapters)
        {
            string ipAddresses = "";
            string macAddresses = "";

            foreach (var networkAdapter in networkAdapters)
            {
                macAddresses = macAddresses + "[" +networkAdapter.MacAddress + "];";
                ipAddresses = ipAddresses + "[" + string.Join(",", networkAdapter.IpAddresses) + "];";
            }

            return new KeyValuePair<string, string>(macAddresses, ipAddresses);
        }

        public static string GetDiskNames(List<AssessedDisk> disks)
        {
            string diskNames = "";

            foreach (var disk in disks)
                diskNames = diskNames + disk.DisplayName + ";";

            return diskNames;
        }

        public static string GetDiskReadiness(List<AssessedDisk> disks)
        {
            string diskReadiness = "";

            foreach (var disk in disks)
                diskReadiness = diskReadiness + new EnumDescriptionHelper().GetEnumDescription(disk.Suitability) + ";";

            return diskReadiness;
        }

        public static string GetRecommendedDiskSKUs(List<AssessedDisk> disks)
        {
            string skus = "";

            foreach (var disk in disks)
                skus = skus + disk.RecommendedDiskSku + ";";

            return skus;
        }

        public static int GetDiskTypeCount(List<AssessedDisk> disks, RecommendedDiskTypes type)
        {
            int count = 0;

            foreach (var disk in disks)
            {
                if (disk.DiskType == type)
                    count += 1;
            }

            return count;
        }

        public static double GetDiskTypeStorageCost(List<AssessedDisk> disks, RecommendedDiskTypes type)
        {
            double cost = 0;

            foreach (var disk in disks)
                if (disk.DiskType == type)
                    cost += disk.DiskCost;

            return cost;
        }

        public static double GetDiskReadInOPS(List<AssessedDisk> disks)
        {
            double value = 0;
            foreach (var disk in disks)
                value += disk.NumberOfReadOperationsPerSecond;
            
            return value;
        }

        public static double GetDiskWriteInOPS(List<AssessedDisk> disks)
        {
            double value = 0;
            foreach (var disk in disks)
                value += disk.NumberOfWriteOperationsPerSecond;
            
            return value;
        }

        public static double GetDiskReadInMBPS(List<AssessedDisk> disks)
        {
            double value = 0;
            foreach (var disk in disks)
                value += disk.MegabytesPerSecondOfRead;
            
            return value;
        }

        public static double GetDiskWriteInMBPS(List<AssessedDisk> disks)
        {
            double value = 0;
            foreach (var disk in disks)
                value += disk.MegabytesPerSecondOfWrite;
            
            return value;
        }

        public static double GetNetworkInMBPS(List<AssessedNetworkAdapter> networkAdapters)
        {
            double value = 0;
            foreach (var networkAdapter in networkAdapters)
                value += networkAdapter.MegabytesPerSecondReceived;
            return value;
        }

        public static double GetNetworkOutMBPS(List<AssessedNetworkAdapter> networkAdapters)
        {
            double value = 0;
            foreach (var networkAdapter in networkAdapters)
                value += networkAdapter.MegaytesPerSecondTransmitted;
            
            return value;
        }

        public static string GetMigrationIssueWarnings(List<AssessedMigrationIssue> migrationIssues)
        {
            string value = "";
            foreach (var migrationIssue in migrationIssues)
                if (migrationIssue.IssueCategory == IssueCategories.Warning)
                    value = value + migrationIssue.IssueId + ";";
            
            return value;
        }

        public static string GetMigrationIssueByType(List<AssessedMigrationIssue> migrationIssues, IssueCategories category)
        {
            string value = "";
            foreach (var migrationIssue in migrationIssues)
                if (migrationIssue.IssueCategory == category)
                    value = value + migrationIssue.IssueId + ";";
            
            return value;
        }

        public static string GetSQLMIConfiguration(AzureSQLInstanceDataset azureSqlInstance)
        {
            string value = "";

            string serviceTier = GetStringValue(azureSqlInstance.AzureSQLMISkuServiceTier);
            string computeTier = GetStringValue(azureSqlInstance.AzureSQLMISkuComputeTier);
            string hardwareGeneration = GetStringValue(azureSqlInstance.AzureSQLMISkuHardwareGeneration);
            int cores = azureSqlInstance.AzureSQLMISkuCores;
            double storageMaxSizeInGB = Math.Round(azureSqlInstance.AzureSQLMISkuStorageMaxSizeInMB / 1024.0);
            
            value = serviceTier + "," +
                    computeTier + "," +
                    hardwareGeneration + "," +
                    cores.ToString() + "vCore," +
                    storageMaxSizeInGB + " GB Storage";

            return value;
        }

        public static void AddColumnHeadersToWorksheet(IXLWorksheet sheet, List<string> columns)
        {
            for (int i = 0; i < columns.Count; i++)
                sheet.Cell(1, i + 1).Value = columns[i];
        }
    }
}