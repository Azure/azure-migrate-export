namespace Azure.Migrate.Export.Models
{
    public class BusinessCaseInformation
    {
        public string BusinessCaseName { get; set; }

        public string BusinessCaseSettings { get; set; }

        public BusinessCaseInformation(string businessCaseName, string businessCaseSettings)
        {
            BusinessCaseName = businessCaseName;
            BusinessCaseSettings = businessCaseSettings;
        }
    }
}