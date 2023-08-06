namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseDatasetCostDetails
    {
        public double ComputeCost { get; set; }
        public double LicenseCost { get; set; }
        public double EsuIaaSLicenseCost { get; set; }
        public double EsuPaaSLicenseCost { get; set; }
        public double StorageCost { get; set; }
        public double NetworkCost { get; set; }
        public double SecurityCost { get; set; }
        public double ITStaffCost { get; set; }
        public double FacilitiesCost { get; set; }
    }
}
