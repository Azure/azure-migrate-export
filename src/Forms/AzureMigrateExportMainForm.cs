using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using Azure.Migrate.Export.Authentication;
using Azure.Migrate.Export.Common;
using Azure.Migrate.Export.Models;

namespace Azure.Migrate.Export.Forms
{
    public partial class AzureMigrateExportMainForm : Form
    {
        private ProjectDetailsForm ProjectDetailsFormObj;
        private ConfigurationForm ConfigurationFormObj;
        private AssessmentSettingsForm AssessmentSettingsFormObj;
        private TrackProgressForm TrackProgressFormObj;

        private Button CurrentButtonTab = null;
        private Form CurrentlyActiveForm = null;

        public AzureMigrateExportMainForm()
        {
            InitializeComponent();
            ProjectDetailsFormObj = new ProjectDetailsForm(this);

            ConfigurationFormObj = new ConfigurationForm(this);
            ConfigurationFormObj.SetDefaultConfigurationValues();

            AssessmentSettingsFormObj = new AssessmentSettingsForm(this);
            AssessmentSettingsFormObj.Initialize();

            TrackProgressFormObj = new TrackProgressForm(this);

            // Display the first tab -> Project details
            HandleTabChange(ProjectDetailsFormObj, ProjectDetailsTabButton);

            // Show verison on UI
            VersionLabel.Text = "v " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            BeginAzureAuthentication();
        }

        #region Azure Authentication
        public async void BeginAzureAuthentication()
        {
            if (ProjectDetailsFormObj == null)
            {
                MessageBox.Show("Azure authentication failed. Please close the application and re-login.");
                CloseForm();
            }

            await ProjectDetailsFormObj.InitializeAuthentication();
        }
        #endregion

