namespace Azure.Migrate.Export.Models
{
    public class AssessmentSiteMachine
    {
        public string AssessmentId { get; set; }
        public string DiscoveryMachineArmId { get; set; }
        public int SqlInstancesCount { get; set; }
        public int WebApplicationsCount { get; set; }
    }
}