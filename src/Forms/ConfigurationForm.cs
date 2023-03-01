using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Azure.Migrate.Export.Forms
{
    public partial class ConfigurationForm : Form
    {
        private AzureMigrateExportMainForm mainFormObj;

        public ConfigurationForm(AzureMigrateExportMainForm obj)
        {
            InitializeComponent();
            mainFormObj = obj;
        }

        #region Set Default Values
        public void SetDefaultConfigurationValues()
        {
            HyperVCheckBox.Checked = true;
            VMwareCheckBox.Checked = true;
            PhysicalCheckBox.Checked = true;

            ExpressWorkflowRadioButton.Checked = true;
        }
        #endregion

        #region Workflow Radio Button Checked Changed
        private void CustomWorkflowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ModuleComboBox.Visible = true;

            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
        }

        private void ExpressWorkflowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ModuleComboBox.Visible = false;
            mainFormObj.MakeConfigurationTabButtonEnableDecisions();
            mainFormObj.MakeConfigurationActionButtonsEnabledDecision();
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

            return true;
        }

        private bool ValidateAzureMigrateSourceAppliance()
        {
            if (VMwareCheckBox.Checked == true || HyperVCheckBox.Checked == true || PhysicalCheckBox.Checked == true)
                return true;

            return false;
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

        private void UpdateAzureMigrateSourceApplianceDescription()
        {
            UpdateDescriptionTextBox("Azure Migrate Source Appliance", "Azure Migrate Export can be used to assess servers in VMware, Hyper-V and Physical/Bare-Metal environments.\n\nSelect appropriate source appliance stacks that were used to discover the respective environments using Azure Migrate: Discovery and Assessment tool.");
        }

        private void UpdateWorkflowDescription()
        {
            UpdateDescriptionTextBox("Workflow", "Azure Migrate Export supports two workflows:\n\t1. Custom - Enables you to make customizations before generating the reports and presentations - classifying workloads into \"Dev\" or\n\t    \"Prod\", moving servers out of scope, and scoping discovered servers to IaaS assessments. This will help you present advantages of\n\t    Dev/Test pricing and customize the scope of presentation.\n\t2. Express - Enables you to generate reports and presentations quickly - assuming all discovered servers are in-scope and are\n\t    production servers.");
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

            return azureMigrateSourceAppliances;
        }

        public bool IsExpressWorkflowSelected()
        {
            return ExpressWorkflowRadioButton.Checked;
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
    }
}
