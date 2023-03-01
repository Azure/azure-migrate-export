namespace Azure.Migrate.Export.Forms
{
    partial class AssessmentSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssessmentSettingsForm));
            this.AzureParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.AssessmentDurationComboBox = new System.Windows.Forms.ComboBox();
            this.CurrencyComboBox = new System.Windows.Forms.ComboBox();
            this.TargetRegionComboBox = new System.Windows.Forms.ComboBox();
            this.AssessmentDurationLabel = new System.Windows.Forms.Label();
            this.CurrencyLabel = new System.Windows.Forms.Label();
            this.TargetRegionLabel = new System.Windows.Forms.Label();
            this.OptimizationPreferenceGroupBox = new System.Windows.Forms.GroupBox();
            this.OptimizationPreferenceInfoPictureBox = new System.Windows.Forms.PictureBox();
            this.OptimizationPreferenceComboBox = new System.Windows.Forms.ComboBox();
            this.OptimizationPreferenceLabel = new System.Windows.Forms.Label();
            this.AssessSqlServicesSeparatelyGroupBox = new System.Windows.Forms.GroupBox();
            this.AssessSqlServicesSeparatelyInfoPictureBox = new System.Windows.Forms.PictureBox();
            this.AssessSqlServicesSeparatelyCheckBox = new System.Windows.Forms.CheckBox();
            this.AssessmentSettingsDescriptionGroupBox = new System.Windows.Forms.GroupBox();
            this.AssessmentSettingsDescriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.AzureParametersGroupBox.SuspendLayout();
            this.OptimizationPreferenceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptimizationPreferenceInfoPictureBox)).BeginInit();
            this.AssessSqlServicesSeparatelyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssessSqlServicesSeparatelyInfoPictureBox)).BeginInit();
            this.AssessmentSettingsDescriptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AzureParametersGroupBox
            // 
            this.AzureParametersGroupBox.Controls.Add(this.AssessmentDurationComboBox);
            this.AzureParametersGroupBox.Controls.Add(this.CurrencyComboBox);
            this.AzureParametersGroupBox.Controls.Add(this.TargetRegionComboBox);
            this.AzureParametersGroupBox.Controls.Add(this.AssessmentDurationLabel);
            this.AzureParametersGroupBox.Controls.Add(this.CurrencyLabel);
            this.AzureParametersGroupBox.Controls.Add(this.TargetRegionLabel);
            resources.ApplyResources(this.AzureParametersGroupBox, "AzureParametersGroupBox");
            this.AzureParametersGroupBox.Name = "AzureParametersGroupBox";
            this.AzureParametersGroupBox.TabStop = false;
            // 
            // AssessmentDurationComboBox
            // 
            this.AssessmentDurationComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.AssessmentDurationComboBox, "AssessmentDurationComboBox");
            this.AssessmentDurationComboBox.Name = "AssessmentDurationComboBox";
            this.AssessmentDurationComboBox.SelectionChangeCommitted += new System.EventHandler(this.AssessmentDurationComboBox_SelectionChangeCommitted);
            this.AssessmentDurationComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AssessmentDurationComboBox_MouseClick);
            // 
            // CurrencyComboBox
            // 
            this.CurrencyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.CurrencyComboBox, "CurrencyComboBox");
            this.CurrencyComboBox.Name = "CurrencyComboBox";
            this.CurrencyComboBox.SelectionChangeCommitted += new System.EventHandler(this.CurrencyComboBox_SelectionChangeCommitted);
            this.CurrencyComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CurrencyComboBox_MouseClick);
            // 
            // TargetRegionComboBox
            // 
            this.TargetRegionComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.TargetRegionComboBox, "TargetRegionComboBox");
            this.TargetRegionComboBox.Name = "TargetRegionComboBox";
            this.TargetRegionComboBox.SelectionChangeCommitted += new System.EventHandler(this.TargetRegionComboBox_SelectionChangeCommitted);
            this.TargetRegionComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TargetRegionComboBox_MouseClick);
            // 
            // AssessmentDurationLabel
            // 
            resources.ApplyResources(this.AssessmentDurationLabel, "AssessmentDurationLabel");
            this.AssessmentDurationLabel.Name = "AssessmentDurationLabel";
            // 
            // CurrencyLabel
            // 
            resources.ApplyResources(this.CurrencyLabel, "CurrencyLabel");
            this.CurrencyLabel.Name = "CurrencyLabel";
            // 
            // TargetRegionLabel
            // 
            resources.ApplyResources(this.TargetRegionLabel, "TargetRegionLabel");
            this.TargetRegionLabel.Name = "TargetRegionLabel";
            // 
            // OptimizationPreferenceGroupBox
            // 
            this.OptimizationPreferenceGroupBox.Controls.Add(this.OptimizationPreferenceInfoPictureBox);
            this.OptimizationPreferenceGroupBox.Controls.Add(this.OptimizationPreferenceComboBox);
            this.OptimizationPreferenceGroupBox.Controls.Add(this.OptimizationPreferenceLabel);
            resources.ApplyResources(this.OptimizationPreferenceGroupBox, "OptimizationPreferenceGroupBox");
            this.OptimizationPreferenceGroupBox.Name = "OptimizationPreferenceGroupBox";
            this.OptimizationPreferenceGroupBox.TabStop = false;
            this.OptimizationPreferenceGroupBox.MouseHover += new System.EventHandler(this.OptimizationPreferenceGroupBox_MouseHover);
            // 
            // OptimizationPreferenceInfoPictureBox
            // 
            this.OptimizationPreferenceInfoPictureBox.Image = global::Azure.Migrate.Export.Properties.Resources.icons8_info_16;
            resources.ApplyResources(this.OptimizationPreferenceInfoPictureBox, "OptimizationPreferenceInfoPictureBox");
            this.OptimizationPreferenceInfoPictureBox.Name = "OptimizationPreferenceInfoPictureBox";
            this.OptimizationPreferenceInfoPictureBox.TabStop = false;
            this.OptimizationPreferenceInfoPictureBox.Click += new System.EventHandler(this.OptimizationPreferenceInfoPictureBox_Click);
            this.OptimizationPreferenceInfoPictureBox.MouseHover += new System.EventHandler(this.OptimizationPreferenceInfoPictureBox_MouseHover);
            // 
            // OptimizationPreferenceComboBox
            // 
            this.OptimizationPreferenceComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.OptimizationPreferenceComboBox, "OptimizationPreferenceComboBox");
            this.OptimizationPreferenceComboBox.Name = "OptimizationPreferenceComboBox";
            this.OptimizationPreferenceComboBox.SelectionChangeCommitted += new System.EventHandler(this.OptimizationPreferenceComboBox_SelectionChangeCommitted);
            this.OptimizationPreferenceComboBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OptimizationPreferenceComboBox_MouseClick);
            this.OptimizationPreferenceComboBox.MouseHover += new System.EventHandler(this.OptimizationPreferenceComboBox_MouseHover);
            // 
            // OptimizationPreferenceLabel
            // 
            resources.ApplyResources(this.OptimizationPreferenceLabel, "OptimizationPreferenceLabel");
            this.OptimizationPreferenceLabel.Name = "OptimizationPreferenceLabel";
            this.OptimizationPreferenceLabel.MouseHover += new System.EventHandler(this.OptimizationPreferenceLabel_MouseHover);
            // 
            // AssessSqlServicesSeparatelyGroupBox
            // 
            this.AssessSqlServicesSeparatelyGroupBox.Controls.Add(this.AssessSqlServicesSeparatelyInfoPictureBox);
            this.AssessSqlServicesSeparatelyGroupBox.Controls.Add(this.AssessSqlServicesSeparatelyCheckBox);
            resources.ApplyResources(this.AssessSqlServicesSeparatelyGroupBox, "AssessSqlServicesSeparatelyGroupBox");
            this.AssessSqlServicesSeparatelyGroupBox.Name = "AssessSqlServicesSeparatelyGroupBox";
            this.AssessSqlServicesSeparatelyGroupBox.TabStop = false;
            this.AssessSqlServicesSeparatelyGroupBox.MouseHover += new System.EventHandler(this.AssessSqlServicesSeparatelyGroupBox_MouseHover);
            // 
            // AssessSqlServicesSeparatelyInfoPictureBox
            // 
            this.AssessSqlServicesSeparatelyInfoPictureBox.Image = global::Azure.Migrate.Export.Properties.Resources.icons8_info_16;
            resources.ApplyResources(this.AssessSqlServicesSeparatelyInfoPictureBox, "AssessSqlServicesSeparatelyInfoPictureBox");
            this.AssessSqlServicesSeparatelyInfoPictureBox.Name = "AssessSqlServicesSeparatelyInfoPictureBox";
            this.AssessSqlServicesSeparatelyInfoPictureBox.TabStop = false;
            this.AssessSqlServicesSeparatelyInfoPictureBox.Click += new System.EventHandler(this.AssessSqlServicesSeparatelyInfoPictureBox_Click);
            this.AssessSqlServicesSeparatelyInfoPictureBox.MouseHover += new System.EventHandler(this.AssessSqlServicesSeparatelyInfoPictureBox_MouseHover);
            // 
            // AssessSqlServicesSeparatelyCheckBox
            // 
            resources.ApplyResources(this.AssessSqlServicesSeparatelyCheckBox, "AssessSqlServicesSeparatelyCheckBox");
            this.AssessSqlServicesSeparatelyCheckBox.Name = "AssessSqlServicesSeparatelyCheckBox";
            this.AssessSqlServicesSeparatelyCheckBox.UseVisualStyleBackColor = true;
            this.AssessSqlServicesSeparatelyCheckBox.MouseHover += new System.EventHandler(this.AssessSqlServicesSeparatelyCheckBox_MouseHover);
            // 
            // AssessmentSettingsDescriptionGroupBox
            // 
            this.AssessmentSettingsDescriptionGroupBox.Controls.Add(this.AssessmentSettingsDescriptionRichTextBox);
            resources.ApplyResources(this.AssessmentSettingsDescriptionGroupBox, "AssessmentSettingsDescriptionGroupBox");
            this.AssessmentSettingsDescriptionGroupBox.Name = "AssessmentSettingsDescriptionGroupBox";
            this.AssessmentSettingsDescriptionGroupBox.TabStop = false;
            // 
            // AssessmentSettingsDescriptionRichTextBox
            // 
            this.AssessmentSettingsDescriptionRichTextBox.BackColor = System.Drawing.Color.White;
            this.AssessmentSettingsDescriptionRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.AssessmentSettingsDescriptionRichTextBox, "AssessmentSettingsDescriptionRichTextBox");
            this.AssessmentSettingsDescriptionRichTextBox.Name = "AssessmentSettingsDescriptionRichTextBox";
            this.AssessmentSettingsDescriptionRichTextBox.ReadOnly = true;
            // 
            // AssessmentSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.AssessmentSettingsDescriptionGroupBox);
            this.Controls.Add(this.AssessSqlServicesSeparatelyGroupBox);
            this.Controls.Add(this.OptimizationPreferenceGroupBox);
            this.Controls.Add(this.AzureParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AssessmentSettingsForm";
            this.AzureParametersGroupBox.ResumeLayout(false);
            this.AzureParametersGroupBox.PerformLayout();
            this.OptimizationPreferenceGroupBox.ResumeLayout(false);
            this.OptimizationPreferenceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptimizationPreferenceInfoPictureBox)).EndInit();
            this.AssessSqlServicesSeparatelyGroupBox.ResumeLayout(false);
            this.AssessSqlServicesSeparatelyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssessSqlServicesSeparatelyInfoPictureBox)).EndInit();
            this.AssessmentSettingsDescriptionGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AzureParametersGroupBox;
        private System.Windows.Forms.Label TargetRegionLabel;
        private System.Windows.Forms.Label CurrencyLabel;
        private System.Windows.Forms.Label AssessmentDurationLabel;
        private System.Windows.Forms.ComboBox TargetRegionComboBox;
        private System.Windows.Forms.ComboBox AssessmentDurationComboBox;
        private System.Windows.Forms.ComboBox CurrencyComboBox;
        private System.Windows.Forms.GroupBox OptimizationPreferenceGroupBox;
        private System.Windows.Forms.Label OptimizationPreferenceLabel;
        private System.Windows.Forms.ComboBox OptimizationPreferenceComboBox;
        private System.Windows.Forms.GroupBox AssessSqlServicesSeparatelyGroupBox;
        private System.Windows.Forms.PictureBox OptimizationPreferenceInfoPictureBox;
        private System.Windows.Forms.CheckBox AssessSqlServicesSeparatelyCheckBox;
        private System.Windows.Forms.PictureBox AssessSqlServicesSeparatelyInfoPictureBox;
        private System.Windows.Forms.GroupBox AssessmentSettingsDescriptionGroupBox;
        private System.Windows.Forms.RichTextBox AssessmentSettingsDescriptionRichTextBox;
    }
}