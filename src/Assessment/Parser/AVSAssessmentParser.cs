using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Assessment.Parser
{
    public class AVSAssessmentParser
    {
        private readonly Dictionary<AssessmentInformation, AssessmentPollResponse> AVSAssessmentStatusMap;
        public AVSAssessmentParser(Dictionary<AssessmentInformation, AssessmentPollResponse> avsAssessmentStatusMap)
        {
            AVSAssessmentStatusMap = avsAssessmentStatusMap;
        }

        public void ParseAVSAssessment(Dictionary<AssessmentInformation, AVSAssessmentPropertiesDataset> AVSAssessmentsData, Dictionary<string, AVSAssessedMachinesDataset> AVSAssessedMachinesData, UserInput userInputObj)
        {
            if (userInputObj == null)
                throw new Exception("Received null user input object.");

            if (AVSAssessmentStatusMap == null)
            {
                userInputObj.LoggerObj.LogError($"AVS assessment status map is null, terminating parsing");
                return;
            }

            if (AVSAssessmentStatusMap.Count <= 0)
            {
                userInputObj.LoggerObj.LogError($"AVS assessment status map is empty, terminating parsing");
                return;
            }

            foreach (var kvp in AVSAssessmentStatusMap)
            {
                if (!UtilityFunctions.IsAssessmentCompleted(kvp))
                {
                    userInputObj.LoggerObj.LogWarning($"Skipping parsing assessment {kvp.Key.AssessmentName} as it is in {new EnumDescriptionHelper().GetEnumDescription(kvp.Value)} state");
                    continue;
                }

                userInputObj.LoggerObj.LogInformation($"Parsing AVS assessment {kvp.Key.AssessmentName}");

                string url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                             Routes.SubscriptionPath + Routes.ForwardSlash + userInputObj.Subscription.Key + Routes.ForwardSlash +
                             Routes.ResourceGroupPath + Routes.ForwardSlash + userInputObj.ResourceGroupName.Value + Routes.ForwardSlash +
                             Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                             Routes.AssessmentProjectsPath + Routes.ForwardSlash + userInputObj.AssessmentProjectName + Routes.ForwardSlash +
                             Routes.GroupsPath + Routes.ForwardSlash + kvp.Key.GroupName + Routes.ForwardSlash +
                             new EnumDescriptionHelper().GetEnumDescription(kvp.Key.AssessmentType) + Routes.ForwardSlash + kvp.Key.AssessmentName +
                             Routes.QueryStringQuestionMark +
                             Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.AssessmentMachineListApiVersion;

                string assessmentPropertiesResponse = "";
                try
                {
                    assessmentPropertiesResponse = new HttpClientHelper().GetHttpRequestJsonStringResponse(url, userInputObj).Result;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (AggregateException aeAVSProperties)
                {
                    string errorMessage = "";
                    foreach (var e in aeAVSProperties.Flatten().InnerExceptions)
                    {
                        if (e is OperationCanceledException)
                            throw e;
                        else
                        {
                            errorMessage = errorMessage + e.Message + " ";
                        }
                    }
                    userInputObj.LoggerObj.LogError($"Failed to retrieve AVS assessment properties for assessment: {errorMessage}");
                    url = "";
                    continue;
                }
                catch (Exception exAVSProperties)
                {
                    userInputObj.LoggerObj.LogError($"Failed to retrieve AVS assessment properties for assessment: {exAVSProperties.Message}");
                    url = "";
                    continue;
                }

                AVSAssessmentPropertiesJSON avsPropertiesObj = JsonConvert.DeserializeObject<AVSAssessmentPropertiesJSON>(assessmentPropertiesResponse);
                UpdateAVSPropertiesDataset(avsPropertiesObj, AVSAssessmentsData, kvp.Key, userInputObj);

                url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                      Routes.SubscriptionPath + Routes.ForwardSlash + userInputObj.Subscription.Key + Routes.ForwardSlash +
                      Routes.ResourceGroupPath + Routes.ForwardSlash + userInputObj.ResourceGroupName.Value + Routes.ForwardSlash +
                      Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                      Routes.AssessmentProjectsPath + Routes.ForwardSlash + userInputObj.AssessmentProjectName + Routes.ForwardSlash +
                      Routes.GroupsPath + Routes.ForwardSlash + kvp.Key.GroupName + Routes.ForwardSlash +
                      new EnumDescriptionHelper().GetEnumDescription(kvp.Key.AssessmentType) + Routes.ForwardSlash + kvp.Key.AssessmentName + Routes.ForwardSlash +
                      Routes.AVSAssessedMachinesPath +
                      Routes.QueryStringQuestionMark +
                      Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.AssessmentMachineListApiVersion;

                while (!string.IsNullOrEmpty(url))
                {
                    if (userInputObj.CancellationContext.IsCancellationRequested)
                        UtilityFunctions.InitiateCancellation(userInputObj);

                    string assessedMachinesResponse = "";
                    try
                    {
                        assessedMachinesResponse = new HttpClientHelper().GetHttpRequestJsonStringResponse(url, userInputObj).Result;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (AggregateException aeAssessedMachines)
                    {
                        string errorMessage = "";
                        foreach (var e in aeAssessedMachines.Flatten().InnerExceptions)
                        {
                            if (e is OperationCanceledException)
                                throw e;
                            else
                            {
                                errorMessage = errorMessage + e.Message + " ";
                            }
                        }
                        userInputObj.LoggerObj.LogError($"Failed to retrieve machine data from assessment: {errorMessage}");
                        url = "";
                        continue;
                    }
                    catch (Exception exAVSAssessedMachines)
                    {
                        userInputObj.LoggerObj.LogError($"Failed to retrieve machine data from assessment: {exAVSAssessedMachines.Message}");
                        url = "";
                        continue;
                    }

                    AVSAssessedMachinesJSON obj = JsonConvert.DeserializeObject<AVSAssessedMachinesJSON>(assessedMachinesResponse);
                    url = obj.NextLink;

                    foreach (var value in obj.Values)
                    {
                        string key = value.Properties.DatacenterMachineArmId?.ToLower();
                        UpdateAVSAssessedMachinesDataset(AVSAssessedMachinesData, value, key, kvp.Key);
                    }
                }
            }
        }

        private void UpdateAVSAssessedMachinesDataset(Dictionary<string, AVSAssessedMachinesDataset> AVSAssessedMachinesData, AVSAssessedMachineValue value, string key, AssessmentInformation assessmentInfo)
        {
            if (AVSAssessedMachinesData.ContainsKey(key))
                return;

            AVSAssessedMachinesData.Add(key, new AVSAssessedMachinesDataset());

            AVSAssessedMachinesData[key].DisplayName = value.Properties.DisplayName;
            AVSAssessedMachinesData[key].DatacenterMachineArmId = value.Properties.DatacenterMachineArmId?.ToLower();
            AVSAssessedMachinesData[key].Suitability = value.Properties.Suitability;
            AVSAssessedMachinesData[key].SuitabilityExplanation = value.Properties.SuitabilityExplanation;
            AVSAssessedMachinesData[key].OperatingSystemName = value.Properties.OperatingSystemName;
            AVSAssessedMachinesData[key].OperatingSystemVersion = value.Properties.OperatingSystemVersion;
            AVSAssessedMachinesData[key].OperatingSystemArchitecture = value.Properties.OperatingSystemArchitecture;
            AVSAssessedMachinesData[key].BootType = value.Properties.BootType;
            AVSAssessedMachinesData[key].NumberOfCores = value.Properties.NumberOfCores;
            AVSAssessedMachinesData[key].MegabytesOfMemory = value.Properties.MegabytesOfMemory;
            AVSAssessedMachinesData[key].Disks = GetAssessedDiskList(value.Properties.Disks);
            AVSAssessedMachinesData[key].StorageInUseGB = value.Properties.StorageInUseGB;
            AVSAssessedMachinesData[key].NetworkAdapters = value.Properties.NetworkAdapters.Count; // TODO: handle null object reference
            AVSAssessedMachinesData[key].NetworkAdapterList = GetAssessedNetworkAdapterList(value.Properties.NetworkAdapters);
            AVSAssessedMachinesData[key].GroupName = assessmentInfo.GroupName;
        }

        private void UpdateAVSPropertiesDataset(AVSAssessmentPropertiesJSON avsPropertiesObj, Dictionary<AssessmentInformation, AVSAssessmentPropertiesDataset> AVSAssessmentsData, AssessmentInformation assessmentInfo, UserInput userInputObj)
        {
            if (AVSAssessmentsData.ContainsKey(assessmentInfo))
                return;

            AVSAssessmentsData.Add(assessmentInfo, new AVSAssessmentPropertiesDataset());

            AVSAssessmentsData[assessmentInfo].SubscriptionId = userInputObj.Subscription.Key;
            AVSAssessmentsData[assessmentInfo].ResourceGroup = userInputObj.ResourceGroupName.Value;
            AVSAssessmentsData[assessmentInfo].AssessmentProjectName = userInputObj.AssessmentProjectName;
            AVSAssessmentsData[assessmentInfo].GroupName = assessmentInfo.GroupName;
            AVSAssessmentsData[assessmentInfo].AssessmentName = avsPropertiesObj.Name;
            AVSAssessmentsData[assessmentInfo].SizingCriterion = new EnumDescriptionHelper().GetEnumDescription(assessmentInfo.AssessmentTag);
            AVSAssessmentsData[assessmentInfo].CreatedOn = avsPropertiesObj.Properties.CreatedTimestamp;
            AVSAssessmentsData[assessmentInfo].TotalMachinesAssessd = avsPropertiesObj.Properties.NumberOfMachines;
            AVSAssessmentsData[assessmentInfo].MachinesReady = avsPropertiesObj.Properties.SuitabilitySummary.Suitable;
            AVSAssessmentsData[assessmentInfo].MachinesReadyWithConditions = avsPropertiesObj.Properties.SuitabilitySummary.ConditionallySuitable;
            AVSAssessmentsData[assessmentInfo].MachinesReadinessUnknown = avsPropertiesObj.Properties.SuitabilitySummary.ReadinessUnknown;
            AVSAssessmentsData[assessmentInfo].MachinesNotReady = avsPropertiesObj.Properties.SuitabilitySummary.NotSuitable;
            AVSAssessmentsData[assessmentInfo].RecommendedNumberOfNodes = avsPropertiesObj.Properties.NumberOfNodes;
            AVSAssessmentsData[assessmentInfo].NodeType = avsPropertiesObj.Properties.NodeType;
            AVSAssessmentsData[assessmentInfo].TotalMonthlyCostEstimate = avsPropertiesObj.Properties.TotalMonthlyCost;
            AVSAssessmentsData[assessmentInfo].PredictedCpuUtilizationPercentage = avsPropertiesObj.Properties.CpuUtilization;
            AVSAssessmentsData[assessmentInfo].PredictedMemoryUtilizationPercentage = avsPropertiesObj.Properties.RamUtilization;
            AVSAssessmentsData[assessmentInfo].PredictedStorageUtilizationPercentage = avsPropertiesObj.Properties.StorageUtilization;
            AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresAvailable = avsPropertiesObj.Properties.TotalCpuCores;
            AVSAssessmentsData[assessmentInfo].MemoryInTBAvailable = avsPropertiesObj.Properties.TotalRamInGB / 1024.0;
            AVSAssessmentsData[assessmentInfo].StorageInTBAvailable = avsPropertiesObj.Properties.TotalStorageInGB / 1024.0;
            AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresUsed = Math.Ceiling(AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresAvailable * AVSAssessmentsData[assessmentInfo].PredictedCpuUtilizationPercentage / 100.0);
            AVSAssessmentsData[assessmentInfo].MemoryInTBUsed = AVSAssessmentsData[assessmentInfo].MemoryInTBAvailable * AVSAssessmentsData[assessmentInfo].PredictedMemoryUtilizationPercentage / 100.0;
            AVSAssessmentsData[assessmentInfo].StorageInTBUsed = AVSAssessmentsData[assessmentInfo].StorageInTBAvailable * AVSAssessmentsData[assessmentInfo].PredictedStorageUtilizationPercentage / 100.0;
            AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresFree = AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresAvailable - AVSAssessmentsData[assessmentInfo].NumberOfCpuCoresUsed;
            AVSAssessmentsData[assessmentInfo].MemoryInTBFree = AVSAssessmentsData[assessmentInfo].MemoryInTBAvailable - AVSAssessmentsData[assessmentInfo].MemoryInTBUsed;
            AVSAssessmentsData[assessmentInfo].StorageInTBFree = AVSAssessmentsData[assessmentInfo].StorageInTBAvailable - AVSAssessmentsData[assessmentInfo].StorageInTBUsed;
            AVSAssessmentsData[assessmentInfo].ConfidenceRating = UtilityFunctions.GetConfidenceRatingInStars(avsPropertiesObj.Properties.ConfidenceRatingInPercentage);
        }

        #region Utilities
        private List<AssessedDisk> GetAssessedDiskList(Dictionary<string, AVSAssessedMachineDisk> disks)
        {
            List<AssessedDisk> diskList = new List<AssessedDisk>();

            foreach (var kvp in disks)
            {
                AssessedDisk obj = new AssessedDisk();
                obj.DisplayName = kvp.Value.DisplayName;
                obj.GigabytesProvisioned = kvp.Value.GigabytesProvisioned;
                obj.MegabytesPerSecondOfRead = kvp.Value.MegabytesPerSecondOfRead;
                obj.MegabytesPerSecondOfWrite = kvp.Value.MegabytesPerSecondOfWrite;
                obj.NumberOfReadOperationsPerSecond = kvp.Value.NumberOfReadOperationsPerSecond;
                obj.NumberOfWriteOperationsPerSecond = kvp.Value.NumberOfWriteOperationsPerSecond;

                diskList.Add(obj);
            }

            return diskList;
        }

        private List<AssessedNetworkAdapter> GetAssessedNetworkAdapterList(Dictionary<string, AVSAssessedMachineNetworkAdapter> networkAdapters)
        {
            List<AssessedNetworkAdapter> networkAdapterList = new List<AssessedNetworkAdapter>();

            foreach (var kvp in networkAdapters)
            {
                AssessedNetworkAdapter obj = new AssessedNetworkAdapter();
                obj.DisplayName = kvp.Value.DisplayName;
                obj.MacAddress = kvp.Value.MacAddress;
                obj.IpAddresses = kvp.Value.IpAddresses;
                obj.MegabytesPerSecondReceived = kvp.Value.MegabytesPerSecondReceived;
                obj.MegaytesPerSecondTransmitted = kvp.Value.MegabytesPerSecondTransmitted;

                networkAdapterList.Add(obj);
            }

            return networkAdapterList;
        }
        #endregion
    }
}