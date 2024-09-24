using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Azure.Migrate.Export.Authentication;
using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.HttpRequestHelper;

namespace Azure.Migrate.Export.Forms
{
    public partial class ProjectDetailsForm : Form
    {
        private AzureMigrateExportMainForm mainFormObj;

        private string DiscoverySiteName = null;
        private string AssessmentProjectName = null;

        public ProjectDetailsForm(AzureMigrateExportMainForm obj)
        {
            InitializeComponent();
            mainFormObj = obj;
        }

        #region Authentication
        public async Task InitializeAuthentication()
        {
            AuthenticationResult authResult = null;
            try
            {
                authResult = await AzureAuthenticationHandler.CommonLogin();
            }
            catch (Exception exCommonLogin)
            {
                MessageBox.Show($"Login failed. Please close the application and re-try: {exCommonLogin.Message}");
                mainFormObj.CloseForm();
            }

            TenantIdTextBox.Text = authResult.TenantId;
            ConfirmTenantIdChangeButton.DisableActionButton();

            await InitializeSubscriptionComboBox();
        }
        #endregion

        #region Initialization
        private async Task InitializeSubscriptionComboBox()
        {
            SubscriptionInfoLabel.Text = "";
            SubscriptionComboBox.Text = "Loading...";
            SubscriptionComboBox.Enabled = false;
            List<KeyValuePair<string, string>> Subscriptions = new List<KeyValuePair<string, string>>();

            HttpClientHelper httpClientHelperObj = new HttpClientHelper();

            string nextLink = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash + 
                              Routes.SubscriptionPath + 
                              Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.ProjectDetailsApiVersion;

            try
            {
                while (!string.IsNullOrEmpty(nextLink))
                {
                    List<KeyValuePair<string, string>> partialSubscriptionsList = new List<KeyValuePair<string, string>>();

                    JToken jsonTokenResponse = await httpClientHelperObj.GetProjectDetailsHttpJsonResponse(nextLink);
                    if (jsonTokenResponse.HasValues)
                    {
                        foreach (var x in jsonTokenResponse["value"])
                            partialSubscriptionsList.Add(new KeyValuePair<string, string>(x["subscriptionId"].Value<string>(), x["displayName"].Value<string>() + " - " + x["subscriptionId"].Value<string>()));
                        Subscriptions.AddRange(partialSubscriptionsList);

                        if (jsonTokenResponse["nextLink"] != null)
                            nextLink = jsonTokenResponse["nextLink"].Value<string>();
                        else
                            nextLink = null;
                    }
                }
            }
            catch (Exception exSubscriptions)
            {
                MessageBox.Show($"Could not retrieve Subscriptions data: {exSubscriptions.Message} Please re-login.");
                mainFormObj.CloseForm();
            }

            try
            {
                Subscriptions.Sort(CompareValue);
                if (Subscriptions.Count <= 0)
                    SubscriptionInfoLabel.Text = "No active subscriptions found. Please change the Tenant ID.";
                SubscriptionComboBox.DataSource = Subscriptions;
                SubscriptionComboBox.ValueMember = "Key";
                SubscriptionComboBox.DisplayMember = "Value";
                SubscriptionComboBox.SelectedItem = null;
            }
            catch (Exception exSubscriptionDataHandling)
            {
                MessageBox.Show($"Error handling subscription data: {exSubscriptionDataHandling.Message} Please log an issue.");
                mainFormObj.CloseForm();
            }

            SubscriptionComboBox.Enabled = true;
            if (Subscriptions.Count > 0)
                SubscriptionComboBox.Text = "Please select Subscription";
        }

