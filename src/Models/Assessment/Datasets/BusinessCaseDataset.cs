namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseDataset
    {
        public BusinessCaseDataset()
        {
            OnPremCostDetails = new BusinessCaseDatasetCostDetails();
            AzureIaaSCostDetails = new BusinessCaseDatasetCostDetails();
            AzurePaaSCostDetails = new BusinessCaseDatasetCostDetails();
            TotalYOYCashFlows = new BusinessCaseYOYCostDetailsJSON();
            IaaSYOYCashFlows = new BusinessCaseYOYCostDetailsJSON();
        }

        public BusinessCaseDatasetCostDetails OnPremCostDetails { get; set; }
        public BusinessCaseDatasetCostDetails AzureIaaSCostDetails { get; set; }
        public BusinessCaseDatasetCostDetails AzurePaaSCostDetails { get; set; }
        public BusinessCaseYOYCostDetailsJSON TotalYOYCashFlows { get; set; }
        public BusinessCaseYOYCostDetailsJSON IaaSYOYCashFlows { get; set; }
    }
}
