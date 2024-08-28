namespace Azure.Migrate.Export.Forms
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.AzureMigrateSourceApplianceGroupBox = new System.Windows.Forms.GroupBox();
            this.ImportCheckBox = new System.Windows.Forms.CheckBox();
            this.PhysicalCheckBox = new System.Windows.Forms.CheckBox();
            this.HyperVCheckBox = new System.Windows.Forms.CheckBox();
            this.VMwareCheckBox = new System.Windows.Forms.CheckBox();
            this.WorkflowGroupBox = new System.Windows.Forms.GroupBox();
            this.ExpressWorkflowInfoPictureBox = new System.Windows.Forms.PictureBox();
            this.CustomWorkflowInfoPictureBox = new System.Windows.Forms.PictureBox();
            this.ModuleComboBox = new System.Windows.Forms.ComboBox();
            this.ExpressWorkflowRadioButton = new System.Windows.Forms.RadioButton();
            this.CustomWorkflowRadioButton = new System.Windows.Forms.RadioButton();
            this.ConfigurationDescriptionGroupBox = new System.Windows.Forms.GroupBox();
            this.ConfigurationDescriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BusinessProposalGroupBox = new System.Windows.Forms.GroupBox();
            this.QuickAvsProposalRadioButton = new System.Windows.Forms.RadioButton();
            this.ComprehensiveProposalRadioButton = new System.Windows.Forms.RadioButton();
            this.AzureMigrateSourceApplianceGroupBox.SuspendLayout();
            this.WorkflowGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpressWorkflowInfoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomWorkflowInfoPictureBox)).BeginInit();
            this.ConfigurationDescriptionGroupBox.SuspendLayout();
            this.BusinessProposalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AzureMigrateSourceApplianceGroupBox
            // 
            this.AzureMigrateSourceApplianceGroupBox.Controls.Add(this.ImportCheckBox);
            this.AzureMigrateSourceApplianceGroupBox.Controls.Add(this.PhysicalCheckBox);
            this.AzureMigrateSourceApplianceGroupBox.Controls.Add(this.HyperVCheckBox);
            this.AzureMigrateSourceApplianceGroupBox.Controls.Add(this.VMwareCheckBox);
            resources.ApplyResources(this.AzureMigrateSourceApplianceGroupBox, "AzureMigrateSourceApplianceGroupBox");
            this.AzureMigrateSourceApplianceGroupBox.Name = "AzureMigrateSourceApplianceGroupBox";
            this.AzureMigrateSourceApplianceGroupBox.TabStop = false;
            this.AzureMigrateSourceApplianceGroupBox.MouseHover += new System.EventHandler(this.AzureMigrateSourceApplianceGroupBox_MouseHover);
            // 
            // ImportCheckBox
            // 
            resources.ApplyResources(this.ImportCheckBox, "ImportCheckBox");
            this.ImportCheckBox.Name = "ImportCheckBox";
            this.ImportCheckBox.UseVisualStyleBackColor = true;
            this.ImportCheckBox.CheckedChanged += new System.EventHandler(this.ImportCheckBox_CheckedChanged);
            // 
            // PhysicalCheckBox
            // 
            resources.ApplyResources(this.PhysicalCheckBox, "PhysicalCheckBox");
            this.PhysicalCheckBox.Name = "PhysicalCheckBox";
            this.PhysicalCheckBox.UseVisualStyleBackColor = true;
            this.PhysicalCheckBox.CheckedChanged += new System.EventHandler(this.PhysicalCheckBox_CheckedChanged);
            this.PhysicalCheckBox.MouseHover += new System.EventHandler(this.PhysicalCheckBox_MouseHover);
            // 
            // HyperVCheckBox
            // 
            resources.ApplyResources(this.HyperVCheckBox, "HyperVCheckBox");
            this.HyperVCheckBox.Name = "HyperVCheckBox";
            this.HyperVCheckBox.UseVisualStyleBackColor = true;
            this.HyperVCheckBox.CheckedChanged += new System.EventHandler(this.HyperVCheckBox_CheckedChanged);
            this.HyperVCheckBox.MouseHover += new System.EventHandler(this.HyperVCheckBox_MouseHover);
            // 
            // VMwareCheckBox
            // 
            resources.ApplyResources(this.VMwareCheckBox, "VMwareCheckBox");
            this.VMwareCheckBox.Name = "VMwareCheckBox";
            this.VMwareCheckBox.UseVisualStyleBackColor = true;
            this.VMwareCheckBox.CheckedChanged += new System.EventHandler(this.VMwareCheckBox_CheckedChanged);
            this.VMwareCheckBox.MouseHover += new System.EventHandler(this.VMwareCheckBox_MouseHover);
            // 
            // WorkflowGroupBox
            // 
            this.WorkflowGroupBox.Controls.Add(this.ExpressWorkflowInfoPictureBox);
            this.WorkflowGroupBox.Controls.Add(this.CustomWorkflowInfoPictureBox);
            this.WorkflowGroupBox.Controls.Add(this.ModuleComboBox);
            this.WorkflowGroupBox.Controls.Add(this.ExpressWorkflowRadioButton);
            this.WorkflowGroupBox.Controls.Add(this.CustomWorkflowRadioButton);
            resources.ApplyResources(this.WorkflowGroupBox, "WorkflowGroupBox");
            this.WorkflowGroupBox.Name = "WorkflowGroupBox";
            this.WorkflowGroupBox.TabStop = false;
            this.WorkflowGroupBox.MouseHover += new System.EventHandler(this.WorkflowGroupBox_MouseHover);
            // 
            // ExpressWorkflowInfoPictureBox
            // 
            this.ExpressWorkflowInfoPictureBox.Image = global::Azure.Migrate.Export.Properties.Resources.icons8_info_16;
            resources.ApplyResources(this.ExpressWorkflowInfoPictureBox, "ExpressWorkflowInfoPictureBox");
            this.ExpressWorkflowInfoPictureBox.Name = "ExpressWorkflowInfoPictureBox";
            this.ExpressWorkflowInfoPictureBox.TabStop = false;
            this.ExpressWorkflowInfoPictureBox.Click += new System.EventHandler(this.ExpressWorkflowInfoPictureBox_Click);
            this.ExpressWorkflowInfoPictureBox.MouseHover += new System.EventHandler(this.ExpressWorkflowInfoPictureBox_MouseHover);
            // 
            // CustomWorkflowInfoPictureBox
            // 
            this.CustomWorkflowInfoPictureBox.Image = global::Azure.Migrate.Export.Properties.Resources.icons8_info_16;
            resources.ApplyResources(this.CustomWorkflowInfoPictureBox, "CustomWorkflowInfoPictureBox");
            this.CustomWorkflowInfoPictureBox.Name = "CustomWorkflowInfoPictureBox";
            this.CustomWorkflowInfoPictureBox.TabStop = false;
            this.CustomWorkflowInfoPictureBox.Click += new System.EventHandler(this.CustomWorkflowInfoPictureBox_Click);
            this.CustomWorkflowInfoPictureBox.MouseHover += new System.EventHandler(this.CustomWorkflowInfoPictureBox_MouseHover);
            // 
            // ModuleComboBox
            // 
            resources.ApplyResources(this.ModuleComboBox, "ModuleComboBox");
            this.ModuleComboBox.FormattingEnabled = true;
            this.ModuleComboBox.Items.AddRange(new object[] {
            resources.GetString("ModuleComboBox.Items"),
            resources.GetString("ModuleComboBox.Items1")});
            this.ModuleComboBox.Name = "ModuleComboBox";
            this.ModuleComboBox.SelectionChangeCommitted += new System.EventHandler(this.ModuleComboBox_SelectionChangeCommitted);
            this.ModuleComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ModuleComboBox_MouseClick);
            this.ModuleComboBox.MouseHover += new System.EventHandler(this.ModuleComboBox_MouseHover);
            // 
            // ExpressWorkflowRadioButton
            // 
            resources.ApplyResources(this.ExpressWorkflowRadioButton, "ExpressWorkflowRadioButton");
            this.ExpressWorkflowRadioButton.Name = "ExpressWorkflowRadioButton";
            this.ExpressWorkflowRadioButton.TabStop = true;
            this.ExpressWorkflowRadioButton.UseVisualStyleBackColor = true;
            this.ExpressWorkflowRadioButton.CheckedChanged += new System.EventHandler(this.ExpressWorkflowRadioButton_CheckedChanged);
            this.ExpressWorkflowRadioButton.MouseHover += new System.EventHandler(this.ExpressWorkflowRadioButton_MouseHover);
            // 
            // CustomWorkflowRadioButton
            // 
            resources.ApplyResources(this.CustomWorkflowRadioButton, "CustomWorkflowRadioButton");
            this.CustomWorkflowRadioButton.Name = "CustomWorkflowRadioButton";
            this.CustomWorkflowRadioButton.TabStop = true;
            this.CustomWorkflowRadioButton.UseVisualStyleBackColor = true;
            this.CustomWorkflowRadioButton.CheckedChanged += new System.EventHandler(this.CustomWorkflowRadioButton_CheckedChanged);
            this.CustomWorkflowRadioButton.MouseHover += new System.EventHandler(this.CustomWorkflowRadioButton_MouseHover);
            // 
            // ConfigurationDescriptionGroupBox
            // 
            this.ConfigurationDescriptionGroupBox.Controls.Add(this.ConfigurationDescriptionRichTextBox);
            resources.ApplyResources(this.ConfigurationDescriptionGroupBox, "ConfigurationDescriptionGroupBox");
            this.ConfigurationDescriptionGroupBox.Name = "ConfigurationDescriptionGroupBox";
            this.ConfigurationDescriptionGroupBox.TabStop = false;
            // 
            // ConfigurationDescriptionRichTextBox
            // 
            this.ConfigurationDescriptionRichTextBox.BackColor = System.Drawing.Color.White;
            this.ConfigurationDescriptionRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ConfigurationDescriptionRichTextBox, "ConfigurationDescriptionRichTextBox");
            this.ConfigurationDescriptionRichTextBox.Name = "ConfigurationDescriptionRichTextBox";
            this.ConfigurationDescriptionRichTextBox.ReadOnly = true;
            // 
            // BusinessProposalGroupBox
            // 
            this.BusinessProposalGroupBox.Controls.Add(this.QuickAvsProposalRadioButton);
            this.BusinessProposalGroupBox.Controls.Add(this.ComprehensiveProposalRadioButton);
            resources.ApplyResources(this.BusinessProposalGroupBox, "BusinessProposalGroupBox");
            this.BusinessProposalGroupBox.Name = "BusinessProposalGroupBox";
            this.BusinessProposalGroupBox.TabStop = false;
            // 
            // QuickAvsProposalRadioButton
            // 
            resources.ApplyResources(this.QuickAvsProposalRadioButton, "QuickAvsProposalRadioButton");
            this.QuickAvsProposalRadioButton.Name = "QuickAvsProposalRadioButton";
            this.QuickAvsProposalRadioButton.TabStop = true;
            this.QuickAvsProposalRadioButton.UseVisualStyleBackColor = true;
            this.QuickAvsProposalRadioButton.CheckedChanged += new System.EventHandler(this.QuickAvsProposalRadioButton_CheckedChanged);
            // 
            // ComprehensiveProposalRadioButton
            // 
            resources.ApplyResources(this.ComprehensiveProposalRadioButton, "ComprehensiveProposalRadioButton");
            this.ComprehensiveProposalRadioButton.Name = "ComprehensiveProposalRadioButton";
            this.ComprehensiveProposalRadioButton.TabStop = true;
            this.ComprehensiveProposalRadioButton.UseVisualStyleBackColor = true;
            this.ComprehensiveProposalRadioButton.CheckedChanged += new System.EventHandler(this.ComprehensiveProposalRadioButton_CheckedChanged);
            // 
            // ConfigurationForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.BusinessProposalGroupBox);
            this.Controls.Add(this.ConfigurationDescriptionGroupBox);
            this.Controls.Add(this.WorkflowGroupBox);
            this.Controls.Add(this.AzureMigrateSourceApplianceGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConfigurationForm";
            this.AzureMigrateSourceApplianceGroupBox.ResumeLayout(false);
            this.AzureMigrateSourceApplianceGroupBox.PerformLayout();
            this.WorkflowGroupBox.ResumeLayout(false);
            this.WorkflowGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpressWorkflowInfoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomWorkflowInfoPictureBox)).EndInit();
            this.ConfigurationDescriptionGroupBox.ResumeLayout(false);
            this.BusinessProposalGroupBox.ResumeLayout(false);
            this.BusinessProposalGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AzureMigrateSourceApplianceGroupBox;
        private System.Windows.Forms.CheckBox VMwareCheckBox;
        private System.Windows.Forms.CheckBox HyperVCheckBox;
        private System.Windows.Forms.CheckBox PhysicalCheckBox;
        private System.Windows.Forms.GroupBox WorkflowGroupBox;
        private System.Windows.Forms.RadioButton CustomWorkflowRadioButton;
        private System.Windows.Forms.RadioButton ExpressWorkflowRadioButton;
        private System.Windows.Forms.ComboBox ModuleComboBox;
        private System.Windows.Forms.GroupBox ConfigurationDescriptionGroupBox;
        private System.Windows.Forms.RichTextBox ConfigurationDescriptionRichTextBox;
        private System.Windows.Forms.PictureBox CustomWorkflowInfoPictureBox;
        private System.Windows.Forms.PictureBox ExpressWorkflowInfoPictureBox;
        private System.Windows.Forms.CheckBox ImportCheckBox;
        private System.Windows.Forms.GroupBox BusinessProposalGroupBox;
        private System.Windows.Forms.RadioButton QuickAvsProposalRadioButton;
        private System.Windows.Forms.RadioButton ComprehensiveProposalRadioButton;
    }
}