        #region Action Button click
        private void NextButton_Click(object sender, EventArgs e)
        {
            if (CurrentButtonTab == ProjectDetailsTabButton)
                HandleTabChange(ConfigurationFormObj, ConfigurationTabButton);
            else if (CurrentButtonTab == ConfigurationTabButton)
                HandleTabChange(AssessmentSettingsFormObj, AssessmentSettingsTabButton);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (CurrentButtonTab == ConfigurationTabButton)
                HandleTabChange(ProjectDetailsFormObj, ProjectDetailsTabButton);
            else if (CurrentButtonTab == AssessmentSettingsTabButton)
                HandleTabChange(ConfigurationFormObj, ConfigurationTabButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Retrieve project details
            string tenantId = ProjectDetailsFormObj.GetTenantId();
            KeyValuePair<string, string> subscription = ProjectDetailsFormObj.GetSelectedSubscription();
            KeyValuePair<string, string> resourceGroup = ProjectDetailsFormObj.GetSelectedResourceGroupName();
            KeyValuePair<string, string> azureMigrateProject = ProjectDetailsFormObj.GetSelectedAzureMigrateProject();
            string discoverySiteName = ProjectDetailsFormObj.GetDiscoverySiteName();
            string assessmentProjectName = ProjectDetailsFormObj.GetAssessmentProjectName();

            // Retrieve configuration
            List<string> azureMigrateSourceAppliances = ConfigurationFormObj.GetAzureMigrateSourceAppliances();
            bool isExpressWorkflow = ConfigurationFormObj.IsExpressWorkflowSelected();
            string businessProposal = ConfigurationFormObj.GetBusinessProposal();
            string module = ConfigurationFormObj.GetModule();

            // Retrieve assessment settings
            KeyValuePair<string, string> targetRegion = AssessmentSettingsFormObj.GetTargetRegion();
            KeyValuePair<string, string> currency = AssessmentSettingsFormObj.GetCurrency();
            KeyValuePair<string, string> assessmentDuration = AssessmentSettingsFormObj.GetAssessmentDuration();
            KeyValuePair<string, string> optimizationPreference = AssessmentSettingsFormObj.GetSelectedOptimizationPreference();
            bool assessSqlServicesSeparately = AssessmentSettingsFormObj.IsAssessSqlServicesSeparatelyChecked();

            UserInput userInputObj = new UserInput(
                tenantId,
                subscription,
                resourceGroup,
                azureMigrateProject,
                discoverySiteName,
                assessmentProjectName,
                azureMigrateSourceAppliances,
                isExpressWorkflow,
                businessProposal,
                module,
                targetRegion,
                currency,
                assessmentDuration,
                optimizationPreference,
                assessSqlServicesSeparately
            );
            HandleTabChange(TrackProgressFormObj, TrackProgressTabButton);
            TrackProgressFormObj.BeginProcess(userInputObj);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (TrackProgressFormObj.IsProcessCancellationRequested())
            {
                MessageBox.Show("Process cancellation request has been received.\nPlease wait until we terminate all processes.", "Azure Migrate Export");
                return;
            }

            DialogResult messageBoxResult = MessageBox.Show("Process has not finished, are you sure you want to stop? Please be patient as the process will stop at the next stable state once you select 'Yes'.", "Azure Migrate Export", MessageBoxButtons.YesNo);
            if (messageBoxResult != DialogResult.Yes)
                return;

            TrackProgressFormObj.CancelProcess();
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            HandleTabChange(ProjectDetailsFormObj, ProjectDetailsTabButton);
        }

        private void AssessButton_Click(object sender, EventArgs e)
        {
            ConfigurationFormObj.SetModule("Assessment");
            HandleTabChange(AssessmentSettingsFormObj, AssessmentSettingsTabButton);
        }
        #endregion

        #region Tab Button Click
        private void ProjectDetailsTabButton_Click(object sender, EventArgs e)
        {
            HandleTabChange(ProjectDetailsFormObj, ProjectDetailsTabButton);
        }

        private void ConfigurationTabButton_Click(object sender, EventArgs e)
        {
            HandleTabChange(ConfigurationFormObj, ConfigurationTabButton);
        }

        private void AssessmentSettingsTabButton_Click(object sender, EventArgs e)
        {
            HandleTabChange(AssessmentSettingsFormObj, AssessmentSettingsTabButton);
        }

        private void TrackProgressTabButton_Click(object sender, EventArgs e)
        {
            HandleTabChange(TrackProgressFormObj, TrackProgressTabButton);
        }
        #endregion

        #region Tab Change Handler
        private void HandleTabChange(Form formObjToActivate, Button nextButtonTab)
        {
            if (CurrentButtonTab != null)
                CurrentButtonTab.BackColor = Color.FromArgb(255, 255, 255);

            DecideVisibleActionButtons(nextButtonTab);
            DecideEnabledActionButtons(nextButtonTab);
            DecideVisibleTabButtons();
            DecideEnabledTabButtons(nextButtonTab);

            if (nextButtonTab != null)
            {
                nextButtonTab.BackColor = Color.FromArgb(120, 190, 255);
                CurrentButtonTab = nextButtonTab;
            }

            OpenChildForm(formObjToActivate);
        }

        private void OpenChildForm(Form formObjToActivate)
        {
            if (formObjToActivate == null)
            {
                MessageBox.Show("Failed to load the form. Please re-login");
                this.Close();
            }

            CurrentlyActiveForm = formObjToActivate;
            formObjToActivate.TopLevel = false;
            formObjToActivate.FormBorderStyle = FormBorderStyle.None;
            formObjToActivate.Dock = DockStyle.Fill;
            FillDockChildFormPanel.Controls.Add(formObjToActivate);
            FillDockChildFormPanel.Tag = formObjToActivate;
            formObjToActivate.BringToFront();
            formObjToActivate.Show();
        }
        #endregion

        #region Action Button Handler
        private void DecideVisibleActionButtons(Button nextButtonTab)
        {
            if (nextButtonTab == ProjectDetailsTabButton)
            {
                HideBackButton();
                ShowNextButton();
                HideStopButton();
                HideAssessButton();
                HideSubmitButton();
                HideRetryButton();
            }
            else if (nextButtonTab == ConfigurationTabButton)
            {
                ShowBackButton();
                HideStopButton();
                HideAssessButton();
      
                if (ConfigurationFormObj.DisplaySubmitButton())
                {
                    ShowSubmitButton();
                    HideNextButton();
                }
                else
                {
                    ShowNextButton();
                    HideSubmitButton();
                }

                HideRetryButton();
            }
            else if (nextButtonTab == AssessmentSettingsTabButton)
            {
                ShowBackButton();
                HideRetryButton();
                HideAssessButton();

                ShowSubmitButton();
                HideStopButton();
                HideNextButton();
            }
            else if (nextButtonTab == TrackProgressTabButton)
            {
                HideRetryButton();
                HideBackButton();
                HideAssessButton();

                ShowStopButton();
                HideNextButton();
                HideSubmitButton();
            }
        }

        private void DecideEnabledActionButtons(Button nextButtonTab)
        {
            if (nextButtonTab == ProjectDetailsTabButton)
            {
                MakeProjectDetailsActionButtonEnableDecision();
            }

            else if (nextButtonTab == ConfigurationTabButton)
            {
                MakeConfigurationActionButtonsEnabledDecision();
            }
            else if (nextButtonTab == AssessmentSettingsTabButton)
            {
                MakeAssessmentSettingsActionButtonsEnabledDecision();
            }
            else if (nextButtonTab == TrackProgressTabButton)
            {
                MakeTrackProgressActionButtonsEnabledDecisions();
            }
        }
        #endregion

        #region Tab Button Handler
        public void DecideVisibleTabButtons()
        {
            ShowProjectDetailsTabButton();
            ShowConfigurationTabButton();
            if (ConfigurationFormObj.DisplaySubmitButton())
                HideAssessmentSettingsTabButton();
            else
                ShowAssessmentSettingsTabButton();
            ShowTrackProgressTabButton();
        }
        private void DecideEnabledTabButtons(Button nextButtonTab)
        {
            if (nextButtonTab == ProjectDetailsTabButton)
            {
                MakeProjectDetailsTabButtonEnableDecision();
                /*
                bool displaySubmitButton = ConfigurationFormObj.DisplaySubmitButton();
                if (ProjectDetailsFormObj == null || !ProjectDetailsFormObj.ValidateProjectDetails())
                {
                    DisableConfigurationTabButton();
                    if (displaySubmitButton)
                        HideAssessmentSettingsTabButton();
                    else
                        ShowAssessmentSettingsTabButton();
                    DisableAssessmentSettingsTabButton();
                    DisableTrackProgressTabButton();
                    return;
                }

                EnableConfigurationTabButton();

                if (ConfigurationFormObj == null || !ConfigurationFormObj.ValidateConfiguration())
                {
                    if (displaySubmitButton)
                        HideAssessmentSettingsTabButton();
                    else
                        ShowAssessmentSettingsTabButton();
                    DisableAssessmentSettingsTabButton();
                    DisableTrackProgressTabButton();
                    return;
                }

                if (displaySubmitButton)
                {
                    HideAssessmentSettingsTabButton();
                    EnableTrackProgressTabButton();
                    return;
                }

                ShowAssessmentSettingsTabButton();
                EnableAssessmentSettingsTabButton();

                if (AssessmentSettingsFormObj == null || !AssessmentSettingsFormObj.ValidateAssessmentSettings())
                {
                    DisableTrackProgressTabButton();
                    return;
                }

                EnableTrackProgressTabButton();
                */
            }

            else if (nextButtonTab == ConfigurationTabButton)
            {
                MakeConfigurationTabButtonEnableDecisions();
                /*
                bool displaySubmitButton = ConfigurationFormObj.DisplaySubmitButton();
                if (ConfigurationFormObj == null || !ConfigurationFormObj.ValidateConfiguration())
                {
                    if (displaySubmitButton)
                        HideAssessmentSettingsTabButton();
                    else
                        ShowAssessmentSettingsTabButton();
                    DisableAssessmentSettingsTabButton();
                    DisableTrackProgressTabButton();
                    return;
                }

                if (displaySubmitButton)
                {
                    HideAssessmentSettingsTabButton();
                    EnableTrackProgressTabButton();
                    return;
                }

                ShowAssessmentSettingsTabButton();
                EnableAssessmentSettingsTabButton();

                if (AssessmentSettingsFormObj == null || !AssessmentSettingsFormObj.ValidateAssessmentSettings())
                {
                    DisableTrackProgressTabButton();
                    return;
                }

                EnableTrackProgressTabButton();
                */
            }
            else if (nextButtonTab == AssessmentSettingsTabButton)
            {
                MakeAssessmentSettingsTabButtonEnableDecisions();
            }
            else if (nextButtonTab == TrackProgressTabButton)
            {
                MakeTrackProgressTabButtonEnableDecisions();
            }
        }
        #endregion

        #region Form Closing
        public void CloseForm()
        {
            this.Close();
        }

        private void BottomDockCloseButton_Click(object sender, EventArgs e)
        {
            this.CloseForm();
        }

        private async void AzureMigrateExportMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TrackProgressFormObj.IsBackGroundWorkerRunning())
            {
                DialogResult messageBoxResult = MessageBox.Show("Process has not finished, are you sure you want to close application?", "Azure Migrate Export", MessageBoxButtons.YesNo);
                if (messageBoxResult != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                TrackProgressFormObj.CancelProcess();
            }

            try
            {
                await AzureAuthenticationHandler.Logout();
            }
            catch (Exception exLogout)
            {
                MessageBox.Show($"Failed to logout. Please delete the cached token file. Error: {exLogout.Message}");
            }
        }
        #endregion

