using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Assessment.Parser
{
    public class AzureSQLRecommendedAssessedEntitiesParser
    {
        private readonly Dictionary<AssessmentInformation, AssessmentPollResponse> AzureSQLAssessmentStatusMap;
        private readonly Dictionary<string, string> SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup;
        private readonly Dictionary<string, string> SQLInstanceAssessmentArmIdToSdsArmIdLookup;
        public AzureSQLRecommendedAssessedEntitiesParser(Dictionary<AssessmentInformation, AssessmentPollResponse> azureSQLAssessmentStatusMap, Dictionary<string, string> sqlMachineAssessementArmIdToDatacenterMachineArmIdLookup, Dictionary<string, string> sqlInstanceAssessmentArmIdToSdsArmIdLookup)
        {
            AzureSQLAssessmentStatusMap = azureSQLAssessmentStatusMap;
            SQLInstanceAssessmentArmIdToSdsArmIdLookup = sqlInstanceAssessmentArmIdToSdsArmIdLookup;
            SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup = sqlMachineAssessementArmIdToDatacenterMachineArmIdLookup;
        }

        public void ParseAssessedSQLRecommendedEntities(HashSet<string> AzureSqlMiRecommendations, HashSet<string> AzureSqlInstanceRehostRecommendations, HashSet<string> AzureSqlServerRehostRecommendations, UserInput userInputObj)
        {
            if (userInputObj == null)
                throw new Exception("Received null user input object.");

            if (AzureSQLAssessmentStatusMap == null)
            {
                userInputObj.LoggerObj.LogError("Azure SQL assessment status map is null, terminating parsing");
                return;
            }

            if (AzureSQLAssessmentStatusMap.Count <= 0)
            {
                userInputObj.LoggerObj.LogError("Azure SQL Assessment status map is empty, terminating parsing");
                return;
            }

            if (SQLInstanceAssessmentArmIdToSdsArmIdLookup == null || SQLInstanceAssessmentArmIdToSdsArmIdLookup.Count <= 0)
            {
                userInputObj.LoggerObj.LogError("Azure SQL Instance assessment ID to SDS ARM ID lookup is empty, terminating parsing");
                return;
            }

            if (SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup == null || SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup.Count <= 0)
            {
                userInputObj.LoggerObj.LogError("Azure SQL machine assessment ID to datacenter machine ARM ID lookup is empty, terminating parsing");
                return;
            }

            bool isDevAssessmentParsed = false;
            bool isProdAssessmentParsed = false;

            foreach (var kvp in AzureSQLAssessmentStatusMap)
            {
                if (!UtilityFunctions.IsAssessmentCompleted(kvp))
                {
                    userInputObj.LoggerObj.LogWarning($"Skipping parsing assessment {kvp.Key.AssessmentName} as it is in {new EnumDescriptionHelper().GetEnumDescription(kvp.Value)} state");
                    continue;
                }

                if (isDevAssessmentParsed && kvp.Key.AssessmentName.Contains("Dev"))
                    continue;
                if (isProdAssessmentParsed && kvp.Key.AssessmentName.Contains("Prod"))
                    continue;

                userInputObj.LoggerObj.LogInformation($"Parsing Azure SQL assessment {kvp.Key.AssessmentName} for recommended entities data");

                string url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                             Routes.SubscriptionPath + Routes.ForwardSlash + userInputObj.Subscription.Key + Routes.ForwardSlash +
                             Routes.ResourceGroupPath + Routes.ForwardSlash + userInputObj.ResourceGroupName.Value + Routes.ForwardSlash +
                             Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                             Routes.AssessmentProjectsPath + Routes.ForwardSlash + userInputObj.AssessmentProjectName + Routes.ForwardSlash +
                             Routes.GroupsPath + Routes.ForwardSlash + kvp.Key.GroupName + Routes.ForwardSlash +
                             new EnumDescriptionHelper().GetEnumDescription(kvp.Key.AssessmentType) + Routes.ForwardSlash + kvp.Key.AssessmentName + Routes.ForwardSlash +
                             Routes.SQLRecommendedAssessedEntitiesPath +
                             Routes.QueryStringQuestionMark +
                             Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.AssessmentMachineListApiVersion;
                
                while (!string.IsNullOrEmpty(url))
                {
                    string recommendedAssessedEntitiesResponse = "";
                    try
                    {
                        recommendedAssessedEntitiesResponse = new HttpClientHelper().GetHttpRequestJsonStringResponse(url, userInputObj).Result;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (AggregateException aeRecommendedSQLEntities)
                    {
                        string errorMessage = "";
                        foreach (var e in aeRecommendedSQLEntities.Flatten().InnerExceptions)
                        {
                            if (e is OperationCanceledException)
                                throw e;
                            else
                            {
                                errorMessage = errorMessage + e.Message + " ";
                            }
                        }
                        userInputObj.LoggerObj.LogError($"Failed to retrieve recommended sql entities from assessment: {errorMessage}");
                        url = "";
                        continue;
                    }
                    catch (Exception exRecommendedSQLEntities)
                    {
                        userInputObj.LoggerObj.LogError($"Failed to retrieve recommended sql entities from assessment: {exRecommendedSQLEntities.Message}");
                        url = "";
                        continue;
                    }

                    AzureSQLRecommendedAssessedEntitiesJSON obj = JsonConvert.DeserializeObject<AzureSQLRecommendedAssessedEntitiesJSON>(recommendedAssessedEntitiesResponse);
                    url = obj.NextLink;

                    foreach (var value in obj.Values)
                    {
                        string key = value.Properties.AssessedSqlEntityArmId?.ToLower();

                        if (value.Properties.RecommendedAzureSqlTargetType == AzureSQLTargetType.AzureSqlManagedInstance)
                            if (SQLInstanceAssessmentArmIdToSdsArmIdLookup.ContainsKey(key))
                                AzureSqlMiRecommendations.Add(SQLInstanceAssessmentArmIdToSdsArmIdLookup[key]);

                        if (value.Properties.RecommendedAzureSqlTargetType == AzureSQLTargetType.AzureSqlVirtualMachine)
                            if (SQLInstanceAssessmentArmIdToSdsArmIdLookup.ContainsKey(key))
                                AzureSqlInstanceRehostRecommendations.Add(SQLInstanceAssessmentArmIdToSdsArmIdLookup[key]);

                        if (value.Properties.RecommendedAzureSqlTargetType == AzureSQLTargetType.AzureVirtualMachine)
                            if (SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup.ContainsKey(key))
                                AzureSqlServerRehostRecommendations.Add(SQLMachineAssessementArmIdToDatacenterMachineArmIdLookup[key]);
                    }
                }

                if (url == "")
                {
                    AzureSqlMiRecommendations = new HashSet<string>();
                    AzureSqlInstanceRehostRecommendations = new HashSet<string>();
                    AzureSqlServerRehostRecommendations = new HashSet<string>();
                }
                else
                {
                    if (kvp.Key.AssessmentName.Contains("Prod"))
                        isProdAssessmentParsed = true;
                    else if (kvp.Key.AssessmentName.Contains("Dev"))
                        isDevAssessmentParsed = true;
                }
            }
        }
    }
}
