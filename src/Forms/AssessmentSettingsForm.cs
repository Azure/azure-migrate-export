using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Azure.Migrate.Export.Forms
{
    public partial class AssessmentSettingsForm : Form
    {
        private AzureMigrateExportMainForm mainFormObj;

        public AssessmentSettingsForm(AzureMigrateExportMainForm obj)
        {
            InitializeComponent();
            mainFormObj = obj;
        }

        #region Initialization
        public void Initialize()
        {
            InitializeTargetRegionComboBox();
            InitializeCurrencyComboBox();
            InitializeAssessmentDurationComboBox();
            InitializeOptimizationPreference();
        }
        
        private void InitializeTargetRegionComboBox()
        {
            List<KeyValuePair<string, string>> location = new List<KeyValuePair<string, string>>();
            location.Add(new KeyValuePair<string, string>("eastus", "East US"));
            location.Add(new KeyValuePair<string, string>("eastus2", "East US 2"));
            location.Add(new KeyValuePair<string, string>("southcentralus", "South Central US"));
            location.Add(new KeyValuePair<string, string>("westus2", "West US 2"));
            location.Add(new KeyValuePair<string, string>("australiaeast", "Australia East"));
            location.Add(new KeyValuePair<string, string>("southeastasia", "Southeast Asia"));
            location.Add(new KeyValuePair<string, string>("northeurope", "North Europe"));
            location.Add(new KeyValuePair<string, string>("swedencentral", "Sweden Central"));
            location.Add(new KeyValuePair<string, string>("uksouth", "UK South"));
            location.Add(new KeyValuePair<string, string>("westeurope", "West Europe"));
            location.Add(new KeyValuePair<string, string>("centralus", "Central US"));
            location.Add(new KeyValuePair<string, string>("southafricanorth", "South Africa North"));
            location.Add(new KeyValuePair<string, string>("centralindia", "Central India"));
            location.Add(new KeyValuePair<string, string>("eastasia", "East Asia"));
            location.Add(new KeyValuePair<string, string>("japaneast", "Japan East"));
            location.Add(new KeyValuePair<string, string>("koreacentral", "Korea Central"));
            location.Add(new KeyValuePair<string, string>("canadacentral", "Canada Central"));
            location.Add(new KeyValuePair<string, string>("francecentral", "France Central"));
            location.Add(new KeyValuePair<string, string>("germanywestcentral", "Germany West Central"));
            location.Add(new KeyValuePair<string, string>("norwayeast", "Norway East"));
            location.Add(new KeyValuePair<string, string>("switzerlandnorth", "Switzerland North"));
            location.Add(new KeyValuePair<string, string>("uaenorth", "UAE North"));
            location.Add(new KeyValuePair<string, string>("brazilsouth", "Brazil South"));
            location.Add(new KeyValuePair<string, string>("eastus2euap", "East US 2 EUAP"));
            location.Add(new KeyValuePair<string, string>("jioindiawest", "Jio India West"));
            location.Add(new KeyValuePair<string, string>("centraluseuap", "Central US EUAP"));
            location.Add(new KeyValuePair<string, string>("westcentralus", "West Central US"));
            location.Add(new KeyValuePair<string, string>("australiasoutheast", "Australia Southeast"));
            location.Add(new KeyValuePair<string, string>("japanwest", "Japan West"));
            location.Add(new KeyValuePair<string, string>("koreasouth", "Korea South"));
            location.Add(new KeyValuePair<string, string>("southindia", "South India"));
            location.Add(new KeyValuePair<string, string>("ukwest", "UK West"));

            TargetRegionComboBox.DataSource = location;
            TargetRegionComboBox.ValueMember = "Key";
            TargetRegionComboBox.DisplayMember = "Value";
            TargetRegionComboBox.SelectedItem = null;
        }

        private void InitializeCurrencyComboBox()
        {
            List<KeyValuePair<string, string>> currency = new List<KeyValuePair<string, string>>();
            currency.Add(new KeyValuePair<string, string>("USD", "United States – Dollar ($) USD"));
            currency.Add(new KeyValuePair<string, string>("AUD", "Australia – Dollar ($) AUD"));
            currency.Add(new KeyValuePair<string, string>("BRL", "Brazil – Real (R$) BRL"));
            currency.Add(new KeyValuePair<string, string>("CAD", "Canada – Dollar ($) CAD"));
            currency.Add(new KeyValuePair<string, string>("DKK", "Denmark – Krone (kr) DKK"));
            currency.Add(new KeyValuePair<string, string>("EUR", "Euro Zone – Euro (€) EUR"));
            currency.Add(new KeyValuePair<string, string>("INR", "India – Rupee (₹) INR"));
            currency.Add(new KeyValuePair<string, string>("JPY", "Japan – Yen (¥) JPY"));
            currency.Add(new KeyValuePair<string, string>("KRW", "Korea – Won (₩) KRW"));
            currency.Add(new KeyValuePair<string, string>("NZD", "New Zealand – Dollar ($) NZD"));
            currency.Add(new KeyValuePair<string, string>("NOK", "Norway – Krone (kr) NOK"));
            currency.Add(new KeyValuePair<string, string>("RUB", "Russia – Ruble (руб) RUB"));
            currency.Add(new KeyValuePair<string, string>("SEK", "Sweden – Krona (kr) SEK"));
            currency.Add(new KeyValuePair<string, string>("CHF", "Switzerland – Franc (chf) CHF"));
            currency.Add(new KeyValuePair<string, string>("TWD", "Taiwan – Dollar (NT$) TWD"));
            currency.Add(new KeyValuePair<string, string>("GBP", "United Kingdom – Pound (£) GBP"));

            CurrencyComboBox.DataSource = currency;
            CurrencyComboBox.ValueMember = "Key";
            CurrencyComboBox.DisplayMember = "Value";
            CurrencyComboBox.SelectedItem = new KeyValuePair<string, string>("USD", "United States – Dollar ($) USD");
        }

        private void InitializeAssessmentDurationComboBox()
        {
            List<KeyValuePair<string, string>> assessmentDurations = new List<KeyValuePair<string, string>>();
            assessmentDurations.Add(new KeyValuePair<string, string>("day", "Day"));
            assessmentDurations.Add(new KeyValuePair<string, string>("week", "Week"));
            assessmentDurations.Add(new KeyValuePair<string, string>("month", "Month"));

            AssessmentDurationComboBox.DataSource = assessmentDurations;
            AssessmentDurationComboBox.ValueMember = "Key";
            AssessmentDurationComboBox.DisplayMember = "Value";
            AssessmentDurationComboBox.SelectedItem = new KeyValuePair<string, string>("week", "Week");
        }

        private void InitializeOptimizationPreference()
        {   
            List<KeyValuePair<string, string>> optimizationPreferences = new List<KeyValuePair<string, string>>();
            optimizationPreferences.Add(new KeyValuePair<string, string>("ModernizeToPaaS", "Modernize to PaaS (PaaS preferred)"));
            optimizationPreferences.Add(new KeyValuePair<string, string>("MigrateToAllIaaS", "Migrate to all IaaS"));

            OptimizationPreferenceComboBox.DataSource = optimizationPreferences;
            OptimizationPreferenceComboBox.ValueMember = "Key";
            OptimizationPreferenceComboBox.DisplayMember = "Value";
            OptimizationPreferenceComboBox.SelectedItem = new KeyValuePair<string, string>("ModernizeToPaaS", "Modernize to PaaS (PaaS preferred)");

            AssessSqlServicesSeparatelyGroupBox.Visible = false;
        }
        #endregion

        #region Validation
        public bool ValidateAssessmentSettings()
        {
            if (!ValidateTargetRegion())
                return false;
            if (!ValidateCurrency())
                return false;
            if (!ValidateAssessmentDuration())
                return false;
            if (!ValidateOptimizationPreference())
                return false;

            return true;
        }

        private bool ValidateTargetRegion()
        {
            if (TargetRegionComboBox.SelectedItem == null)
                return false;

            return true;
        }

        private bool ValidateCurrency()
        {
            if (CurrencyComboBox.SelectedItem == null)
                return false;
            return true;
        }

        private bool ValidateAssessmentDuration()
        {
            if (AssessmentDurationComboBox.SelectedItem == null)
                return false;

            return true;
        }

        private bool ValidateOptimizationPreference()
        {
            if (!mainFormObj.IsAvsBusinessProposalSelected() && OptimizationPreferenceComboBox.SelectedItem == null)
                return false;

            /*else if (!mainFormObj.IsAvsBusinessProposalSelected() &&
                     ((KeyValuePair<string, string>)OptimizationPreferenceComboBox.SelectedItem).Key == "ModernizeToPaaS" &&
                     ((KeyValuePair<string, string>)OptimizationPreferenceComboBox.SelectedItem).Value == "Modernize to PaaS (PaaS preferred)" &&
                     AssessSqlServicesSeparatelyGroupBox.Visible == false)
            {
                return false;
            }*/

            return true;
        }
        #endregion

        #region ComboBox Mouse Click
        private void TargetRegionComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            TargetRegionComboBox.DroppedDown = true;
        }

        private void CurrencyComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            CurrencyComboBox.DroppedDown = true;
        }

        private void AssessmentDurationComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            AssessmentDurationComboBox.DroppedDown = true;
        }

        private void OptimizationPreferenceComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            OptimizationPreferenceComboBox.DroppedDown = true;
        }
        #endregion

        #region ComboBox Selection Change Committed
        private void OptimizationPreferenceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            KeyValuePair<string, string> selectedOptimizationPreference = GetSelectedOptimizationPreference();
            if (!string.IsNullOrEmpty(selectedOptimizationPreference.Value) && !string.IsNullOrEmpty(selectedOptimizationPreference.Key) 
                && selectedOptimizationPreference.Value == "Modernize to PaaS (PaaS preferred)")
                AssessSqlServicesSeparatelyGroupBox.Visible = false; //AssessSqlServicesSeparatelyGroupBox.Visible = true;
            else
                AssessSqlServicesSeparatelyGroupBox.Visible = false;

            mainFormObj.MakeAssessmentSettingsActionButtonsEnabledDecision();
            mainFormObj.MakeAssessmentSettingsTabButtonEnableDecisions();

            this.ActiveControl = null;
        }

        private void TargetRegionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainFormObj.MakeAssessmentSettingsActionButtonsEnabledDecision();
            mainFormObj.MakeAssessmentSettingsTabButtonEnableDecisions();

            this.ActiveControl = null;
        }

        private void CurrencyComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainFormObj.MakeAssessmentSettingsActionButtonsEnabledDecision();
            mainFormObj.MakeAssessmentSettingsTabButtonEnableDecisions();

            this.ActiveControl = null;
        }

        private void AssessmentDurationComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainFormObj.MakeAssessmentSettingsActionButtonsEnabledDecision();
            mainFormObj.MakeAssessmentSettingsTabButtonEnableDecisions();

            this.ActiveControl = null;
        }
        #endregion

        #region Getter Methods
        public KeyValuePair<string, string> GetTargetRegion()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (TargetRegionComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)TargetRegionComboBox.SelectedItem;
        }

        public KeyValuePair<string, string> GetCurrency()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (CurrencyComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)CurrencyComboBox.SelectedItem;
        }

        public KeyValuePair<string, string> GetAssessmentDuration()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (AssessmentDurationComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)AssessmentDurationComboBox.SelectedItem;
        }

        public KeyValuePair<string, string> GetSelectedOptimizationPreference()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (mainFormObj.IsAvsBusinessProposalSelected())
                return new KeyValuePair<string, string>("MigrateToAvs", "AVSOnly");
            else if (!mainFormObj.IsAvsBusinessProposalSelected() && OptimizationPreferenceComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)OptimizationPreferenceComboBox.SelectedItem;
        }

        public bool IsAssessSqlServicesSeparatelyChecked()
        {
            return AssessSqlServicesSeparatelyCheckBox.Checked;
        }
        #endregion

        #region Mouse Hover Descriptions
        private void OptimizationPreferenceGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateOptimizationPreferenceDescription();
        }

        private void OptimizationPreferenceLabel_MouseHover(object sender, EventArgs e)
        {
            UpdateOptimizationPreferenceDescription();
        }

        private void OptimizationPreferenceComboBox_MouseHover(object sender, EventArgs e)
        {
            UpdateOptimizationPreferenceDescription();
        }

        private void OptimizationPreferenceInfoPictureBox_MouseHover(object sender, EventArgs e)
        {
            UpdateOptimizationPreferenceDescription();
        }

        private void AssessSqlServicesSeparatelyGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAssessSqlServicesSeparatelyDescription();
        }

        private void AssessSqlServicesSeparatelyCheckBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAssessSqlServicesSeparatelyDescription();
        }

        private void AssessSqlServicesSeparatelyInfoPictureBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAssessSqlServicesSeparatelyDescription();
        }

        private void UpdateOptimizationPreferenceDescription()
        {
            UpdateDescriptionTextBox("Migration Strategy", "Select the strategy for migrating workloads to Azure. The workloads are recommended for respective Azure Targets as per the logic below:\n\t1. Modernize to PaaS (PaaS Preferred) - Workloads that have SQL Server or web application running on them are first\n\t    assessed for Migration to respective PaaS targets; if the workload is not ready for migration to the respective PaaS target,\n\t    then the workload will be considered for migration to Azure VM.\n\t2. Migrate to IaaS only - Workloads are assessed for migration to Azure VM.");
        }

        private void UpdateAssessSqlServicesSeparatelyDescription()
        {
            UpdateDescriptionTextBox("Assess SQL Services Separately", "Select the checkbox if SQL Servers or Web App Servers containing SQL services such as SSAS, SSIS or SSRS need to be additionally assessed for migration to Azure VM when the SQL Server instances, or .NET Web Applications are recommended for migration to Azure SQL Managed Instance or Azure App Service respectively.\n\nFor example:\nA server running SQL Server instance also hosts SQL services such as SQL Server reporting service, and the SQL Server instance hosted on the server is ready for migration to Azure SQL Managed Instance.\nSelecting this option will ensure that the server is also considered for migration to Azure VM when the SQL Server instance is recommended for migration to Azure SQL Managed Instance. That will ensure that SQL services are migrated to Azure as well.\nIf this option is not selected, then the SQL Server instance running on the server will be recommended for migration to Azure SQL Managed Instance, and the SQL services will not be considered for migration to Azure.");
        }

        private void UpdateDescriptionTextBox(string descriptionHeader, string description)
        {
            AssessmentSettingsDescriptionGroupBox.Visible = true;
            AssessmentSettingsDescriptionGroupBox.Text = descriptionHeader;
            AssessmentSettingsDescriptionRichTextBox.Text = description;
        }
        #endregion

        #region visit links
        private void OptimizationPreferenceInfoPictureBox_Click(object sender, EventArgs e)
        {
            VisitLink("https://go.microsoft.com/fwlink/?linkid=2215652");
        }

        private void AssessSqlServicesSeparatelyInfoPictureBox_Click(object sender, EventArgs e)
        {
            VisitLink("https://go.microsoft.com/fwlink/?linkid=2215755");
        }

        private void VisitLink(string goLinkUrl)
        {
            /* Free golinks
             * https://go.microsoft.com/fwlink/?linkid=2215754
            */

            ProcessStartInfo processDescription = new ProcessStartInfo(goLinkUrl)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processDescription);
        }
        #endregion

        #region Utilities
        public void DisableOptimizationPreferenceComboBox()
        {
            OptimizationPreferenceComboBox.Enabled = false;
        }

        public void EnableOptimizationPreferenceComboBox()
        {
            OptimizationPreferenceComboBox.Enabled = true;
        }
        #endregion
    }
}