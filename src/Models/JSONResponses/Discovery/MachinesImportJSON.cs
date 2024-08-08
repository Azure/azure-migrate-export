using Newtonsoft.Json;
using System.Collections.Generic;

namespace Azure.Migrate.Export.Models
{
    public class MachinesImportJSON
    {
        [JsonProperty("value")]
        public List<ImportMachinesValue> Values { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    public class ImportMachinesValue
    {
        [JsonProperty("properties")]
        public ImportMachinesProperty Properties { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ImportMachinesProperty
    {
        [JsonProperty("firmware")]
        public string Firmware { get; set; }

        [JsonProperty("percentageCpuUtilization")]
        public double? PercentageCpuUtilization { get; set; }

        [JsonProperty("percentageMemoryUtilization")]
        public double? PercentageMemoryUtilization { get; set; }

        [JsonProperty("numberOfDisks")]
        public int? NumberOfDisks { get; set; }

        [JsonProperty("totalDiskReadOperationsPerSecond")]
        public double? TotalDiskReadOperationsPerSecond { get; set; }

        [JsonProperty("totalDiskWriteOperationsPerSecond")]
        public double? TotalDiskWriteOperationsPerSecond { get; set; }

        [JsonProperty("totalDiskWriteThroughput")]
        public double? TotalDiskWriteThroughput { get; set; }

        [JsonProperty("totalDiskReadThroughput")]
        public double? TotalDiskReadThroughput { get; set; }

        [JsonProperty("macAddress")]
        public string MacAddress { get; set; }

        [JsonProperty("ipAddresses")]
        public List<string> IPAddresses { get; set; }

        [JsonProperty("machineId")]
        public string MachineId { get; set; }

        [JsonProperty("machineManagerId")]
        public string MachineManagerId { get; set; }

        [JsonProperty("numberOfNetworkAdapters")]
        public int? NumberOfNetworkAdapters { get; set; }

        [JsonProperty("networkInThroughput")]
        public double? NetworkInThroughput { get; set; }

        [JsonProperty("networkOutThroughput")]
        public double? NetworkOutThroughput { get; set; }

        [JsonProperty("serverType")]
        public string ServerType { get; set; }

        [JsonProperty("hypervisor")]
        public string Hypervisor { get; set; }

        [JsonProperty("hypervisorVersionNumber")]
        public string HypervisorVersionNumber { get; set; }

        [JsonProperty("disks")]
        public List<ImportMachinesDisk> Disks { get; set; }

        [JsonProperty("vmFqdn")]
        public string VMFqdn { get; set; }

        [JsonProperty("storageInUseGB")]
        public double? StorageInUseGB { get; set; }

        [JsonProperty("csvGenerationSource")]
        public string CsvGenerationSource { get; set; }

        [JsonProperty("eTag")]
        public string ETag { get; set; }

        [JsonProperty("numberOfProcessorCore")]
        public int? NumberOfProcessorCore { get; set; }

        [JsonProperty("allocatedMemoryInMB")]
        public double? AllocatedMemoryInMB { get; set; }

        [JsonProperty("operatingSystemDetails")]
        public ImportMachinesOperatingSystemDetailsInfo OperatingSystemDetails { get; set; }

        [JsonProperty("biosSerialNumber")]
        public List<ImportMachinesDisk> BiosSerialNumber { get; set; }

        [JsonProperty("biosGuid")]
        public string BiosGuid { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("isDeleted")]
        public string IsDeleted { get; set; }

        [JsonProperty("createdTimestamp")]
        public string CreatedTimestamp { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, object> Tags { get; set; }

        [JsonProperty("updatedTimestamp")]
        public string UpdatedTimestamp { get; set; }
    }

    public class ImportMachinesDisk
    {
        [JsonProperty("megabytesPerSecondOfRead")]
        public double? MegabytesPerSecondOfRead { get; set; }

        [JsonProperty("megabytesPerSecondOfWrite")]
        public double? MegabytesPerSecondOfWrite { get; set; }

        [JsonProperty("numberOfReadOperationsPerSecond")]
        public double? NumberOfReadOperationsPerSecond { get; set; }

        [JsonProperty("numberOfWriteOperationsPerSecond")]
        public double? NumberOfWriteOperationsPerSecond { get; set; }

        [JsonProperty("maxSizeInBytes")]
        public long? MaxSizeInBytes { get; set; }

        [JsonProperty("usedSpaceInBytes")]
        public int? UsedSpaceInBytes { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("diskType")]
        public string DiskType { get; set; }

        [JsonProperty("lun")]
        public int? Lun { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }

    public class ImportMachinesOperatingSystemDetailsInfo
    {
        [JsonProperty("osType")]
        public string OSType { get; set; }

        [JsonProperty("osName")]
        public string OSName { get; set; }

        [JsonProperty("osVersion")]
        public string OSVersion { get; set; }

        [JsonProperty("osArchitecture")]
        public string OSArchitecture { get; set; }
    }
}