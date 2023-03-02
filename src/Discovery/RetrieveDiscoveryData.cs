using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Discovery
{
    public class RetrieveDiscoveryData
    {
        public List<DiscoveryData> BeginRetrieval(UserInput userInputObj ,List<string> siteUrls, DiscoverySites site)
        {
            List<DiscoveryData> discoveredData = new List<DiscoveryData>();

            foreach (string siteUrl in siteUrls)
            {
                string url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + siteUrl + Routes.ForwardSlash +
                             Routes.MachinesPath +
                             Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.DiscoverySitesApiVersion +
                             Routes.QueryStringAmpersand +
                             Routes.AzureMigrateQueryStringDollar + Routes.AzureMigrateQueryParameterFilter + Routes.QueryStringEquals +
                             Routes.AzureMigrateQueryStringOpenBracket +
                             Routes.AzureMigrateQueryPathProperties + Routes.ForwardSlash + Routes.AzureMigrateQueryPathIsDeleted + Routes.Space +
                             Routes.AzureMigrateQueryStringEq + Routes.Space + Routes.BoolFalse +
                             Routes.AzureMigrateQueryStringCloseBracket;

                while (!string.IsNullOrEmpty(url))
                {
                    if (userInputObj.CancellationContext.IsCancellationRequested)
                        UtilityFunctions.InitiateCancellation(userInputObj);

                    List<DiscoveryData> partialDiscoveryData = new List<DiscoveryData>();

                    string jsonResponse = "";
                    try
                    {
                        jsonResponse = new HttpClientHelper().GetHttpRequestJsonStringResponse(url, userInputObj).Result;
                    }
                    catch(OperationCanceledException)
                    {
                        throw;
                    }
                    catch (AggregateException aeRetrieveDiscoveryData)
                    {
                        string errorMessage = "";
                        foreach (var e in aeRetrieveDiscoveryData.Flatten().InnerExceptions)
                        {
                            if (e is OperationCanceledException)
                                throw e;
                            else
                            {
                                errorMessage = errorMessage + e.Message + " ";
                            }
                        }
                        userInputObj.LoggerObj.LogError($"URL: {url} for {site.ToString()} site failed with messsage: {errorMessage}");
                        return discoveredData;
                    }
                    catch (Exception exJsonResponse)
                    {
                        userInputObj.LoggerObj.LogError($"URL: {url} for {site.ToString()} site failed with messsage: {exJsonResponse.Message}");
                        return discoveredData;
                    }

                    if (site == DiscoverySites.VMWare)
                    {
                        MachinesVMWareJSON jsonObj = JsonConvert.DeserializeObject<MachinesVMWareJSON>(jsonResponse);
                        partialDiscoveryData = ParseVMWareDiscoveryData(userInputObj, jsonObj);
                        url = jsonObj.NextLink;
                        discoveredData.AddRange(partialDiscoveryData);
                    }
                    else if (site == DiscoverySites.HyperV)
                    {
                        MachinesHyperVJSON jsonObj = JsonConvert.DeserializeObject<MachinesHyperVJSON>(jsonResponse);
                        partialDiscoveryData = ParseHyperVDiscoveryData(userInputObj, jsonObj);
                        url = jsonObj.NextLink;
                        discoveredData.AddRange(partialDiscoveryData);
                    }
                    else if (site == DiscoverySites.Physical)
                    {
                        MachinesPhysicalJSON jsonObj = JsonConvert.DeserializeObject<MachinesPhysicalJSON>(jsonResponse);
                        partialDiscoveryData = ParsePhysicalDiscoveryData(userInputObj, jsonObj);
                        url = jsonObj.NextLink;
                        discoveredData.AddRange(partialDiscoveryData);
                    }
                }
            }

            if (userInputObj.CancellationContext.IsCancellationRequested)
                UtilityFunctions.InitiateCancellation(userInputObj);

            // Update devices, if they have sql services present
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 15
            };

            Parallel.For(0, discoveredData.Count, parallelOptions, i =>
            {
                discoveredData[i].IsSqlServicePresent = new IdentifySqlServices().IdentifyPresenceOfSqlServices(discoveredData[i].MachineId, discoveredData[i].MachineName, userInputObj);
            });

            return discoveredData;
        }

        #region Discovery site parsers
        private List<DiscoveryData> ParseVMWareDiscoveryData(UserInput userInputObj, MachinesVMWareJSON jsonObj)
        {
            List<DiscoveryData> data = new List<DiscoveryData>();

            foreach (var value in jsonObj.Values)
            {
                DiscoveryData discoveryDataObj = new DiscoveryData();

                // Assign values
                discoveryDataObj.MachineName = value.Properties.DisplayName;
                discoveryDataObj.EnvironmentType = ""; // only 'Dev' is treated as 'Dev' environment

                discoveryDataObj.SoftwareInventory = value.Properties.NumberOfApplications;
                discoveryDataObj.SqlDiscoveryServerCount = value.Properties.SqlDiscovery.TotalServerCount;
                discoveryDataObj.IsSqlServicePresent = false;
                discoveryDataObj.WebAppCount = value.Properties.WebAppDiscovery.TotalWebApplicationCount;

                discoveryDataObj.OperatingSystem = value.Properties.OperatingSystemDetails.OSName;
                discoveryDataObj.Cores = value.Properties.NumberOfProcessorCore;
                discoveryDataObj.MemoryInMB = value.Properties.AllocatedMemoryInMB;
                discoveryDataObj.TotalDisks = value.Properties.Disks.Count;

                // parse IP Address and MAC Address
                Dictionary<string, List<string>> macIpAddressMap = new Dictionary<string, List<string>>();
                foreach (var networkAdapter in value.Properties.NetworkAdapters)
                    if (!macIpAddressMap.ContainsKey(networkAdapter.MacAddress))
                        macIpAddressMap.Add(networkAdapter.MacAddress, networkAdapter.IpAddressList);
                KeyValuePair<string, string> parsedMacIpAddressMap = ParseMacIpAddressMap(macIpAddressMap, userInputObj);

                discoveryDataObj.IpAddress = parsedMacIpAddressMap.Value;
                discoveryDataObj.MacAddress = parsedMacIpAddressMap.Key;
                discoveryDataObj.TotalNetworkAdapters = value.Properties.NetworkAdapters.Count;

                discoveryDataObj.BootType = string.IsNullOrEmpty(value.Properties.Firmware) ? "" : value.Properties.Firmware.ToUpper();
                discoveryDataObj.PowerStatus = string.IsNullOrEmpty(value.Properties.PowerStatus) ? "" : value.Properties.PowerStatus;
                discoveryDataObj.FirstDiscoveryTime = value.Properties.CreatedTimeStamp;
                discoveryDataObj.LastUpdatedTime = value.Properties.UpdatedTimestamp;

                discoveryDataObj.MachineId = value.Id?.ToLower();

                data.Add(discoveryDataObj);
            }

            return data;
        }

        private List<DiscoveryData> ParseHyperVDiscoveryData(UserInput userInputObj, MachinesHyperVJSON jsonObj)
        {
            List<DiscoveryData> data = new List<DiscoveryData>();

            foreach (var value in jsonObj.Values)
            {
                DiscoveryData discoveryDataObj = new DiscoveryData();

                // Assign values
                discoveryDataObj.MachineName = value.Properties.DisplayName;
                discoveryDataObj.EnvironmentType = ""; // only 'Dev' is treated as 'Dev' environment

                discoveryDataObj.SoftwareInventory = value.Properties.NumberOfApplications;
                discoveryDataObj.SqlDiscoveryServerCount = value.Properties.SqlDiscovery.TotalServerCount;
                discoveryDataObj.IsSqlServicePresent = false;
                discoveryDataObj.WebAppCount = value.Properties.WebAppDiscovery.TotalWebApplicationCount;

                discoveryDataObj.OperatingSystem = value.Properties.OperatingSystemDetails.OSName;
                discoveryDataObj.Cores = value.Properties.NumberOfProcessorCore;
                discoveryDataObj.MemoryInMB = value.Properties.AllocatedMemoryInMB;
                discoveryDataObj.TotalDisks = value.Properties.Disks.Count;

                // parse IP Address and MAC Address
                Dictionary<string, List<string>> macIpAddressMap = new Dictionary<string, List<string>>();
                foreach (var networkAdapter in value.Properties.NetworkAdapters)
                    if (!macIpAddressMap.ContainsKey(networkAdapter.MacAddress))
                        macIpAddressMap.Add(networkAdapter.MacAddress, networkAdapter.IpAddressList);
                KeyValuePair<string, string> parsedMacIpAddressMap = ParseMacIpAddressMap(macIpAddressMap, userInputObj);

                discoveryDataObj.IpAddress = parsedMacIpAddressMap.Value;
                discoveryDataObj.MacAddress = parsedMacIpAddressMap.Key;
                discoveryDataObj.TotalNetworkAdapters = value.Properties.NetworkAdapters.Count;

                discoveryDataObj.BootType = string.IsNullOrEmpty(value.Properties.Firmware) ? "" : value.Properties.Firmware.ToUpper();
                discoveryDataObj.PowerStatus = string.IsNullOrEmpty(value.Properties.PowerStatus) ? "" : value.Properties.PowerStatus;
                discoveryDataObj.FirstDiscoveryTime = value.Properties.CreatedTimeStamp;
                discoveryDataObj.LastUpdatedTime = value.Properties.UpdatedTimestamp;

                discoveryDataObj.MachineId = value.Id?.ToLower();

                data.Add(discoveryDataObj);
            }

            return data;
        }

        private List<DiscoveryData> ParsePhysicalDiscoveryData(UserInput userInputObj, MachinesPhysicalJSON jsonObj)
        {
            List<DiscoveryData> data = new List<DiscoveryData>();

            foreach (var value in jsonObj.Values)
            {
                DiscoveryData discoveryDataObj = new DiscoveryData();

                // Assign values
                discoveryDataObj.MachineName = value.Properties.DisplayName;
                discoveryDataObj.EnvironmentType = ""; // only 'Dev' is treated as 'Dev' environment

                discoveryDataObj.SoftwareInventory = value.Properties.NumberOfApplications;
                discoveryDataObj.SqlDiscoveryServerCount = value.Properties.SqlDiscovery.TotalServerCount;
                discoveryDataObj.IsSqlServicePresent = false;
                discoveryDataObj.WebAppCount = value.Properties.WebAppDiscovery.TotalWebApplicationCount;

                discoveryDataObj.OperatingSystem = value.Properties.OperatingSystemDetails.OSName;
                discoveryDataObj.Cores = value.Properties.NumberOfProcessorCore;
                discoveryDataObj.MemoryInMB = value.Properties.AllocatedMemoryInMB;
                discoveryDataObj.TotalDisks = value.Properties.Disks.Count;

                // parse IP Address and MAC Address
                Dictionary<string, List<string>> macIpAddressMap = new Dictionary<string, List<string>>();
                foreach (var networkAdapter in value.Properties.NetworkAdapters)
                    if (!macIpAddressMap.ContainsKey(networkAdapter.MacAddress))
                        macIpAddressMap.Add(networkAdapter.MacAddress, networkAdapter.IpAddressList);
                KeyValuePair<string, string> parsedMacIpAddressMap = ParseMacIpAddressMap(macIpAddressMap, userInputObj);

                discoveryDataObj.IpAddress = parsedMacIpAddressMap.Value;
                discoveryDataObj.MacAddress = parsedMacIpAddressMap.Key;
                discoveryDataObj.TotalNetworkAdapters = value.Properties.NetworkAdapters.Count;

                discoveryDataObj.BootType = string.IsNullOrEmpty(value.Properties.Firmware) ? "" : value.Properties.Firmware.ToUpper();
                discoveryDataObj.PowerStatus = string.IsNullOrEmpty(value.Properties.PowerStatus) ? "" : value.Properties.PowerStatus;
                discoveryDataObj.FirstDiscoveryTime = value.Properties.CreatedTimeStamp;
                discoveryDataObj.LastUpdatedTime = value.Properties.UpdatedTimestamp;

                discoveryDataObj.MachineId = value.Id?.ToLower();

                data.Add(discoveryDataObj);
            }

            return data;
        }
        #endregion

        #region Utilities
        private KeyValuePair<string, string> ParseMacIpAddressMap(Dictionary<string, List<string>>macIpAddressMap, UserInput userInputObj)
        {
            string macAddresses = "";
            string ipAddresses = "";

            foreach (var kvp in macIpAddressMap)
            {
                macAddresses = macAddresses + "[" + kvp.Key + "];";
                ipAddresses = ipAddresses + "[" + string.Join(",", kvp.Value) + "];";
            }

            return new KeyValuePair<string, string>(macAddresses, ipAddresses);
        }
        #endregion
    }
}