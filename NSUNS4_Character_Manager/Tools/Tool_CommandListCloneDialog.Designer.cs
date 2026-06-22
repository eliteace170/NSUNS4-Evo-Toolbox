namespace NSUNS4_Character_Manager
{
    partial class Tool_CommandListCloneDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Label sourceCharacodeLabel;
        private System.Windows.Forms.ComboBox sourceCharacodeComboBox;
        private System.Windows.Forms.Label targetCharacodeLabel;
        private System.Windows.Forms.ComboBox targetCharacodeComboBox;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.instructionLabel = new System.Windows.Forms.Label();
            this.sourceCharacodeLabel = new System.Windows.Forms.Label();
            this.sourceCharacodeComboBox = new System.Windows.Forms.ComboBox();
            this.targetCharacodeLabel = new System.Windows.Forms.Label();
            this.targetCharacodeComboBox = new System.Windows.Forms.ComboBox();
            this.cloneButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // instructionLabel
            // 
            this.instructionLabel.Location = new System.Drawing.Point(12, 12);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(416, 34);
            this.instructionLabel.TabIndex = 0;
            this.instructionLabel.Text = "Choose the character to copy and the Characode that will receive the cloned entries.";
            // 
            // sourceCharacodeLabel
            // 
            this.sourceCharacodeLabel.AutoSize = true;
            this.sourceCharacodeLabel.Location = new System.Drawing.Point(12, 57);
            this.sourceCharacodeLabel.Name = "sourceCharacodeLabel";
            this.sourceCharacodeLabel.Size = new System.Drawing.Size(101, 15);
            this.sourceCharacodeLabel.TabIndex = 1;
            this.sourceCharacodeLabel.Text = "Source Characode";
            // 
            // sourceCharacodeComboBox
            // 
            this.sourceCharacodeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.sourceCharacodeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.sourceCharacodeComboBox.FormattingEnabled = true;
            this.sourceCharacodeComboBox.Location = new System.Drawing.Point(139, 54);
            this.sourceCharacodeComboBox.Name = "sourceCharacodeComboBox";
            this.sourceCharacodeComboBox.Size = new System.Drawing.Size(289, 23);
            this.sourceCharacodeComboBox.TabIndex = 0;
            // 
            // targetCharacodeLabel
            // 
            this.targetCharacodeLabel.AutoSize = true;
            this.targetCharacodeLabel.Location = new System.Drawing.Point(12, 91);
            this.targetCharacodeLabel.Name = "targetCharacodeLabel";
            this.targetCharacodeLabel.Size = new System.Drawing.Size(97, 15);
            this.targetCharacodeLabel.TabIndex = 3;
            this.targetCharacodeLabel.Text = "Clone Characode";
            // 
            // targetCharacodeComboBox
            // 
            this.targetCharacodeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.targetCharacodeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.targetCharacodeComboBox.FormattingEnabled = true;
            this.targetCharacodeComboBox.Location = new System.Drawing.Point(139, 88);
            this.targetCharacodeComboBox.Name = "targetCharacodeComboBox";
            this.targetCharacodeComboBox.Size = new System.Drawing.Size(289, 23);
            this.targetCharacodeComboBox.TabIndex = 1;
            // 
            // cloneButton
            // 
            this.cloneButton.Location = new System.Drawing.Point(248, 132);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(87, 30);
            this.cloneButton.TabIndex = 2;
            this.cloneButton.Text = "Clone";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.cloneButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 132);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 30);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // Tool_CommandListCloneDialog
            // 
            this.AcceptButton = this.cloneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 176);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.targetCharacodeComboBox);
            this.Controls.Add(this.targetCharacodeLabel);
            this.Controls.Add(this.sourceCharacodeComboBox);
            this.Controls.Add(this.sourceCharacodeLabel);
            this.Controls.Add(this.instructionLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tool_CommandListCloneDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clone Character Entries";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
