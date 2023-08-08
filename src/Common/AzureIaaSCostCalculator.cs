using System.Collections.Generic;

using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Common
{

    public class AzureIaaSCostCalculator
    {
        // IaaS datasets
        private List<SQL_IaaS_Instance_Rehost_Perf> SQL_IaaS_Instance_Rehost_Perf_List;
        private List<SQL_IaaS_Server_Rehost_Perf> SQL_IaaS_Server_Rehost_Perf_List;
        private List<WebApp_IaaS_Server_Rehost_Perf> WebApp_IaaS_Server_Rehost_Perf_List;
        private List<VM_IaaS_Server_Rehost_Perf> VM_IaaS_Server_Rehost_Perf_List;

        private bool IsCalculated;

        private double SqlIaaSComputeCost;
        private double WebAppIaaSComputeCost;
        private double VmIaaSComputeCost;
        private double TotalIaaSComputeCost;

        private double SqlIaaSStorageCost;
        private double WebAppIaaSStorageCost;
        private double VmIaaSStorageCost;
        private double TotalIaaSStorageCost;

        private double SqlIaaSSecurityCost;
        private double WebAppIaaSSecurityCost;
        private double VmIaaSSecurityCost;
        private double TotalIaaSSecurityCost;

        private double SqlIaaSAhubSavings;
        private double VmIaaSAhubSavings;
        private double WebAppIaaSAhubSavings;
        private double TotalIaaSAhubSavings;

        public AzureIaaSCostCalculator()
        {
            SQL_IaaS_Instance_Rehost_Perf_List = new List<SQL_IaaS_Instance_Rehost_Perf>();
            SQL_IaaS_Server_Rehost_Perf_List = new List<SQL_IaaS_Server_Rehost_Perf>();
            WebApp_IaaS_Server_Rehost_Perf_List = new List<WebApp_IaaS_Server_Rehost_Perf>();
            VM_IaaS_Server_Rehost_Perf_List = new List<VM_IaaS_Server_Rehost_Perf>();

            IsCalculated = false;

            SqlIaaSComputeCost = 0.0;
            WebAppIaaSComputeCost = 0.0;
            VmIaaSComputeCost = 0.0;
            TotalIaaSComputeCost = 0.0;

            SqlIaaSStorageCost = 0.0;
            WebAppIaaSStorageCost = 0.0;
            VmIaaSStorageCost = 0.0;
            TotalIaaSStorageCost = 0.0;

            SqlIaaSSecurityCost = 0.0;
            WebAppIaaSSecurityCost = 0.0;
            VmIaaSSecurityCost = 0.0;
            TotalIaaSSecurityCost = 0.0;

            SqlIaaSAhubSavings = 0.0;
            VmIaaSAhubSavings = 0.0;
            WebAppIaaSAhubSavings = 0.0;
            TotalIaaSAhubSavings = 0.0;
        }

        public bool IsCalculationComplete()
        {
            return IsCalculated;
        }

        public double GetTotalStorageCost()
        {
            return TotalIaaSStorageCost;
        }

        public double GetTotalComputeCost()
        {
            return TotalIaaSComputeCost;
        }
        
        public double GetTotalAhubSavings()
        {
            return TotalIaaSAhubSavings;
        }

        public double GetTotalSecurityCost()
        {
            return TotalIaaSSecurityCost;
        }

        public void SetParameters(List<SQL_IaaS_Instance_Rehost_Perf> sql_IaaS_Instance_Rehost_Perf_List,
                                  List<SQL_IaaS_Server_Rehost_Perf> sql_IaaS_Server_Rehost_Perf_List,
                                  List<WebApp_IaaS_Server_Rehost_Perf> webApp_IaaS_Server_Rehost_Perf_List,
                                  List<VM_IaaS_Server_Rehost_Perf> vm_IaaS_Server_Rehost_Perf_List)
        {
            SQL_IaaS_Instance_Rehost_Perf_List = sql_IaaS_Instance_Rehost_Perf_List;
            SQL_IaaS_Server_Rehost_Perf_List = sql_IaaS_Server_Rehost_Perf_List;
            WebApp_IaaS_Server_Rehost_Perf_List = webApp_IaaS_Server_Rehost_Perf_List;
            VM_IaaS_Server_Rehost_Perf_List = vm_IaaS_Server_Rehost_Perf_List;
        }

        public void Calculate()
        {
            CalculateSqlIaaSCost();
            CalculateWebAppIaaSCost();
            CalculateVMIaaSCost();

            TotalIaaSComputeCost = SqlIaaSComputeCost + WebAppIaaSComputeCost + VmIaaSComputeCost;
            TotalIaaSStorageCost = SqlIaaSStorageCost + WebAppIaaSStorageCost + VmIaaSStorageCost;
            TotalIaaSSecurityCost = SqlIaaSSecurityCost + WebAppIaaSSecurityCost + VmIaaSSecurityCost;
            TotalIaaSAhubSavings = SqlIaaSAhubSavings + WebAppIaaSAhubSavings + VmIaaSAhubSavings;

            IsCalculated = true;
        }

        private void CalculateSqlIaaSCost()
        {
            double nonAhubCost = 0.0;
            foreach (var sqlInstance in SQL_IaaS_Instance_Rehost_Perf_List)
            {
                if (sqlInstance.Environment.Equals("Dev"))
                {
                    SqlIaaSComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate_AHUB : sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year == 0 ? sqlInstance.MonthlyComputeCostEstimate : sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    SqlIaaSComputeCost += sqlInstance.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlInstance.MonthlyComputeCostEstimate_RI3year;
                }

                SqlIaaSStorageCost += sqlInstance.MonthlyStorageCostEstimate;
                SqlIaaSSecurityCost += sqlInstance.MonthlySecurityCostEstimate;
            }

            foreach (var sqlServer in SQL_IaaS_Server_Rehost_Perf_List)
            {
                if (sqlServer.Environment.Equals("Dev"))
                {
                    SqlIaaSComputeCost += sqlServer.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? sqlServer.MonthlyComputeCostEstimate_AHUB : sqlServer.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlServer.MonthlyComputeCostEstimate_RI3year == 0 ? sqlServer.MonthlyComputeCostEstimate : sqlServer.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    SqlIaaSComputeCost += sqlServer.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += sqlServer.MonthlyComputeCostEstimate_RI3year;
                }

                SqlIaaSStorageCost += sqlServer.MonthlyStorageCostEstimate;
                SqlIaaSSecurityCost += sqlServer.MonthlySecurityCostEstimate;
            }

            SqlIaaSAhubSavings = nonAhubCost - SqlIaaSComputeCost;
        }

        private void CalculateWebAppIaaSCost()
        {
            double nonAhubCost = 0.0;
            foreach (var webappServer in WebApp_IaaS_Server_Rehost_Perf_List)
            {
                if (webappServer.Environment.Equals("Dev"))
                {
                    WebAppIaaSComputeCost += webappServer.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? webappServer.MonthlyComputeCostEstimate_AHUB : webappServer.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += webappServer.MonthlyComputeCostEstimate_RI3year == 0 ? webappServer.MonthlyComputeCostEstimate : webappServer.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    WebAppIaaSComputeCost += webappServer.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += webappServer.MonthlyComputeCostEstimate_RI3year;
                }

                WebAppIaaSStorageCost += webappServer.MonthlyStorageCostEstimate;
                WebAppIaaSSecurityCost += webappServer.MonthlySecurityCostEstimate;
            }

            WebAppIaaSAhubSavings = nonAhubCost - WebAppIaaSComputeCost;
        }

        private void CalculateVMIaaSCost()
        {
            double nonAhubCost = 0.0;
            foreach (var vm in VM_IaaS_Server_Rehost_Perf_List)
            {
                if (vm.Environment.Equals("Dev"))
                {
                    VmIaaSComputeCost += vm.MonthlyComputeCostEstimate_AHUB_RI3year == 0 ? vm.MonthlyComputeCostEstimate_AHUB : vm.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += vm.MonthlyComputeCostEstimate_RI3year == 0 ? vm.MonthlyComputeCostEstimate : vm.MonthlyComputeCostEstimate_RI3year;
                }
                else
                {
                    VmIaaSComputeCost += vm.MonthlyComputeCostEstimate_AHUB_RI3year;
                    nonAhubCost += vm.MonthlyComputeCostEstimate_RI3year;
                }

                VmIaaSStorageCost += vm.MonthlyStorageCostEstimate;
                VmIaaSSecurityCost += vm.MonthlySecurityCostEstimate;
            }

            VmIaaSAhubSavings = nonAhubCost - VmIaaSComputeCost;
        }
    }
}
