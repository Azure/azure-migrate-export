namespace Azure.Migrate.Export.Models
{
    public class Cash_Flows
    {
        public BusinessCaseYOYCostDetailsJSON IaaSYOYCosts { get; set; }
        public BusinessCaseYOYCostDetailsJSON TotalYOYCosts { get; set; }
        public BusinessCaseYOYCostDetailsJSON PaaSYOYCosts { get; set; }
        public BusinessCaseYOYCostDetailsJSON AvsYOYCosts { get; set; }

        public Cash_Flows()
        {
            IaaSYOYCosts = new BusinessCaseYOYCostDetailsJSON();
            TotalYOYCosts = new BusinessCaseYOYCostDetailsJSON();
            PaaSYOYCosts = new BusinessCaseYOYCostDetailsJSON();
            AvsYOYCosts = new BusinessCaseYOYCostDetailsJSON();
        }
    }
}