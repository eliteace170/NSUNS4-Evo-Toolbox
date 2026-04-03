using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_MusicParamEditor
    {
        private IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ListBox entryListBox;
        private Button addEntryButton;
        private Button duplicateEntryButton;
        private Button deleteEntryButton;
        private Button moveUpButton;
        private Button moveDownButton;
        private Button saveSelectedButton;
        private Panel editorPanel;
        private Label musicStringLabel;
        private Label musicIdLabel;
        private Label indexLabel;
        private Label costLabel;
        private Label unknown1Label;
        private Label unknown2Label;
        private Label unknown3Label;
        private Label unknown4Label;
        private Label unknown5Label;
        private Label unknown6Label;
        private TextBox musicStringTextBox;
        private TextBox musicIdTextBox;
        private NumericUpDown indexValue;
        private NumericUpDown costValue;
        private NumericUpDown unknown1Value;
        private NumericUpDown unknown2Value;
        private NumericUpDown unknown3Value;
        private NumericUpDown unknown4Value;
        private NumericUpDown unknown5Value;
        private NumericUpDown unknown6Value;

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
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.saveSelectedButton = new System.Windows.Forms.Button();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.unknown6Value = new System.Windows.Forms.NumericUpDown();
            this.unknown5Value = new System.Windows.Forms.NumericUpDown();
            this.unknown4Value = new System.Windows.Forms.NumericUpDown();
            this.unknown3Value = new System.Windows.Forms.NumericUpDown();
            this.unknown2Value = new System.Windows.Forms.NumericUpDown();
            this.unknown1Value = new System.Windows.Forms.NumericUpDown();
            this.costValue = new System.Windows.Forms.NumericUpDown();
            this.indexValue = new System.Windows.Forms.NumericUpDown();
            this.musicIdTextBox = new System.Windows.Forms.TextBox();
            this.musicStringTextBox = new System.Windows.Forms.TextBox();
            this.unknown6Label = new System.Windows.Forms.Label();
            this.unknown5Label = new System.Windows.Forms.Label();
            this.unknown4Label = new System.Windows.Forms.Label();
            this.unknown3Label = new System.Windows.Forms.Label();
            this.unknown2Label = new System.Windows.Forms.Label();
            this.unknown1Label = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.indexLabel = new System.Windows.Forms.Label();
            this.musicIdLabel = new System.Windows.Forms.Label();
            this.musicStringLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.editorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknown6Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown5Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown4Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown3Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexValue)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(758, 24);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // entryListBox
            // 
            this.entryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.entryListBox.FormattingEnabled = true;
            this.entryListBox.Location = new System.Drawing.Point(10, 29);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(289, 342);
            this.entryListBox.TabIndex = 1;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addEntryButton.Location = new System.Drawing.Point(97, 377);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(69, 24);
            this.addEntryButton.TabIndex = 4;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateEntryButton.Location = new System.Drawing.Point(172, 377);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(69, 24);
            this.duplicateEntryButton.TabIndex = 5;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.UseVisualStyleBackColor = true;
            this.duplicateEntryButton.Click += new System.EventHandler(this.duplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteEntryButton.Location = new System.Drawing.Point(97, 407);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(69, 24);
            this.deleteEntryButton.TabIndex = 6;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.UseVisualStyleBackColor = true;
            this.deleteEntryButton.Click += new System.EventHandler(this.deleteEntryButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveUpButton.Location = new System.Drawing.Point(10, 377);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(81, 24);
            this.moveUpButton.TabIndex = 7;
            this.moveUpButton.Text = "Move Up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveDownButton.Location = new System.Drawing.Point(10, 407);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(81, 24);
            this.moveDownButton.TabIndex = 8;
            this.moveDownButton.Text = "Move Down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // saveSelectedButton
            // 
            this.saveSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveSelectedButton.Location = new System.Drawing.Point(172, 407);
            this.saveSelectedButton.Name = "saveSelectedButton";
            this.saveSelectedButton.Size = new System.Drawing.Size(69, 24);
            this.saveSelectedButton.TabIndex = 9;
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
            this.editorPanel.Controls.Add(this.unknown6Value);
            this.editorPanel.Controls.Add(this.unknown5Value);
            this.editorPanel.Controls.Add(this.unknown4Value);
            this.editorPanel.Controls.Add(this.unknown3Value);
            this.editorPanel.Controls.Add(this.unknown2Value);
            this.editorPanel.Controls.Add(this.unknown1Value);
            this.editorPanel.Controls.Add(this.costValue);
            this.editorPanel.Controls.Add(this.indexValue);
            this.editorPanel.Controls.Add(this.musicIdTextBox);
            this.editorPanel.Controls.Add(this.musicStringTextBox);
            this.editorPanel.Controls.Add(this.unknown6Label);
            this.editorPanel.Controls.Add(this.unknown5Label);
            this.editorPanel.Controls.Add(this.unknown4Label);
            this.editorPanel.Controls.Add(this.unknown3Label);
            this.editorPanel.Controls.Add(this.unknown2Label);
            this.editorPanel.Controls.Add(this.unknown1Label);
            this.editorPanel.Controls.Add(this.costLabel);
            this.editorPanel.Controls.Add(this.indexLabel);
            this.editorPanel.Controls.Add(this.musicIdLabel);
            this.editorPanel.Controls.Add(this.musicStringLabel);
            this.editorPanel.Location = new System.Drawing.Point(312, 29);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(436, 405);
            this.editorPanel.TabIndex = 6;
            // 
            // unknown6Value
            // 
            this.unknown6Value.Location = new System.Drawing.Point(94, 261);
            this.unknown6Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown6Value.Name = "unknown6Value";
            this.unknown6Value.Size = new System.Drawing.Size(154, 20);
            this.unknown6Value.TabIndex = 19;
            // 
            // unknown5Value
            // 
            this.unknown5Value.Location = new System.Drawing.Point(94, 236);
            this.unknown5Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown5Value.Name = "unknown5Value";
            this.unknown5Value.Size = new System.Drawing.Size(154, 20);
            this.unknown5Value.TabIndex = 18;
            // 
            // unknown4Value
            // 
            this.unknown4Value.Location = new System.Drawing.Point(94, 211);
            this.unknown4Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown4Value.Name = "unknown4Value";
            this.unknown4Value.Size = new System.Drawing.Size(154, 20);
            this.unknown4Value.TabIndex = 17;
            // 
            // unknown3Value
            // 
            this.unknown3Value.Location = new System.Drawing.Point(94, 185);
            this.unknown3Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown3Value.Name = "unknown3Value";
            this.unknown3Value.Size = new System.Drawing.Size(154, 20);
            this.unknown3Value.TabIndex = 16;
            // 
            // unknown2Value
            // 
            this.unknown2Value.Location = new System.Drawing.Point(94, 160);
            this.unknown2Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown2Value.Name = "unknown2Value";
            this.unknown2Value.Size = new System.Drawing.Size(154, 20);
            this.unknown2Value.TabIndex = 11;
            // 
            // unknown1Value
            // 
            this.unknown1Value.Location = new System.Drawing.Point(94, 135);
            this.unknown1Value.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.unknown1Value.Name = "unknown1Value";
            this.unknown1Value.Size = new System.Drawing.Size(154, 20);
            this.unknown1Value.TabIndex = 10;
            // 
            // costValue
            // 
            this.costValue.Location = new System.Drawing.Point(94, 110);
            this.costValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.costValue.Name = "costValue";
            this.costValue.Size = new System.Drawing.Size(154, 20);
            this.costValue.TabIndex = 9;
            // 
            // indexValue
            // 
            this.indexValue.Location = new System.Drawing.Point(94, 85);
            this.indexValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.indexValue.Name = "indexValue";
            this.indexValue.Size = new System.Drawing.Size(154, 20);
            this.indexValue.TabIndex = 8;
            // 
            // musicIdTextBox
            // 
            this.musicIdTextBox.Location = new System.Drawing.Point(94, 35);
            this.musicIdTextBox.Name = "musicIdTextBox";
            this.musicIdTextBox.Size = new System.Drawing.Size(327, 20);
            this.musicIdTextBox.TabIndex = 7;
            // 
            // musicStringTextBox
            // 
            this.musicStringTextBox.Location = new System.Drawing.Point(94, 10);
            this.musicStringTextBox.Name = "musicStringTextBox";
            this.musicStringTextBox.Size = new System.Drawing.Size(327, 20);
            this.musicStringTextBox.TabIndex = 6;
            // 
            // unknown6Label
            // 
            this.unknown6Label.AutoSize = true;
            this.unknown6Label.Location = new System.Drawing.Point(16, 263);
            this.unknown6Label.Name = "unknown6Label";
            this.unknown6Label.Size = new System.Drawing.Size(62, 13);
            this.unknown6Label.TabIndex = 15;
            this.unknown6Label.Text = "Unknown6:";
            // 
            // unknown5Label
            // 
            this.unknown5Label.AutoSize = true;
            this.unknown5Label.Location = new System.Drawing.Point(16, 237);
            this.unknown5Label.Name = "unknown5Label";
            this.unknown5Label.Size = new System.Drawing.Size(62, 13);
            this.unknown5Label.TabIndex = 14;
            this.unknown5Label.Text = "Unknown5:";
            // 
            // unknown4Label
            // 
            this.unknown4Label.AutoSize = true;
            this.unknown4Label.Location = new System.Drawing.Point(16, 212);
            this.unknown4Label.Name = "unknown4Label";
            this.unknown4Label.Size = new System.Drawing.Size(62, 13);
            this.unknown4Label.TabIndex = 13;
            this.unknown4Label.Text = "Unknown4:";
            // 
            // unknown3Label
            // 
            this.unknown3Label.AutoSize = true;
            this.unknown3Label.Location = new System.Drawing.Point(16, 187);
            this.unknown3Label.Name = "unknown3Label";
            this.unknown3Label.Size = new System.Drawing.Size(62, 13);
            this.unknown3Label.TabIndex = 12;
            this.unknown3Label.Text = "Unknown3:";
            // 
            // unknown2Label
            // 
            this.unknown2Label.AutoSize = true;
            this.unknown2Label.Location = new System.Drawing.Point(16, 162);
            this.unknown2Label.Name = "unknown2Label";
            this.unknown2Label.Size = new System.Drawing.Size(62, 13);
            this.unknown2Label.TabIndex = 5;
            this.unknown2Label.Text = "Unknown2:";
            // 
            // unknown1Label
            // 
            this.unknown1Label.AutoSize = true;
            this.unknown1Label.Location = new System.Drawing.Point(16, 137);
            this.unknown1Label.Name = "unknown1Label";
            this.unknown1Label.Size = new System.Drawing.Size(62, 13);
            this.unknown1Label.TabIndex = 4;
            this.unknown1Label.Text = "Unknown1:";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(16, 112);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(31, 13);
            this.costLabel.TabIndex = 3;
            this.costLabel.Text = "Cost:";
            // 
            // indexLabel
            // 
            this.indexLabel.AutoSize = true;
            this.indexLabel.Location = new System.Drawing.Point(16, 87);
            this.indexLabel.Name = "indexLabel";
            this.indexLabel.Size = new System.Drawing.Size(58, 13);
            this.indexLabel.TabIndex = 2;
            this.indexLabel.Text = "Cue Index:";
            // 
            // musicIdLabel
            // 
            this.musicIdLabel.AutoSize = true;
            this.musicIdLabel.Location = new System.Drawing.Point(16, 37);
            this.musicIdLabel.Name = "musicIdLabel";
            this.musicIdLabel.Size = new System.Drawing.Size(52, 13);
            this.musicIdLabel.TabIndex = 1;
            this.musicIdLabel.Text = "Music ID:";
            // 
            // musicStringLabel
            // 
            this.musicStringLabel.AutoSize = true;
            this.musicStringLabel.Location = new System.Drawing.Point(16, 12);
            this.musicStringLabel.Name = "musicStringLabel";
            this.musicStringLabel.Size = new System.Drawing.Size(68, 13);
            this.musicStringLabel.TabIndex = 0;
            this.musicStringLabel.Text = "Music String:";
            // 
            // Tool_MusicParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 446);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.saveSelectedButton);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(774, 485);
            this.Name = "Tool_MusicParamEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music Param Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknown6Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown5Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown4Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown3Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
