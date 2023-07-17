namespace Azure.Migrate.Export.Common
{
    public class Routes
    {
        public const string ProtocolScheme = @"https://";
        public const string AzureManagementApiHostname = @"management.azure.com";
        public const string SubscriptionPath = @"subscriptions";
        public const string ResourceGroupPath = @"resourceGroups";
        public const string GroupsPath = @"groups";
        public const string ProvidersPath = @"providers";
        public const string MigrateProvidersPath = @"Microsoft.Migrate";
        public const string OffAzureProvidersPath = @"Microsoft.OffAzure";
        public const string MasterSitesPath = @"masterSites";
        public const string MigrateProjectsPath = @"MigrateProjects";
        public const string AssessedMachinesPath = @"assessedMachines";
        public const string AssessedSQLMachinesPath = @"assessedSqlMachines";
        public const string SolutionsPath = @"Solutions";
        public const string MachinesPath = @"machines";
        public const string UpdateMachinesPath = @"updateMachines";
        public const string ApplicationsPath = @"applications";
        public const string ServerDiscoveryPath = @"Servers-Discovery-ServerDiscovery";
        public const string ServerAssessmentPath = @"Servers-Assessment-ServerAssessment";
        public const string AVSAssessedMachinesPath = @"avsAssessedMachines";
        public const string AssessedSQLInstancesPath = @"assessedSqlInstances";
        public const string AzureAppServiceAssessedWebAppsPath = @"assessedWebApps";
        public const string AssessmentProjectsPath = @"assessmentProjects";
        public const string CreateGroupApiVersion = @"2019-10-01";
        public const string ProjectDetailsApiVersion = @"2020-05-01";
        public const string MasterSiteApiVersion = @"2020-11-11-preview";
        public const string DiscoverySitesApiVersion = @"2020-08-01-preview";
        public const string AssessmentMachineListApiVersion = @"2022-02-02-preview";
        public const string QueryParameterApiVersion = @"api-version";
        public const string AzureMigrateQueryParameterFilter = @"filter";
        public const string AzureMigrateQueryPathProperties = @"Properties";
        public const string AzureMigrateQueryPathIsDeleted = @"IsDeleted";
        public const string QueryStringEquals = @"=";
        public const string QueryStringQuestionMark = @"?";
        public const string QueryStringAmpersand = @"&";
        public const string AzureMigrateQueryStringDollar = @"$";
        public const string AzureMigrateQueryStringOpenBracket = @"(";
        public const string AzureMigrateQueryStringCloseBracket = @")";
        public const string AzureMigrateQueryStringEq = @"eq";
        public const string BoolTrue = "true";
        public const string BoolFalse = "false";
        public const string ForwardSlash = @"/";
        public const string Space = " ";

        public const string ForexApi = "api.exchangerate.host";
        public const string ForexApiLatestEndpoint = "latest";
        public const string ForexApiBasePriceQueryParameter = "base";
        public const string ForexApiCurrencySymbolsQueryParameter = "symbols";
    }
}