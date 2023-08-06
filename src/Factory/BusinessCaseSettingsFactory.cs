using Newtonsoft.Json;
using System;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Factory
{
    public class BusinessCaseSettingsFactory
    {
        public BusinessCaseInformation GetBusinessCaseSettings(UserInput userInputObj, string sessionId)
        {
            if (userInputObj == null)
                throw new Exception("Received invalid null user input.");

            if (string.IsNullOrEmpty(sessionId))
                throw new Exception("Received invalid session ID.");

            userInputObj.LoggerObj.LogInformation($"Obtaining Business case settings");

            BusinessCaseSettingsJSON obj = new BusinessCaseSettingsJSON();
            obj.Name = "bizcase-ame-" + sessionId;
            obj.Location = userInputObj.TargetRegion.Key;
            obj.Properties.Settings.AzureSettings.Currency = "USD"; // userInputObj.Currency.Key;

            BusinessCaseTypes type = BusinessCaseTypes.OptimizeForPaaS;
            if (userInputObj.PreferredOptimizationObj.OptimizationPreference.Key.Equals("MigrateToAllIaaS"))
                type = BusinessCaseTypes.IaaSOnly;

            obj.Properties.Settings.AzureSettings.BusinessCaseType = type.ToString();

            return new BusinessCaseInformation(obj.Name, JsonConvert.SerializeObject(obj));
        } 
    }
}