        private async Task InitializeResourceGroupNameComboBox()
        {
            KeyValuePair<string, string> selectedSubscription = GetSelectedSubscription();
            if (string.IsNullOrEmpty(selectedSubscription.Key) || string.IsNullOrEmpty(selectedSubscription.Value))
            {
                MessageBox.Show("Empty Subscription ID or name. Please select Subscription again.");
                return;
            }
            ResourceGroupNameInfoLabel.Text = "";
            ResourceGroupNameComboBox.Text = "Loading...";
            ResourceGroupNameComboBox.Enabled = false;

            List<KeyValuePair<string, string>> ResourceGroups = new List<KeyValuePair<string, string>>();

            HttpClientHelper httpClientHelperObj = new HttpClientHelper();

            string nextLink = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                              Routes.SubscriptionPath + Routes.ForwardSlash + selectedSubscription.Key + Routes.ForwardSlash +
                              Routes.ResourceGroupPath +
                              Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.ProjectDetailsApiVersion;

            try
            {
                while (!string.IsNullOrEmpty(nextLink))
                {
                    List<KeyValuePair<string, string>> partialResourceGroupList = new List<KeyValuePair<string, string>>();

                    JToken jsonTokenResponse = await httpClientHelperObj.GetProjectDetailsHttpJsonResponse(nextLink);
                    if (jsonTokenResponse.HasValues)
                    {
                        foreach (var x in jsonTokenResponse["value"])
                            partialResourceGroupList.Add(new KeyValuePair<string, string>(x["id"].Value<string>(), x["name"].Value<string>()));
                        ResourceGroups.AddRange(partialResourceGroupList);

                        if (jsonTokenResponse["nextLink"] != null)
                            nextLink = jsonTokenResponse["nextLink"].Value<string>();
                        else
                            nextLink = null;
                    }
                }
            }
            catch (Exception exResourceGroups)
            {
                MessageBox.Show($"Could not retrieve Resource Group data: {exResourceGroups.Message} Please re-login");
                mainFormObj.CloseForm();
            }

            try
            {
                ResourceGroups.Sort(CompareValue);
                if (ResourceGroups.Count <= 0)
                    ResourceGroupNameInfoLabel.Text = "No Resource Groups found. Please select a different Subscription ID";
                ResourceGroupNameComboBox.DataSource = ResourceGroups;
                ResourceGroupNameComboBox.ValueMember = "Key";
                ResourceGroupNameComboBox.DisplayMember = "Value";
                ResourceGroupNameComboBox.SelectedItem = null;
            }
            catch (Exception exResourceGroupDataHandling)
            {
                MessageBox.Show($"Error handling Resource Group data: {exResourceGroupDataHandling.Message} Please log an issue.");
                mainFormObj.CloseForm();
            }

            ResourceGroupNameComboBox.Enabled = true;
            if (ResourceGroups.Count > 0)
                ResourceGroupNameComboBox.Text = "Please select Resource Group";
        }

        private async Task InitializeAzureMigrateProjectNameComboBox()
        {
            KeyValuePair<string, string> selectedSubscription = GetSelectedSubscription();
            KeyValuePair<string, string> selectedResourceGroup = GetSelectedResourceGroupName();

            if (string.IsNullOrEmpty(selectedSubscription.Key) || string.IsNullOrEmpty(selectedSubscription.Value))
            {
                MessageBox.Show("Empty Subscription ID or name. Please Select Subscription again.");
                return;
            }

            if (string.IsNullOrEmpty(selectedResourceGroup.Key) || string.IsNullOrEmpty(selectedResourceGroup.Value))
            {
                MessageBox.Show("Empty Resource Group ID or name. Please select Resource Group again.");
                return;
            }

            AzureMigrateProjectNameInfoLabel.Text = "";
            AzureMigrateProjectNameComboBox.Text = "Loading...";
            AzureMigrateProjectNameComboBox.Enabled = false;

            List<KeyValuePair<string, string>> AzureMigrateProjects = new List<KeyValuePair<string, string>>();

            HttpClientHelper httpClientHelperObj = new HttpClientHelper();

            string nextLink = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                              Routes.SubscriptionPath + Routes.ForwardSlash + selectedSubscription.Key + Routes.ForwardSlash +
                              Routes.ResourceGroupPath + Routes.ForwardSlash + selectedResourceGroup.Value + Routes.ForwardSlash +
                              Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                              Routes.MigrateProjectsPath +
                              Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.ProjectDetailsApiVersion;

            try
            {
                while (!string.IsNullOrEmpty(nextLink))
                {
                    List<KeyValuePair<string, string>> partialAzureMigrateProjectList = new List<KeyValuePair<string, string>>();

                    JToken jsonTokenResponse = await httpClientHelperObj.GetProjectDetailsHttpJsonResponse(nextLink);
                    if (jsonTokenResponse.HasValues)
                    {
                        foreach (var x in jsonTokenResponse["value"])
                            partialAzureMigrateProjectList.Add(new KeyValuePair<string, string>(x["id"].Value<string>(), x["name"].Value<string>()));
                        AzureMigrateProjects.AddRange(partialAzureMigrateProjectList);

                        if (jsonTokenResponse["nextLink"] != null)
                            nextLink = jsonTokenResponse["nextLink"].Value<string>();
                        else
                            nextLink = null;
                    }
                }
            }
            catch (Exception exAzureMigrateProjects)
            {
                MessageBox.Show($"Could not retrieve Azure Migrate projects data: {exAzureMigrateProjects.Message} Please re-login");
                mainFormObj.CloseForm();
            }

            try
            {
                AzureMigrateProjects.Sort(CompareValue);
                if (AzureMigrateProjects.Count <= 0)
                    AzureMigrateProjectNameInfoLabel.Text = "No Migrate projects found. Please select a different Resource group.";
                AzureMigrateProjectNameComboBox.DataSource = AzureMigrateProjects;
                AzureMigrateProjectNameComboBox.ValueMember = "Key";
                AzureMigrateProjectNameComboBox.DisplayMember = "Value";
                AzureMigrateProjectNameComboBox.SelectedItem = null;
            }
            catch (Exception exAzureMigrateProjectDataHandling)
            {
                MessageBox.Show($"Error handling Migrate project data: {exAzureMigrateProjectDataHandling.Message} Please log an issue.");
                mainFormObj.CloseForm();
            }

            AzureMigrateProjectNameComboBox.Enabled = true;
            if (AzureMigrateProjects.Count > 0)
                AzureMigrateProjectNameComboBox.Text = "Please select Migrate Project";
        }

