using System.Collections.Generic;

using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Common
{
    public class AzurePaaSCostCalculator
    {
        // PaaS datasets
        private List<SQL_MI_PaaS> SQL_MI_PaaS_List;
        private List<WebApp_PaaS> WebApp_PaaS_List;
        private Dictionary<string, double> SqlPaaSCostMap = new Dictionary<string, double>();
        private Dictionary<string, double> WebAppPaaSCostMap = new Dictionary<string, double>();

        private bool IsCalculated;

        private double SqlPaaSComputeCost;
        private double SqlPaaSDevComputeCost;
        private double SqlPaaSProdComputeCost;
        private double WebAppPaaSComputeCost;
        private double WebAppPaaSDevComputeCost;
        private double WebAppPaaSProdComputeCost;
        private double TotalPaaSComputeCost;

        private double SqlPaaSStorageCost;
        private double SqlPaaSDevStorageCost;
        private double SqlPaaSProdStorageCost;
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

            SqlPaaSDevComputeCost = 0.0;
            SqlPaaSProdComputeCost = 0.0;
            WebAppPaaSDevComputeCost = 0.0;
            WebAppPaaSProdComputeCost = 0.0;
            TotalPaaSComputeCost = 0.0;

            SqlPaaSDevStorageCost = 0.0;
            SqlPaaSProdStorageCost = 0.0;
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

        public Dictionary<string, double> GetSqlPaaSCosts()
        {
            return SqlPaaSCostMap;
        }
        public Dictionary<string, double> GetWebAppPaaSCosts()
        {
            return WebAppPaaSCostMap;
        }
        private void CalculateSqlPaaSCost()
        {
            double nonAhubCost = 0.0;
            foreach (var sqlInstance in SQL_MI_PaaS_List)
            {
                if (sqlInstance.Environment.Equals("Dev"))
                {
                    SqlPaaSDevComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate_AHUB : sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    SqlPaaSDevStorageCost += sqlInstance.MonthlyStorageCostEstimate;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate : sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    SqlPaaSProdComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    SqlPaaSProdStorageCost += sqlInstance.MonthlyStorageCostEstimate;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }
                SqlPaaSSecurityCost += sqlInstance.MonthlySecurityCostEstimate;
            }
            SqlPaaSComputeCost = SqlPaaSDevComputeCost + SqlPaaSProdComputeCost;
            SqlPaaSStorageCost = SqlPaaSDevStorageCost + SqlPaaSProdStorageCost;
            SqlPaaSAhubSavings = nonAhubCost - SqlPaaSComputeCost;
            SqlPaaSCostMap.Add("SqlPaaSDevComputeCost", SqlPaaSDevComputeCost);
            SqlPaaSCostMap.Add("SqlPaaSProdComputeCost", SqlPaaSProdComputeCost);
            SqlPaaSCostMap.Add("SqlPaaSDevStorageCost", SqlPaaSDevStorageCost);
            SqlPaaSCostMap.Add("SqlPaaSProdStorageCost", SqlPaaSProdStorageCost);
        }

        private void CalculateWebAppPaaSCost()
        {
            foreach (var webapp in WebApp_PaaS_List)
            {
                if (webapp.Environment.Equals("Dev"))
                    WebAppPaaSDevComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year == 0 ? webapp.MonthlyComputeCostEstimate : webapp.MonthlyComputeCostEstimate_ASP3year;
                else
                    WebAppPaaSProdComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year;

                WebAppPaaSSecurityCost += webapp.MonthlySecurityCostEstimate;
            }
            WebAppPaaSComputeCost = WebAppPaaSDevComputeCost + WebAppPaaSProdComputeCost;
            WebAppPaaSStorageCost = 0.0;
            WebAppPaaSCostMap.Add("WebAppPaaSDevComputeCost", WebAppPaaSDevComputeCost);
            WebAppPaaSCostMap.Add("WebAppPaaSProdComputeCost", WebAppPaaSProdComputeCost);
            WebAppPaaSCostMap.Add("WebAppPaaSStorageCost", WebAppPaaSStorageCost);
        }

        public void SetParameters(List<SQL_MI_PaaS> sql_MI_PaaS_List, List<WebApp_PaaS> webApp_PaaS_List)
        {
            SQL_MI_PaaS_List = sql_MI_PaaS_List;
            WebApp_PaaS_List = webApp_PaaS_List;
        }
    }
}
