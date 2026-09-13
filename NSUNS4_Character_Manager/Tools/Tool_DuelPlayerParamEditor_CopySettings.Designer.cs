namespace NSUNS4_Character_Manager
{
    partial class Tool_DuelPlayerParamEditor_CopySettings
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.ListBox optionsList;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.promptLabel = new System.Windows.Forms.Label();
            this.optionsList = new System.Windows.Forms.ListBox();
            this.copyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // promptLabel
            this.promptLabel.Location = new System.Drawing.Point(12, 12);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(296, 36);
            this.promptLabel.Text = "Choose which settings to copy to the clipboard.";
            // optionsList
            this.optionsList.FormattingEnabled = true;
            this.optionsList.Items.AddRange(new object[] {
                "1. Items only",
                "2. Conditions list",
                "3. Battle settings",
                "4. Conditions + Battle settings",
                "5. Everything" });
            this.optionsList.Location = new System.Drawing.Point(12, 52);
            this.optionsList.Name = "optionsList";
            this.optionsList.Size = new System.Drawing.Size(296, 121);
            this.optionsList.TabIndex = 0;
            this.optionsList.SelectedIndex = 0;
            // copyButton
            this.copyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.copyButton.Location = new System.Drawing.Point(152, 190);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 25);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            // cancelButton
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(233, 190);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // Tool_DuelPlayerParamEditor_CopySettings
            this.AcceptButton = this.copyButton;
            this.CancelButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 235);
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.optionsList);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tool_DuelPlayerParamEditor_CopySettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy settings";
            this.ResumeLayout(false);
        }
    }
}