        private async Task InitializeDiscoverySiteName()
        {
            KeyValuePair<string, string> selectedSubscription = GetSelectedSubscription();
            KeyValuePair<string, string> selectedResourceGroup = GetSelectedResourceGroupName();
            KeyValuePair<string, string> selectedAzureMigrateProject = GetSelectedAzureMigrateProject();

            if (string.IsNullOrEmpty(selectedSubscription.Key) || string.IsNullOrEmpty(selectedSubscription.Value))
            {
                MessageBox.Show("Empty Subscription ID or name. Please Select Subscription again.");
                return;
            }

            if (string.IsNullOrEmpty(selectedResourceGroup.Key) || string.IsNullOrEmpty(selectedResourceGroup.Value))
            {
                MessageBox.Show("Empty Resource Group ID or name. Please select Resource Group again.");
                return;
            }

            if (string.IsNullOrEmpty(selectedAzureMigrateProject.Key) || string.IsNullOrEmpty(selectedAzureMigrateProject.Value))
            {
                MessageBox.Show("Empty Azure Migrate project ID or name. Please select Migrate project again.");
                return;
            }

            DiscoverySiteNameInfoLabel.Text = "";
            HttpClientHelper httpClientHelperObj = new HttpClientHelper();

            string url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                         Routes.SubscriptionPath + Routes.ForwardSlash + selectedSubscription.Key + Routes.ForwardSlash +
                         Routes.ResourceGroupPath + Routes.ForwardSlash + selectedResourceGroup.Value + Routes.ForwardSlash +
                         Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                         Routes.MigrateProjectsPath + Routes.ForwardSlash + selectedAzureMigrateProject.Value + Routes.ForwardSlash +
                         Routes.SolutionsPath + Routes.ForwardSlash + Routes.ServerDiscoveryPath +
                         Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.ProjectDetailsApiVersion;

            try
            {
                JToken jsonTokenResponse = await httpClientHelperObj.GetProjectDetailsHttpJsonResponse(url);
                if (jsonTokenResponse.HasValues)
                {
                    string masterSiteId = jsonTokenResponse["properties"]["details"]["extendedDetails"]["masterSiteId"].Value<string>();
                    var masterSiteIdContents = masterSiteId.Split('/').ToList();
                    if (masterSiteIdContents.Count > 0)
                    {
                        DiscoverySiteName = masterSiteIdContents[masterSiteIdContents.Count - 1];
                        DiscoverySiteNameInfoLabel.Text = DiscoverySiteName;
                    }
                    else
                        throw new Exception("masterSiteId response in json does not contain enough elements");
                }
            }
            catch (Exception exDiscoverySiteName)
            {
                MessageBox.Show($"Could not obtain discovery site name: {exDiscoverySiteName.Message} Please re-login.");
                mainFormObj.CloseForm();
            }
        }