        #region Enable/Disable/Hide/Show Action Buttons
        public void EnableNextButton()
        {
            NextButton.EnableActionButton();
        }

        public void DisableNextButton()
        {
            NextButton.DisableActionButton();
        }

        public void EnableSubmitButton()
        {
            SubmitButton.EnableActionButton();
        }

        public void DisableSubmitButton()
        {
            SubmitButton.DisableActionButton();
        }

        public void EnableBackButton()
        {
            BackButton.EnableActionButton();
        }

        public void DisableBackButton()
        {
            BackButton.DisableActionButton();
        }

        public void EnableStopButton()
        {
            StopButton.EnableActionButton();
        }

        public void DisableStopButton()
        {
            StopButton.DisableActionButton();
        }

        public void EnableRetryButton()
        {
            RetryButton.EnableActionButton();
        }

        public void DisableRetryButton()
        {
            RetryButton.DisableActionButton();
        }

        public void EnableAssessButton()
        {
            AssessButton.EnableActionButton();
        }

        public void DisableAssessButton()
        {
            AssessButton.DisableActionButton();
        }

        public void HideAssessButton()
        {
            AssessButton.Visible = false;
        }

        public void ShowAssessButton()
        {
            AssessButton.Visible = true;
        }

        public void HideNextButton()
        {
            NextButton.Visible = false;
        }

