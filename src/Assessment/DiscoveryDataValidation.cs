using System.Collections.Generic;

using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Assessment
{
    public class DiscoveryDataValidation
    {
        public void BeginValidation(UserInput userInputObj, List<DiscoveryData> discoveredData)
        {
            bool isEnvironmentValid = ValidateEnvironmentType(userInputObj, discoveredData);
            if (!isEnvironmentValid)
                userInputObj.LoggerObj.LogWarning(2, "Discovery data validated with assumptions"); // IsExpressWorkflow ? 22 : 2 % complete
            else
                userInputObj.LoggerObj.LogInformation(2, "Discovery data validated successfully"); // IsExpressWorkflow ? 22 : 2 % complete
        }

        private bool ValidateEnvironmentType(UserInput userInputObj, List<DiscoveryData> discoveredData)
        {
            bool isValid = true;
            foreach (var machine in discoveredData)
            {
                if (string.IsNullOrEmpty(machine.EnvironmentType)) // Prod
                {
                    machine.EnvironmentType = "Prod";
                    continue;
                }

                else if (machine.EnvironmentType.ToLower().Equals("prod"))
                {
                    machine.EnvironmentType = "Prod";
                    continue;
                }

                else if (machine.EnvironmentType.ToLower().Equals("dev"))
                {
                    machine.EnvironmentType = "Dev";
                    continue;
                }

                // Invalid/Un-recognized envrionment type
                userInputObj.LoggerObj.LogWarning($"Treating environment type for {machine.MachineName} as 'Prod' because received input is invalid");
                machine.EnvironmentType = "Prod";
                isValid = false;
            }

            return isValid;
        }
    }
}