        private async Task InitializeAssessmentProjectName()
        {
            KeyValuePair<string, string> selectedSubscription = GetSelectedSubscription();
            KeyValuePair<string, string> selectedResourceGroup = GetSelectedResourceGroupName();
            KeyValuePair<string, string> selectedAzureMigrateProject = GetSelectedAzureMigrateProject();

            if (string.IsNullOrEmpty(selectedSubscription.Key) || string.IsNullOrEmpty(selectedSubscription.Value))
            {
                MessageBox.Show("Empty Subscription ID or name. Please Select Subscription again.");
                return;
            }

            if (string.IsNullOrEmpty(selectedResourceGroup.Key) || string.IsNullOrEmpty(selectedResourceGroup.Value))
            {
                MessageBox.Show("Empty Resource Group ID or name. Please select Resource Group again.");
                return;
            }

            if (string.IsNullOrEmpty(selectedAzureMigrateProject.Key) || string.IsNullOrEmpty(selectedAzureMigrateProject.Value))
            {
                MessageBox.Show("Empty Azure Migrate project ID or name. Please select Migrate project again.");
                return;
            }

            AssessmentProjectNameInfoLabel.Text = "";
            HttpClientHelper httpClientHelperObj = new HttpClientHelper();

            string url = Routes.ProtocolScheme + Routes.AzureManagementApiHostname + Routes.ForwardSlash +
                         Routes.SubscriptionPath + Routes.ForwardSlash + selectedSubscription.Key + Routes.ForwardSlash +
                         Routes.ResourceGroupPath + Routes.ForwardSlash + selectedResourceGroup.Value + Routes.ForwardSlash +
                         Routes.ProvidersPath + Routes.ForwardSlash + Routes.MigrateProvidersPath + Routes.ForwardSlash +
                         Routes.MigrateProjectsPath + Routes.ForwardSlash + selectedAzureMigrateProject.Value + Routes.ForwardSlash +
                         Routes.SolutionsPath + Routes.ForwardSlash + Routes.ServerAssessmentPath +
                         Routes.QueryStringQuestionMark + Routes.QueryParameterApiVersion + Routes.QueryStringEquals + Routes.ProjectDetailsApiVersion;

            try
            {
                JToken jsonTokenResponse = await httpClientHelperObj.GetProjectDetailsHttpJsonResponse(url);
                if (jsonTokenResponse.HasValues)
                {
                    string projectId = jsonTokenResponse["properties"]["details"]["extendedDetails"]["projectId"].Value<string>();
                    var projectIdContents = projectId.Split('/').ToList();
                    if (projectIdContents.Count > 0)
                    {
                        AssessmentProjectName = projectIdContents[projectIdContents.Count - 1];
                        AssessmentProjectNameInfoLabel.Text = AssessmentProjectName;
                    }
                    else
                        throw new Exception("projectId response in json does not contain enough elements");
                }
            }
            catch (Exception exAssessmentProjectName)
            {
                MessageBox.Show($"Could not obtain assessment project name: {exAssessmentProjectName.Message} Please re-login.");
                mainFormObj.CloseForm();
            }
        }
        #endregion

        #region Utilities
        private static int CompareKey(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private static int CompareValue(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Value.CompareTo(b.Value);
        }
        #endregion

        #region Hover Descriptions
        private void AzureMigrateProjectDetailsGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void TenantIdLabel_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void SubscriptionNameLabel_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void ResourceGroupNameLabel_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void AzureMigrateProjectNameLabel_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void TenantIdTextBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void ConfirmTenantIdChangeButton_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void SubscriptionComboBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void ResourceGroupNameComboBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void AzureMigrateProjectNameComboBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateProjectDetails();
        }

        private void UpdateAzureMigrateProjectDetails()
        {
            UpdateDescriptionTextBox("Azure Migrate Project details", "Select the Subscription and Resource Group of the Azure Migrate Project that was used to discover the datacenter.\n\nBefore you begin, please ensure the following:\n\t1. You have created an Azure Migrate Project.\n\t2. You have contributor access to the Resource Group where the Azure Migrate Project resides.\n\t3. You have completed the discovery of your environment by deploying an Azure Migrate appliance or the discovery of inventory hosted \ton VMWare using RVTools import.");
        }