        public void ShowNextButton()
        {
            NextButton.Visible = true;
        }

        public void HideSubmitButton()
        {
            SubmitButton.Visible = false;
        }

        public void ShowSubmitButton()
        {
            SubmitButton.Visible = true;
        }

        public void ShowBackButton()
        {
            BackButton.Visible = true;
        }

        public void HideBackButton()
        {
            BackButton.Visible = false;
        }

        public void ShowRetryButton()
        {
            RetryButton.Visible = true;
        }

        public void HideRetryButton()
        {
            RetryButton.Visible = false;
        }

        public void ShowStopButton()
        {
            StopButton.Visible = true;
        }

        public void HideStopButton()
        {
            StopButton.Visible = false;
        }
        #endregion

        #region Enable/Disable/Hide/Show Tab Buttons
        public void EnableProjectDetailsTabButton()
        {
            ProjectDetailsTabButton.EnableTabButton();
        }

        public void DisableProjectDetailsTabButton()
        {
            ProjectDetailsTabButton.DisableTabButton();
        }

        public void EnableConfigurationTabButton()
        {
            ConfigurationTabButton.EnableTabButton();
        }

        public void DisableConfigurationTabButton()
        {
            ConfigurationTabButton.DisableTabButton();
        }

        public void EnableAssessmentSettingsTabButton()
        {
            AssessmentSettingsTabButton.EnableTabButton();
        }

        public void DisableAssessmentSettingsTabButton()
        {
            AssessmentSettingsTabButton.DisableTabButton();
        }

