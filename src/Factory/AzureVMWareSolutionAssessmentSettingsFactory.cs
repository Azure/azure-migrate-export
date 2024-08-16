using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Factory
{
    public class AzureVMWareSolutionAssessmentSettingsFactory
    {
        public List<AssessmentInformation> GetAzureVMWareSolutionAssessmentSettings(UserInput userInputObj, string groupName)
        {
            List<AssessmentInformation> result = new List<AssessmentInformation>();

            if (userInputObj == null)
                throw new Exception("Received invalid null user input.");

            if (string.IsNullOrEmpty(groupName))
                throw new Exception("Received invalid group name.");

            userInputObj.LoggerObj.LogInformation($"Obtaining Azure VMWare solution assessment settings for group {groupName}");

            if (!groupName.Contains("Azure-VMWareSolution"))
                throw new Exception($"Invalid group name {groupName} for Azure VMWare solution assessment settings factory.");

            // AVS only has Prod pricing
            result = GetAzureVMWareSolutionProdAssessmentSettings(userInputObj, groupName);

            if (result.Count <= 0)
                throw new Exception($"Azure VMWare solution assessment factory provided no settings for group {groupName}.");

            return result;
        }

        private List<AssessmentInformation> GetAzureVMWareSolutionProdAssessmentSettings(UserInput userInputObj, string groupName)
        {
            List<AssessmentInformation> result = new List<AssessmentInformation>();
            List<string> nodeTypes = new List<string>();

            if (AvsAssessmentConstants.RegionToAvsNodeTypeMap.ContainsKey(userInputObj.TargetRegion.Key))
                nodeTypes = AvsAssessmentConstants.RegionToAvsNodeTypeMap[userInputObj.TargetRegion.Key];
            else
                throw new Exception($"Import based AVS assessment cannot be created for region: {userInputObj.TargetRegion.Value}");

            // Performance based - Pay as you go
            AzureVMWareSolutionAssessmentSettingsJSON obj1 = new AzureVMWareSolutionAssessmentSettingsJSON();
            obj1.Properties.ReservedInstance = "None";
            obj1.Properties.TimeRange = userInputObj.AssessmentDuration.Key;
            obj1.Properties.Currency = userInputObj.Currency.Key;
            obj1.Properties.AzureLocation = userInputObj.TargetRegion.Key;
            obj1.Properties.NodeTypes = nodeTypes;
            result.Add(new AssessmentInformation(groupName, "AVS-Prod-AzMigExport-1", AssessmentType.AVSAssessment, AssessmentTag.PerformanceBased, JsonConvert.SerializeObject(obj1)));

            // Performance based - Pay as you go + RI 1 year
            AzureVMWareSolutionAssessmentSettingsJSON obj2 = new AzureVMWareSolutionAssessmentSettingsJSON();
            obj2.Properties.ReservedInstance = "RI1Year";
            obj2.Properties.TimeRange = userInputObj.AssessmentDuration.Key;
            obj2.Properties.Currency = userInputObj.Currency.Key;
            obj2.Properties.AzureLocation = userInputObj.TargetRegion.Key;
            obj2.Properties.NodeTypes = nodeTypes;
            result.Add(new AssessmentInformation(groupName, "AVS-Prod-AzMigExport-2", AssessmentType.AVSAssessment, AssessmentTag.PerformanceBased_RI1year, JsonConvert.SerializeObject(obj2)));

            // Performance based - Pay as you go + RI 3 year
            AzureVMWareSolutionAssessmentSettingsJSON obj3 = new AzureVMWareSolutionAssessmentSettingsJSON();
            obj3.Properties.ReservedInstance = "RI3Year";
            obj3.Properties.TimeRange = userInputObj.AssessmentDuration.Key;
            obj3.Properties.Currency = userInputObj.Currency.Key;
            obj3.Properties.AzureLocation = userInputObj.TargetRegion.Key;
            obj3.Properties.NodeTypes = nodeTypes;
            result.Add(new AssessmentInformation(groupName, "AVS-Prod-AzMigExport-3", AssessmentType.AVSAssessment, AssessmentTag.PerformanceBased_RI3year, JsonConvert.SerializeObject(obj3)));


            // As- Onpremises - Pay as you go + RI 3 Year
            AzureVMWareSolutionAssessmentSettingsJSON obj4 = new AzureVMWareSolutionAssessmentSettingsJSON();
            obj4.Properties.SizingCriterion = "AsOnPremises";
            obj4.Properties.ReservedInstance = "RI3Year";
            obj4.Properties.Currency = userInputObj.Currency.Key; 
            obj4.Properties.AzureLocation = userInputObj.TargetRegion.Key;
            obj4.Properties.TimeRange = userInputObj.AssessmentDuration.Key;
            obj4.Properties.NodeTypes = nodeTypes;
            result.Add(new AssessmentInformation(groupName, "AVS-Prod-AzMigExport-4", AssessmentType.AVSAssessment, AssessmentTag.AsOnPremises, JsonConvert.SerializeObject(obj4)));

            return result;
        }
    }
}