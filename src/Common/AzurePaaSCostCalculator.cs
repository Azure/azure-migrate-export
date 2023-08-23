using System.Collections.Generic;

using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Common
{
    public class AzurePaaSCostCalculator
    {
        // PaaS datasets
        private List<SQL_MI_PaaS> SQL_MI_PaaS_List;
        private List<WebApp_PaaS> WebApp_PaaS_List;
        HashSet<string> WebappUniqueDevMachines;
        HashSet<string> WebappUniqueProdMachines;
        HashSet<string> SqlUniqueDevMachines;
        HashSet<string> SqlUniqueProdMachines;

        private bool IsCalculated;

        int SqlDevRowCount;
        int SqlProdRowCount;

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
        private double WebAppPaaSDevStorageCost;
        private double WebAppPaaSProdStorageCost;
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
            WebappUniqueDevMachines = new HashSet<string>();
            WebappUniqueProdMachines = new HashSet<string>();
            SqlUniqueDevMachines = new HashSet<string>();
            SqlUniqueProdMachines = new HashSet<string>();

            IsCalculated = false;

            SqlDevRowCount = 0;
            SqlProdRowCount = 0;

            SqlPaaSComputeCost = 0.0;
            SqlPaaSDevComputeCost = 0.0;
            SqlPaaSProdComputeCost = 0.0;
            WebAppPaaSComputeCost = 0.0;
            WebAppPaaSDevComputeCost = 0.0;
            WebAppPaaSProdComputeCost = 0.0;
            TotalPaaSComputeCost = 0.0;

            SqlPaaSStorageCost = 0.0;
            SqlPaaSDevStorageCost = 0.0;
            SqlPaaSProdStorageCost = 0.0;
            WebAppPaaSStorageCost = 0.0;
            WebAppPaaSDevStorageCost = 0.0;
            WebAppPaaSProdStorageCost = 0.0;
            TotalPaaSStorageCost = 0.0;

            SqlPaaSSecurityCost = 0.0;
            WebAppPaaSSecurityCost = 0.0;
            TotalPaaSSecurityCost = 0.0;

            SqlPaaSStorageCost = 0.0;

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

        public double GetSqlPaaSDevComputeCost()
        {
            return SqlPaaSDevComputeCost;
        }

        public double GetSqlPaaSProdComputeCost()
        {
            return SqlPaaSProdComputeCost;
        }

        public double GetSqlPaaSDevStorageCost()
        {
            return SqlPaaSDevStorageCost;
        }

        public double GetSqlPaaSProdStorageCost()
        {
            return SqlPaaSProdStorageCost;
        }

        public double GetWebAppPaaSDevComputeCost()
        {
            return WebAppPaaSDevComputeCost;
        }
        public double GetWebAppPaaSProdComputeCost()
        {
            return WebAppPaaSProdComputeCost;
        }
        public double GetWebAppPaaSDevStorageCost()
        {
            return WebAppPaaSDevStorageCost;
        }

        public double GetWebAppPaaSProdStorageCost()
        {
            return WebAppPaaSProdStorageCost;
        }

        public int GetWebAppPaaSDevMachineIdCount()
        {
            return WebappUniqueDevMachines.Count;
        }

        public int GetWebAppPaaSProdMachineIdCount()
        {
            return WebappUniqueProdMachines.Count;
        }

        public int GetSqlPaaSDevMachineIdCount()
        {
            return SqlUniqueDevMachines.Count;
        }

        public int GetSqlPaaSProdMachineIdCount()
        {
            return SqlUniqueProdMachines.Count;
        }

        public int GetSqlPaaSDevMachinesCountTarget()
        {
            return SqlDevRowCount;
        }

        public int GetSqlPaaSProdMachinesCountTarget()
        {
            return SqlProdRowCount;
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

                    SqlDevRowCount += 1;

                    if (!SqlUniqueDevMachines.Contains(sqlInstance.MachineId))
                        SqlUniqueDevMachines.Add(sqlInstance.MachineId);
                }
                else
                {
                    SqlPaaSProdComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    SqlPaaSProdStorageCost += sqlInstance.MonthlyStorageCostEstimate;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year;

                    SqlProdRowCount += 1;

                    if (!SqlUniqueProdMachines.Contains(sqlInstance.MachineId))
                        SqlUniqueProdMachines.Add(sqlInstance.MachineId);
                }
                SqlPaaSSecurityCost += sqlInstance.MonthlySecurityCostEstimate;
            }
            SqlPaaSComputeCost = SqlPaaSDevComputeCost + SqlPaaSProdComputeCost;
            SqlPaaSStorageCost = SqlPaaSDevStorageCost + SqlPaaSProdStorageCost;
            SqlPaaSAhubSavings = nonAhubCost - SqlPaaSComputeCost;
        }

        private void CalculateWebAppPaaSCost()
        {
            foreach (var webapp in WebApp_PaaS_List)
            {
                if (webapp.Environment.Equals("Dev"))
                {
                    WebAppPaaSDevComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year == 0 ? webapp.MonthlyComputeCostEstimate : webapp.MonthlyComputeCostEstimate_ASP3year;

                    if (!WebappUniqueDevMachines.Contains(webapp.MachineId))
                        WebappUniqueDevMachines.Add(webapp.MachineId);
                }
                else
                {
                    WebAppPaaSProdComputeCost += webapp.MonthlyComputeCostEstimate_ASP3year == 0 ? webapp.MonthlyComputeCostEstimate : webapp.MonthlyComputeCostEstimate_ASP3year;

                    if (!WebappUniqueProdMachines.Contains(webapp.MachineId))
                        WebappUniqueProdMachines.Add(webapp.MachineId);
                } 

                WebAppPaaSSecurityCost += webapp.MonthlySecurityCostEstimate;
            }
            WebAppPaaSComputeCost = WebAppPaaSDevComputeCost + WebAppPaaSProdComputeCost;
        }

        public void SetParameters(List<SQL_MI_PaaS> sql_MI_PaaS_List, List<WebApp_PaaS> webApp_PaaS_List)
        {
            SQL_MI_PaaS_List = sql_MI_PaaS_List;
            WebApp_PaaS_List = webApp_PaaS_List;
        }
    }
}