        public void EnableTrackProgressTabButton()
        {
            TrackProgressTabButton.EnableTabButton();
        }

        public void DisableTrackProgressTabButton()
        {
            TrackProgressTabButton.DisableTabButton();
        }

        public void ShowProjectDetailsTabButton()
        {
            ProjectDetailsTabButton.Visible = true;
        }

        public void HideProjectDetailsTabButton()
        {
            ProjectDetailsTabButton.Visible = false;
        }

        public void ShowConfigurationTabButton()
        {
            ConfigurationTabButton.Visible = true;
        }

        public void HideConfigurationTabButton()
        {
            ConfigurationTabButton.Visible = false;
        }

        public void ShowAssessmentSettingsTabButton()
        {
            AssessmentSettingsTabButton.Visible = true;
        }

        public void HideAssessmentSettingsTabButton()
        {
            AssessmentSettingsTabButton.Visible = false;
        }

        public void ShowTrackProgressTabButton()
        {
            TrackProgressTabButton.Visible = true;
        }

        public void HideTrackProgressTabButton()
        {
            TrackProgressTabButton.Visible = false;
        }
        #endregion

        #region Tab specific action button Enable/Disable/Hide/Show decision makers
        public void MakeProjectDetailsActionButtonEnableDecision()
        {
            HideBackButton();
            HideRetryButton();
            HideAssessButton();

            if (ProjectDetailsFormObj.ValidateProjectDetails())
            {
                ShowNextButton();
                EnableNextButton();

                HideSubmitButton();
                HideStopButton();
            }
            else
            {
                ShowNextButton();
                DisableNextButton();

                HideSubmitButton();
                HideStopButton();
            }
        }

        public void MakeConfigurationActionButtonsEnabledDecision()
        {
            ShowBackButton();
            EnableBackButton();
            HideRetryButton();
            HideAssessButton();

            if (ConfigurationFormObj.ValidateConfiguration())
            {
                if (ConfigurationFormObj.DisplaySubmitButton())
                {
                    ShowSubmitButton();
                    if (!TrackProgressFormObj.IsBackGroundWorkerRunning())
                        EnableSubmitButton();
                    else
                        DisableSubmitButton();

                    HideNextButton();
                    HideStopButton();
                }
                else
                {
                    ShowNextButton();
                    EnableNextButton();

                    HideStopButton();
                    HideSubmitButton();
                }
            }
            else
            {
                if (ConfigurationFormObj.DisplaySubmitButton())
                {
                    ShowSubmitButton();
                    DisableSubmitButton();

                    HideStopButton();
                    HideNextButton();
                }
                else
                {
                    ShowNextButton();
                    DisableNextButton();

                    HideStopButton();
                    HideSubmitButton();
                }
            }
        }

        public void MakeAssessmentSettingsActionButtonsEnabledDecision()
        {
            ShowBackButton();
            EnableBackButton();
            HideRetryButton();
            HideAssessButton();

            ShowSubmitButton();
            HideNextButton();
            HideStopButton();

            if (AssessmentSettingsFormObj.ValidateAssessmentSettings() && !TrackProgressFormObj.IsBackGroundWorkerRunning())
                EnableSubmitButton();
            else
                DisableSubmitButton();
        }

        public void MakeTrackProgressActionButtonsEnabledDecisions(bool showAndEnableAssessButton = false, bool showAndEnableRetryButton = false)
        {
            HideStopButton();
            HideNextButton();
            HideSubmitButton();
            HideRetryButton();
            HideBackButton();
            HideAssessButton();

            if (TrackProgressFormObj.DisplayActionButtonDecision() == 1)
            {
                ShowAssessButton();
                EnableAssessButton();

                DisableRetryButton();
                HideRetryButton();
            }
            else if (TrackProgressFormObj.DisplayActionButtonDecision() == 2)
            {
                ShowRetryButton();
                EnableRetryButton();

                DisableAssessButton();
                HideAssessButton();
            }
            else if (TrackProgressFormObj.DisplayActionButtonDecision() == 3)
            {
                DisableRetryButton();
                HideRetryButton();

                DisableAssessButton();
                HideAssessButton();
            }

            if (TrackProgressFormObj.IsBackGroundWorkerRunning())
            {
                ShowStopButton();
                EnableStopButton();
            }
            else
            {
                DisableStopButton();
                HideStopButton();
            }

            if (showAndEnableAssessButton)
            {
                ShowAssessButton();
                EnableAssessButton();
            }
            else if (showAndEnableRetryButton)
            {
                ShowRetryButton();
                EnableRetryButton();
            }
        }
        #endregion

