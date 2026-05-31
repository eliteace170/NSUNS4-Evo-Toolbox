using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_ItemInfoEditor
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
        private Label battleItemNameLabel;
        private TextBox battleItemNameTextBox;
        private Label unknown1Label;
        private NumericUpDown unknown1Value;
        private Label itemPaddingLabel;
        private NumericUpDown itemPaddingValue;
        private Label battleIconName1Label;
        private TextBox battleIconName1TextBox;
        private Label battleIconName2Label;
        private TextBox battleIconName2TextBox;
        private Label battleIconName3Label;
        private TextBox battleIconName3TextBox;
        private Label unknown2Label;
        private NumericUpDown unknown2Value;
        private Label unknown3Label;
        private NumericUpDown unknown3Value;
        private Label unknown4Label;
        private NumericUpDown unknown4Value;
        private Label unknown5Label;
        private NumericUpDown unknown5Value;
        private Label unknown6Label;
        private NumericUpDown unknown6Value;
        private Label unknown7Label;
        private NumericUpDown unknown7Value;
        private Label itemNameCommentLabel;
        private TextBox itemNameCommentTextBox;
        private Label unknown9Label;
        private NumericUpDown unknown9Value;
        private Label unknown10Label;
        private NumericUpDown unknown10Value;
        private Label unknown11Label;
        private NumericUpDown unknown11Value;
        private Label unknown12Label;
        private NumericUpDown unknown12Value;
        private Label conditionPrmNameLabel;
        private TextBox conditionPrmNameTextBox;
        private Label unknown13Label;
        private NumericUpDown unknown13Value;
        private Label unknown14Label;
        private NumericUpDown unknown14Value;
        private Label unknown15Label;
        private NumericUpDown unknown15Value;
        private Label unknown16Label;
        private NumericUpDown unknown16Value;
        private Label unknown17Label;
        private NumericUpDown unknown17Value;
        private Label unknown18Label;
        private NumericUpDown unknown18Value;

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
            this.unknown18Value = new System.Windows.Forms.NumericUpDown();
            this.unknown18Label = new System.Windows.Forms.Label();
            this.unknown17Value = new System.Windows.Forms.NumericUpDown();
            this.unknown17Label = new System.Windows.Forms.Label();
            this.unknown16Value = new System.Windows.Forms.NumericUpDown();
            this.unknown16Label = new System.Windows.Forms.Label();
            this.unknown15Value = new System.Windows.Forms.NumericUpDown();
            this.unknown15Label = new System.Windows.Forms.Label();
            this.unknown14Value = new System.Windows.Forms.NumericUpDown();
            this.unknown14Label = new System.Windows.Forms.Label();
            this.unknown13Value = new System.Windows.Forms.NumericUpDown();
            this.unknown13Label = new System.Windows.Forms.Label();
            this.conditionPrmNameTextBox = new System.Windows.Forms.TextBox();
            this.conditionPrmNameLabel = new System.Windows.Forms.Label();
            this.unknown12Value = new System.Windows.Forms.NumericUpDown();
            this.unknown12Label = new System.Windows.Forms.Label();
            this.unknown11Value = new System.Windows.Forms.NumericUpDown();
            this.unknown11Label = new System.Windows.Forms.Label();
            this.unknown10Value = new System.Windows.Forms.NumericUpDown();
            this.unknown10Label = new System.Windows.Forms.Label();
            this.unknown9Value = new System.Windows.Forms.NumericUpDown();
            this.unknown9Label = new System.Windows.Forms.Label();
            this.itemNameCommentTextBox = new System.Windows.Forms.TextBox();
            this.itemNameCommentLabel = new System.Windows.Forms.Label();
            this.unknown7Value = new System.Windows.Forms.NumericUpDown();
            this.unknown7Label = new System.Windows.Forms.Label();
            this.unknown6Value = new System.Windows.Forms.NumericUpDown();
            this.unknown6Label = new System.Windows.Forms.Label();
            this.unknown5Value = new System.Windows.Forms.NumericUpDown();
            this.unknown5Label = new System.Windows.Forms.Label();
            this.unknown4Value = new System.Windows.Forms.NumericUpDown();
            this.unknown4Label = new System.Windows.Forms.Label();
            this.unknown3Value = new System.Windows.Forms.NumericUpDown();
            this.unknown3Label = new System.Windows.Forms.Label();
            this.unknown2Value = new System.Windows.Forms.NumericUpDown();
            this.unknown2Label = new System.Windows.Forms.Label();
            this.battleIconName3TextBox = new System.Windows.Forms.TextBox();
            this.battleIconName3Label = new System.Windows.Forms.Label();
            this.battleIconName2TextBox = new System.Windows.Forms.TextBox();
            this.battleIconName2Label = new System.Windows.Forms.Label();
            this.battleIconName1TextBox = new System.Windows.Forms.TextBox();
            this.battleIconName1Label = new System.Windows.Forms.Label();
            this.itemPaddingValue = new System.Windows.Forms.NumericUpDown();
            this.itemPaddingLabel = new System.Windows.Forms.Label();
            this.unknown1Value = new System.Windows.Forms.NumericUpDown();
            this.unknown1Label = new System.Windows.Forms.Label();
            this.battleItemNameTextBox = new System.Windows.Forms.TextBox();
            this.battleItemNameLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.editorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknown18Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown17Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown16Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown15Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown14Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown13Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown12Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown11Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown10Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown9Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown7Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown6Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown5Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown4Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown3Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemPaddingValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1Value)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(1030, 26);
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
            this.entryListBox.Size = new System.Drawing.Size(462, 472);
            this.entryListBox.TabIndex = 1;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addEntryButton.Location = new System.Drawing.Point(10, 511);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(74, 24);
            this.addEntryButton.TabIndex = 2;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateEntryButton.Location = new System.Drawing.Point(90, 511);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(74, 24);
            this.duplicateEntryButton.TabIndex = 3;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.UseVisualStyleBackColor = true;
            this.duplicateEntryButton.Click += new System.EventHandler(this.duplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteEntryButton.Location = new System.Drawing.Point(250, 511);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(74, 24);
            this.deleteEntryButton.TabIndex = 4;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.UseVisualStyleBackColor = true;
            this.deleteEntryButton.Click += new System.EventHandler(this.deleteEntryButton_Click);
            // 
            // saveSelectedButton
            // 
            this.saveSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveSelectedButton.Location = new System.Drawing.Point(170, 511);
            this.saveSelectedButton.Name = "saveSelectedButton";
            this.saveSelectedButton.Size = new System.Drawing.Size(74, 24);
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
            this.editorPanel.AutoScroll = true;
            this.editorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorPanel.Controls.Add(this.unknown18Value);
            this.editorPanel.Controls.Add(this.unknown18Label);
            this.editorPanel.Controls.Add(this.unknown17Value);
            this.editorPanel.Controls.Add(this.unknown17Label);
            this.editorPanel.Controls.Add(this.unknown16Value);
            this.editorPanel.Controls.Add(this.unknown16Label);
            this.editorPanel.Controls.Add(this.unknown15Value);
            this.editorPanel.Controls.Add(this.unknown15Label);
            this.editorPanel.Controls.Add(this.unknown14Value);
            this.editorPanel.Controls.Add(this.unknown14Label);
            this.editorPanel.Controls.Add(this.unknown13Value);
            this.editorPanel.Controls.Add(this.unknown13Label);
            this.editorPanel.Controls.Add(this.conditionPrmNameTextBox);
            this.editorPanel.Controls.Add(this.conditionPrmNameLabel);
            this.editorPanel.Controls.Add(this.unknown12Value);
            this.editorPanel.Controls.Add(this.unknown12Label);
            this.editorPanel.Controls.Add(this.unknown11Value);
            this.editorPanel.Controls.Add(this.unknown11Label);
            this.editorPanel.Controls.Add(this.unknown10Value);
            this.editorPanel.Controls.Add(this.unknown10Label);
            this.editorPanel.Controls.Add(this.unknown9Value);
            this.editorPanel.Controls.Add(this.unknown9Label);
            this.editorPanel.Controls.Add(this.itemNameCommentTextBox);
            this.editorPanel.Controls.Add(this.itemNameCommentLabel);
            this.editorPanel.Controls.Add(this.unknown7Value);
            this.editorPanel.Controls.Add(this.unknown7Label);
            this.editorPanel.Controls.Add(this.unknown6Value);
            this.editorPanel.Controls.Add(this.unknown6Label);
            this.editorPanel.Controls.Add(this.unknown5Value);
            this.editorPanel.Controls.Add(this.unknown5Label);
            this.editorPanel.Controls.Add(this.unknown4Value);
            this.editorPanel.Controls.Add(this.unknown4Label);
            this.editorPanel.Controls.Add(this.unknown3Value);
            this.editorPanel.Controls.Add(this.unknown3Label);
            this.editorPanel.Controls.Add(this.unknown2Value);
            this.editorPanel.Controls.Add(this.unknown2Label);
            this.editorPanel.Controls.Add(this.battleIconName3TextBox);
            this.editorPanel.Controls.Add(this.battleIconName3Label);
            this.editorPanel.Controls.Add(this.battleIconName2TextBox);
            this.editorPanel.Controls.Add(this.battleIconName2Label);
            this.editorPanel.Controls.Add(this.battleIconName1TextBox);
            this.editorPanel.Controls.Add(this.battleIconName1Label);
            this.editorPanel.Controls.Add(this.itemPaddingValue);
            this.editorPanel.Controls.Add(this.itemPaddingLabel);
            this.editorPanel.Controls.Add(this.unknown1Value);
            this.editorPanel.Controls.Add(this.unknown1Label);
            this.editorPanel.Controls.Add(this.battleItemNameTextBox);
            this.editorPanel.Controls.Add(this.battleItemNameLabel);
            this.editorPanel.Location = new System.Drawing.Point(478, 29);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(542, 506);
            this.editorPanel.TabIndex = 7;
            // 
            // unknown18Value
            // 
            this.unknown18Value.Location = new System.Drawing.Point(378, 380);
            this.unknown18Value.Name = "unknown18Value";
            this.unknown18Value.Size = new System.Drawing.Size(154, 20);
            this.unknown18Value.TabIndex = 47;
            // 
            // unknown18Label
            // 
            this.unknown18Label.AutoSize = true;
            this.unknown18Label.Location = new System.Drawing.Point(286, 382);
            this.unknown18Label.Name = "unknown18Label";
            this.unknown18Label.Size = new System.Drawing.Size(68, 13);
            this.unknown18Label.TabIndex = 46;
            this.unknown18Label.Text = "Unknown18:";
            // 
            // unknown17Value
            // 
            this.unknown17Value.Location = new System.Drawing.Point(106, 380);
            this.unknown17Value.Name = "unknown17Value";
            this.unknown17Value.Size = new System.Drawing.Size(154, 20);
            this.unknown17Value.TabIndex = 45;
            // 
            // unknown17Label
            // 
            this.unknown17Label.AutoSize = true;
            this.unknown17Label.Location = new System.Drawing.Point(14, 382);
            this.unknown17Label.Name = "unknown17Label";
            this.unknown17Label.Size = new System.Drawing.Size(68, 13);
            this.unknown17Label.TabIndex = 44;
            this.unknown17Label.Text = "Unknown17:";
            // 
            // unknown16Value
            // 
            this.unknown16Value.Location = new System.Drawing.Point(378, 354);
            this.unknown16Value.Name = "unknown16Value";
            this.unknown16Value.Size = new System.Drawing.Size(154, 20);
            this.unknown16Value.TabIndex = 43;
            // 
            // unknown16Label
            // 
            this.unknown16Label.AutoSize = true;
            this.unknown16Label.Location = new System.Drawing.Point(286, 356);
            this.unknown16Label.Name = "unknown16Label";
            this.unknown16Label.Size = new System.Drawing.Size(68, 13);
            this.unknown16Label.TabIndex = 42;
            this.unknown16Label.Text = "Unknown16:";
            // 
            // unknown15Value
            // 
            this.unknown15Value.Location = new System.Drawing.Point(106, 354);
            this.unknown15Value.Name = "unknown15Value";
            this.unknown15Value.Size = new System.Drawing.Size(154, 20);
            this.unknown15Value.TabIndex = 41;
            // 
            // unknown15Label
            // 
            this.unknown15Label.AutoSize = true;
            this.unknown15Label.Location = new System.Drawing.Point(14, 356);
            this.unknown15Label.Name = "unknown15Label";
            this.unknown15Label.Size = new System.Drawing.Size(68, 13);
            this.unknown15Label.TabIndex = 40;
            this.unknown15Label.Text = "Unknown15:";
            // 
            // unknown14Value
            // 
            this.unknown14Value.Location = new System.Drawing.Point(378, 328);
            this.unknown14Value.Name = "unknown14Value";
            this.unknown14Value.Size = new System.Drawing.Size(154, 20);
            this.unknown14Value.TabIndex = 39;
            // 
            // unknown14Label
            // 
            this.unknown14Label.AutoSize = true;
            this.unknown14Label.Location = new System.Drawing.Point(286, 330);
            this.unknown14Label.Name = "unknown14Label";
            this.unknown14Label.Size = new System.Drawing.Size(68, 13);
            this.unknown14Label.TabIndex = 38;
            this.unknown14Label.Text = "Unknown14:";
            // 
            // unknown13Value
            // 
            this.unknown13Value.Location = new System.Drawing.Point(106, 328);
            this.unknown13Value.Name = "unknown13Value";
            this.unknown13Value.Size = new System.Drawing.Size(154, 20);
            this.unknown13Value.TabIndex = 37;
            // 
            // unknown13Label
            // 
            this.unknown13Label.AutoSize = true;
            this.unknown13Label.Location = new System.Drawing.Point(14, 330);
            this.unknown13Label.Name = "unknown13Label";
            this.unknown13Label.Size = new System.Drawing.Size(68, 13);
            this.unknown13Label.TabIndex = 36;
            this.unknown13Label.Text = "Unknown13:";
            // 
            // conditionPrmNameTextBox
            // 
            this.conditionPrmNameTextBox.Location = new System.Drawing.Point(106, 302);
            this.conditionPrmNameTextBox.Name = "conditionPrmNameTextBox";
            this.conditionPrmNameTextBox.Size = new System.Drawing.Size(426, 20);
            this.conditionPrmNameTextBox.TabIndex = 35;
            // 
            // conditionPrmNameLabel
            // 
            this.conditionPrmNameLabel.AutoSize = true;
            this.conditionPrmNameLabel.Location = new System.Drawing.Point(14, 304);
            this.conditionPrmNameLabel.Name = "conditionPrmNameLabel";
            this.conditionPrmNameLabel.Size = new System.Drawing.Size(75, 13);
            this.conditionPrmNameLabel.TabIndex = 34;
            this.conditionPrmNameLabel.Text = "Condition Prm:";
            // 
            // unknown12Value
            // 
            this.unknown12Value.Location = new System.Drawing.Point(378, 276);
            this.unknown12Value.Name = "unknown12Value";
            this.unknown12Value.Size = new System.Drawing.Size(154, 20);
            this.unknown12Value.TabIndex = 33;
            // 
            // unknown12Label
            // 
            this.unknown12Label.AutoSize = true;
            this.unknown12Label.Location = new System.Drawing.Point(286, 278);
            this.unknown12Label.Name = "unknown12Label";
            this.unknown12Label.Size = new System.Drawing.Size(68, 13);
            this.unknown12Label.TabIndex = 32;
            this.unknown12Label.Text = "Unknown12:";
            // 
            // unknown11Value
            // 
            this.unknown11Value.Location = new System.Drawing.Point(106, 276);
            this.unknown11Value.Name = "unknown11Value";
            this.unknown11Value.Size = new System.Drawing.Size(154, 20);
            this.unknown11Value.TabIndex = 31;
            // 
            // unknown11Label
            // 
            this.unknown11Label.AutoSize = true;
            this.unknown11Label.Location = new System.Drawing.Point(14, 278);
            this.unknown11Label.Name = "unknown11Label";
            this.unknown11Label.Size = new System.Drawing.Size(68, 13);
            this.unknown11Label.TabIndex = 30;
            this.unknown11Label.Text = "Unknown11:";
            // 
            // unknown10Value
            // 
            this.unknown10Value.Location = new System.Drawing.Point(378, 250);
            this.unknown10Value.Name = "unknown10Value";
            this.unknown10Value.Size = new System.Drawing.Size(154, 20);
            this.unknown10Value.TabIndex = 29;
            // 
            // unknown10Label
            // 
            this.unknown10Label.AutoSize = true;
            this.unknown10Label.Location = new System.Drawing.Point(286, 252);
            this.unknown10Label.Name = "unknown10Label";
            this.unknown10Label.Size = new System.Drawing.Size(68, 13);
            this.unknown10Label.TabIndex = 28;
            this.unknown10Label.Text = "Unknown10:";
            // 
            // unknown9Value
            // 
            this.unknown9Value.Location = new System.Drawing.Point(106, 250);
            this.unknown9Value.Name = "unknown9Value";
            this.unknown9Value.Size = new System.Drawing.Size(154, 20);
            this.unknown9Value.TabIndex = 27;
            // 
            // unknown9Label
            // 
            this.unknown9Label.AutoSize = true;
            this.unknown9Label.Location = new System.Drawing.Point(14, 252);
            this.unknown9Label.Name = "unknown9Label";
            this.unknown9Label.Size = new System.Drawing.Size(62, 13);
            this.unknown9Label.TabIndex = 26;
            this.unknown9Label.Text = "Unknown9:";
            // 
            // itemNameCommentTextBox
            // 
            this.itemNameCommentTextBox.Location = new System.Drawing.Point(106, 224);
            this.itemNameCommentTextBox.Multiline = true;
            this.itemNameCommentTextBox.Name = "itemNameCommentTextBox";
            this.itemNameCommentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.itemNameCommentTextBox.Size = new System.Drawing.Size(426, 20);
            this.itemNameCommentTextBox.TabIndex = 25;
            // 
            // itemNameCommentLabel
            // 
            this.itemNameCommentLabel.AutoSize = true;
            this.itemNameCommentLabel.Location = new System.Drawing.Point(15, 227);
            this.itemNameCommentLabel.Name = "itemNameCommentLabel";
            this.itemNameCommentLabel.Size = new System.Drawing.Size(85, 13);
            this.itemNameCommentLabel.TabIndex = 24;
            this.itemNameCommentLabel.Text = "Item Name Text:";
            // 
            // unknown7Value
            // 
            this.unknown7Value.Location = new System.Drawing.Point(378, 198);
            this.unknown7Value.Name = "unknown7Value";
            this.unknown7Value.Size = new System.Drawing.Size(154, 20);
            this.unknown7Value.TabIndex = 23;
            // 
            // unknown7Label
            // 
            this.unknown7Label.AutoSize = true;
            this.unknown7Label.Location = new System.Drawing.Point(286, 200);
            this.unknown7Label.Name = "unknown7Label";
            this.unknown7Label.Size = new System.Drawing.Size(62, 13);
            this.unknown7Label.TabIndex = 22;
            this.unknown7Label.Text = "Unknown7:";
            // 
            // unknown6Value
            // 
            this.unknown6Value.Location = new System.Drawing.Point(106, 198);
            this.unknown6Value.Name = "unknown6Value";
            this.unknown6Value.Size = new System.Drawing.Size(154, 20);
            this.unknown6Value.TabIndex = 21;
            // 
            // unknown6Label
            // 
            this.unknown6Label.AutoSize = true;
            this.unknown6Label.Location = new System.Drawing.Point(14, 200);
            this.unknown6Label.Name = "unknown6Label";
            this.unknown6Label.Size = new System.Drawing.Size(62, 13);
            this.unknown6Label.TabIndex = 20;
            this.unknown6Label.Text = "Unknown6:";
            // 
            // unknown5Value
            // 
            this.unknown5Value.Location = new System.Drawing.Point(378, 172);
            this.unknown5Value.Name = "unknown5Value";
            this.unknown5Value.Size = new System.Drawing.Size(154, 20);
            this.unknown5Value.TabIndex = 19;
            // 
            // unknown5Label
            // 
            this.unknown5Label.AutoSize = true;
            this.unknown5Label.Location = new System.Drawing.Point(286, 174);
            this.unknown5Label.Name = "unknown5Label";
            this.unknown5Label.Size = new System.Drawing.Size(62, 13);
            this.unknown5Label.TabIndex = 18;
            this.unknown5Label.Text = "Unknown5:";
            // 
            // unknown4Value
            // 
            this.unknown4Value.Location = new System.Drawing.Point(106, 172);
            this.unknown4Value.Name = "unknown4Value";
            this.unknown4Value.Size = new System.Drawing.Size(154, 20);
            this.unknown4Value.TabIndex = 17;
            // 
            // unknown4Label
            // 
            this.unknown4Label.AutoSize = true;
            this.unknown4Label.Location = new System.Drawing.Point(14, 174);
            this.unknown4Label.Name = "unknown4Label";
            this.unknown4Label.Size = new System.Drawing.Size(62, 13);
            this.unknown4Label.TabIndex = 16;
            this.unknown4Label.Text = "Unknown4:";
            // 
            // unknown3Value
            // 
            this.unknown3Value.Location = new System.Drawing.Point(378, 146);
            this.unknown3Value.Name = "unknown3Value";
            this.unknown3Value.Size = new System.Drawing.Size(154, 20);
            this.unknown3Value.TabIndex = 15;
            // 
            // unknown3Label
            // 
            this.unknown3Label.AutoSize = true;
            this.unknown3Label.Location = new System.Drawing.Point(286, 148);
            this.unknown3Label.Name = "unknown3Label";
            this.unknown3Label.Size = new System.Drawing.Size(62, 13);
            this.unknown3Label.TabIndex = 14;
            this.unknown3Label.Text = "Unknown3:";
            // 
            // unknown2Value
            // 
            this.unknown2Value.Location = new System.Drawing.Point(106, 146);
            this.unknown2Value.Name = "unknown2Value";
            this.unknown2Value.Size = new System.Drawing.Size(154, 20);
            this.unknown2Value.TabIndex = 13;
            // 
            // unknown2Label
            // 
            this.unknown2Label.AutoSize = true;
            this.unknown2Label.Location = new System.Drawing.Point(14, 148);
            this.unknown2Label.Name = "unknown2Label";
            this.unknown2Label.Size = new System.Drawing.Size(62, 13);
            this.unknown2Label.TabIndex = 12;
            this.unknown2Label.Text = "Unknown2:";
            // 
            // battleIconName3TextBox
            // 
            this.battleIconName3TextBox.Location = new System.Drawing.Point(106, 120);
            this.battleIconName3TextBox.Name = "battleIconName3TextBox";
            this.battleIconName3TextBox.Size = new System.Drawing.Size(426, 20);
            this.battleIconName3TextBox.TabIndex = 11;
            // 
            // battleIconName3Label
            // 
            this.battleIconName3Label.AutoSize = true;
            this.battleIconName3Label.Location = new System.Drawing.Point(14, 122);
            this.battleIconName3Label.Name = "battleIconName3Label";
            this.battleIconName3Label.Size = new System.Drawing.Size(70, 13);
            this.battleIconName3Label.TabIndex = 10;
            this.battleIconName3Label.Text = "Battle Icon 3:";
            // 
            // battleIconName2TextBox
            // 
            this.battleIconName2TextBox.Location = new System.Drawing.Point(106, 94);
            this.battleIconName2TextBox.Name = "battleIconName2TextBox";
            this.battleIconName2TextBox.Size = new System.Drawing.Size(426, 20);
            this.battleIconName2TextBox.TabIndex = 9;
            // 
            // battleIconName2Label
            // 
            this.battleIconName2Label.AutoSize = true;
            this.battleIconName2Label.Location = new System.Drawing.Point(14, 96);
            this.battleIconName2Label.Name = "battleIconName2Label";
            this.battleIconName2Label.Size = new System.Drawing.Size(70, 13);
            this.battleIconName2Label.TabIndex = 8;
            this.battleIconName2Label.Text = "Battle Icon 2:";
            // 
            // battleIconName1TextBox
            // 
            this.battleIconName1TextBox.Location = new System.Drawing.Point(106, 68);
            this.battleIconName1TextBox.Name = "battleIconName1TextBox";
            this.battleIconName1TextBox.Size = new System.Drawing.Size(426, 20);
            this.battleIconName1TextBox.TabIndex = 7;
            // 
            // battleIconName1Label
            // 
            this.battleIconName1Label.AutoSize = true;
            this.battleIconName1Label.Location = new System.Drawing.Point(14, 70);
            this.battleIconName1Label.Name = "battleIconName1Label";
            this.battleIconName1Label.Size = new System.Drawing.Size(70, 13);
            this.battleIconName1Label.TabIndex = 6;
            this.battleIconName1Label.Text = "Battle Icon 1:";
            // 
            // itemPaddingValue
            // 
            this.itemPaddingValue.Location = new System.Drawing.Point(378, 42);
            this.itemPaddingValue.Name = "itemPaddingValue";
            this.itemPaddingValue.Size = new System.Drawing.Size(154, 20);
            this.itemPaddingValue.TabIndex = 5;
            // 
            // itemPaddingLabel
            // 
            this.itemPaddingLabel.AutoSize = true;
            this.itemPaddingLabel.Location = new System.Drawing.Point(286, 44);
            this.itemPaddingLabel.Name = "itemPaddingLabel";
            this.itemPaddingLabel.Size = new System.Drawing.Size(72, 13);
            this.itemPaddingLabel.TabIndex = 4;
            this.itemPaddingLabel.Text = "Item Padding:";
            // 
            // unknown1Value
            // 
            this.unknown1Value.Location = new System.Drawing.Point(106, 42);
            this.unknown1Value.Name = "unknown1Value";
            this.unknown1Value.Size = new System.Drawing.Size(154, 20);
            this.unknown1Value.TabIndex = 3;
            // 
            // unknown1Label
            // 
            this.unknown1Label.AutoSize = true;
            this.unknown1Label.Location = new System.Drawing.Point(14, 44);
            this.unknown1Label.Name = "unknown1Label";
            this.unknown1Label.Size = new System.Drawing.Size(62, 13);
            this.unknown1Label.TabIndex = 2;
            this.unknown1Label.Text = "Unknown1:";
            // 
            // battleItemNameTextBox
            // 
            this.battleItemNameTextBox.Location = new System.Drawing.Point(106, 16);
            this.battleItemNameTextBox.Multiline = true;
            this.battleItemNameTextBox.Name = "battleItemNameTextBox";
            this.battleItemNameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.battleItemNameTextBox.Size = new System.Drawing.Size(426, 20);
            this.battleItemNameTextBox.TabIndex = 1;
            // 
            // battleItemNameLabel
            // 
            this.battleItemNameLabel.AutoSize = true;
            this.battleItemNameLabel.Location = new System.Drawing.Point(14, 16);
            this.battleItemNameLabel.Name = "battleItemNameLabel";
            this.battleItemNameLabel.Size = new System.Drawing.Size(84, 13);
            this.battleItemNameLabel.TabIndex = 0;
            this.battleItemNameLabel.Text = "Battle Item Text:";
            // 
            // Tool_ItemInfoEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 546);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.saveSelectedButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(911, 585);
            this.Name = "Tool_ItemInfoEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemInfo Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknown18Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown17Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown16Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown15Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown14Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown13Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown12Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown11Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown10Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown9Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown7Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown6Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown5Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown4Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown3Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemPaddingValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1Value)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
