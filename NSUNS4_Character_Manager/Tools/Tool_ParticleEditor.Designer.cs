using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_ParticleEditor
    {
        private IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private Panel chunkEditorPanel;
        private Label chunkLabel;
        private ComboBox chunkComboBox;
        private Label chunkNameLabel;
        private TextBox chunkNameTextBox;
        private Label chunkPathLabel;
        private TextBox chunkPathTextBox;
        private Button saveChunkMetaButton;
        private Button addChunkButton;
        private Button deleteChunkButton;
        private Button editReferencesButton;
        private TabControl particleTabControl;
        private TabPage managerTabPage;
        private TabPage resourceTabPage;
        private TabPage positionTabPage;
        private TabPage forceFieldTabPage;
        private TabPage nodeTabPage;
        private ListBox managerListBox;
        private PropertyGrid managerPropertyGrid;
        private Button managerAddButton;
        private Button managerCopyButton;
        private Button managerPasteButton;
        private Button managerDuplicateButton;
        private Button managerDeleteButton;
        private Button managerSaveButton;
        private Label managerHintLabel;
        private Label managerAnimationLabel;
        private ComboBox managerAnimationComboBox;
        private Label managerEntryIndexLabel;
        private NumericUpDown managerEntryIndexNumericUpDown;
        private ListBox resourceListBox;
        private PropertyGrid resourcePropertyGrid;
        private Button resourceAddButton;
        private Button resourceCopyButton;
        private Button resourcePasteButton;
        private Button resourceDuplicateButton;
        private Button resourceDeleteButton;
        private Button resourceSaveButton;
        private Label resourceHintLabel;
        private Label resourceEffectLabel;
        private ComboBox resourceEffectComboBox;
        private Label resourceParticleIndexLabel;
        private NumericUpDown resourceParticleIndexNumericUpDown;
        private ListBox positionListBox;
        private PropertyGrid positionPropertyGrid;
        private Button positionAddButton;
        private Button positionCopyButton;
        private Button positionPasteButton;
        private Button positionDuplicateButton;
        private Button positionDeleteButton;
        private Button positionSaveButton;
        private Label positionHintLabel;
        private Label positionCoordLabel;
        private ComboBox positionCoordComboBox;
        private Label positionParticleIndexLabel;
        private NumericUpDown positionParticleIndexNumericUpDown;
        private Label positionClumpLabel;
        private ComboBox positionClumpComboBox;
        private ListBox forceFieldListBox;
        private PropertyGrid forceFieldPropertyGrid;
        private Button forceFieldAddButton;
        private Button forceFieldCopyButton;
        private Button forceFieldPasteButton;
        private Button forceFieldDuplicateButton;
        private Button forceFieldDeleteButton;
        private Button forceFieldSaveButton;
        private Label forceFieldHintLabel;
        private Label forceFieldCoordLabel;
        private ComboBox forceFieldCoordComboBox;
        private Label forceFieldParticleIndexLabel;
        private NumericUpDown forceFieldParticleIndexNumericUpDown;
        private Label forceFieldClumpLabel;
        private ComboBox forceFieldClumpComboBox;
        private ListBox nodeListBox;
        private DataGridView nodeEventsGrid;
        private Button nodeAddButton;
        private Button nodeCopyButton;
        private Button nodePasteButton;
        private Button nodeDuplicateButton;
        private Button nodeDeleteButton;
        private Button nodeSaveButton;
        private Label nodeHintLabel;
        private Button nodeAddEventButton;
        private Button nodeDeleteEventButton;
        private Label nodeParticleIndexLabel;
        private NumericUpDown nodeParticleIndexNumericUpDown;

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
            this.chunkEditorPanel = new System.Windows.Forms.Panel();
            this.editReferencesButton = new System.Windows.Forms.Button();
            this.deleteChunkButton = new System.Windows.Forms.Button();
            this.addChunkButton = new System.Windows.Forms.Button();
            this.saveChunkMetaButton = new System.Windows.Forms.Button();
            this.chunkPathTextBox = new System.Windows.Forms.TextBox();
            this.chunkPathLabel = new System.Windows.Forms.Label();
            this.chunkNameTextBox = new System.Windows.Forms.TextBox();
            this.chunkNameLabel = new System.Windows.Forms.Label();
            this.chunkComboBox = new System.Windows.Forms.ComboBox();
            this.chunkLabel = new System.Windows.Forms.Label();
            this.particleTabControl = new System.Windows.Forms.TabControl();
            this.managerTabPage = new System.Windows.Forms.TabPage();
            this.managerListBox = new System.Windows.Forms.ListBox();
            this.managerAddButton = new System.Windows.Forms.Button();
            this.managerCopyButton = new System.Windows.Forms.Button();
            this.managerPasteButton = new System.Windows.Forms.Button();
            this.managerDuplicateButton = new System.Windows.Forms.Button();
            this.managerDeleteButton = new System.Windows.Forms.Button();
            this.managerSaveButton = new System.Windows.Forms.Button();
            this.managerHintLabel = new System.Windows.Forms.Label();
            this.managerAnimationLabel = new System.Windows.Forms.Label();
            this.managerAnimationComboBox = new System.Windows.Forms.ComboBox();
            this.managerEntryIndexLabel = new System.Windows.Forms.Label();
            this.managerEntryIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.managerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.resourceTabPage = new System.Windows.Forms.TabPage();
            this.resourceListBox = new System.Windows.Forms.ListBox();
            this.resourceAddButton = new System.Windows.Forms.Button();
            this.resourceCopyButton = new System.Windows.Forms.Button();
            this.resourcePasteButton = new System.Windows.Forms.Button();
            this.resourceDuplicateButton = new System.Windows.Forms.Button();
            this.resourceDeleteButton = new System.Windows.Forms.Button();
            this.resourceSaveButton = new System.Windows.Forms.Button();
            this.resourceHintLabel = new System.Windows.Forms.Label();
            this.resourceEffectLabel = new System.Windows.Forms.Label();
            this.resourceEffectComboBox = new System.Windows.Forms.ComboBox();
            this.resourceParticleIndexLabel = new System.Windows.Forms.Label();
            this.resourceParticleIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resourcePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.positionTabPage = new System.Windows.Forms.TabPage();
            this.positionListBox = new System.Windows.Forms.ListBox();
            this.positionAddButton = new System.Windows.Forms.Button();
            this.positionCopyButton = new System.Windows.Forms.Button();
            this.positionPasteButton = new System.Windows.Forms.Button();
            this.positionDuplicateButton = new System.Windows.Forms.Button();
            this.positionDeleteButton = new System.Windows.Forms.Button();
            this.positionSaveButton = new System.Windows.Forms.Button();
            this.positionHintLabel = new System.Windows.Forms.Label();
            this.positionCoordLabel = new System.Windows.Forms.Label();
            this.positionCoordComboBox = new System.Windows.Forms.ComboBox();
            this.positionClumpLabel = new System.Windows.Forms.Label();
            this.positionClumpComboBox = new System.Windows.Forms.ComboBox();
            this.positionParticleIndexLabel = new System.Windows.Forms.Label();
            this.positionParticleIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.positionPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.forceFieldTabPage = new System.Windows.Forms.TabPage();
            this.forceFieldListBox = new System.Windows.Forms.ListBox();
            this.forceFieldAddButton = new System.Windows.Forms.Button();
            this.forceFieldCopyButton = new System.Windows.Forms.Button();
            this.forceFieldPasteButton = new System.Windows.Forms.Button();
            this.forceFieldDuplicateButton = new System.Windows.Forms.Button();
            this.forceFieldDeleteButton = new System.Windows.Forms.Button();
            this.forceFieldSaveButton = new System.Windows.Forms.Button();
            this.forceFieldHintLabel = new System.Windows.Forms.Label();
            this.forceFieldCoordLabel = new System.Windows.Forms.Label();
            this.forceFieldCoordComboBox = new System.Windows.Forms.ComboBox();
            this.forceFieldClumpLabel = new System.Windows.Forms.Label();
            this.forceFieldClumpComboBox = new System.Windows.Forms.ComboBox();
            this.forceFieldParticleIndexLabel = new System.Windows.Forms.Label();
            this.forceFieldParticleIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.forceFieldPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nodeTabPage = new System.Windows.Forms.TabPage();
            this.nodeListBox = new System.Windows.Forms.ListBox();
            this.nodeAddButton = new System.Windows.Forms.Button();
            this.nodeCopyButton = new System.Windows.Forms.Button();
            this.nodePasteButton = new System.Windows.Forms.Button();
            this.nodeDuplicateButton = new System.Windows.Forms.Button();
            this.nodeDeleteButton = new System.Windows.Forms.Button();
            this.nodeSaveButton = new System.Windows.Forms.Button();
            this.nodeHintLabel = new System.Windows.Forms.Label();
            this.nodeParticleIndexLabel = new System.Windows.Forms.Label();
            this.nodeParticleIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nodeEventsGrid = new System.Windows.Forms.DataGridView();
            this.nodeAddEventButton = new System.Windows.Forms.Button();
            this.nodeDeleteEventButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.chunkEditorPanel.SuspendLayout();
            this.particleTabControl.SuspendLayout();
            this.managerTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.managerEntryIndexNumericUpDown)).BeginInit();
            this.resourceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resourceParticleIndexNumericUpDown)).BeginInit();
            this.positionTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionParticleIndexNumericUpDown)).BeginInit();
            this.forceFieldTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFieldParticleIndexNumericUpDown)).BeginInit();
            this.nodeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeParticleIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeEventsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 2;
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
            // chunkEditorPanel
            // 
            this.chunkEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chunkEditorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chunkEditorPanel.Controls.Add(this.editReferencesButton);
            this.chunkEditorPanel.Controls.Add(this.deleteChunkButton);
            this.chunkEditorPanel.Controls.Add(this.addChunkButton);
            this.chunkEditorPanel.Controls.Add(this.saveChunkMetaButton);
            this.chunkEditorPanel.Controls.Add(this.chunkPathTextBox);
            this.chunkEditorPanel.Controls.Add(this.chunkPathLabel);
            this.chunkEditorPanel.Controls.Add(this.chunkNameTextBox);
            this.chunkEditorPanel.Controls.Add(this.chunkNameLabel);
            this.chunkEditorPanel.Controls.Add(this.chunkComboBox);
            this.chunkEditorPanel.Controls.Add(this.chunkLabel);
            this.chunkEditorPanel.Location = new System.Drawing.Point(12, 30);
            this.chunkEditorPanel.Name = "chunkEditorPanel";
            this.chunkEditorPanel.Size = new System.Drawing.Size(1160, 72);
            this.chunkEditorPanel.TabIndex = 1;
            // 
            // editReferencesButton
            // 
            this.editReferencesButton.Location = new System.Drawing.Point(972, 9);
            this.editReferencesButton.Name = "editReferencesButton";
            this.editReferencesButton.Size = new System.Drawing.Size(173, 23);
            this.editReferencesButton.TabIndex = 0;
            this.editReferencesButton.Text = "Manage Linked Chunks";
            this.editReferencesButton.Click += new System.EventHandler(this.editReferencesButton_Click);
            // 
            // deleteChunkButton
            // 
            this.deleteChunkButton.Location = new System.Drawing.Point(872, 9);
            this.deleteChunkButton.Name = "deleteChunkButton";
            this.deleteChunkButton.Size = new System.Drawing.Size(94, 23);
            this.deleteChunkButton.TabIndex = 1;
            this.deleteChunkButton.Text = "Delete Chunk";
            this.deleteChunkButton.Click += new System.EventHandler(this.deleteChunkButton_Click);
            // 
            // addChunkButton
            // 
            this.addChunkButton.Location = new System.Drawing.Point(776, 9);
            this.addChunkButton.Name = "addChunkButton";
            this.addChunkButton.Size = new System.Drawing.Size(90, 23);
            this.addChunkButton.TabIndex = 2;
            this.addChunkButton.Text = "Add Chunk";
            this.addChunkButton.Click += new System.EventHandler(this.addChunkButton_Click);
            // 
            // saveChunkMetaButton
            // 
            this.saveChunkMetaButton.Location = new System.Drawing.Point(666, 9);
            this.saveChunkMetaButton.Name = "saveChunkMetaButton";
            this.saveChunkMetaButton.Size = new System.Drawing.Size(104, 23);
            this.saveChunkMetaButton.TabIndex = 3;
            this.saveChunkMetaButton.Text = "Apply Header";
            this.saveChunkMetaButton.Click += new System.EventHandler(this.saveChunkMetaButton_Click);
            // 
            // chunkPathTextBox
            // 
            this.chunkPathTextBox.Location = new System.Drawing.Point(88, 41);
            this.chunkPathTextBox.Name = "chunkPathTextBox";
            this.chunkPathTextBox.Size = new System.Drawing.Size(562, 20);
            this.chunkPathTextBox.TabIndex = 4;
            // 
            // chunkPathLabel
            // 
            this.chunkPathLabel.AutoSize = true;
            this.chunkPathLabel.Location = new System.Drawing.Point(10, 44);
            this.chunkPathLabel.Name = "chunkPathLabel";
            this.chunkPathLabel.Size = new System.Drawing.Size(29, 13);
            this.chunkPathLabel.TabIndex = 5;
            this.chunkPathLabel.Text = "Path";
            // 
            // chunkNameTextBox
            // 
            this.chunkNameTextBox.Location = new System.Drawing.Point(430, 10);
            this.chunkNameTextBox.Name = "chunkNameTextBox";
            this.chunkNameTextBox.Size = new System.Drawing.Size(220, 20);
            this.chunkNameTextBox.TabIndex = 6;
            // 
            // chunkNameLabel
            // 
            this.chunkNameLabel.AutoSize = true;
            this.chunkNameLabel.Location = new System.Drawing.Point(386, 13);
            this.chunkNameLabel.Name = "chunkNameLabel";
            this.chunkNameLabel.Size = new System.Drawing.Size(35, 13);
            this.chunkNameLabel.TabIndex = 7;
            this.chunkNameLabel.Text = "Name";
            // 
            // chunkComboBox
            // 
            this.chunkComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chunkComboBox.Location = new System.Drawing.Point(88, 10);
            this.chunkComboBox.Name = "chunkComboBox";
            this.chunkComboBox.Size = new System.Drawing.Size(280, 21);
            this.chunkComboBox.TabIndex = 8;
            this.chunkComboBox.SelectedIndexChanged += new System.EventHandler(this.chunkComboBox_SelectedIndexChanged);
            // 
            // chunkLabel
            // 
            this.chunkLabel.AutoSize = true;
            this.chunkLabel.Location = new System.Drawing.Point(10, 13);
            this.chunkLabel.Name = "chunkLabel";
            this.chunkLabel.Size = new System.Drawing.Size(76, 13);
            this.chunkLabel.TabIndex = 9;
            this.chunkLabel.Text = "Particle Chunk";
            // 
            // particleTabControl
            // 
            this.particleTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.particleTabControl.Controls.Add(this.managerTabPage);
            this.particleTabControl.Controls.Add(this.resourceTabPage);
            this.particleTabControl.Controls.Add(this.positionTabPage);
            this.particleTabControl.Controls.Add(this.forceFieldTabPage);
            this.particleTabControl.Controls.Add(this.nodeTabPage);
            this.particleTabControl.Location = new System.Drawing.Point(12, 108);
            this.particleTabControl.Name = "particleTabControl";
            this.particleTabControl.SelectedIndex = 0;
            this.particleTabControl.Size = new System.Drawing.Size(1160, 610);
            this.particleTabControl.TabIndex = 0;
            // 
            // managerTabPage
            // 
            this.managerTabPage.Controls.Add(this.managerListBox);
            this.managerTabPage.Controls.Add(this.managerAddButton);
            this.managerTabPage.Controls.Add(this.managerCopyButton);
            this.managerTabPage.Controls.Add(this.managerPasteButton);
            this.managerTabPage.Controls.Add(this.managerDuplicateButton);
            this.managerTabPage.Controls.Add(this.managerDeleteButton);
            this.managerTabPage.Controls.Add(this.managerSaveButton);
            this.managerTabPage.Controls.Add(this.managerHintLabel);
            this.managerTabPage.Controls.Add(this.managerAnimationLabel);
            this.managerTabPage.Controls.Add(this.managerAnimationComboBox);
            this.managerTabPage.Controls.Add(this.managerEntryIndexLabel);
            this.managerTabPage.Controls.Add(this.managerEntryIndexNumericUpDown);
            this.managerTabPage.Controls.Add(this.managerPropertyGrid);
            this.managerTabPage.Location = new System.Drawing.Point(4, 22);
            this.managerTabPage.Name = "managerTabPage";
            this.managerTabPage.Size = new System.Drawing.Size(1152, 584);
            this.managerTabPage.TabIndex = 0;
            this.managerTabPage.Text = "Particle Settings";
            this.managerTabPage.UseVisualStyleBackColor = true;
            // 
            // managerListBox
            // 
            this.managerListBox.Location = new System.Drawing.Point(6, 13);
            this.managerListBox.Name = "managerListBox";
            this.managerListBox.Size = new System.Drawing.Size(290, 485);
            this.managerListBox.TabIndex = 0;
            this.managerListBox.SelectedIndexChanged += new System.EventHandler(this.managerListBox_SelectedIndexChanged);
            // 
            // managerAddButton
            // 
            this.managerAddButton.Location = new System.Drawing.Point(6, 513);
            this.managerAddButton.Name = "managerAddButton";
            this.managerAddButton.Size = new System.Drawing.Size(64, 24);
            this.managerAddButton.TabIndex = 0;
            this.managerAddButton.Text = "Add";
            this.managerAddButton.Click += new System.EventHandler(this.managerAddButton_Click);
            // 
            // managerCopyButton
            // 
            this.managerCopyButton.Location = new System.Drawing.Point(76, 513);
            this.managerCopyButton.Name = "managerCopyButton";
            this.managerCopyButton.Size = new System.Drawing.Size(60, 24);
            this.managerCopyButton.TabIndex = 0;
            this.managerCopyButton.Text = "Copy";
            this.managerCopyButton.Click += new System.EventHandler(this.managerCopyButton_Click);
            // 
            // managerPasteButton
            // 
            this.managerPasteButton.Location = new System.Drawing.Point(142, 513);
            this.managerPasteButton.Name = "managerPasteButton";
            this.managerPasteButton.Size = new System.Drawing.Size(60, 24);
            this.managerPasteButton.TabIndex = 0;
            this.managerPasteButton.Text = "Paste";
            this.managerPasteButton.Click += new System.EventHandler(this.managerPasteButton_Click);
            // 
            // managerDuplicateButton
            // 
            this.managerDuplicateButton.Location = new System.Drawing.Point(208, 513);
            this.managerDuplicateButton.Name = "managerDuplicateButton";
            this.managerDuplicateButton.Size = new System.Drawing.Size(75, 24);
            this.managerDuplicateButton.TabIndex = 0;
            this.managerDuplicateButton.Text = "Duplicate";
            this.managerDuplicateButton.Click += new System.EventHandler(this.managerDuplicateButton_Click);
            // 
            // managerDeleteButton
            // 
            this.managerDeleteButton.Location = new System.Drawing.Point(6, 543);
            this.managerDeleteButton.Name = "managerDeleteButton";
            this.managerDeleteButton.Size = new System.Drawing.Size(64, 24);
            this.managerDeleteButton.TabIndex = 0;
            this.managerDeleteButton.Text = "Remove";
            this.managerDeleteButton.Click += new System.EventHandler(this.managerDeleteButton_Click);
            // 
            // managerSaveButton
            // 
            this.managerSaveButton.Location = new System.Drawing.Point(76, 543);
            this.managerSaveButton.Name = "managerSaveButton";
            this.managerSaveButton.Size = new System.Drawing.Size(60, 24);
            this.managerSaveButton.TabIndex = 0;
            this.managerSaveButton.Text = "Save";
            this.managerSaveButton.Click += new System.EventHandler(this.managerSaveButton_Click);
            // 
            // managerHintLabel
            // 
            this.managerHintLabel.AutoSize = true;
            this.managerHintLabel.Location = new System.Drawing.Point(314, 11);
            this.managerHintLabel.MaximumSize = new System.Drawing.Size(820, 0);
            this.managerHintLabel.Name = "managerHintLabel";
            this.managerHintLabel.Size = new System.Drawing.Size(654, 13);
            this.managerHintLabel.TabIndex = 0;
            this.managerHintLabel.Text = "Select a particle setting on the left, use the quick link controls to adjust its " +
    "animation and stored index, then edit the detailed values below.";
            // 
            // managerAnimationLabel
            // 
            this.managerAnimationLabel.AutoSize = true;
            this.managerAnimationLabel.Location = new System.Drawing.Point(314, 64);
            this.managerAnimationLabel.Name = "managerAnimationLabel";
            this.managerAnimationLabel.Size = new System.Drawing.Size(87, 13);
            this.managerAnimationLabel.TabIndex = 0;
            this.managerAnimationLabel.Text = "Animation Chunk";
            // 
            // managerAnimationComboBox
            // 
            this.managerAnimationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.managerAnimationComboBox.Location = new System.Drawing.Point(430, 61);
            this.managerAnimationComboBox.Name = "managerAnimationComboBox";
            this.managerAnimationComboBox.Size = new System.Drawing.Size(300, 21);
            this.managerAnimationComboBox.TabIndex = 0;
            this.managerAnimationComboBox.SelectedIndexChanged += new System.EventHandler(this.managerAnimationComboBox_SelectedIndexChanged);
            // 
            // managerEntryIndexLabel
            // 
            this.managerEntryIndexLabel.AutoSize = true;
            this.managerEntryIndexLabel.Location = new System.Drawing.Point(314, 92);
            this.managerEntryIndexLabel.Name = "managerEntryIndexLabel";
            this.managerEntryIndexLabel.Size = new System.Drawing.Size(94, 13);
            this.managerEntryIndexLabel.TabIndex = 0;
            this.managerEntryIndexLabel.Text = "Stored Entry Index";
            // 
            // managerEntryIndexNumericUpDown
            // 
            this.managerEntryIndexNumericUpDown.Location = new System.Drawing.Point(430, 90);
            this.managerEntryIndexNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.managerEntryIndexNumericUpDown.Name = "managerEntryIndexNumericUpDown";
            this.managerEntryIndexNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.managerEntryIndexNumericUpDown.TabIndex = 0;
            this.managerEntryIndexNumericUpDown.ValueChanged += new System.EventHandler(this.managerEntryIndexNumericUpDown_ValueChanged);
            // 
            // managerPropertyGrid
            // 
            this.managerPropertyGrid.Location = new System.Drawing.Point(317, 144);
            this.managerPropertyGrid.Name = "managerPropertyGrid";
            this.managerPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.managerPropertyGrid.Size = new System.Drawing.Size(825, 393);
            this.managerPropertyGrid.TabIndex = 0;
            this.managerPropertyGrid.ToolbarVisible = false;
            // 
            // resourceTabPage
            // 
            this.resourceTabPage.Controls.Add(this.resourceListBox);
            this.resourceTabPage.Controls.Add(this.resourceAddButton);
            this.resourceTabPage.Controls.Add(this.resourceCopyButton);
            this.resourceTabPage.Controls.Add(this.resourcePasteButton);
            this.resourceTabPage.Controls.Add(this.resourceDuplicateButton);
            this.resourceTabPage.Controls.Add(this.resourceDeleteButton);
            this.resourceTabPage.Controls.Add(this.resourceSaveButton);
            this.resourceTabPage.Controls.Add(this.resourceHintLabel);
            this.resourceTabPage.Controls.Add(this.resourceEffectLabel);
            this.resourceTabPage.Controls.Add(this.resourceEffectComboBox);
            this.resourceTabPage.Controls.Add(this.resourceParticleIndexLabel);
            this.resourceTabPage.Controls.Add(this.resourceParticleIndexNumericUpDown);
            this.resourceTabPage.Controls.Add(this.resourcePropertyGrid);
            this.resourceTabPage.Location = new System.Drawing.Point(4, 22);
            this.resourceTabPage.Name = "resourceTabPage";
            this.resourceTabPage.Size = new System.Drawing.Size(1152, 584);
            this.resourceTabPage.TabIndex = 1;
            this.resourceTabPage.Text = "Resources";
            this.resourceTabPage.UseVisualStyleBackColor = true;
            // 
            // resourceListBox
            // 
            this.resourceListBox.Location = new System.Drawing.Point(6, 13);
            this.resourceListBox.Name = "resourceListBox";
            this.resourceListBox.Size = new System.Drawing.Size(290, 485);
            this.resourceListBox.TabIndex = 0;
            this.resourceListBox.SelectedIndexChanged += new System.EventHandler(this.resourceListBox_SelectedIndexChanged);
            // 
            // resourceAddButton
            // 
            this.resourceAddButton.Location = new System.Drawing.Point(6, 513);
            this.resourceAddButton.Name = "resourceAddButton";
            this.resourceAddButton.Size = new System.Drawing.Size(64, 24);
            this.resourceAddButton.TabIndex = 0;
            this.resourceAddButton.Text = "Add";
            this.resourceAddButton.Click += new System.EventHandler(this.resourceAddButton_Click);
            // 
            // resourceCopyButton
            // 
            this.resourceCopyButton.Location = new System.Drawing.Point(76, 513);
            this.resourceCopyButton.Name = "resourceCopyButton";
            this.resourceCopyButton.Size = new System.Drawing.Size(60, 24);
            this.resourceCopyButton.TabIndex = 0;
            this.resourceCopyButton.Text = "Copy";
            this.resourceCopyButton.Click += new System.EventHandler(this.resourceCopyButton_Click);
            // 
            // resourcePasteButton
            // 
            this.resourcePasteButton.Location = new System.Drawing.Point(142, 513);
            this.resourcePasteButton.Name = "resourcePasteButton";
            this.resourcePasteButton.Size = new System.Drawing.Size(60, 24);
            this.resourcePasteButton.TabIndex = 0;
            this.resourcePasteButton.Text = "Paste";
            this.resourcePasteButton.Click += new System.EventHandler(this.resourcePasteButton_Click);
            // 
            // resourceDuplicateButton
            // 
            this.resourceDuplicateButton.Location = new System.Drawing.Point(208, 513);
            this.resourceDuplicateButton.Name = "resourceDuplicateButton";
            this.resourceDuplicateButton.Size = new System.Drawing.Size(75, 24);
            this.resourceDuplicateButton.TabIndex = 0;
            this.resourceDuplicateButton.Text = "Duplicate";
            this.resourceDuplicateButton.Click += new System.EventHandler(this.resourceDuplicateButton_Click);
            // 
            // resourceDeleteButton
            // 
            this.resourceDeleteButton.Location = new System.Drawing.Point(6, 543);
            this.resourceDeleteButton.Name = "resourceDeleteButton";
            this.resourceDeleteButton.Size = new System.Drawing.Size(64, 24);
            this.resourceDeleteButton.TabIndex = 0;
            this.resourceDeleteButton.Text = "Remove";
            this.resourceDeleteButton.Click += new System.EventHandler(this.resourceDeleteButton_Click);
            // 
            // resourceSaveButton
            // 
            this.resourceSaveButton.Location = new System.Drawing.Point(76, 543);
            this.resourceSaveButton.Name = "resourceSaveButton";
            this.resourceSaveButton.Size = new System.Drawing.Size(60, 24);
            this.resourceSaveButton.TabIndex = 0;
            this.resourceSaveButton.Text = "Save";
            this.resourceSaveButton.Click += new System.EventHandler(this.resourceSaveButton_Click);
            // 
            // resourceHintLabel
            // 
            this.resourceHintLabel.AutoSize = true;
            this.resourceHintLabel.Location = new System.Drawing.Point(314, 11);
            this.resourceHintLabel.MaximumSize = new System.Drawing.Size(820, 0);
            this.resourceHintLabel.Name = "resourceHintLabel";
            this.resourceHintLabel.Size = new System.Drawing.Size(494, 13);
            this.resourceHintLabel.TabIndex = 0;
            this.resourceHintLabel.Text = "Select a resource entry, then use the quick link controls to point it at a partic" +
    "le setting and effect chunk.";
            // 
            // resourceEffectLabel
            // 
            this.resourceEffectLabel.AutoSize = true;
            this.resourceEffectLabel.Location = new System.Drawing.Point(314, 64);
            this.resourceEffectLabel.Name = "resourceEffectLabel";
            this.resourceEffectLabel.Size = new System.Drawing.Size(69, 13);
            this.resourceEffectLabel.TabIndex = 0;
            this.resourceEffectLabel.Text = "Effect Chunk";
            // 
            // resourceEffectComboBox
            // 
            this.resourceEffectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceEffectComboBox.Location = new System.Drawing.Point(430, 61);
            this.resourceEffectComboBox.Name = "resourceEffectComboBox";
            this.resourceEffectComboBox.Size = new System.Drawing.Size(300, 21);
            this.resourceEffectComboBox.TabIndex = 0;
            this.resourceEffectComboBox.SelectedIndexChanged += new System.EventHandler(this.resourceEffectComboBox_SelectedIndexChanged);
            // 
            // resourceParticleIndexLabel
            // 
            this.resourceParticleIndexLabel.AutoSize = true;
            this.resourceParticleIndexLabel.Location = new System.Drawing.Point(314, 92);
            this.resourceParticleIndexLabel.Name = "resourceParticleIndexLabel";
            this.resourceParticleIndexLabel.Size = new System.Drawing.Size(106, 13);
            this.resourceParticleIndexLabel.TabIndex = 0;
            this.resourceParticleIndexLabel.Text = "Linked Particle Index";
            // 
            // resourceParticleIndexNumericUpDown
            // 
            this.resourceParticleIndexNumericUpDown.Location = new System.Drawing.Point(430, 90);
            this.resourceParticleIndexNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.resourceParticleIndexNumericUpDown.Name = "resourceParticleIndexNumericUpDown";
            this.resourceParticleIndexNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.resourceParticleIndexNumericUpDown.TabIndex = 0;
            this.resourceParticleIndexNumericUpDown.ValueChanged += new System.EventHandler(this.resourceParticleIndexNumericUpDown_ValueChanged);
            // 
            // resourcePropertyGrid
            // 
            this.resourcePropertyGrid.Location = new System.Drawing.Point(317, 144);
            this.resourcePropertyGrid.Name = "resourcePropertyGrid";
            this.resourcePropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.resourcePropertyGrid.Size = new System.Drawing.Size(825, 393);
            this.resourcePropertyGrid.TabIndex = 0;
            this.resourcePropertyGrid.ToolbarVisible = false;
            // 
            // positionTabPage
            // 
            this.positionTabPage.Controls.Add(this.positionListBox);
            this.positionTabPage.Controls.Add(this.positionAddButton);
            this.positionTabPage.Controls.Add(this.positionCopyButton);
            this.positionTabPage.Controls.Add(this.positionPasteButton);
            this.positionTabPage.Controls.Add(this.positionDuplicateButton);
            this.positionTabPage.Controls.Add(this.positionDeleteButton);
            this.positionTabPage.Controls.Add(this.positionSaveButton);
            this.positionTabPage.Controls.Add(this.positionHintLabel);
            this.positionTabPage.Controls.Add(this.positionCoordLabel);
            this.positionTabPage.Controls.Add(this.positionCoordComboBox);
            this.positionTabPage.Controls.Add(this.positionClumpLabel);
            this.positionTabPage.Controls.Add(this.positionClumpComboBox);
            this.positionTabPage.Controls.Add(this.positionParticleIndexLabel);
            this.positionTabPage.Controls.Add(this.positionParticleIndexNumericUpDown);
            this.positionTabPage.Controls.Add(this.positionPropertyGrid);
            this.positionTabPage.Location = new System.Drawing.Point(4, 22);
            this.positionTabPage.Name = "positionTabPage";
            this.positionTabPage.Size = new System.Drawing.Size(1152, 584);
            this.positionTabPage.TabIndex = 2;
            this.positionTabPage.Text = "Bone / Position";
            this.positionTabPage.UseVisualStyleBackColor = true;
            // 
            // positionListBox
            // 
            this.positionListBox.Location = new System.Drawing.Point(6, 13);
            this.positionListBox.Name = "positionListBox";
            this.positionListBox.Size = new System.Drawing.Size(290, 485);
            this.positionListBox.TabIndex = 0;
            this.positionListBox.SelectedIndexChanged += new System.EventHandler(this.positionListBox_SelectedIndexChanged);
            // 
            // positionAddButton
            // 
            this.positionAddButton.Location = new System.Drawing.Point(6, 513);
            this.positionAddButton.Name = "positionAddButton";
            this.positionAddButton.Size = new System.Drawing.Size(64, 24);
            this.positionAddButton.TabIndex = 0;
            this.positionAddButton.Text = "Add";
            this.positionAddButton.Click += new System.EventHandler(this.positionAddButton_Click);
            // 
            // positionCopyButton
            // 
            this.positionCopyButton.Location = new System.Drawing.Point(76, 513);
            this.positionCopyButton.Name = "positionCopyButton";
            this.positionCopyButton.Size = new System.Drawing.Size(60, 24);
            this.positionCopyButton.TabIndex = 0;
            this.positionCopyButton.Text = "Copy";
            this.positionCopyButton.Click += new System.EventHandler(this.positionCopyButton_Click);
            // 
            // positionPasteButton
            // 
            this.positionPasteButton.Location = new System.Drawing.Point(142, 513);
            this.positionPasteButton.Name = "positionPasteButton";
            this.positionPasteButton.Size = new System.Drawing.Size(60, 24);
            this.positionPasteButton.TabIndex = 0;
            this.positionPasteButton.Text = "Paste";
            this.positionPasteButton.Click += new System.EventHandler(this.positionPasteButton_Click);
            // 
            // positionDuplicateButton
            // 
            this.positionDuplicateButton.Location = new System.Drawing.Point(208, 513);
            this.positionDuplicateButton.Name = "positionDuplicateButton";
            this.positionDuplicateButton.Size = new System.Drawing.Size(75, 24);
            this.positionDuplicateButton.TabIndex = 0;
            this.positionDuplicateButton.Text = "Duplicate";
            this.positionDuplicateButton.Click += new System.EventHandler(this.positionDuplicateButton_Click);
            // 
            // positionDeleteButton
            // 
            this.positionDeleteButton.Location = new System.Drawing.Point(6, 543);
            this.positionDeleteButton.Name = "positionDeleteButton";
            this.positionDeleteButton.Size = new System.Drawing.Size(64, 24);
            this.positionDeleteButton.TabIndex = 0;
            this.positionDeleteButton.Text = "Remove";
            this.positionDeleteButton.Click += new System.EventHandler(this.positionDeleteButton_Click);
            // 
            // positionSaveButton
            // 
            this.positionSaveButton.Location = new System.Drawing.Point(76, 543);
            this.positionSaveButton.Name = "positionSaveButton";
            this.positionSaveButton.Size = new System.Drawing.Size(60, 24);
            this.positionSaveButton.TabIndex = 0;
            this.positionSaveButton.Text = "Save";
            this.positionSaveButton.Click += new System.EventHandler(this.positionSaveButton_Click);
            // 
            // positionHintLabel
            // 
            this.positionHintLabel.AutoSize = true;
            this.positionHintLabel.Location = new System.Drawing.Point(314, 11);
            this.positionHintLabel.MaximumSize = new System.Drawing.Size(820, 0);
            this.positionHintLabel.Name = "positionHintLabel";
            this.positionHintLabel.Size = new System.Drawing.Size(518, 13);
            this.positionHintLabel.TabIndex = 0;
            this.positionHintLabel.Text = "Select a position entry, choose the linked particle setting, then swap coord and " +
    "clump references as needed.";
            // 
            // positionCoordLabel
            // 
            this.positionCoordLabel.AutoSize = true;
            this.positionCoordLabel.Location = new System.Drawing.Point(314, 64);
            this.positionCoordLabel.Name = "positionCoordLabel";
            this.positionCoordLabel.Size = new System.Drawing.Size(69, 13);
            this.positionCoordLabel.TabIndex = 0;
            this.positionCoordLabel.Text = "Coord Chunk";
            // 
            // positionCoordComboBox
            // 
            this.positionCoordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionCoordComboBox.Location = new System.Drawing.Point(430, 61);
            this.positionCoordComboBox.Name = "positionCoordComboBox";
            this.positionCoordComboBox.Size = new System.Drawing.Size(300, 21);
            this.positionCoordComboBox.TabIndex = 0;
            this.positionCoordComboBox.SelectedIndexChanged += new System.EventHandler(this.positionCoordComboBox_SelectedIndexChanged);
            // 
            // positionClumpLabel
            // 
            this.positionClumpLabel.AutoSize = true;
            this.positionClumpLabel.Location = new System.Drawing.Point(750, 64);
            this.positionClumpLabel.Name = "positionClumpLabel";
            this.positionClumpLabel.Size = new System.Drawing.Size(70, 13);
            this.positionClumpLabel.TabIndex = 0;
            this.positionClumpLabel.Text = "Clump Chunk";
            // 
            // positionClumpComboBox
            // 
            this.positionClumpComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionClumpComboBox.Location = new System.Drawing.Point(838, 61);
            this.positionClumpComboBox.Name = "positionClumpComboBox";
            this.positionClumpComboBox.Size = new System.Drawing.Size(304, 21);
            this.positionClumpComboBox.TabIndex = 0;
            this.positionClumpComboBox.SelectedIndexChanged += new System.EventHandler(this.positionClumpComboBox_SelectedIndexChanged);
            // 
            // positionParticleIndexLabel
            // 
            this.positionParticleIndexLabel.AutoSize = true;
            this.positionParticleIndexLabel.Location = new System.Drawing.Point(314, 92);
            this.positionParticleIndexLabel.Name = "positionParticleIndexLabel";
            this.positionParticleIndexLabel.Size = new System.Drawing.Size(106, 13);
            this.positionParticleIndexLabel.TabIndex = 0;
            this.positionParticleIndexLabel.Text = "Linked Particle Index";
            // 
            // positionParticleIndexNumericUpDown
            // 
            this.positionParticleIndexNumericUpDown.Location = new System.Drawing.Point(430, 90);
            this.positionParticleIndexNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.positionParticleIndexNumericUpDown.Name = "positionParticleIndexNumericUpDown";
            this.positionParticleIndexNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.positionParticleIndexNumericUpDown.TabIndex = 0;
            this.positionParticleIndexNumericUpDown.ValueChanged += new System.EventHandler(this.positionParticleIndexNumericUpDown_ValueChanged);
            // 
            // positionPropertyGrid
            // 
            this.positionPropertyGrid.Location = new System.Drawing.Point(317, 144);
            this.positionPropertyGrid.Name = "positionPropertyGrid";
            this.positionPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.positionPropertyGrid.Size = new System.Drawing.Size(825, 393);
            this.positionPropertyGrid.TabIndex = 0;
            this.positionPropertyGrid.ToolbarVisible = false;
            // 
            // forceFieldTabPage
            // 
            this.forceFieldTabPage.Controls.Add(this.forceFieldListBox);
            this.forceFieldTabPage.Controls.Add(this.forceFieldAddButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldCopyButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldPasteButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldDuplicateButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldDeleteButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldSaveButton);
            this.forceFieldTabPage.Controls.Add(this.forceFieldHintLabel);
            this.forceFieldTabPage.Controls.Add(this.forceFieldCoordLabel);
            this.forceFieldTabPage.Controls.Add(this.forceFieldCoordComboBox);
            this.forceFieldTabPage.Controls.Add(this.forceFieldClumpLabel);
            this.forceFieldTabPage.Controls.Add(this.forceFieldClumpComboBox);
            this.forceFieldTabPage.Controls.Add(this.forceFieldParticleIndexLabel);
            this.forceFieldTabPage.Controls.Add(this.forceFieldParticleIndexNumericUpDown);
            this.forceFieldTabPage.Controls.Add(this.forceFieldPropertyGrid);
            this.forceFieldTabPage.Location = new System.Drawing.Point(4, 22);
            this.forceFieldTabPage.Name = "forceFieldTabPage";
            this.forceFieldTabPage.Size = new System.Drawing.Size(1152, 584);
            this.forceFieldTabPage.TabIndex = 3;
            this.forceFieldTabPage.Text = "Force Fields";
            this.forceFieldTabPage.UseVisualStyleBackColor = true;
            // 
            // forceFieldListBox
            // 
            this.forceFieldListBox.Location = new System.Drawing.Point(6, 13);
            this.forceFieldListBox.Name = "forceFieldListBox";
            this.forceFieldListBox.Size = new System.Drawing.Size(290, 485);
            this.forceFieldListBox.TabIndex = 0;
            this.forceFieldListBox.SelectedIndexChanged += new System.EventHandler(this.forceFieldListBox_SelectedIndexChanged);
            // 
            // forceFieldAddButton
            // 
            this.forceFieldAddButton.Location = new System.Drawing.Point(6, 513);
            this.forceFieldAddButton.Name = "forceFieldAddButton";
            this.forceFieldAddButton.Size = new System.Drawing.Size(64, 24);
            this.forceFieldAddButton.TabIndex = 0;
            this.forceFieldAddButton.Text = "Add";
            this.forceFieldAddButton.Click += new System.EventHandler(this.forceFieldAddButton_Click);
            // 
            // forceFieldCopyButton
            // 
            this.forceFieldCopyButton.Location = new System.Drawing.Point(76, 513);
            this.forceFieldCopyButton.Name = "forceFieldCopyButton";
            this.forceFieldCopyButton.Size = new System.Drawing.Size(60, 24);
            this.forceFieldCopyButton.TabIndex = 0;
            this.forceFieldCopyButton.Text = "Copy";
            this.forceFieldCopyButton.Click += new System.EventHandler(this.forceFieldCopyButton_Click);
            // 
            // forceFieldPasteButton
            // 
            this.forceFieldPasteButton.Location = new System.Drawing.Point(142, 513);
            this.forceFieldPasteButton.Name = "forceFieldPasteButton";
            this.forceFieldPasteButton.Size = new System.Drawing.Size(60, 24);
            this.forceFieldPasteButton.TabIndex = 0;
            this.forceFieldPasteButton.Text = "Paste";
            this.forceFieldPasteButton.Click += new System.EventHandler(this.forceFieldPasteButton_Click);
            // 
            // forceFieldDuplicateButton
            // 
            this.forceFieldDuplicateButton.Location = new System.Drawing.Point(208, 513);
            this.forceFieldDuplicateButton.Name = "forceFieldDuplicateButton";
            this.forceFieldDuplicateButton.Size = new System.Drawing.Size(75, 24);
            this.forceFieldDuplicateButton.TabIndex = 0;
            this.forceFieldDuplicateButton.Text = "Duplicate";
            this.forceFieldDuplicateButton.Click += new System.EventHandler(this.forceFieldDuplicateButton_Click);
            // 
            // forceFieldDeleteButton
            // 
            this.forceFieldDeleteButton.Location = new System.Drawing.Point(6, 543);
            this.forceFieldDeleteButton.Name = "forceFieldDeleteButton";
            this.forceFieldDeleteButton.Size = new System.Drawing.Size(64, 24);
            this.forceFieldDeleteButton.TabIndex = 0;
            this.forceFieldDeleteButton.Text = "Remove";
            this.forceFieldDeleteButton.Click += new System.EventHandler(this.forceFieldDeleteButton_Click);
            // 
            // forceFieldSaveButton
            // 
            this.forceFieldSaveButton.Location = new System.Drawing.Point(76, 543);
            this.forceFieldSaveButton.Name = "forceFieldSaveButton";
            this.forceFieldSaveButton.Size = new System.Drawing.Size(60, 24);
            this.forceFieldSaveButton.TabIndex = 0;
            this.forceFieldSaveButton.Text = "Save";
            this.forceFieldSaveButton.Click += new System.EventHandler(this.forceFieldSaveButton_Click);
            // 
            // forceFieldHintLabel
            // 
            this.forceFieldHintLabel.AutoSize = true;
            this.forceFieldHintLabel.Location = new System.Drawing.Point(314, 11);
            this.forceFieldHintLabel.MaximumSize = new System.Drawing.Size(820, 0);
            this.forceFieldHintLabel.Name = "forceFieldHintLabel";
            this.forceFieldHintLabel.Size = new System.Drawing.Size(616, 13);
            this.forceFieldHintLabel.TabIndex = 0;
            this.forceFieldHintLabel.Text = "Select a force field entry, choose the linked particle setting, then adjust coord" +
    " and clump links before editing the remaining values.";
            // 
            // forceFieldCoordLabel
            // 
            this.forceFieldCoordLabel.AutoSize = true;
            this.forceFieldCoordLabel.Location = new System.Drawing.Point(314, 64);
            this.forceFieldCoordLabel.Name = "forceFieldCoordLabel";
            this.forceFieldCoordLabel.Size = new System.Drawing.Size(69, 13);
            this.forceFieldCoordLabel.TabIndex = 0;
            this.forceFieldCoordLabel.Text = "Coord Chunk";
            // 
            // forceFieldCoordComboBox
            // 
            this.forceFieldCoordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forceFieldCoordComboBox.Location = new System.Drawing.Point(430, 61);
            this.forceFieldCoordComboBox.Name = "forceFieldCoordComboBox";
            this.forceFieldCoordComboBox.Size = new System.Drawing.Size(300, 21);
            this.forceFieldCoordComboBox.TabIndex = 0;
            this.forceFieldCoordComboBox.SelectedIndexChanged += new System.EventHandler(this.forceFieldCoordComboBox_SelectedIndexChanged);
            // 
            // forceFieldClumpLabel
            // 
            this.forceFieldClumpLabel.AutoSize = true;
            this.forceFieldClumpLabel.Location = new System.Drawing.Point(750, 64);
            this.forceFieldClumpLabel.Name = "forceFieldClumpLabel";
            this.forceFieldClumpLabel.Size = new System.Drawing.Size(70, 13);
            this.forceFieldClumpLabel.TabIndex = 0;
            this.forceFieldClumpLabel.Text = "Clump Chunk";
            // 
            // forceFieldClumpComboBox
            // 
            this.forceFieldClumpComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forceFieldClumpComboBox.Location = new System.Drawing.Point(838, 61);
            this.forceFieldClumpComboBox.Name = "forceFieldClumpComboBox";
            this.forceFieldClumpComboBox.Size = new System.Drawing.Size(304, 21);
            this.forceFieldClumpComboBox.TabIndex = 0;
            this.forceFieldClumpComboBox.SelectedIndexChanged += new System.EventHandler(this.forceFieldClumpComboBox_SelectedIndexChanged);
            // 
            // forceFieldParticleIndexLabel
            // 
            this.forceFieldParticleIndexLabel.AutoSize = true;
            this.forceFieldParticleIndexLabel.Location = new System.Drawing.Point(314, 92);
            this.forceFieldParticleIndexLabel.Name = "forceFieldParticleIndexLabel";
            this.forceFieldParticleIndexLabel.Size = new System.Drawing.Size(106, 13);
            this.forceFieldParticleIndexLabel.TabIndex = 0;
            this.forceFieldParticleIndexLabel.Text = "Linked Particle Index";
            // 
            // forceFieldParticleIndexNumericUpDown
            // 
            this.forceFieldParticleIndexNumericUpDown.Location = new System.Drawing.Point(430, 90);
            this.forceFieldParticleIndexNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.forceFieldParticleIndexNumericUpDown.Name = "forceFieldParticleIndexNumericUpDown";
            this.forceFieldParticleIndexNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.forceFieldParticleIndexNumericUpDown.TabIndex = 0;
            this.forceFieldParticleIndexNumericUpDown.ValueChanged += new System.EventHandler(this.forceFieldParticleIndexNumericUpDown_ValueChanged);
            // 
            // forceFieldPropertyGrid
            // 
            this.forceFieldPropertyGrid.Location = new System.Drawing.Point(317, 144);
            this.forceFieldPropertyGrid.Name = "forceFieldPropertyGrid";
            this.forceFieldPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.forceFieldPropertyGrid.Size = new System.Drawing.Size(825, 393);
            this.forceFieldPropertyGrid.TabIndex = 0;
            this.forceFieldPropertyGrid.ToolbarVisible = false;
            // 
            // nodeTabPage
            // 
            this.nodeTabPage.Controls.Add(this.nodeListBox);
            this.nodeTabPage.Controls.Add(this.nodeAddButton);
            this.nodeTabPage.Controls.Add(this.nodeCopyButton);
            this.nodeTabPage.Controls.Add(this.nodePasteButton);
            this.nodeTabPage.Controls.Add(this.nodeDuplicateButton);
            this.nodeTabPage.Controls.Add(this.nodeDeleteButton);
            this.nodeTabPage.Controls.Add(this.nodeSaveButton);
            this.nodeTabPage.Controls.Add(this.nodeHintLabel);
            this.nodeTabPage.Controls.Add(this.nodeParticleIndexLabel);
            this.nodeTabPage.Controls.Add(this.nodeParticleIndexNumericUpDown);
            this.nodeTabPage.Controls.Add(this.nodeEventsGrid);
            this.nodeTabPage.Controls.Add(this.nodeAddEventButton);
            this.nodeTabPage.Controls.Add(this.nodeDeleteEventButton);
            this.nodeTabPage.Location = new System.Drawing.Point(4, 22);
            this.nodeTabPage.Name = "nodeTabPage";
            this.nodeTabPage.Size = new System.Drawing.Size(1152, 584);
            this.nodeTabPage.TabIndex = 4;
            this.nodeTabPage.Text = "Frames / Nodes";
            this.nodeTabPage.UseVisualStyleBackColor = true;
            // 
            // nodeListBox
            // 
            this.nodeListBox.Location = new System.Drawing.Point(6, 13);
            this.nodeListBox.Name = "nodeListBox";
            this.nodeListBox.Size = new System.Drawing.Size(290, 485);
            this.nodeListBox.TabIndex = 0;
            this.nodeListBox.SelectedIndexChanged += new System.EventHandler(this.nodeListBox_SelectedIndexChanged);
            // 
            // nodeAddButton
            // 
            this.nodeAddButton.Location = new System.Drawing.Point(6, 513);
            this.nodeAddButton.Name = "nodeAddButton";
            this.nodeAddButton.Size = new System.Drawing.Size(64, 24);
            this.nodeAddButton.TabIndex = 1;
            this.nodeAddButton.Text = "Add";
            this.nodeAddButton.Click += new System.EventHandler(this.nodeAddButton_Click);
            // 
            // nodeCopyButton
            // 
            this.nodeCopyButton.Location = new System.Drawing.Point(76, 513);
            this.nodeCopyButton.Name = "nodeCopyButton";
            this.nodeCopyButton.Size = new System.Drawing.Size(60, 24);
            this.nodeCopyButton.TabIndex = 2;
            this.nodeCopyButton.Text = "Copy";
            this.nodeCopyButton.Click += new System.EventHandler(this.nodeCopyButton_Click);
            // 
            // nodePasteButton
            // 
            this.nodePasteButton.Location = new System.Drawing.Point(142, 513);
            this.nodePasteButton.Name = "nodePasteButton";
            this.nodePasteButton.Size = new System.Drawing.Size(60, 24);
            this.nodePasteButton.TabIndex = 3;
            this.nodePasteButton.Text = "Paste";
            this.nodePasteButton.Click += new System.EventHandler(this.nodePasteButton_Click);
            // 
            // nodeDuplicateButton
            // 
            this.nodeDuplicateButton.Location = new System.Drawing.Point(208, 513);
            this.nodeDuplicateButton.Name = "nodeDuplicateButton";
            this.nodeDuplicateButton.Size = new System.Drawing.Size(75, 24);
            this.nodeDuplicateButton.TabIndex = 4;
            this.nodeDuplicateButton.Text = "Duplicate";
            this.nodeDuplicateButton.Click += new System.EventHandler(this.nodeDuplicateButton_Click);
            // 
            // nodeDeleteButton
            // 
            this.nodeDeleteButton.Location = new System.Drawing.Point(6, 543);
            this.nodeDeleteButton.Name = "nodeDeleteButton";
            this.nodeDeleteButton.Size = new System.Drawing.Size(64, 24);
            this.nodeDeleteButton.TabIndex = 5;
            this.nodeDeleteButton.Text = "Remove";
            this.nodeDeleteButton.Click += new System.EventHandler(this.nodeDeleteButton_Click);
            // 
            // nodeSaveButton
            // 
            this.nodeSaveButton.Location = new System.Drawing.Point(76, 543);
            this.nodeSaveButton.Name = "nodeSaveButton";
            this.nodeSaveButton.Size = new System.Drawing.Size(60, 24);
            this.nodeSaveButton.TabIndex = 6;
            this.nodeSaveButton.Text = "Save";
            this.nodeSaveButton.Click += new System.EventHandler(this.nodeSaveButton_Click);
            // 
            // nodeHintLabel
            // 
            this.nodeHintLabel.AutoSize = true;
            this.nodeHintLabel.Location = new System.Drawing.Point(314, 11);
            this.nodeHintLabel.MaximumSize = new System.Drawing.Size(820, 0);
            this.nodeHintLabel.Name = "nodeHintLabel";
            this.nodeHintLabel.Size = new System.Drawing.Size(571, 13);
            this.nodeHintLabel.TabIndex = 7;
            this.nodeHintLabel.Text = "Select a frame timeline on the left, set the linked particle index, then add simp" +
    "le Spawn and Despawn timing rows below.";
            // 
            // nodeParticleIndexLabel
            // 
            this.nodeParticleIndexLabel.AutoSize = true;
            this.nodeParticleIndexLabel.Location = new System.Drawing.Point(314, 44);
            this.nodeParticleIndexLabel.Name = "nodeParticleIndexLabel";
            this.nodeParticleIndexLabel.Size = new System.Drawing.Size(106, 13);
            this.nodeParticleIndexLabel.TabIndex = 8;
            this.nodeParticleIndexLabel.Text = "Linked Particle Index";
            // 
            // nodeParticleIndexNumericUpDown
            // 
            this.nodeParticleIndexNumericUpDown.Location = new System.Drawing.Point(426, 42);
            this.nodeParticleIndexNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.nodeParticleIndexNumericUpDown.Name = "nodeParticleIndexNumericUpDown";
            this.nodeParticleIndexNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.nodeParticleIndexNumericUpDown.TabIndex = 9;
            this.nodeParticleIndexNumericUpDown.ValueChanged += new System.EventHandler(this.nodeParticleIndexNumericUpDown_ValueChanged);
            // 
            // nodeEventsGrid
            // 
            this.nodeEventsGrid.AllowUserToAddRows = false;
            this.nodeEventsGrid.AllowUserToDeleteRows = false;
            this.nodeEventsGrid.Location = new System.Drawing.Point(317, 72);
            this.nodeEventsGrid.Name = "nodeEventsGrid";
            this.nodeEventsGrid.RowHeadersVisible = false;
            this.nodeEventsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.nodeEventsGrid.Size = new System.Drawing.Size(825, 432);
            this.nodeEventsGrid.TabIndex = 10;
            // 
            // nodeAddEventButton
            // 
            this.nodeAddEventButton.Location = new System.Drawing.Point(317, 513);
            this.nodeAddEventButton.Name = "nodeAddEventButton";
            this.nodeAddEventButton.Size = new System.Drawing.Size(92, 24);
            this.nodeAddEventButton.TabIndex = 11;
            this.nodeAddEventButton.Text = "Add Timing Row";
            this.nodeAddEventButton.Click += new System.EventHandler(this.nodeAddEventButton_Click);
            // 
            // nodeDeleteEventButton
            // 
            this.nodeDeleteEventButton.Location = new System.Drawing.Point(415, 513);
            this.nodeDeleteEventButton.Name = "nodeDeleteEventButton";
            this.nodeDeleteEventButton.Size = new System.Drawing.Size(95, 24);
            this.nodeDeleteEventButton.TabIndex = 12;
            this.nodeDeleteEventButton.Text = "Remove Timing Row";
            this.nodeDeleteEventButton.Click += new System.EventHandler(this.nodeDeleteEventButton_Click);
            // 
            // Tool_ParticleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 730);
            this.Controls.Add(this.particleTabControl);
            this.Controls.Add(this.chunkEditorPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1200, 769);
            this.Name = "Tool_ParticleEditor";
            this.Text = "Particle Chunk Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.chunkEditorPanel.ResumeLayout(false);
            this.chunkEditorPanel.PerformLayout();
            this.particleTabControl.ResumeLayout(false);
            this.managerTabPage.ResumeLayout(false);
            this.managerTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.managerEntryIndexNumericUpDown)).EndInit();
            this.resourceTabPage.ResumeLayout(false);
            this.resourceTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resourceParticleIndexNumericUpDown)).EndInit();
            this.positionTabPage.ResumeLayout(false);
            this.positionTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionParticleIndexNumericUpDown)).EndInit();
            this.forceFieldTabPage.ResumeLayout(false);
            this.forceFieldTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFieldParticleIndexNumericUpDown)).EndInit();
            this.nodeTabPage.ResumeLayout(false);
            this.nodeTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeParticleIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeEventsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private static void BuildPropertyTab(TabPage tabPage, ListBox listBox, Label hintLabel, string hintText, Label combo1Label, string combo1Text, ComboBox combo1, Label indexLabel, string indexText, NumericUpDown indexNumericUpDown, Label combo2Label, ComboBox combo2, PropertyGrid propertyGrid, Button addButton, Button copyButton, Button pasteButton, Button duplicateButton, Button deleteButton, Button saveButton, System.EventHandler listChanged, System.EventHandler combo1Changed, System.EventHandler indexChanged, System.EventHandler combo2Changed, System.EventHandler addClick, System.EventHandler copyClick, System.EventHandler pasteClick, System.EventHandler duplicateClick, System.EventHandler deleteClick, System.EventHandler saveClick)
        {
            Label entriesLabel = new Label();
            Label quickLinksLabel = new Label();
            Label detailsLabel = new Label();
            entriesLabel.AutoSize = true;
            entriesLabel.Location = new Point(6, 6);
            entriesLabel.Text = "Entries";
            quickLinksLabel.AutoSize = true;
            quickLinksLabel.Location = new Point(314, 44);
            quickLinksLabel.Text = "Quick Links";
            detailsLabel.AutoSize = true;
            detailsLabel.Location = new Point(314, 124);
            detailsLabel.Text = "Detailed Values";
            listBox.Location = new Point(6, 26);
            listBox.Size = new Size(290, 478);
            listBox.SelectedIndexChanged += listChanged;
            addButton.Location = new Point(6, 513);
            addButton.Size = new Size(64, 24);
            addButton.Text = "Add";
            addButton.Click += addClick;
            copyButton.Location = new Point(76, 513);
            copyButton.Size = new Size(60, 24);
            copyButton.Text = "Copy";
            copyButton.Click += copyClick;
            pasteButton.Location = new Point(142, 513);
            pasteButton.Size = new Size(60, 24);
            pasteButton.Text = "Paste";
            pasteButton.Click += pasteClick;
            duplicateButton.Location = new Point(208, 513);
            duplicateButton.Size = new Size(75, 24);
            duplicateButton.Text = "Duplicate";
            duplicateButton.Click += duplicateClick;
            deleteButton.Location = new Point(289, 513);
            deleteButton.Size = new Size(70, 24);
            deleteButton.Text = "Remove";
            deleteButton.Click += deleteClick;
            saveButton.Location = new Point(365, 513);
            saveButton.Size = new Size(63, 24);
            saveButton.Text = "Apply";
            saveButton.Click += saveClick;
            hintLabel.AutoSize = true;
            hintLabel.Location = new Point(314, 11);
            hintLabel.MaximumSize = new Size(820, 0);
            hintLabel.Text = hintText;
            combo1Label.AutoSize = true;
            combo1Label.Location = new Point(314, 64);
            combo1Label.Text = combo1Text;
            combo1.DropDownStyle = ComboBoxStyle.DropDownList;
            combo1.Location = new Point(430, 61);
            combo1.Size = new Size(300, 21);
            combo1.SelectedIndexChanged += combo1Changed;
            indexLabel.AutoSize = true;
            indexLabel.Location = new Point(314, 92);
            indexLabel.Text = indexText;
            indexNumericUpDown.Location = new Point(430, 90);
            indexNumericUpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            indexNumericUpDown.Size = new Size(120, 20);
            indexNumericUpDown.ValueChanged += indexChanged;
            propertyGrid.Location = new Point(317, 144);
            propertyGrid.Size = new Size(825, 393);
            propertyGrid.HelpVisible = true;
            propertyGrid.PropertySort = PropertySort.Categorized;
            propertyGrid.ToolbarVisible = false;
            tabPage.Controls.Add(entriesLabel);
            tabPage.Controls.Add(quickLinksLabel);
            tabPage.Controls.Add(detailsLabel);
            tabPage.Controls.Add(listBox);
            tabPage.Controls.Add(addButton);
            tabPage.Controls.Add(copyButton);
            tabPage.Controls.Add(pasteButton);
            tabPage.Controls.Add(duplicateButton);
            tabPage.Controls.Add(deleteButton);
            tabPage.Controls.Add(saveButton);
            tabPage.Controls.Add(hintLabel);
            tabPage.Controls.Add(combo1Label);
            tabPage.Controls.Add(combo1);
            tabPage.Controls.Add(indexLabel);
            tabPage.Controls.Add(indexNumericUpDown);
            if (combo2Label != null && combo2 != null)
            {
                combo2Label.AutoSize = true;
                combo2Label.Location = new Point(750, 64);
                combo2Label.Text = "Clump Chunk";
                combo2.DropDownStyle = ComboBoxStyle.DropDownList;
                combo2.Location = new Point(838, 61);
                combo2.Size = new Size(304, 21);
                combo2.SelectedIndexChanged += combo2Changed;
                tabPage.Controls.Add(combo2Label);
                tabPage.Controls.Add(combo2);
            }
            tabPage.Controls.Add(propertyGrid);
        }
    }
}