        #region Tab specific tab button Enable/Disable/Hide/Show decision makers
        public void MakeProjectDetailsTabButtonEnableDecision()
        {
            bool displaySubmitButton = ConfigurationFormObj.DisplaySubmitButton();
            if (ProjectDetailsFormObj == null || !ProjectDetailsFormObj.ValidateProjectDetails())
            {
                DisableConfigurationTabButton();
                if (displaySubmitButton)
                    HideAssessmentSettingsTabButton();
                else
                    ShowAssessmentSettingsTabButton();
                DisableAssessmentSettingsTabButton();
                DisableTrackProgressTabButton();
                return;
            }

            EnableConfigurationTabButton();

            if (ConfigurationFormObj == null || !ConfigurationFormObj.ValidateConfiguration())
            {
                if (displaySubmitButton)
                    HideAssessmentSettingsTabButton();
                else
                    ShowAssessmentSettingsTabButton();
                DisableAssessmentSettingsTabButton();
                DisableTrackProgressTabButton();
                return;
            }

            if (displaySubmitButton)
            {
                HideAssessmentSettingsTabButton();
                EnableTrackProgressTabButton();
                return;
            }

            ShowAssessmentSettingsTabButton();
            EnableAssessmentSettingsTabButton();
            
            if(AssessmentSettingsFormObj == null || !AssessmentSettingsFormObj.ValidateAssessmentSettings())
            {
                DisableTrackProgressTabButton();
                return;
            }

            EnableTrackProgressTabButton();
        }

        public void MakeConfigurationTabButtonEnableDecisions()
        {
            bool displaySubmitButton = ConfigurationFormObj.DisplaySubmitButton();
            if (ConfigurationFormObj == null || !ConfigurationFormObj.ValidateConfiguration())
            {
                if (displaySubmitButton)
                    HideAssessmentSettingsTabButton();
                else
                    ShowAssessmentSettingsTabButton();
                DisableAssessmentSettingsTabButton();
                DisableTrackProgressTabButton();
                return;
            }

            if (displaySubmitButton)
            {
                HideAssessmentSettingsTabButton();
                EnableTrackProgressTabButton();
                return;
            }

            ShowAssessmentSettingsTabButton();
            EnableAssessmentSettingsTabButton();

            if (AssessmentSettingsFormObj == null || !AssessmentSettingsFormObj.ValidateAssessmentSettings())
            {
                DisableTrackProgressTabButton();
                return;
            }

            EnableTrackProgressTabButton();
        }

        public void MakeAssessmentSettingsTabButtonEnableDecisions()
        {
            if (AssessmentSettingsFormObj == null || !AssessmentSettingsFormObj.ValidateAssessmentSettings())
            {
                DisableTrackProgressTabButton();
                return;
            }

            EnableTrackProgressTabButton();
        }

        public void MakeTrackProgressTabButtonEnableDecisions()
        {
            // Previous state must be retained, enabled tab buttons do not change.
        }
        #endregion

        #region Utilities
        public bool IsAvsBusinessProposalSelected()
        {
            return (ConfigurationFormObj.GetBusinessProposal() == BusinessProposal.AVS.ToString());
        }

        public void DisableOptimizationPreferenceComboBox()
        {
            if (AssessmentSettingsFormObj == null)
                return;
            AssessmentSettingsFormObj.DisableOptimizationPreferenceComboBox();
        }

        public void EnableOptimizationPreferenceComboBox()
        {
            if (AssessmentSettingsFormObj == null)
                return;
            AssessmentSettingsFormObj.EnableOptimizationPreferenceComboBox();
        }
        #endregion
    }
}