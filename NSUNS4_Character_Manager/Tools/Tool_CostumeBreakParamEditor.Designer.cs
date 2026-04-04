using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_CostumeBreakParamEditor
    {
        private IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripButton sortButton;
        private ToolStripButton displayModeButton;
        private ListBox entryListBox;
        private Button addEntryButton;
        private Button duplicateEntryButton;
        private Button deleteEntryButton;
        private Button saveSelectedButton;
        private Panel editorPanel;
        private Label charaCodeLabel;
        private Label costumeIndexLabel;
        private Label pathLabel;
        private Label clonesCountLabel;
        private Label unkLabel;
        private NumericUpDown charaCodeValue;
        private NumericUpDown costumeIndexValue;
        private TextBox pathTextBox;
        private CheckBox awakeningModelFlagCheckBox;
        private CheckBox clonesFlagCheckBox;
        private NumericUpDown clonesCountValue;
        private NumericUpDown unkValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortButton = new System.Windows.Forms.ToolStripButton();
            this.displayModeButton = new System.Windows.Forms.ToolStripButton();
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.saveSelectedButton = new System.Windows.Forms.Button();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.unkValue = new System.Windows.Forms.NumericUpDown();
            this.clonesCountValue = new System.Windows.Forms.NumericUpDown();
            this.clonesFlagCheckBox = new System.Windows.Forms.CheckBox();
            this.awakeningModelFlagCheckBox = new System.Windows.Forms.CheckBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.costumeIndexValue = new System.Windows.Forms.NumericUpDown();
            this.charaCodeValue = new System.Windows.Forms.NumericUpDown();
            this.unkLabel = new System.Windows.Forms.Label();
            this.clonesCountLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.costumeIndexLabel = new System.Windows.Forms.Label();
            this.charaCodeLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.editorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unkValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clonesCountValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeIndexValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.charaCodeValue)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortButton,
            this.displayModeButton});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(723, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // sortButton
            // 
            this.sortButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sortButton.Enabled = false;
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(32, 19);
            this.sortButton.Text = "Sort";
            this.sortButton.Click += new System.EventHandler(this.sortButton_Click);
            // 
            // displayModeButton
            // 
            this.displayModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.displayModeButton.Enabled = false;
            this.displayModeButton.Name = "displayModeButton";
            this.displayModeButton.Size = new System.Drawing.Size(31, 19);
            this.displayModeButton.Text = "Dec";
            this.displayModeButton.Click += new System.EventHandler(this.displayModeButton_Click);
            // 
            // entryListBox
            // 
            this.entryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.entryListBox.FormattingEnabled = true;
            this.entryListBox.Location = new System.Drawing.Point(10, 27);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(289, 329);
            this.entryListBox.TabIndex = 1;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addEntryButton.Location = new System.Drawing.Point(10, 367);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(69, 24);
            this.addEntryButton.TabIndex = 2;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateEntryButton.Location = new System.Drawing.Point(84, 367);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(69, 24);
            this.duplicateEntryButton.TabIndex = 3;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.UseVisualStyleBackColor = true;
            this.duplicateEntryButton.Click += new System.EventHandler(this.duplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteEntryButton.Location = new System.Drawing.Point(231, 367);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(67, 24);
            this.deleteEntryButton.TabIndex = 4;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.UseVisualStyleBackColor = true;
            this.deleteEntryButton.Click += new System.EventHandler(this.deleteEntryButton_Click);
            // 
            // saveSelectedButton
            // 
            this.saveSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveSelectedButton.Location = new System.Drawing.Point(158, 367);
            this.saveSelectedButton.Name = "saveSelectedButton";
            this.saveSelectedButton.Size = new System.Drawing.Size(69, 24);
            this.saveSelectedButton.TabIndex = 5;
            this.saveSelectedButton.Text = "Save";
            this.saveSelectedButton.UseVisualStyleBackColor = true;
            this.saveSelectedButton.Click += new System.EventHandler(this.saveSelectedButton_Click);
            // 
            // editorPanel
            // 
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorPanel.Controls.Add(this.unkValue);
            this.editorPanel.Controls.Add(this.clonesCountValue);
            this.editorPanel.Controls.Add(this.clonesFlagCheckBox);
            this.editorPanel.Controls.Add(this.awakeningModelFlagCheckBox);
            this.editorPanel.Controls.Add(this.pathTextBox);
            this.editorPanel.Controls.Add(this.costumeIndexValue);
            this.editorPanel.Controls.Add(this.charaCodeValue);
            this.editorPanel.Controls.Add(this.unkLabel);
            this.editorPanel.Controls.Add(this.clonesCountLabel);
            this.editorPanel.Controls.Add(this.pathLabel);
            this.editorPanel.Controls.Add(this.costumeIndexLabel);
            this.editorPanel.Controls.Add(this.charaCodeLabel);
            this.editorPanel.Location = new System.Drawing.Point(313, 27);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(401, 365);
            this.editorPanel.TabIndex = 6;
            // 
            // unkValue
            // 
            this.unkValue.Location = new System.Drawing.Point(103, 160);
            this.unkValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unkValue.Name = "unkValue";
            this.unkValue.Size = new System.Drawing.Size(154, 20);
            this.unkValue.TabIndex = 10;
            // 
            // clonesCountValue
            // 
            this.clonesCountValue.Location = new System.Drawing.Point(103, 135);
            this.clonesCountValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.clonesCountValue.Name = "clonesCountValue";
            this.clonesCountValue.Size = new System.Drawing.Size(154, 20);
            this.clonesCountValue.TabIndex = 9;
            // 
            // clonesFlagCheckBox
            // 
            this.clonesFlagCheckBox.AutoSize = true;
            this.clonesFlagCheckBox.Location = new System.Drawing.Point(103, 114);
            this.clonesFlagCheckBox.Name = "clonesFlagCheckBox";
            this.clonesFlagCheckBox.Size = new System.Drawing.Size(58, 17);
            this.clonesFlagCheckBox.TabIndex = 8;
            this.clonesFlagCheckBox.Text = "Clones";
            this.clonesFlagCheckBox.UseVisualStyleBackColor = true;
            this.clonesFlagCheckBox.CheckedChanged += new System.EventHandler(this.clonesFlagCheckBox_CheckedChanged);
            // 
            // awakeningModelFlagCheckBox
            // 
            this.awakeningModelFlagCheckBox.AutoSize = true;
            this.awakeningModelFlagCheckBox.Location = new System.Drawing.Point(103, 92);
            this.awakeningModelFlagCheckBox.Name = "awakeningModelFlagCheckBox";
            this.awakeningModelFlagCheckBox.Size = new System.Drawing.Size(111, 17);
            this.awakeningModelFlagCheckBox.TabIndex = 7;
            this.awakeningModelFlagCheckBox.Text = "Awakening Model";
            this.awakeningModelFlagCheckBox.UseVisualStyleBackColor = true;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(103, 67);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(281, 20);
            this.pathTextBox.TabIndex = 6;
            // 
            // costumeIndexValue
            // 
            this.costumeIndexValue.Location = new System.Drawing.Point(103, 42);
            this.costumeIndexValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.costumeIndexValue.Name = "costumeIndexValue";
            this.costumeIndexValue.Size = new System.Drawing.Size(154, 20);
            this.costumeIndexValue.TabIndex = 5;
            // 
            // charaCodeValue
            // 
            this.charaCodeValue.Location = new System.Drawing.Point(103, 16);
            this.charaCodeValue.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.charaCodeValue.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.charaCodeValue.Name = "charaCodeValue";
            this.charaCodeValue.Size = new System.Drawing.Size(154, 20);
            this.charaCodeValue.TabIndex = 4;
            // 
            // unkLabel
            // 
            this.unkLabel.AutoSize = true;
            this.unkLabel.Location = new System.Drawing.Point(15, 162);
            this.unkLabel.Name = "unkLabel";
            this.unkLabel.Size = new System.Drawing.Size(30, 13);
            this.unkLabel.TabIndex = 4;
            this.unkLabel.Text = "Unk:";
            // 
            // clonesCountLabel
            // 
            this.clonesCountLabel.AutoSize = true;
            this.clonesCountLabel.Location = new System.Drawing.Point(15, 137);
            this.clonesCountLabel.Name = "clonesCountLabel";
            this.clonesCountLabel.Size = new System.Drawing.Size(73, 13);
            this.clonesCountLabel.TabIndex = 3;
            this.clonesCountLabel.Text = "Clones Count:";
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(15, 69);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(32, 13);
            this.pathLabel.TabIndex = 2;
            this.pathLabel.Text = "Path:";
            // 
            // costumeIndexLabel
            // 
            this.costumeIndexLabel.AutoSize = true;
            this.costumeIndexLabel.Location = new System.Drawing.Point(15, 44);
            this.costumeIndexLabel.Name = "costumeIndexLabel";
            this.costumeIndexLabel.Size = new System.Drawing.Size(80, 13);
            this.costumeIndexLabel.TabIndex = 1;
            this.costumeIndexLabel.Text = "Costume Index:";
            // 
            // charaCodeLabel
            // 
            this.charaCodeLabel.AutoSize = true;
            this.charaCodeLabel.Location = new System.Drawing.Point(15, 19);
            this.charaCodeLabel.Name = "charaCodeLabel";
            this.charaCodeLabel.Size = new System.Drawing.Size(62, 13);
            this.charaCodeLabel.TabIndex = 0;
            this.charaCodeLabel.Text = "Characode:";
            // 
            // Tool_CostumeBreakParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 402);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.saveSelectedButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(739, 441);
            this.Name = "Tool_CostumeBreakParamEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Costume Break Param Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unkValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clonesCountValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeIndexValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.charaCodeValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
