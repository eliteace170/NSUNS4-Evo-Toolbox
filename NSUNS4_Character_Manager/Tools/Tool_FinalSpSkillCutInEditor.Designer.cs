using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_FinalSpSkillCutInEditor
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
        private ListBox victimListBox;
        private Button addEntryButton;
        private Button duplicateEntryButton;
        private Button deleteEntryButton;
        private Button saveSelectedButton;
        private Panel entryPanel;
        private Label storyModeIdLabel;
        private Label teamUltIdLabel;
        private Label playerSettingIdLabel;
        private Label costumeSlotLabel;
        private Label ougiName1Label;
        private Label ougiName2Label;
        private Label padding1Label;
        private Label padding2Label;
        private NumericUpDown storyModeIdValue;
        private NumericUpDown teamUltIdValue;
        private NumericUpDown playerSettingIdValue;
        private NumericUpDown costumeSlotValue;
        private TextBox ougiName1TextBox;
        private TextBox ougiName2TextBox;
        private NumericUpDown padding1Value;
        private NumericUpDown padding2Value;
        private Panel victimPanel;
        private Label victimPlayerSettingIdLabel;
        private Label victimFileNameLabel;
        private Label victimTextureNameLabel;
        private Label victimPaddingLabel;
        private NumericUpDown victimPlayerSettingIdValue;
        private TextBox victimFileNameTextBox;
        private TextBox victimTextureNameTextBox;
        private NumericUpDown victimPaddingValue;
        private Button saveVictimButton;
        private Button addVictimButton;
        private Button deleteVictimButton;
        private Panel duplicateVictimPanel;
        private Label duplicateVictimSourceIdLabel;
        private Label duplicateVictimNewIdLabel;
        private NumericUpDown duplicateVictimSourceIdValue;
        private NumericUpDown duplicateVictimNewIdValue;
        private Button duplicateVictimAcrossEntriesButton;

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
            this.victimListBox = new System.Windows.Forms.ListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.saveSelectedButton = new System.Windows.Forms.Button();
            this.entryPanel = new System.Windows.Forms.Panel();
            this.padding2Value = new System.Windows.Forms.NumericUpDown();
            this.padding1Value = new System.Windows.Forms.NumericUpDown();
            this.ougiName2TextBox = new System.Windows.Forms.TextBox();
            this.ougiName1TextBox = new System.Windows.Forms.TextBox();
            this.costumeSlotValue = new System.Windows.Forms.NumericUpDown();
            this.playerSettingIdValue = new System.Windows.Forms.NumericUpDown();
            this.teamUltIdValue = new System.Windows.Forms.NumericUpDown();
            this.storyModeIdValue = new System.Windows.Forms.NumericUpDown();
            this.padding2Label = new System.Windows.Forms.Label();
            this.padding1Label = new System.Windows.Forms.Label();
            this.ougiName2Label = new System.Windows.Forms.Label();
            this.ougiName1Label = new System.Windows.Forms.Label();
            this.costumeSlotLabel = new System.Windows.Forms.Label();
            this.playerSettingIdLabel = new System.Windows.Forms.Label();
            this.teamUltIdLabel = new System.Windows.Forms.Label();
            this.storyModeIdLabel = new System.Windows.Forms.Label();
            this.victimPanel = new System.Windows.Forms.Panel();
            this.deleteVictimButton = new System.Windows.Forms.Button();
            this.addVictimButton = new System.Windows.Forms.Button();
            this.saveVictimButton = new System.Windows.Forms.Button();
            this.victimPaddingValue = new System.Windows.Forms.NumericUpDown();
            this.victimTextureNameTextBox = new System.Windows.Forms.TextBox();
            this.victimFileNameTextBox = new System.Windows.Forms.TextBox();
            this.victimPlayerSettingIdValue = new System.Windows.Forms.NumericUpDown();
            this.victimPaddingLabel = new System.Windows.Forms.Label();
            this.victimTextureNameLabel = new System.Windows.Forms.Label();
            this.victimFileNameLabel = new System.Windows.Forms.Label();
            this.victimPlayerSettingIdLabel = new System.Windows.Forms.Label();
            this.duplicateVictimPanel = new System.Windows.Forms.Panel();
            this.duplicateVictimAcrossEntriesButton = new System.Windows.Forms.Button();
            this.duplicateVictimNewIdValue = new System.Windows.Forms.NumericUpDown();
            this.duplicateVictimSourceIdValue = new System.Windows.Forms.NumericUpDown();
            this.duplicateVictimNewIdLabel = new System.Windows.Forms.Label();
            this.duplicateVictimSourceIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.entryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.padding2Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.padding1Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeSlotValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSettingIdValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teamUltIdValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storyModeIdValue)).BeginInit();
            this.victimPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.victimPaddingValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.victimPlayerSettingIdValue)).BeginInit();
            this.duplicateVictimPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.duplicateVictimNewIdValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.duplicateVictimSourceIdValue)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(980, 26);
            this.menuStrip1.TabIndex = 0;
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
            this.entryListBox.Location = new System.Drawing.Point(10, 29);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(292, 498);
            this.entryListBox.TabIndex = 1;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // victimListBox
            // 
            this.victimListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.victimListBox.FormattingEnabled = true;
            this.victimListBox.Location = new System.Drawing.Point(314, 200);
            this.victimListBox.Name = "victimListBox";
            this.victimListBox.Size = new System.Drawing.Size(292, 355);
            this.victimListBox.TabIndex = 2;
            this.victimListBox.SelectedIndexChanged += new System.EventHandler(this.victimListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addEntryButton.Location = new System.Drawing.Point(10, 536);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(69, 24);
            this.addEntryButton.TabIndex = 3;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateEntryButton.Location = new System.Drawing.Point(84, 536);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(69, 24);
            this.duplicateEntryButton.TabIndex = 4;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.UseVisualStyleBackColor = true;
            this.duplicateEntryButton.Click += new System.EventHandler(this.duplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteEntryButton.Location = new System.Drawing.Point(232, 536);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(69, 24);
            this.deleteEntryButton.TabIndex = 5;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.UseVisualStyleBackColor = true;
            this.deleteEntryButton.Click += new System.EventHandler(this.deleteEntryButton_Click);
            // 
            // saveSelectedButton
            // 
            this.saveSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveSelectedButton.Location = new System.Drawing.Point(158, 536);
            this.saveSelectedButton.Name = "saveSelectedButton";
            this.saveSelectedButton.Size = new System.Drawing.Size(69, 24);
            this.saveSelectedButton.TabIndex = 6;
            this.saveSelectedButton.Text = "Save";
            this.saveSelectedButton.UseVisualStyleBackColor = true;
            this.saveSelectedButton.Click += new System.EventHandler(this.saveSelectedButton_Click);
            // 
            // entryPanel
            // 
            this.entryPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.entryPanel.Controls.Add(this.padding2Value);
            this.entryPanel.Controls.Add(this.padding1Value);
            this.entryPanel.Controls.Add(this.ougiName2TextBox);
            this.entryPanel.Controls.Add(this.ougiName1TextBox);
            this.entryPanel.Controls.Add(this.costumeSlotValue);
            this.entryPanel.Controls.Add(this.playerSettingIdValue);
            this.entryPanel.Controls.Add(this.teamUltIdValue);
            this.entryPanel.Controls.Add(this.storyModeIdValue);
            this.entryPanel.Controls.Add(this.padding2Label);
            this.entryPanel.Controls.Add(this.padding1Label);
            this.entryPanel.Controls.Add(this.ougiName2Label);
            this.entryPanel.Controls.Add(this.ougiName1Label);
            this.entryPanel.Controls.Add(this.costumeSlotLabel);
            this.entryPanel.Controls.Add(this.playerSettingIdLabel);
            this.entryPanel.Controls.Add(this.teamUltIdLabel);
            this.entryPanel.Controls.Add(this.storyModeIdLabel);
            this.entryPanel.Location = new System.Drawing.Point(314, 29);
            this.entryPanel.Name = "entryPanel";
            this.entryPanel.Size = new System.Drawing.Size(650, 166);
            this.entryPanel.TabIndex = 8;
            // 
            // padding2Value
            // 
            this.padding2Value.Location = new System.Drawing.Point(417, 134);
            this.padding2Value.Name = "padding2Value";
            this.padding2Value.Size = new System.Drawing.Size(160, 20);
            this.padding2Value.TabIndex = 15;
            // 
            // padding1Value
            // 
            this.padding1Value.Location = new System.Drawing.Point(122, 134);
            this.padding1Value.Name = "padding1Value";
            this.padding1Value.Size = new System.Drawing.Size(160, 20);
            this.padding1Value.TabIndex = 14;
            // 
            // ougiName2TextBox
            // 
            this.ougiName2TextBox.Location = new System.Drawing.Point(122, 105);
            this.ougiName2TextBox.Name = "ougiName2TextBox";
            this.ougiName2TextBox.Size = new System.Drawing.Size(455, 20);
            this.ougiName2TextBox.TabIndex = 13;
            // 
            // ougiName1TextBox
            // 
            this.ougiName1TextBox.Location = new System.Drawing.Point(122, 79);
            this.ougiName1TextBox.Name = "ougiName1TextBox";
            this.ougiName1TextBox.Size = new System.Drawing.Size(455, 20);
            this.ougiName1TextBox.TabIndex = 12;
            // 
            // costumeSlotValue
            // 
            this.costumeSlotValue.Location = new System.Drawing.Point(417, 51);
            this.costumeSlotValue.Name = "costumeSlotValue";
            this.costumeSlotValue.Size = new System.Drawing.Size(160, 20);
            this.costumeSlotValue.TabIndex = 11;
            // 
            // playerSettingIdValue
            // 
            this.playerSettingIdValue.Location = new System.Drawing.Point(122, 51);
            this.playerSettingIdValue.Name = "playerSettingIdValue";
            this.playerSettingIdValue.Size = new System.Drawing.Size(160, 20);
            this.playerSettingIdValue.TabIndex = 10;
            // 
            // teamUltIdValue
            // 
            this.teamUltIdValue.Location = new System.Drawing.Point(417, 25);
            this.teamUltIdValue.Name = "teamUltIdValue";
            this.teamUltIdValue.Size = new System.Drawing.Size(160, 20);
            this.teamUltIdValue.TabIndex = 9;
            // 
            // storyModeIdValue
            // 
            this.storyModeIdValue.Location = new System.Drawing.Point(122, 25);
            this.storyModeIdValue.Name = "storyModeIdValue";
            this.storyModeIdValue.Size = new System.Drawing.Size(160, 20);
            this.storyModeIdValue.TabIndex = 8;
            // 
            // padding2Label
            // 
            this.padding2Label.AutoSize = true;
            this.padding2Label.Location = new System.Drawing.Point(323, 136);
            this.padding2Label.Name = "padding2Label";
            this.padding2Label.Size = new System.Drawing.Size(53, 13);
            this.padding2Label.TabIndex = 7;
            this.padding2Label.Text = "Unknow2";
            // 
            // padding1Label
            // 
            this.padding1Label.AutoSize = true;
            this.padding1Label.Location = new System.Drawing.Point(15, 136);
            this.padding1Label.Name = "padding1Label";
            this.padding1Label.Size = new System.Drawing.Size(53, 13);
            this.padding1Label.TabIndex = 6;
            this.padding1Label.Text = "Unknow1";
            // 
            // ougiName2Label
            // 
            this.ougiName2Label.AutoSize = true;
            this.ougiName2Label.Location = new System.Drawing.Point(15, 108);
            this.ougiName2Label.Name = "ougiName2Label";
            this.ougiName2Label.Size = new System.Drawing.Size(77, 13);
            this.ougiName2Label.TabIndex = 5;
            this.ougiName2Label.Text = "OUGI Name 2:";
            // 
            // ougiName1Label
            // 
            this.ougiName1Label.AutoSize = true;
            this.ougiName1Label.Location = new System.Drawing.Point(15, 82);
            this.ougiName1Label.Name = "ougiName1Label";
            this.ougiName1Label.Size = new System.Drawing.Size(77, 13);
            this.ougiName1Label.TabIndex = 4;
            this.ougiName1Label.Text = "OUGI Name 1:";
            // 
            // costumeSlotLabel
            // 
            this.costumeSlotLabel.AutoSize = true;
            this.costumeSlotLabel.Location = new System.Drawing.Point(323, 53);
            this.costumeSlotLabel.Name = "costumeSlotLabel";
            this.costumeSlotLabel.Size = new System.Drawing.Size(72, 13);
            this.costumeSlotLabel.TabIndex = 3;
            this.costumeSlotLabel.Text = "Costume Slot:";
            // 
            // playerSettingIdLabel
            // 
            this.playerSettingIdLabel.AutoSize = true;
            this.playerSettingIdLabel.Location = new System.Drawing.Point(15, 53);
            this.playerSettingIdLabel.Name = "playerSettingIdLabel";
            this.playerSettingIdLabel.Size = new System.Drawing.Size(89, 13);
            this.playerSettingIdLabel.TabIndex = 2;
            this.playerSettingIdLabel.Text = "Player Setting ID:";
            // 
            // teamUltIdLabel
            // 
            this.teamUltIdLabel.AutoSize = true;
            this.teamUltIdLabel.Location = new System.Drawing.Point(322, 27);
            this.teamUltIdLabel.Name = "teamUltIdLabel";
            this.teamUltIdLabel.Size = new System.Drawing.Size(67, 13);
            this.teamUltIdLabel.TabIndex = 1;
            this.teamUltIdLabel.Text = "Team Ult ID:";
            // 
            // storyModeIdLabel
            // 
            this.storyModeIdLabel.AutoSize = true;
            this.storyModeIdLabel.Location = new System.Drawing.Point(14, 27);
            this.storyModeIdLabel.Name = "storyModeIdLabel";
            this.storyModeIdLabel.Size = new System.Drawing.Size(78, 13);
            this.storyModeIdLabel.TabIndex = 0;
            this.storyModeIdLabel.Text = "Story Mode ID:";
            // 
            // victimPanel
            // 
            this.victimPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.victimPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.victimPanel.Controls.Add(this.label2);
            this.victimPanel.Controls.Add(this.deleteVictimButton);
            this.victimPanel.Controls.Add(this.addVictimButton);
            this.victimPanel.Controls.Add(this.saveVictimButton);
            this.victimPanel.Controls.Add(this.victimPaddingValue);
            this.victimPanel.Controls.Add(this.victimTextureNameTextBox);
            this.victimPanel.Controls.Add(this.victimFileNameTextBox);
            this.victimPanel.Controls.Add(this.victimPlayerSettingIdValue);
            this.victimPanel.Controls.Add(this.victimPaddingLabel);
            this.victimPanel.Controls.Add(this.victimTextureNameLabel);
            this.victimPanel.Controls.Add(this.victimFileNameLabel);
            this.victimPanel.Controls.Add(this.victimPlayerSettingIdLabel);
            this.victimPanel.Location = new System.Drawing.Point(612, 201);
            this.victimPanel.Name = "victimPanel";
            this.victimPanel.Size = new System.Drawing.Size(352, 208);
            this.victimPanel.TabIndex = 9;
            // 
            // deleteVictimButton
            // 
            this.deleteVictimButton.Location = new System.Drawing.Point(126, 172);
            this.deleteVictimButton.Name = "deleteVictimButton";
            this.deleteVictimButton.Size = new System.Drawing.Size(103, 24);
            this.deleteVictimButton.TabIndex = 10;
            this.deleteVictimButton.Text = "Delete";
            this.deleteVictimButton.UseVisualStyleBackColor = true;
            this.deleteVictimButton.Click += new System.EventHandler(this.deleteVictimButton_Click);
            // 
            // addVictimButton
            // 
            this.addVictimButton.Location = new System.Drawing.Point(17, 172);
            this.addVictimButton.Name = "addVictimButton";
            this.addVictimButton.Size = new System.Drawing.Size(103, 24);
            this.addVictimButton.TabIndex = 9;
            this.addVictimButton.Text = "Add/Duplicate";
            this.addVictimButton.UseVisualStyleBackColor = true;
            this.addVictimButton.Click += new System.EventHandler(this.addVictimButton_Click);
            // 
            // saveVictimButton
            // 
            this.saveVictimButton.Location = new System.Drawing.Point(235, 172);
            this.saveVictimButton.Name = "saveVictimButton";
            this.saveVictimButton.Size = new System.Drawing.Size(104, 24);
            this.saveVictimButton.TabIndex = 8;
            this.saveVictimButton.Text = "Save";
            this.saveVictimButton.UseVisualStyleBackColor = true;
            this.saveVictimButton.Click += new System.EventHandler(this.saveVictimButton_Click);
            // 
            // victimPaddingValue
            // 
            this.victimPaddingValue.Location = new System.Drawing.Point(119, 134);
            this.victimPaddingValue.Name = "victimPaddingValue";
            this.victimPaddingValue.Size = new System.Drawing.Size(220, 20);
            this.victimPaddingValue.TabIndex = 7;
            // 
            // victimTextureNameTextBox
            // 
            this.victimTextureNameTextBox.Location = new System.Drawing.Point(119, 100);
            this.victimTextureNameTextBox.Name = "victimTextureNameTextBox";
            this.victimTextureNameTextBox.Size = new System.Drawing.Size(220, 20);
            this.victimTextureNameTextBox.TabIndex = 6;
            // 
            // victimFileNameTextBox
            // 
            this.victimFileNameTextBox.Location = new System.Drawing.Point(119, 66);
            this.victimFileNameTextBox.Name = "victimFileNameTextBox";
            this.victimFileNameTextBox.Size = new System.Drawing.Size(220, 20);
            this.victimFileNameTextBox.TabIndex = 5;
            // 
            // victimPlayerSettingIdValue
            // 
            this.victimPlayerSettingIdValue.Location = new System.Drawing.Point(119, 32);
            this.victimPlayerSettingIdValue.Name = "victimPlayerSettingIdValue";
            this.victimPlayerSettingIdValue.Size = new System.Drawing.Size(220, 20);
            this.victimPlayerSettingIdValue.TabIndex = 4;
            // 
            // victimPaddingLabel
            // 
            this.victimPaddingLabel.AutoSize = true;
            this.victimPaddingLabel.Location = new System.Drawing.Point(14, 136);
            this.victimPaddingLabel.Name = "victimPaddingLabel";
            this.victimPaddingLabel.Size = new System.Drawing.Size(47, 13);
            this.victimPaddingLabel.TabIndex = 3;
            this.victimPaddingLabel.Text = "Unknow";
            // 
            // victimTextureNameLabel
            // 
            this.victimTextureNameLabel.AutoSize = true;
            this.victimTextureNameLabel.Location = new System.Drawing.Point(14, 103);
            this.victimTextureNameLabel.Name = "victimTextureNameLabel";
            this.victimTextureNameLabel.Size = new System.Drawing.Size(77, 13);
            this.victimTextureNameLabel.TabIndex = 2;
            this.victimTextureNameLabel.Text = "Texture Name:";
            // 
            // victimFileNameLabel
            // 
            this.victimFileNameLabel.AutoSize = true;
            this.victimFileNameLabel.Location = new System.Drawing.Point(14, 69);
            this.victimFileNameLabel.Name = "victimFileNameLabel";
            this.victimFileNameLabel.Size = new System.Drawing.Size(57, 13);
            this.victimFileNameLabel.TabIndex = 1;
            this.victimFileNameLabel.Text = "File Name:";
            // 
            // victimPlayerSettingIdLabel
            // 
            this.victimPlayerSettingIdLabel.AutoSize = true;
            this.victimPlayerSettingIdLabel.Location = new System.Drawing.Point(14, 34);
            this.victimPlayerSettingIdLabel.Name = "victimPlayerSettingIdLabel";
            this.victimPlayerSettingIdLabel.Size = new System.Drawing.Size(89, 13);
            this.victimPlayerSettingIdLabel.TabIndex = 0;
            this.victimPlayerSettingIdLabel.Text = "Player Setting ID:";
            // 
            // duplicateVictimPanel
            // 
            this.duplicateVictimPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateVictimPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.duplicateVictimPanel.Controls.Add(this.label1);
            this.duplicateVictimPanel.Controls.Add(this.duplicateVictimAcrossEntriesButton);
            this.duplicateVictimPanel.Controls.Add(this.duplicateVictimNewIdValue);
            this.duplicateVictimPanel.Controls.Add(this.duplicateVictimSourceIdValue);
            this.duplicateVictimPanel.Controls.Add(this.duplicateVictimNewIdLabel);
            this.duplicateVictimPanel.Controls.Add(this.duplicateVictimSourceIdLabel);
            this.duplicateVictimPanel.Location = new System.Drawing.Point(616, 415);
            this.duplicateVictimPanel.Name = "duplicateVictimPanel";
            this.duplicateVictimPanel.Size = new System.Drawing.Size(348, 140);
            this.duplicateVictimPanel.TabIndex = 10;
            // 
            // duplicateVictimAcrossEntriesButton
            // 
            this.duplicateVictimAcrossEntriesButton.Location = new System.Drawing.Point(13, 104);
            this.duplicateVictimAcrossEntriesButton.Name = "duplicateVictimAcrossEntriesButton";
            this.duplicateVictimAcrossEntriesButton.Size = new System.Drawing.Size(318, 24);
            this.duplicateVictimAcrossEntriesButton.TabIndex = 4;
            this.duplicateVictimAcrossEntriesButton.Text = "Duplicate Across Entries";
            this.duplicateVictimAcrossEntriesButton.UseVisualStyleBackColor = true;
            this.duplicateVictimAcrossEntriesButton.Click += new System.EventHandler(this.duplicateVictimAcrossEntriesButton_Click);
            // 
            // duplicateVictimNewIdValue
            // 
            this.duplicateVictimNewIdValue.Location = new System.Drawing.Point(171, 69);
            this.duplicateVictimNewIdValue.Name = "duplicateVictimNewIdValue";
            this.duplicateVictimNewIdValue.Size = new System.Drawing.Size(160, 20);
            this.duplicateVictimNewIdValue.TabIndex = 3;
            // 
            // duplicateVictimSourceIdValue
            // 
            this.duplicateVictimSourceIdValue.Location = new System.Drawing.Point(171, 34);
            this.duplicateVictimSourceIdValue.Name = "duplicateVictimSourceIdValue";
            this.duplicateVictimSourceIdValue.Size = new System.Drawing.Size(160, 20);
            this.duplicateVictimSourceIdValue.TabIndex = 2;
            // 
            // duplicateVictimNewIdLabel
            // 
            this.duplicateVictimNewIdLabel.AutoSize = true;
            this.duplicateVictimNewIdLabel.Location = new System.Drawing.Point(10, 71);
            this.duplicateVictimNewIdLabel.Name = "duplicateVictimNewIdLabel";
            this.duplicateVictimNewIdLabel.Size = new System.Drawing.Size(164, 13);
            this.duplicateVictimNewIdLabel.TabIndex = 1;
            this.duplicateVictimNewIdLabel.Text = "Duplicate PlayerSettingParam ID:";
            // 
            // duplicateVictimSourceIdLabel
            // 
            this.duplicateVictimSourceIdLabel.AutoSize = true;
            this.duplicateVictimSourceIdLabel.Location = new System.Drawing.Point(10, 36);
            this.duplicateVictimSourceIdLabel.Name = "duplicateVictimSourceIdLabel";
            this.duplicateVictimSourceIdLabel.Size = new System.Drawing.Size(153, 13);
            this.duplicateVictimSourceIdLabel.TabIndex = 0;
            this.duplicateVictimSourceIdLabel.Text = "Source PlayerSettingParam ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label1.Location = new System.Drawing.Point(111, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Duplication Panel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label2.Location = new System.Drawing.Point(115, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Victims Panel";
            // 
            // Tool_FinalSpSkillCutInEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 571);
            this.Controls.Add(this.duplicateVictimPanel);
            this.Controls.Add(this.victimPanel);
            this.Controls.Add(this.entryPanel);
            this.Controls.Add(this.saveSelectedButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.victimListBox);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(996, 610);
            this.Name = "Tool_FinalSpSkillCutInEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Final Sp Skill CutIn Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.entryPanel.ResumeLayout(false);
            this.entryPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.padding2Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.padding1Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeSlotValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSettingIdValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teamUltIdValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storyModeIdValue)).EndInit();
            this.victimPanel.ResumeLayout(false);
            this.victimPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.victimPaddingValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.victimPlayerSettingIdValue)).EndInit();
            this.duplicateVictimPanel.ResumeLayout(false);
            this.duplicateVictimPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.duplicateVictimNewIdValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.duplicateVictimSourceIdValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
        private Label label2;
    }
}
