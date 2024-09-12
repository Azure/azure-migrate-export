using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Azure.Migrate.Export.Common;

namespace Azure.Migrate.Export.Forms
{
    public partial class ConfigurationForm : Form
    {
        private AzureMigrateExportMainForm mainFormObj;

        public ConfigurationForm(AzureMigrateExportMainForm obj)
        {
            InitializeComponent();
            mainFormObj = obj;

            // Set the DropDownStyle to DropDownList to disable text entry
            ModuleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #region Set Default Values
        public void SetDefaultConfigurationValues()
        {
            ApplianceRadioButton.Checked = true;
            VMwareCheckBox.Checked = true;
            HyperVCheckBox.Checked = true;
            PhysicalCheckBox.Checked = true;

            ExpressWorkflowRadioButton.Checked = true;
            ComprehensiveProposalRadioButton.Checked = true;
        }
        #endregion

        #region Radio Buttons Checked Changed
        private void CustomWorkflowRadioButton_CheckedChanged(object sender, EventArgs e)
        {            
            if (CustomWorkflowRadioButton.Checked)
            {
                ModuleComboBox.Visible = true;
                string selectedModule = (string)ModuleComboBox.SelectedItem;
                if (selectedModule != null && selectedModule.Equals("Assessment"))
                {
                    EnableBusinessProposal();
                    if (ImportRadioButton.Checked)
                    {
                        CheckOnlyQuickAvsProposal();
                    }
                }
                else
                {
                    DisableBusinessProposal();
                }
            }

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }

        private void ExpressWorkflowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ExpressWorkflowRadioButton.Checked)
            {
                ModuleComboBox.Visible = false;
                EnableBusinessProposal();
                if (ImportRadioButton.Checked)
                {
                    CheckOnlyQuickAvsProposal();
                }
            }            

            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
        }

