namespace Azure.Migrate.Export.Models
{
    public class Business_Case
    {
        public BusinessCaseDatasetCostDetails OnPremisesCost { get; set; }
        public BusinessCaseDatasetCostDetails AzureIaaSCost { get; set; }
        public BusinessCaseDatasetCostDetails AzurePaaSCost { get; set; }
        public BusinessCaseDatasetCostDetails TotalAzureCost { get; set; }

        public Business_Case()
        {
            OnPremisesCost = new BusinessCaseDatasetCostDetails();
            AzureIaaSCost = new BusinessCaseDatasetCostDetails();
            AzurePaaSCost = new BusinessCaseDatasetCostDetails();
            TotalAzureCost = new BusinessCaseDatasetCostDetails();
        }
    }
}
