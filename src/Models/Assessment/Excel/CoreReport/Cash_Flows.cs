using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Migrate.Export.Models
{
    public class Cash_Flows
    {
        public BusinessCaseYOYCostDetailsJSON IaaSYOYCosts { get; set; }
        public BusinessCaseYOYCostDetailsJSON TotalYOYCosts { get; set; }
        public BusinessCaseYOYCostDetailsJSON PaaSYOYCosts { get; set; }

        public Cash_Flows()
        {
            IaaSYOYCosts = new BusinessCaseYOYCostDetailsJSON();
            TotalYOYCosts = new BusinessCaseYOYCostDetailsJSON();
            PaaSYOYCosts = new BusinessCaseYOYCostDetailsJSON();
        }
    }
}
