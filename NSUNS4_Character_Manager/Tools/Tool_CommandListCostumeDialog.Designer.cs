namespace NSUNS4_Character_Manager
{
    partial class Tool_CommandListCostumeDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Label characodeLabel;
        private System.Windows.Forms.ComboBox characodeComboBox;
        private System.Windows.Forms.Label totalCostumesLabel;
        private System.Windows.Forms.NumericUpDown totalCostumesNumericUpDown;
        private System.Windows.Forms.Label rangeHelpLabel;
        private System.Windows.Forms.Button addButton;
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
            this.characodeLabel = new System.Windows.Forms.Label();
            this.characodeComboBox = new System.Windows.Forms.ComboBox();
            this.totalCostumesLabel = new System.Windows.Forms.Label();
            this.totalCostumesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rangeHelpLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.totalCostumesNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // instructionLabel
            // 
            this.instructionLabel.Location = new System.Drawing.Point(12, 12);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(416, 34);
            this.instructionLabel.TabIndex = 0;
            this.instructionLabel.Text = "Choose a character and the total number of costumes it should have.";
            // 
            // characodeLabel
            // 
            this.characodeLabel.AutoSize = true;
            this.characodeLabel.Location = new System.Drawing.Point(12, 57);
            this.characodeLabel.Name = "characodeLabel";
            this.characodeLabel.Size = new System.Drawing.Size(64, 15);
            this.characodeLabel.TabIndex = 1;
            this.characodeLabel.Text = "Characode";
            // 
            // characodeComboBox
            // 
            this.characodeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.characodeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.characodeComboBox.FormattingEnabled = true;
            this.characodeComboBox.Location = new System.Drawing.Point(139, 54);
            this.characodeComboBox.Name = "characodeComboBox";
            this.characodeComboBox.Size = new System.Drawing.Size(289, 23);
            this.characodeComboBox.TabIndex = 0;
            // 
            // totalCostumesLabel
            // 
            this.totalCostumesLabel.AutoSize = true;
            this.totalCostumesLabel.Location = new System.Drawing.Point(12, 91);
            this.totalCostumesLabel.Name = "totalCostumesLabel";
            this.totalCostumesLabel.Size = new System.Drawing.Size(88, 15);
            this.totalCostumesLabel.TabIndex = 3;
            this.totalCostumesLabel.Text = "Total costumes";
            // 
            // totalCostumesNumericUpDown
            // 
            this.totalCostumesNumericUpDown.Location = new System.Drawing.Point(139, 88);
            this.totalCostumesNumericUpDown.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.totalCostumesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.totalCostumesNumericUpDown.Name = "totalCostumesNumericUpDown";
            this.totalCostumesNumericUpDown.Size = new System.Drawing.Size(90, 23);
            this.totalCostumesNumericUpDown.TabIndex = 1;
            this.totalCostumesNumericUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // rangeHelpLabel
            // 
            this.rangeHelpLabel.AutoSize = true;
            this.rangeHelpLabel.Location = new System.Drawing.Point(239, 91);
            this.rangeHelpLabel.Name = "rangeHelpLabel";
            this.rangeHelpLabel.Size = new System.Drawing.Size(189, 15);
            this.rangeHelpLabel.TabIndex = 5;
            this.rangeHelpLabel.Text = "1–20, including the base costume";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(248, 132);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(87, 30);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
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
            // Tool_CommandListCostumeDialog
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 176);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.rangeHelpLabel);
            this.Controls.Add(this.totalCostumesNumericUpDown);
            this.Controls.Add(this.totalCostumesLabel);
            this.Controls.Add(this.characodeComboBox);
            this.Controls.Add(this.characodeLabel);
            this.Controls.Add(this.instructionLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tool_CommandListCostumeDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Costume Entries";
            ((System.ComponentModel.ISupportInitialize)(this.totalCostumesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