        private void UpdateDescriptionTextBox(string descriptionHeader, string description)
        {
            ProjectDetailsDescriptionGroupBox.Visible = true;
            ProjectDetailsDescriptionGroupBox.Text = descriptionHeader;
            ProjectDetailsDescriptionRichTextBox.Text = description;
        }
        #endregion

        #region Tenant ID Change Handler
        private async void TenantIdTextBox_TextChanged(object sender, EventArgs e)
        {
            AuthenticationResult authResult = null;
            try
            {
                authResult = await AzureAuthenticationHandler.RetrieveAuthenticationToken();
            }
            catch (Exception exRetrieveAzureAuthenticationToken)
            {
                MessageBox.Show($"Failed to retrieve authentication token. Please close the application and re-login: {exRetrieveAzureAuthenticationToken}");
                return;
            }

            if (string.IsNullOrEmpty(TenantIdTextBox.Text) || !ValidateTenantIdTextBox() || TenantIdTextBox.Text.Equals(authResult.TenantId))
                ConfirmTenantIdChangeButton.DisableActionButton();
            else
                ConfirmTenantIdChangeButton.EnableActionButton();
        }

        private async void ConfirmTenantIdChangeButton_Click(object sender, EventArgs e)
        {
            AuthenticationResult authResult = null;

            TenantIdInfoLabel.Text = "";

            SubscriptionComboBox.SelectedItem = null;
            SubscriptionComboBox.DataSource = null;
            SubscriptionInfoLabel.Text = "";

            ResourceGroupNameComboBox.SelectedItem = null;
            ResourceGroupNameComboBox.DataSource = null;
            ResourceGroupNameInfoLabel.Text = "";

            AzureMigrateProjectNameComboBox.SelectedItem = null;
            AzureMigrateProjectNameComboBox.DataSource = null;
            AzureMigrateProjectNameInfoLabel.Text = "";

            DiscoverySiteName = null;
            DiscoverySiteNameInfoLabel.Text = "";

            AssessmentProjectName = null;
            AssessmentProjectNameInfoLabel.Text = "";

            mainFormObj.MakeProjectDetailsActionButtonEnableDecision();
            mainFormObj.MakeProjectDetailsTabButtonEnableDecision();

            try
            {
                await AzureAuthenticationHandler.Logout();
            }
            catch (Exception exLogout)
            {
                MessageBox.Show($"Logout failed: {exLogout.Message} Please re-login.");
                mainFormObj.CloseForm();
            }

            try
            {
                authResult = await AzureAuthenticationHandler.TenantLogin(TenantIdTextBox.Text);
            }
            catch (Exception exTenantLogin)
            {
                MessageBox.Show($"Tenant login failed: {exTenantLogin.Message} Please restart application");
                mainFormObj.Close();
            }

            if (!authResult.TenantId.Equals(TenantIdTextBox.Text))
                TenantIdInfoLabel.Text = @"Entered and logged-in Tenant ID do not match";

            ConfirmTenantIdChangeButton.DisableActionButton();

            await InitializeSubscriptionComboBox();
        }
        #endregion

        #region ComboBox Mouse Click
        private void SubscriptionComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            SubscriptionComboBox.DroppedDown = true;
        }

