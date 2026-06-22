namespace NSUNS4_Character_Manager
{
    partial class Tool_CommandListParamEditor
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator fileToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCharacodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMessageInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem costumesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton sortToolStripButton;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.GroupBox entriesGroupBox;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListBox entryListBox;
        private System.Windows.Forms.Button addEntryButton;
        private System.Windows.Forms.Button duplicateEntryButton;
        private System.Windows.Forms.Button deleteEntryButton;
        private System.Windows.Forms.GroupBox entryEditorPanel;
        private System.Windows.Forms.Label commandLinkLabel;
        private System.Windows.Forms.TextBox commandLinkTextBox;
        private System.Windows.Forms.Label commandListIndexLabel;
        private System.Windows.Forms.NumericUpDown commandListIndexNumericUpDown;
        private System.Windows.Forms.Label characterNameLabel;
        private System.Windows.Forms.TextBox characterNameTextBox;
        private System.Windows.Forms.Label characodeLabel;
        private System.Windows.Forms.TextBox characodeTextBox;
        private System.Windows.Forms.Label costumeIndexLabel;
        private System.Windows.Forms.NumericUpDown costumeIndexNumericUpDown;
        private System.Windows.Forms.Label attackNameLabel;
        private System.Windows.Forms.TextBox attackNameHashTextBox;
        private System.Windows.Forms.TextBox attackNameTextBox;
        private System.Windows.Forms.Label buttonPressLabel;
        private System.Windows.Forms.TextBox buttonPressHashTextBox;
        private System.Windows.Forms.TextBox buttonPressTextBox;
        private System.Windows.Forms.Label buttonFormulaLabel;
        private System.Windows.Forms.ComboBox buttonFormulaInputComboBox;
        private System.Windows.Forms.Button buttonFormulaAddButton;
        private System.Windows.Forms.Button buttonFormulaRemoveButton;
        private System.Windows.Forms.Button buttonFormulaClearButton;
        private System.Windows.Forms.Button buttonFormulaSearchButton;
        private System.Windows.Forms.FlowLayoutPanel buttonFormulaFlowLayoutPanel;
        private System.Windows.Forms.Label buttonFormulaResultLabel;
        private System.Windows.Forms.ComboBox buttonFormulaResultsComboBox;
        private System.Windows.Forms.Label commandType1Label;
        private System.Windows.Forms.ComboBox commandType1ComboBox;
        private System.Windows.Forms.Label commandTypeSkillLabel;
        private System.Windows.Forms.ComboBox commandTypeSkillComboBox;
        private System.Windows.Forms.Label commandTypeAwakeLabel;
        private System.Windows.Forms.ComboBox commandTypeAwakeComboBox;
        private System.Windows.Forms.Label commandTypeTeamLabel;
        private System.Windows.Forms.ComboBox commandTypeTeamComboBox;
        private System.Windows.Forms.Label commandType5Label;
        private System.Windows.Forms.ComboBox commandType5ComboBox;
        private System.Windows.Forms.Label commandType6Label;
        private System.Windows.Forms.NumericUpDown commandType6NumericUpDown;
        private System.Windows.Forms.Label hashHelpLabel;
        private System.Windows.Forms.Button saveEntryButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

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
            this.fileToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCharacodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMessageInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.costumesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.entriesGroupBox = new System.Windows.Forms.GroupBox();
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.saveEntryButton = new System.Windows.Forms.Button();
            this.entryEditorPanel = new System.Windows.Forms.GroupBox();
            this.commandLinkLabel = new System.Windows.Forms.Label();
            this.commandLinkTextBox = new System.Windows.Forms.TextBox();
            this.commandListIndexLabel = new System.Windows.Forms.Label();
            this.commandListIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.characterNameLabel = new System.Windows.Forms.Label();
            this.characterNameTextBox = new System.Windows.Forms.TextBox();
            this.characodeLabel = new System.Windows.Forms.Label();
            this.characodeTextBox = new System.Windows.Forms.TextBox();
            this.costumeIndexLabel = new System.Windows.Forms.Label();
            this.costumeIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.attackNameLabel = new System.Windows.Forms.Label();
            this.attackNameHashTextBox = new System.Windows.Forms.TextBox();
            this.attackNameTextBox = new System.Windows.Forms.TextBox();
            this.buttonPressLabel = new System.Windows.Forms.Label();
            this.buttonPressHashTextBox = new System.Windows.Forms.TextBox();
            this.buttonPressTextBox = new System.Windows.Forms.TextBox();
            this.buttonFormulaLabel = new System.Windows.Forms.Label();
            this.buttonFormulaInputComboBox = new System.Windows.Forms.ComboBox();
            this.buttonFormulaAddButton = new System.Windows.Forms.Button();
            this.buttonFormulaRemoveButton = new System.Windows.Forms.Button();
            this.buttonFormulaClearButton = new System.Windows.Forms.Button();
            this.buttonFormulaSearchButton = new System.Windows.Forms.Button();
            this.buttonFormulaFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonFormulaResultLabel = new System.Windows.Forms.Label();
            this.buttonFormulaResultsComboBox = new System.Windows.Forms.ComboBox();
            this.commandType1Label = new System.Windows.Forms.Label();
            this.commandType1ComboBox = new System.Windows.Forms.ComboBox();
            this.commandTypeSkillLabel = new System.Windows.Forms.Label();
            this.commandTypeSkillComboBox = new System.Windows.Forms.ComboBox();
            this.commandTypeAwakeLabel = new System.Windows.Forms.Label();
            this.commandTypeAwakeComboBox = new System.Windows.Forms.ComboBox();
            this.commandTypeTeamLabel = new System.Windows.Forms.Label();
            this.commandTypeTeamComboBox = new System.Windows.Forms.ComboBox();
            this.commandType5Label = new System.Windows.Forms.Label();
            this.commandType5ComboBox = new System.Windows.Forms.ComboBox();
            this.commandType6Label = new System.Windows.Forms.Label();
            this.commandType6NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.hashHelpLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.entriesGroupBox.SuspendLayout();
            this.entryEditorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandListIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandType6NumericUpDown)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.referencesToolStripMenuItem,
            this.cloneToolStripMenuItem,
            this.costumesToolStripMenuItem,
            this.sortToolStripButton});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1303, 26);
            this.menuStrip1.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.fileToolStripSeparator,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // fileToolStripSeparator
            // 
            this.fileToolStripSeparator.Name = "fileToolStripSeparator";
            this.fileToolStripSeparator.Size = new System.Drawing.Size(192, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // referencesToolStripMenuItem
            // 
            this.referencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadCharacodeToolStripMenuItem,
            this.loadMessageInfoToolStripMenuItem});
            this.referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
            this.referencesToolStripMenuItem.Size = new System.Drawing.Size(76, 22);
            this.referencesToolStripMenuItem.Text = "References";
            // 
            // loadCharacodeToolStripMenuItem
            // 
            this.loadCharacodeToolStripMenuItem.Name = "loadCharacodeToolStripMenuItem";
            this.loadCharacodeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.loadCharacodeToolStripMenuItem.Text = "Load characode...";
            this.loadCharacodeToolStripMenuItem.Click += new System.EventHandler(this.loadCharacodeToolStripMenuItem_Click);
            // 
            // loadMessageInfoToolStripMenuItem
            // 
            this.loadMessageInfoToolStripMenuItem.Name = "loadMessageInfoToolStripMenuItem";
            this.loadMessageInfoToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.loadMessageInfoToolStripMenuItem.Text = "Load messageInfo...";
            this.loadMessageInfoToolStripMenuItem.Click += new System.EventHandler(this.loadMessageInfoToolStripMenuItem_Click);
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(50, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // costumesToolStripMenuItem
            // 
            this.costumesToolStripMenuItem.Name = "costumesToolStripMenuItem";
            this.costumesToolStripMenuItem.Size = new System.Drawing.Size(72, 22);
            this.costumesToolStripMenuItem.Text = "Costumes";
            this.costumesToolStripMenuItem.Click += new System.EventHandler(this.costumesToolStripMenuItem_Click);
            // 
            // sortToolStripButton
            // 
            this.sortToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sortToolStripButton.Enabled = false;
            this.sortToolStripButton.Name = "sortToolStripButton";
            this.sortToolStripButton.Size = new System.Drawing.Size(32, 19);
            this.sortToolStripButton.Text = "Sort";
            this.sortToolStripButton.ToolTipText = "Disabled because command entries must remain in their required order.";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.IsSplitterFixed = true;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 26);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.searchButton);
            this.mainSplitContainer.Panel1.Controls.Add(this.searchTextBox);
            this.mainSplitContainer.Panel1.Controls.Add(this.entriesGroupBox);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.entryEditorPanel);
            this.mainSplitContainer.Size = new System.Drawing.Size(1303, 587);
            this.mainSplitContainer.SplitterDistance = 602;
            this.mainSplitContainer.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(495, 549);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(94, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(13, 549);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(476, 23);
            this.searchTextBox.TabIndex = 0;
            // 
            // entriesGroupBox
            // 
            this.entriesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entriesGroupBox.Controls.Add(this.entryListBox);
            this.entriesGroupBox.Controls.Add(this.addEntryButton);
            this.entriesGroupBox.Controls.Add(this.duplicateEntryButton);
            this.entriesGroupBox.Controls.Add(this.deleteEntryButton);
            this.entriesGroupBox.Controls.Add(this.saveEntryButton);
            this.entriesGroupBox.Location = new System.Drawing.Point(3, 3);
            this.entriesGroupBox.Name = "entriesGroupBox";
            this.entriesGroupBox.Size = new System.Drawing.Size(596, 544);
            this.entriesGroupBox.TabIndex = 0;
            this.entriesGroupBox.TabStop = false;
            // 
            // entryListBox
            // 
            this.entryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entryListBox.FormattingEnabled = true;
            this.entryListBox.IntegralHeight = false;
            this.entryListBox.ItemHeight = 15;
            this.entryListBox.Location = new System.Drawing.Point(10, 22);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(576, 477);
            this.entryListBox.TabIndex = 1;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.entryListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addEntryButton.Location = new System.Drawing.Point(10, 505);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(92, 30);
            this.addEntryButton.TabIndex = 1;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.UseVisualStyleBackColor = true;
            this.addEntryButton.Click += new System.EventHandler(this.addEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.duplicateEntryButton.Location = new System.Drawing.Point(111, 505);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(92, 30);
            this.duplicateEntryButton.TabIndex = 2;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.UseVisualStyleBackColor = true;
            this.duplicateEntryButton.Click += new System.EventHandler(this.duplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteEntryButton.Location = new System.Drawing.Point(211, 505);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(92, 30);
            this.deleteEntryButton.TabIndex = 3;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.UseVisualStyleBackColor = true;
            this.deleteEntryButton.Click += new System.EventHandler(this.deleteEntryButton_Click);
            // 
            // saveEntryButton
            // 
            this.saveEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveEntryButton.Location = new System.Drawing.Point(309, 505);
            this.saveEntryButton.Name = "saveEntryButton";
            this.saveEntryButton.Size = new System.Drawing.Size(123, 30);
            this.saveEntryButton.TabIndex = 13;
            this.saveEntryButton.Text = "Save";
            this.saveEntryButton.UseVisualStyleBackColor = true;
            this.saveEntryButton.Click += new System.EventHandler(this.saveEntryButton_Click);
            // 
            // entryEditorPanel
            // 
            this.entryEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entryEditorPanel.Controls.Add(this.commandLinkLabel);
            this.entryEditorPanel.Controls.Add(this.commandLinkTextBox);
            this.entryEditorPanel.Controls.Add(this.commandListIndexLabel);
            this.entryEditorPanel.Controls.Add(this.commandListIndexNumericUpDown);
            this.entryEditorPanel.Controls.Add(this.characterNameLabel);
            this.entryEditorPanel.Controls.Add(this.characterNameTextBox);
            this.entryEditorPanel.Controls.Add(this.characodeLabel);
            this.entryEditorPanel.Controls.Add(this.characodeTextBox);
            this.entryEditorPanel.Controls.Add(this.costumeIndexLabel);
            this.entryEditorPanel.Controls.Add(this.costumeIndexNumericUpDown);
            this.entryEditorPanel.Controls.Add(this.attackNameLabel);
            this.entryEditorPanel.Controls.Add(this.attackNameHashTextBox);
            this.entryEditorPanel.Controls.Add(this.attackNameTextBox);
            this.entryEditorPanel.Controls.Add(this.buttonPressLabel);
            this.entryEditorPanel.Controls.Add(this.buttonPressHashTextBox);
            this.entryEditorPanel.Controls.Add(this.buttonPressTextBox);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaLabel);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaInputComboBox);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaAddButton);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaRemoveButton);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaClearButton);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaSearchButton);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaFlowLayoutPanel);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaResultLabel);
            this.entryEditorPanel.Controls.Add(this.buttonFormulaResultsComboBox);
            this.entryEditorPanel.Controls.Add(this.commandType1Label);
            this.entryEditorPanel.Controls.Add(this.commandType1ComboBox);
            this.entryEditorPanel.Controls.Add(this.commandTypeSkillLabel);
            this.entryEditorPanel.Controls.Add(this.commandTypeSkillComboBox);
            this.entryEditorPanel.Controls.Add(this.commandTypeAwakeLabel);
            this.entryEditorPanel.Controls.Add(this.commandTypeAwakeComboBox);
            this.entryEditorPanel.Controls.Add(this.commandTypeTeamLabel);
            this.entryEditorPanel.Controls.Add(this.commandTypeTeamComboBox);
            this.entryEditorPanel.Controls.Add(this.commandType5Label);
            this.entryEditorPanel.Controls.Add(this.commandType5ComboBox);
            this.entryEditorPanel.Controls.Add(this.commandType6Label);
            this.entryEditorPanel.Controls.Add(this.commandType6NumericUpDown);
            this.entryEditorPanel.Controls.Add(this.hashHelpLabel);
            this.entryEditorPanel.Location = new System.Drawing.Point(3, 3);
            this.entryEditorPanel.Name = "entryEditorPanel";
            this.entryEditorPanel.Size = new System.Drawing.Size(682, 572);
            this.entryEditorPanel.TabIndex = 0;
            this.entryEditorPanel.TabStop = false;
            this.entryEditorPanel.Text = "Selected entry ";
            // 
            // commandLinkLabel
            // 
            this.commandLinkLabel.AutoSize = true;
            this.commandLinkLabel.Location = new System.Drawing.Point(12, 32);
            this.commandLinkLabel.Name = "commandLinkLabel";
            this.commandLinkLabel.Size = new System.Drawing.Size(89, 15);
            this.commandLinkLabel.TabIndex = 0;
            this.commandLinkLabel.Text = "Command Link";
            // 
            // commandLinkTextBox
            // 
            this.commandLinkTextBox.Location = new System.Drawing.Point(150, 29);
            this.commandLinkTextBox.Name = "commandLinkTextBox";
            this.commandLinkTextBox.Size = new System.Drawing.Size(170, 23);
            this.commandLinkTextBox.TabIndex = 0;
            // 
            // commandListIndexLabel
            // 
            this.commandListIndexLabel.AutoSize = true;
            this.commandListIndexLabel.Location = new System.Drawing.Point(12, 60);
            this.commandListIndexLabel.Name = "commandListIndexLabel";
            this.commandListIndexLabel.Size = new System.Drawing.Size(117, 15);
            this.commandListIndexLabel.TabIndex = 1;
            this.commandListIndexLabel.Text = "Command List Index";
            // 
            // commandListIndexNumericUpDown
            // 
            this.commandListIndexNumericUpDown.Location = new System.Drawing.Point(150, 58);
            this.commandListIndexNumericUpDown.Name = "commandListIndexNumericUpDown";
            this.commandListIndexNumericUpDown.Size = new System.Drawing.Size(170, 23);
            this.commandListIndexNumericUpDown.TabIndex = 1;
            // 
            // characterNameLabel
            // 
            this.characterNameLabel.AutoSize = true;
            this.characterNameLabel.Location = new System.Drawing.Point(14, 90);
            this.characterNameLabel.Name = "characterNameLabel";
            this.characterNameLabel.Size = new System.Drawing.Size(93, 15);
            this.characterNameLabel.TabIndex = 2;
            this.characterNameLabel.Text = "Character Name";
            // 
            // characterNameTextBox
            // 
            this.characterNameTextBox.Location = new System.Drawing.Point(150, 87);
            this.characterNameTextBox.Name = "characterNameTextBox";
            this.characterNameTextBox.Size = new System.Drawing.Size(170, 23);
            this.characterNameTextBox.TabIndex = 2;
            // 
            // characodeLabel
            // 
            this.characodeLabel.AutoSize = true;
            this.characodeLabel.Location = new System.Drawing.Point(14, 148);
            this.characodeLabel.Name = "characodeLabel";
            this.characodeLabel.Size = new System.Drawing.Size(64, 15);
            this.characodeLabel.TabIndex = 3;
            this.characodeLabel.Text = "Characode";
            // 
            // characodeTextBox
            // 
            this.characodeTextBox.Location = new System.Drawing.Point(150, 145);
            this.characodeTextBox.Name = "characodeTextBox";
            this.characodeTextBox.Size = new System.Drawing.Size(170, 23);
            this.characodeTextBox.TabIndex = 3;
            // 
            // costumeIndexLabel
            // 
            this.costumeIndexLabel.AutoSize = true;
            this.costumeIndexLabel.Location = new System.Drawing.Point(14, 118);
            this.costumeIndexLabel.Name = "costumeIndexLabel";
            this.costumeIndexLabel.Size = new System.Drawing.Size(87, 15);
            this.costumeIndexLabel.TabIndex = 4;
            this.costumeIndexLabel.Text = "Costume Index";
            // 
            // costumeIndexNumericUpDown
            // 
            this.costumeIndexNumericUpDown.Location = new System.Drawing.Point(150, 116);
            this.costumeIndexNumericUpDown.Name = "costumeIndexNumericUpDown";
            this.costumeIndexNumericUpDown.Size = new System.Drawing.Size(170, 23);
            this.costumeIndexNumericUpDown.TabIndex = 4;
            // 
            // attackNameLabel
            // 
            this.attackNameLabel.AutoSize = true;
            this.attackNameLabel.Location = new System.Drawing.Point(17, 206);
            this.attackNameLabel.Name = "attackNameLabel";
            this.attackNameLabel.Size = new System.Drawing.Size(102, 15);
            this.attackNameLabel.TabIndex = 5;
            this.attackNameLabel.Text = "Attack Hash Bytes";
            // 
            // attackNameHashTextBox
            // 
            this.attackNameHashTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.attackNameHashTextBox.Location = new System.Drawing.Point(148, 203);
            this.attackNameHashTextBox.MaxLength = 8;
            this.attackNameHashTextBox.Name = "attackNameHashTextBox";
            this.attackNameHashTextBox.Size = new System.Drawing.Size(79, 23);
            this.attackNameHashTextBox.TabIndex = 5;
            // 
            // attackNameTextBox
            // 
            this.attackNameTextBox.Location = new System.Drawing.Point(233, 203);
            this.attackNameTextBox.Name = "attackNameTextBox";
            this.attackNameTextBox.ReadOnly = true;
            this.attackNameTextBox.Size = new System.Drawing.Size(443, 23);
            this.attackNameTextBox.TabIndex = 6;
            // 
            // buttonPressLabel
            // 
            this.buttonPressLabel.AutoSize = true;
            this.buttonPressLabel.Location = new System.Drawing.Point(17, 235);
            this.buttonPressLabel.Name = "buttonPressLabel";
            this.buttonPressLabel.Size = new System.Drawing.Size(104, 15);
            this.buttonPressLabel.TabIndex = 7;
            this.buttonPressLabel.Text = "Button Press Bytes";
            // 
            // buttonPressHashTextBox
            // 
            this.buttonPressHashTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.buttonPressHashTextBox.Location = new System.Drawing.Point(148, 235);
            this.buttonPressHashTextBox.MaxLength = 8;
            this.buttonPressHashTextBox.Name = "buttonPressHashTextBox";
            this.buttonPressHashTextBox.Size = new System.Drawing.Size(79, 23);
            this.buttonPressHashTextBox.TabIndex = 7;
            // 
            // buttonPressTextBox
            // 
            this.buttonPressTextBox.Location = new System.Drawing.Point(233, 235);
            this.buttonPressTextBox.Name = "buttonPressTextBox";
            this.buttonPressTextBox.ReadOnly = true;
            this.buttonPressTextBox.Size = new System.Drawing.Size(443, 23);
            this.buttonPressTextBox.TabIndex = 8;
            // 
            // buttonFormulaLabel
            // 
            this.buttonFormulaLabel.AutoSize = true;
            this.buttonFormulaLabel.Location = new System.Drawing.Point(17, 274);
            this.buttonFormulaLabel.Name = "buttonFormulaLabel";
            this.buttonFormulaLabel.Size = new System.Drawing.Size(60, 15);
            this.buttonFormulaLabel.TabIndex = 14;
            this.buttonFormulaLabel.Text = "Add Input";
            // 
            // buttonFormulaInputComboBox
            // 
            this.buttonFormulaInputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buttonFormulaInputComboBox.FormattingEnabled = true;
            this.buttonFormulaInputComboBox.Location = new System.Drawing.Point(148, 271);
            this.buttonFormulaInputComboBox.Name = "buttonFormulaInputComboBox";
            this.buttonFormulaInputComboBox.Size = new System.Drawing.Size(210, 23);
            this.buttonFormulaInputComboBox.TabIndex = 14;
            // 
            // buttonFormulaAddButton
            // 
            this.buttonFormulaAddButton.Location = new System.Drawing.Point(364, 270);
            this.buttonFormulaAddButton.Name = "buttonFormulaAddButton";
            this.buttonFormulaAddButton.Size = new System.Drawing.Size(60, 25);
            this.buttonFormulaAddButton.TabIndex = 15;
            this.buttonFormulaAddButton.Text = "Add";
            this.buttonFormulaAddButton.UseVisualStyleBackColor = true;
            this.buttonFormulaAddButton.Click += new System.EventHandler(this.buttonFormulaAddButton_Click);
            // 
            // buttonFormulaRemoveButton
            // 
            this.buttonFormulaRemoveButton.Location = new System.Drawing.Point(430, 270);
            this.buttonFormulaRemoveButton.Name = "buttonFormulaRemoveButton";
            this.buttonFormulaRemoveButton.Size = new System.Drawing.Size(72, 25);
            this.buttonFormulaRemoveButton.TabIndex = 16;
            this.buttonFormulaRemoveButton.Text = "Remove";
            this.buttonFormulaRemoveButton.UseVisualStyleBackColor = true;
            this.buttonFormulaRemoveButton.Click += new System.EventHandler(this.buttonFormulaRemoveButton_Click);
            // 
            // buttonFormulaClearButton
            // 
            this.buttonFormulaClearButton.Location = new System.Drawing.Point(508, 270);
            this.buttonFormulaClearButton.Name = "buttonFormulaClearButton";
            this.buttonFormulaClearButton.Size = new System.Drawing.Size(62, 25);
            this.buttonFormulaClearButton.TabIndex = 17;
            this.buttonFormulaClearButton.Text = "Clear";
            this.buttonFormulaClearButton.UseVisualStyleBackColor = true;
            this.buttonFormulaClearButton.Click += new System.EventHandler(this.buttonFormulaClearButton_Click);
            // 
            // buttonFormulaSearchButton
            // 
            this.buttonFormulaSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFormulaSearchButton.Location = new System.Drawing.Point(576, 270);
            this.buttonFormulaSearchButton.Name = "buttonFormulaSearchButton";
            this.buttonFormulaSearchButton.Size = new System.Drawing.Size(100, 25);
            this.buttonFormulaSearchButton.TabIndex = 18;
            this.buttonFormulaSearchButton.Text = "Search";
            this.buttonFormulaSearchButton.UseVisualStyleBackColor = true;
            this.buttonFormulaSearchButton.Click += new System.EventHandler(this.buttonFormulaSearchButton_Click);
            // 
            // buttonFormulaFlowLayoutPanel
            // 
            this.buttonFormulaFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFormulaFlowLayoutPanel.AutoScroll = true;
            this.buttonFormulaFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonFormulaFlowLayoutPanel.Location = new System.Drawing.Point(20, 301);
            this.buttonFormulaFlowLayoutPanel.Name = "buttonFormulaFlowLayoutPanel";
            this.buttonFormulaFlowLayoutPanel.Size = new System.Drawing.Size(656, 59);
            this.buttonFormulaFlowLayoutPanel.TabIndex = 19;
            this.buttonFormulaFlowLayoutPanel.WrapContents = false;
            // 
            // buttonFormulaResultLabel
            // 
            this.buttonFormulaResultLabel.AutoSize = true;
            this.buttonFormulaResultLabel.Location = new System.Drawing.Point(17, 375);
            this.buttonFormulaResultLabel.Name = "buttonFormulaResultLabel";
            this.buttonFormulaResultLabel.Size = new System.Drawing.Size(71, 15);
            this.buttonFormulaResultLabel.TabIndex = 20;
            this.buttonFormulaResultLabel.Text = "Hash Match";
            // 
            // buttonFormulaResultsComboBox
            // 
            this.buttonFormulaResultsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFormulaResultsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buttonFormulaResultsComboBox.Enabled = false;
            this.buttonFormulaResultsComboBox.FormattingEnabled = true;
            this.buttonFormulaResultsComboBox.Location = new System.Drawing.Point(148, 371);
            this.buttonFormulaResultsComboBox.Name = "buttonFormulaResultsComboBox";
            this.buttonFormulaResultsComboBox.Size = new System.Drawing.Size(528, 23);
            this.buttonFormulaResultsComboBox.TabIndex = 20;
            this.buttonFormulaResultsComboBox.SelectedIndexChanged += new System.EventHandler(this.buttonFormulaResultsComboBox_SelectedIndexChanged);
            // 
            // commandType1Label
            // 
            this.commandType1Label.AutoSize = true;
            this.commandType1Label.Location = new System.Drawing.Point(338, 26);
            this.commandType1Label.Name = "commandType1Label";
            this.commandType1Label.Size = new System.Drawing.Size(100, 15);
            this.commandType1Label.TabIndex = 7;
            this.commandType1Label.Text = "Command Type 1";
            // 
            // commandType1ComboBox
            // 
            this.commandType1ComboBox.FormattingEnabled = true;
            this.commandType1ComboBox.Location = new System.Drawing.Point(488, 23);
            this.commandType1ComboBox.Name = "commandType1ComboBox";
            this.commandType1ComboBox.Size = new System.Drawing.Size(188, 23);
            this.commandType1ComboBox.TabIndex = 7;
            // 
            // commandTypeSkillLabel
            // 
            this.commandTypeSkillLabel.AutoSize = true;
            this.commandTypeSkillLabel.Location = new System.Drawing.Point(338, 55);
            this.commandTypeSkillLabel.Name = "commandTypeSkillLabel";
            this.commandTypeSkillLabel.Size = new System.Drawing.Size(115, 15);
            this.commandTypeSkillLabel.TabIndex = 8;
            this.commandTypeSkillLabel.Text = "Command Type Skill";
            // 
            // commandTypeSkillComboBox
            // 
            this.commandTypeSkillComboBox.FormattingEnabled = true;
            this.commandTypeSkillComboBox.Location = new System.Drawing.Point(488, 52);
            this.commandTypeSkillComboBox.Name = "commandTypeSkillComboBox";
            this.commandTypeSkillComboBox.Size = new System.Drawing.Size(188, 23);
            this.commandTypeSkillComboBox.TabIndex = 8;
            // 
            // commandTypeAwakeLabel
            // 
            this.commandTypeAwakeLabel.AutoSize = true;
            this.commandTypeAwakeLabel.Location = new System.Drawing.Point(338, 84);
            this.commandTypeAwakeLabel.Name = "commandTypeAwakeLabel";
            this.commandTypeAwakeLabel.Size = new System.Drawing.Size(129, 15);
            this.commandTypeAwakeLabel.TabIndex = 9;
            this.commandTypeAwakeLabel.Text = "Command Type Awake";
            // 
            // commandTypeAwakeComboBox
            // 
            this.commandTypeAwakeComboBox.FormattingEnabled = true;
            this.commandTypeAwakeComboBox.Location = new System.Drawing.Point(488, 81);
            this.commandTypeAwakeComboBox.Name = "commandTypeAwakeComboBox";
            this.commandTypeAwakeComboBox.Size = new System.Drawing.Size(188, 23);
            this.commandTypeAwakeComboBox.TabIndex = 9;
            // 
            // commandTypeTeamLabel
            // 
            this.commandTypeTeamLabel.AutoSize = true;
            this.commandTypeTeamLabel.Location = new System.Drawing.Point(338, 113);
            this.commandTypeTeamLabel.Name = "commandTypeTeamLabel";
            this.commandTypeTeamLabel.Size = new System.Drawing.Size(122, 15);
            this.commandTypeTeamLabel.TabIndex = 10;
            this.commandTypeTeamLabel.Text = "Command Type Team";
            // 
            // commandTypeTeamComboBox
            // 
            this.commandTypeTeamComboBox.FormattingEnabled = true;
            this.commandTypeTeamComboBox.Location = new System.Drawing.Point(488, 110);
            this.commandTypeTeamComboBox.Name = "commandTypeTeamComboBox";
            this.commandTypeTeamComboBox.Size = new System.Drawing.Size(188, 23);
            this.commandTypeTeamComboBox.TabIndex = 10;
            // 
            // commandType5Label
            // 
            this.commandType5Label.AutoSize = true;
            this.commandType5Label.Location = new System.Drawing.Point(338, 142);
            this.commandType5Label.Name = "commandType5Label";
            this.commandType5Label.Size = new System.Drawing.Size(100, 15);
            this.commandType5Label.TabIndex = 11;
            this.commandType5Label.Text = "Command Type 5";
            // 
            // commandType5ComboBox
            // 
            this.commandType5ComboBox.FormattingEnabled = true;
            this.commandType5ComboBox.Location = new System.Drawing.Point(488, 139);
            this.commandType5ComboBox.Name = "commandType5ComboBox";
            this.commandType5ComboBox.Size = new System.Drawing.Size(188, 23);
            this.commandType5ComboBox.TabIndex = 11;
            // 
            // commandType6Label
            // 
            this.commandType6Label.AutoSize = true;
            this.commandType6Label.Location = new System.Drawing.Point(338, 173);
            this.commandType6Label.Name = "commandType6Label";
            this.commandType6Label.Size = new System.Drawing.Size(100, 15);
            this.commandType6Label.TabIndex = 12;
            this.commandType6Label.Text = "Command Type 6";
            // 
            // commandType6NumericUpDown
            // 
            this.commandType6NumericUpDown.Location = new System.Drawing.Point(488, 171);
            this.commandType6NumericUpDown.Name = "commandType6NumericUpDown";
            this.commandType6NumericUpDown.Size = new System.Drawing.Size(188, 23);
            this.commandType6NumericUpDown.TabIndex = 12;
            // 
            // hashHelpLabel
            // 
            this.hashHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashHelpLabel.Location = new System.Drawing.Point(12, 500);
            this.hashHelpLabel.Name = "hashHelpLabel";
            this.hashHelpLabel.Size = new System.Drawing.Size(670, 69);
            this.hashHelpLabel.TabIndex = 13;
            this.hashHelpLabel.Text = "Build the input sequence by selecting an action and clicking Add. Select a node t" +
    "o remove it. Search copies the selected messageInfo hash into Button Press; clic" +
    "k Save to apply it to the entry.";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 613);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1303, 22);
            this.statusStrip1.TabIndex = 3;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // Tool_CommandListParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 635);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Tool_CommandListParamEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Command List Param Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.entriesGroupBox.ResumeLayout(false);
            this.entryEditorPanel.ResumeLayout(false);
            this.entryEditorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandListIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costumeIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandType6NumericUpDown)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
