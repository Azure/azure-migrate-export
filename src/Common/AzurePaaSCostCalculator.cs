using System.Collections.Generic;

using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Common
{
    public class AzurePaaSCostCalculator
    {
        // PaaS datasets
        private List<SQL_MI_PaaS> SQL_MI_PaaS_List;
        private List<WebApp_PaaS> WebApp_PaaS_List;

        private bool IsCalculated;

        private double SqlPaaSComputeCost;
        private double WebAppPaaSComputeCost;
        private double TotalPaaSComputeCost;

        private double SqlPaaSStorageCost;
        private double WebAppPaaSStorageCost;
        private double TotalPaaSStorageCost;

        private double SqlPaaSSecurityCost;
        private double WebAppPaaSSecurityCost;
        private double TotalPaaSSecurityCost;

        private double SqlPaaSAhubSavings;
        private double TotalPaaSAhubSavings;

        public AzurePaaSCostCalculator()
        {
            SQL_MI_PaaS_List = new List<SQL_MI_PaaS>();
            WebApp_PaaS_List = new List<WebApp_PaaS>();

            IsCalculated = false;

            SqlPaaSComputeCost = 0.0;
            WebAppPaaSComputeCost = 0.0;
            TotalPaaSComputeCost = 0.0;

            SqlPaaSStorageCost = 0.0;
            WebAppPaaSStorageCost = 0.0;
            TotalPaaSStorageCost = 0.0;

            SqlPaaSSecurityCost = 0.0;
            WebAppPaaSSecurityCost = 0.0;
            TotalPaaSSecurityCost = 0.0;

            SqlPaaSAhubSavings = 0.0;
            TotalPaaSAhubSavings = 0.0;
        }

        public void Calculate()
        {
            CalculateSqlPaaSCost();
            CalculateWebAppPaaSCost();

            TotalPaaSComputeCost = SqlPaaSComputeCost + WebAppPaaSComputeCost;
            TotalPaaSStorageCost = SqlPaaSStorageCost + WebAppPaaSStorageCost;
            TotalPaaSSecurityCost = SqlPaaSSecurityCost + WebAppPaaSSecurityCost;
            TotalPaaSAhubSavings = SqlPaaSAhubSavings;

            IsCalculated = true;
        }

        public bool IsCalculationComplete()
        {
            return IsCalculated;
        }

        public double GetTotalSecurityCost()
        {
            return TotalPaaSSecurityCost;
        }

        public double GetTotalAhubSavings()
        {
            return TotalPaaSAhubSavings;
        }

        public double GetTotalComputeCost()
        {
            return TotalPaaSComputeCost;
        }

        public double GetTotalStorageCost()
        {
            return TotalPaaSStorageCost;
        }

        private void CalculateSqlPaaSCost()
        {
            double nonAhubCost = 0.0;
            foreach (var sqlInstance in SQL_MI_PaaS_List)
            {
                if (sqlInstance.Environment.Equals("Dev"))
                {
                    SqlPaaSComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate_AHUB : sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate : sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    SqlPaaSComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }

                SqlPaaSStorageCost += sqlInstance.MonthlyStorageCostEstimate;
                SqlPaaSSecurityCost += sqlInstance.MonthlySecurityCostEstimate;
            }

            SqlPaaSAhubSavings = nonAhubCost - SqlPaaSComputeCost;
        }

        private void CalculateWebAppPaaSCost()
        {
            foreach (var webapp in WebApp_PaaS_List)
            {
                if (webapp.Environment.Equals("Dev"))
                    WebAppPaaSComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year == 0 ? webapp.MonthlyComputeCostEstimate : webapp.MonthlyComputeCostEstimate_ASP3year;
                else
                    WebAppPaaSComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year;

                WebAppPaaSSecurityCost += webapp.MonthlySecurityCostEstimate;
            }

            WebAppPaaSStorageCost = 0.0;
        }

        public void SetParameters(List<SQL_MI_PaaS> sql_MI_PaaS_List, List<WebApp_PaaS> webApp_PaaS_List)
        {
            SQL_MI_PaaS_List = sql_MI_PaaS_List;
            WebApp_PaaS_List = webApp_PaaS_List;
        }
    }
}