        private void ResourceGroupNameComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            ResourceGroupNameComboBox.DroppedDown = true;
        }

        private void AzureMigrateProjectNameComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            AzureMigrateProjectNameComboBox.DroppedDown = true;
        }
        #endregion

        #region ComboBox Selection Change Committed
        private async void SubscriptionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ResourceGroupNameComboBox.SelectedItem = null;
            ResourceGroupNameComboBox.DataSource = null;
            ResourceGroupNameInfoLabel.Text = "";

            AzureMigrateProjectNameComboBox.SelectedItem = null;
            AzureMigrateProjectNameComboBox.DataSource = null;
            AzureMigrateProjectNameInfoLabel.Text = "";

            DiscoverySiteName = null;
            DiscoverySiteNameInfoLabel.Text = "";

            AssessmentProjectName = null;
            AssessmentProjectNameInfoLabel.Text = "";

            this.ActiveControl = null;

            mainFormObj.MakeProjectDetailsActionButtonEnableDecision();
            mainFormObj.MakeProjectDetailsTabButtonEnableDecision();

            await InitializeResourceGroupNameComboBox();
        }

        private async void ResourceGroupNameComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AzureMigrateProjectNameComboBox.SelectedItem = null;
            AzureMigrateProjectNameComboBox.DataSource = null;
            AzureMigrateProjectNameInfoLabel.Text = "";

            DiscoverySiteName = null;
            DiscoverySiteNameInfoLabel.Text = "";

            AssessmentProjectName = null;
            AssessmentProjectNameInfoLabel.Text = "";

            this.ActiveControl = null;

            mainFormObj.MakeProjectDetailsActionButtonEnableDecision();
            mainFormObj.MakeProjectDetailsTabButtonEnableDecision();

            await InitializeAzureMigrateProjectNameComboBox();
        }

        private async void AzureMigrateProjectNameComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DiscoverySiteName = null;
            DiscoverySiteNameInfoLabel.Text = "";

            AssessmentProjectName = null;
            AssessmentProjectNameInfoLabel.Text = "";

            this.ActiveControl = null;

            mainFormObj.MakeProjectDetailsActionButtonEnableDecision();
            mainFormObj.MakeProjectDetailsTabButtonEnableDecision();

            await InitializeDiscoverySiteName();
            await InitializeAssessmentProjectName();

            mainFormObj.MakeProjectDetailsActionButtonEnableDecision();
            mainFormObj.MakeProjectDetailsTabButtonEnableDecision();
        }
        #endregion

        #region Validation
        public bool ValidateProjectDetails()
        {
            if (!ValidateTenantIdTextBox())
                return false;

            if (!ValidateSubscriptionComboBox())
                return false;

            if (!ValidateResourceGroupNameComboBox())
                return false;

            if (!ValidateAzureMigrateProjectNameComboBox())
                return false;

            if (!ValidateDiscoverySiteName() && !ValidateAssessmentProjectName())
                return false;

            return true;
        }

        private bool ValidateTenantIdTextBox()
        {
            if (string.IsNullOrEmpty(TenantIdTextBox.Text))
                return false;

            else if (!Regex.IsMatch(TenantIdTextBox.Text, @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$"))
                return false;

            return true;
        }

        private bool ValidateSubscriptionComboBox()
        {
            if (SubscriptionComboBox.SelectedItem == null)
                return false;

            else if (!Regex.IsMatch(((KeyValuePair<string, string>)SubscriptionComboBox.SelectedItem).Key, @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$"))
                return false;

            return true;
        }

        private bool ValidateResourceGroupNameComboBox()
        {
            if (ResourceGroupNameComboBox.SelectedItem == null)
                return false;

            return true;
        }

        private bool ValidateAzureMigrateProjectNameComboBox()
        {
            if (AzureMigrateProjectNameComboBox.SelectedItem == null)
                return false;

            return true;
        }

        private bool ValidateDiscoverySiteName()
        {
            if (string.IsNullOrEmpty(DiscoverySiteName))
                return false;

            return true;
        }

        private bool ValidateAssessmentProjectName()
        {
            if (string.IsNullOrEmpty(AssessmentProjectName))
                return false;

            return true;
        }
        #endregion

        #region Getter Methods
        public string GetTenantId()
        {
            return TenantIdTextBox.Text;
        }

        public KeyValuePair<string, string> GetSelectedSubscription()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (SubscriptionComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)SubscriptionComboBox.SelectedItem;
        }

        public KeyValuePair<string, string> GetSelectedResourceGroupName()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (ResourceGroupNameComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)ResourceGroupNameComboBox.SelectedItem;
        }

        public KeyValuePair<string, string> GetSelectedAzureMigrateProject()
        {
            KeyValuePair<string, string> empty = new KeyValuePair<string, string>("", "");
            if (AzureMigrateProjectNameComboBox.SelectedItem == null)
                return empty;

            return (KeyValuePair<string, string>)AzureMigrateProjectNameComboBox.SelectedItem;
        }

        public string GetDiscoverySiteName()
        {
            return DiscoverySiteName;
        }

        public string GetAssessmentProjectName()
        {
            return AssessmentProjectName;
        }
        #endregion
    }
}