        private void ComprehensiveProposalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ComprehensiveProposalRadioButton.Checked)
            {
                mainFormObj.EnableOptimizationPreferenceComboBox();
/*                Enable*/
            }

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }

        private void QuickAvsProposalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (QuickAvsProposalRadioButton.Checked)
            {
                mainFormObj.DisableOptimizationPreferenceComboBox();
                if (ApplianceRadioButton.Checked)
                {
                    DisableHypervAndPhysicalCheckBoxes();
                }
            }

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }
        #endregion

        #region Radio Buttons Clicked
        private void ImportRadioButton_Click(object sender, EventArgs e)
        {
            if (ImportRadioButton.Checked)
            {
                ApplianceRadioButton.Checked = false;
                VMwareCheckBox.Enabled = false;
                VMwareCheckBox.Checked = false;
                HyperVCheckBox.Enabled = false;
                HyperVCheckBox.Checked = false;
                PhysicalCheckBox.Enabled = false;
                PhysicalCheckBox.Checked = false;

                string selectedModule = (string)ModuleComboBox.SelectedItem;
                if (ExpressWorkflowRadioButton.Checked ||
                   (selectedModule != null && selectedModule.Equals("Assessment")))
                {
                    CheckOnlyQuickAvsProposal();
                }
                else
                {
                    DisableBusinessProposal();
                }
            }           

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }

        private void ApplianceRadioButton_Click(object sender, EventArgs e)
        {
            if (ApplianceRadioButton.Checked)
            {
                ImportRadioButton.Checked = false;
                VMwareCheckBox.Enabled = true;
                VMwareCheckBox.Checked = true;
                HyperVCheckBox.Enabled = true;
                HyperVCheckBox.Checked = true;
                PhysicalCheckBox.Enabled = true;
                PhysicalCheckBox.Checked = true;

                EnableBusinessProposal();
                if (QuickAvsProposalRadioButton.Checked)
                {
                    DisableHypervAndPhysicalCheckBoxes();
                }
            }            

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }
        #endregion

        #region CheckBox Checked Changed
        private void VMwareCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
        }

        private void HyperVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
        }

        private void PhysicalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
        }
        #endregion

        #region ComboBox Selection Change Committed
        private void ModuleComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            string selectedModule = (string)ModuleComboBox.SelectedItem;
            if (selectedModule.Equals("Assessment"))
            {
                if (!IsDiscoveryReportPresent())
                {
                    MessageBox.Show("No discovery report found. Please complete discovery before running assessments.", "Azure Migrate Export");
                    ModuleComboBox.SelectedItem = "Discovery";
                }
                else
                {
                    EnableBusinessProposal();
                    if (ImportRadioButton.Checked)
                    {
                        CheckOnlyQuickAvsProposal();
                    }
                }                   
            }
            else if (selectedModule.Equals("Discovery"))
            {
                DisableBusinessProposal();
            }

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }
        #endregion

        #region Validataion
        public bool ValidateConfiguration()
        {
            if (!ValidateAzureMigrateSourceAppliance())
                return false;

            if (!ValidateWorkflow())
                return false;

            if (!ValidateBusinessProposal())
                return false;

            return true;
        }

        private bool ValidateAzureMigrateSourceAppliance()
        {
            if (!ImportRadioButton.Checked && !ApplianceRadioButton.Checked)
                return false;

            if (ImportRadioButton.Checked && ApplianceRadioButton.Checked)
                return false;

            if (ApplianceRadioButton.Checked && 
                !VMwareCheckBox.Checked && 
                !HyperVCheckBox.Checked &&
                !PhysicalCheckBox.Checked)
                return false;
            
            if (ImportRadioButton.Checked &&
                (VMwareCheckBox.Checked ||
                HyperVCheckBox.Checked ||
                PhysicalCheckBox.Checked))
                return false;

            return true;
        }

        private bool ValidateWorkflow()
        {
            if (CustomWorkflowRadioButton.Checked == false && ExpressWorkflowRadioButton.Checked == false)
                return false;

            if (CustomWorkflowRadioButton.Checked == true)
            {
                if (ModuleComboBox.SelectedItem == null)
                    return false;

                else if ((string)ModuleComboBox.SelectedItem != "Discovery" && (string)ModuleComboBox.SelectedItem != "Assessment")
                    return false;
            }

            return true;
        }

        private bool ValidateBusinessProposal()
        {
            string selectedModule = (string)ModuleComboBox.SelectedItem;
            if (((selectedModule != null && selectedModule.Equals("Assessment")) || ExpressWorkflowRadioButton.Checked) &&
               (ComprehensiveProposalRadioButton.Checked == false && QuickAvsProposalRadioButton.Checked == false))
                return false;

            if (selectedModule != null && selectedModule.Equals("Discovery") &&
               (ComprehensiveProposalRadioButton.Checked == true || QuickAvsProposalRadioButton.Checked == true))
                return false;

            return true;
        }
        #endregion

        #region Display assessment settings tab and submit button decision maker
        // If Submit Button is displayed, Assessment settings tab will not be displayed.
        public bool DisplaySubmitButton()
        {
            if (CustomWorkflowRadioButton.Checked == true && (string)ModuleComboBox.SelectedItem == "Discovery")
                return true;

            return false;
        }
        #endregion

        #region ComboBox Mouse Click
        private void ModuleComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            ModuleComboBox.DroppedDown = true;
        }
        #endregion

        #region Mouse Hover Descriptions
        private void AzureMigrateSourceApplianceGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateSourceApplianceDescription();
        }

        private void VMwareCheckBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateSourceApplianceDescription();
        }

        private void HyperVCheckBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateSourceApplianceDescription();
        }

        private void PhysicalCheckBox_MouseHover(object sender, EventArgs e)
        {
            UpdateAzureMigrateSourceApplianceDescription();
        }

        private void WorkflowGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void CustomWorkflowRadioButton_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void ExpressWorkflowRadioButton_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void CustomWorkflowInfoPictureBox_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void ExpressWorkflowInfoPictureBox_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void ModuleComboBox_MouseHover(object sender, EventArgs e)
        {
            UpdateWorkflowDescription();
        }

        private void BusinessProposalGroupBox_MouseHover(object sender, EventArgs e)
        {
            UpdateBusinessProposalDescription();
        }

        private void ComprehensiveProposalRadioButton_MouseHover(object sender, EventArgs e)
        {
            UpdateBusinessProposalDescription();
        }

        private void QuickAvsProposalRadioButton_MouseHover(object sender, EventArgs e)
        {
            UpdateBusinessProposalDescription();
        }

        private void UpdateAzureMigrateSourceApplianceDescription()
        {
            UpdateDescriptionTextBox("Discovery Appliance", "Azure Migrate Export can be used to assess servers in VMware, Hyper-V and Physical/Bare-Metal environments.\n\nTo create a business proposal, you can use the inventory of servers either discovered using appliance or via import.​\nIn case of appliance-based discovery, select appropriate source appliance stacks that were used to discover the inventory.​\nIn case of you have used import-based discovery via .csv or RVTools, select Import. ");
        }

        private void UpdateWorkflowDescription()
        {
            UpdateDescriptionTextBox("Workflow", "Azure Migrate Export supports two workflows:\n\t1. Custom - Enables you to make customizations before generating the reports and presentations - classifying workloads into \"Dev\" or\n\t    \"Prod\", moving servers out of scope, and scoping discovered servers to IaaS assessments. This will help you present advantages of\n\t    Dev/Test pricing and customize the scope of presentation.\n\t2. Express - Enables you to generate reports and presentations quickly - assuming all discovered servers are in-scope and are\n\t    production servers.");
        }

        private void UpdateBusinessProposalDescription()
        {
            UpdateDescriptionTextBox("Business Proposal", "Create a comprehensive business proposal that includes IaaS, PaaS and AVS targets for your servers and databases. The proposal includes in-depth analysis for transformations to recommended targets. ​\n\nCreate a Quick AVS proposal if you want to quickly migrate your VMware hosted servers to Azure VMware service. ​");
        }

        private void UpdateDescriptionTextBox(string descriptionHeader, string description)
        {
            ConfigurationDescriptionGroupBox.Visible = true;
            ConfigurationDescriptionGroupBox.Text = descriptionHeader;
            ConfigurationDescriptionRichTextBox.Text = description;
        }
        #endregion

        #region Getter Methods
        public List<string> GetAzureMigrateSourceAppliances()
        {
            List<string> azureMigrateSourceAppliances = new List<string>();

            if (VMwareCheckBox.Checked == true)
                azureMigrateSourceAppliances.Add("vmware");
            if (HyperVCheckBox.Checked == true)
                azureMigrateSourceAppliances.Add("hyperv");
            if (PhysicalCheckBox.Checked == true)
                azureMigrateSourceAppliances.Add("physical");
            if (ImportRadioButton.Checked == true)
                azureMigrateSourceAppliances.Add("import");

            return azureMigrateSourceAppliances;
        }

        public bool IsExpressWorkflowSelected()
        {
            return ExpressWorkflowRadioButton.Checked;
        }

        public string GetBusinessProposal()
        {
            if (QuickAvsProposalRadioButton.Checked)
                return BusinessProposal.AVS.ToString();

            if (ComprehensiveProposalRadioButton.Checked)
                return BusinessProposal.Comprehensive.ToString();

            return string.Empty;
        }

        public string GetModule()
        {
            return (string)ModuleComboBox.SelectedItem;
        }
        #endregion

        #region Setter Methods
        public void SetModule(string setModuleAs)
        {
            ModuleComboBox.SelectedItem = setModuleAs;

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }
        #endregion

        #region visit links
        private void CustomWorkflowInfoPictureBox_Click(object sender, EventArgs e)
        {
            VisitLink("https://go.microsoft.com/fwlink/?linkid=2215651");
        }

        private void ExpressWorkflowInfoPictureBox_Click(object sender, EventArgs e)
        {
            VisitLink("https://go.microsoft.com/fwlink/?linkid=2215823");
        }

        private void VisitLink(string goLinkUrl)
        {
            ProcessStartInfo processDescription = new ProcessStartInfo(goLinkUrl)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processDescription);
        }
        #endregion

        #region Utilities
        private bool IsDiscoveryReportPresent()
        {
            if (!Directory.Exists(DiscoveryReportConstants.DiscoveryReportDirectory))
                return false;
            if (!File.Exists(DiscoveryReportConstants.DiscoveryReportPath))
                return false;

            return true;
        }

        public void DisableBusinessProposal()
        {
            ComprehensiveProposalRadioButton.Enabled = false;
            ComprehensiveProposalRadioButton.Checked = false;
            QuickAvsProposalRadioButton.Enabled = false;
            QuickAvsProposalRadioButton.Checked = false;
        }

        public void EnableBusinessProposal()
        {
            ComprehensiveProposalRadioButton.Enabled = true;
            QuickAvsProposalRadioButton.Enabled = true;
        }

        public void CheckOnlyQuickAvsProposal()
        {
            ComprehensiveProposalRadioButton.Enabled = false;
            QuickAvsProposalRadioButton.Enabled = true;
            QuickAvsProposalRadioButton.Checked = true;
        }

        private void DisableHypervAndPhysicalCheckBoxes()
        {
            ImportRadioButton.Checked = false;
            VMwareCheckBox.Enabled = true;
            VMwareCheckBox.Checked = true;
            HyperVCheckBox.Enabled = false;
            HyperVCheckBox.Checked = false;
            PhysicalCheckBox.Enabled = false;
            PhysicalCheckBox.Checked = false;
        }
        #endregion
    }
}