namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseDataset
    {
        public BusinessCaseDataset()
        {
            OnPremIaaSCostDetails = new BusinessCaseDatasetCostDetails();
            OnPremPaaSCostDetails = new BusinessCaseDatasetCostDetails();
            AzureIaaSCostDetails = new BusinessCaseDatasetCostDetails();
            AzurePaaSCostDetails = new BusinessCaseDatasetCostDetails();
            TotalYOYCashFlows = new BusinessCaseYOYCostDetailsJSON();
            IaaSYOYCashFlows = new BusinessCaseYOYCostDetailsJSON();
        }

        public BusinessCaseDatasetCostDetails OnPremIaaSCostDetails { get; set; }
        public BusinessCaseDatasetCostDetails OnPremPaaSCostDetails { get; set; }
        public BusinessCaseDatasetCostDetails AzureIaaSCostDetails { get; set; }
        public BusinessCaseDatasetCostDetails AzurePaaSCostDetails { get; set; }
        public BusinessCaseYOYCostDetailsJSON TotalYOYCashFlows { get; set; }
        public BusinessCaseYOYCostDetailsJSON IaaSYOYCashFlows { get; set; }
    }
}
