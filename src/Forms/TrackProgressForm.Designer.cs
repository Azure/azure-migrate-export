namespace Azure.Migrate.Export.Forms
{
    partial class TrackProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackProgressForm));
            this.ProgressGroupBox = new System.Windows.Forms.GroupBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProcessLogsGroupBox = new System.Windows.Forms.GroupBox();
            this.ProcessLogsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ExcelFilesLocationLabel = new System.Windows.Forms.Label();
            this.AzureMigrateExportBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ProcessInfoTextBox = new System.Windows.Forms.RichTextBox();
            this.ProgressGroupBox.SuspendLayout();
            this.ProcessLogsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressGroupBox
            // 
            this.ProgressGroupBox.Controls.Add(this.ProgressBar);
            resources.ApplyResources(this.ProgressGroupBox, "ProgressGroupBox");
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.TabStop = false;
            // 
            // ProgressBar
            // 
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            this.ProgressBar.Name = "ProgressBar";
            // 
            // ProcessLogsGroupBox
            // 
            this.ProcessLogsGroupBox.Controls.Add(this.ProcessLogsRichTextBox);
            resources.ApplyResources(this.ProcessLogsGroupBox, "ProcessLogsGroupBox");
            this.ProcessLogsGroupBox.Name = "ProcessLogsGroupBox";
            this.ProcessLogsGroupBox.TabStop = false;
            // 
            // ProcessLogsRichTextBox
            // 
            this.ProcessLogsRichTextBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.ProcessLogsRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProcessLogsRichTextBox.DetectUrls = false;
            this.ProcessLogsRichTextBox.ForeColor = System.Drawing.SystemColors.Menu;
            resources.ApplyResources(this.ProcessLogsRichTextBox, "ProcessLogsRichTextBox");
            this.ProcessLogsRichTextBox.Name = "ProcessLogsRichTextBox";
            this.ProcessLogsRichTextBox.ReadOnly = true;
            // 
            // ExcelFilesLocationLabel
            // 
            resources.ApplyResources(this.ExcelFilesLocationLabel, "ExcelFilesLocationLabel");
            this.ExcelFilesLocationLabel.Name = "ExcelFilesLocationLabel";
            // 
            // AzureMigrateExportBackGroundWorker
            // 
            this.AzureMigrateExportBackGroundWorker.WorkerReportsProgress = true;
            this.AzureMigrateExportBackGroundWorker.WorkerSupportsCancellation = true;
            // 
            // ProcessInfoTextBox
            // 
            this.ProcessInfoTextBox.BackColor = System.Drawing.Color.White;
            this.ProcessInfoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ProcessInfoTextBox, "ProcessInfoTextBox");
            this.ProcessInfoTextBox.Name = "ProcessInfoTextBox";
            this.ProcessInfoTextBox.ReadOnly = true;
            // 
            // TrackProgressForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ProcessInfoTextBox);
            this.Controls.Add(this.ExcelFilesLocationLabel);
            this.Controls.Add(this.ProcessLogsGroupBox);
            this.Controls.Add(this.ProgressGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TrackProgressForm";
            this.ProgressGroupBox.ResumeLayout(false);
            this.ProcessLogsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ProgressGroupBox;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.GroupBox ProcessLogsGroupBox;
        private System.Windows.Forms.RichTextBox ProcessLogsRichTextBox;
        private System.Windows.Forms.Label ExcelFilesLocationLabel;
        private System.ComponentModel.BackgroundWorker AzureMigrateExportBackGroundWorker;
        private System.Windows.Forms.RichTextBox ProcessInfoTextBox;
    }
}