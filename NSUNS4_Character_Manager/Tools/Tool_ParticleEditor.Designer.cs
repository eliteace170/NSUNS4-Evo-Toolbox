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
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.openToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new ToolStripMenuItem();
            this.closeToolStripMenuItem = new ToolStripMenuItem();
            this.chunkEditorPanel = new Panel();
            this.editReferencesButton = new Button();
            this.deleteChunkButton = new Button();
            this.addChunkButton = new Button();
            this.saveChunkMetaButton = new Button();
            this.chunkPathTextBox = new TextBox();
            this.chunkPathLabel = new Label();
            this.chunkNameTextBox = new TextBox();
            this.chunkNameLabel = new Label();
            this.chunkComboBox = new ComboBox();
            this.chunkLabel = new Label();
            this.particleTabControl = new TabControl();
            this.managerTabPage = new TabPage();
            this.resourceTabPage = new TabPage();
            this.positionTabPage = new TabPage();
            this.forceFieldTabPage = new TabPage();
            this.nodeTabPage = new TabPage();
            this.managerListBox = new ListBox();
            this.managerPropertyGrid = new PropertyGrid();
            this.managerAddButton = new Button();
            this.managerDuplicateButton = new Button();
            this.managerDeleteButton = new Button();
            this.managerSaveButton = new Button();
            this.managerHintLabel = new Label();
            this.managerAnimationLabel = new Label();
            this.managerAnimationComboBox = new ComboBox();
            this.managerEntryIndexLabel = new Label();
            this.managerEntryIndexNumericUpDown = new NumericUpDown();
            this.resourceListBox = new ListBox();
            this.resourcePropertyGrid = new PropertyGrid();
            this.resourceAddButton = new Button();
            this.resourceDuplicateButton = new Button();
            this.resourceDeleteButton = new Button();
            this.resourceSaveButton = new Button();
            this.resourceHintLabel = new Label();
            this.resourceEffectLabel = new Label();
            this.resourceEffectComboBox = new ComboBox();
            this.resourceParticleIndexLabel = new Label();
            this.resourceParticleIndexNumericUpDown = new NumericUpDown();
            this.positionListBox = new ListBox();
            this.positionPropertyGrid = new PropertyGrid();
            this.positionAddButton = new Button();
            this.positionDuplicateButton = new Button();
            this.positionDeleteButton = new Button();
            this.positionSaveButton = new Button();
            this.positionHintLabel = new Label();
            this.positionCoordLabel = new Label();
            this.positionCoordComboBox = new ComboBox();
            this.positionParticleIndexLabel = new Label();
            this.positionParticleIndexNumericUpDown = new NumericUpDown();
            this.positionClumpLabel = new Label();
            this.positionClumpComboBox = new ComboBox();
            this.forceFieldListBox = new ListBox();
            this.forceFieldPropertyGrid = new PropertyGrid();
            this.forceFieldAddButton = new Button();
            this.forceFieldDuplicateButton = new Button();
            this.forceFieldDeleteButton = new Button();
            this.forceFieldSaveButton = new Button();
            this.forceFieldHintLabel = new Label();
            this.forceFieldCoordLabel = new Label();
            this.forceFieldCoordComboBox = new ComboBox();
            this.forceFieldParticleIndexLabel = new Label();
            this.forceFieldParticleIndexNumericUpDown = new NumericUpDown();
            this.forceFieldClumpLabel = new Label();
            this.forceFieldClumpComboBox = new ComboBox();
            this.nodeListBox = new ListBox();
            this.nodeEventsGrid = new DataGridView();
            this.nodeAddButton = new Button();
            this.nodeDuplicateButton = new Button();
            this.nodeDeleteButton = new Button();
            this.nodeSaveButton = new Button();
            this.nodeHintLabel = new Label();
            this.nodeAddEventButton = new Button();
            this.nodeDeleteEventButton = new Button();
            this.nodeParticleIndexLabel = new Label();
            this.nodeParticleIndexNumericUpDown = new NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.chunkEditorPanel.SuspendLayout();
            this.particleTabControl.SuspendLayout();
            ((ISupportInitialize)(this.managerEntryIndexNumericUpDown)).BeginInit();
            ((ISupportInitialize)(this.resourceParticleIndexNumericUpDown)).BeginInit();
            ((ISupportInitialize)(this.positionParticleIndexNumericUpDown)).BeginInit();
            ((ISupportInitialize)(this.forceFieldParticleIndexNumericUpDown)).BeginInit();
            ((ISupportInitialize)(this.nodeEventsGrid)).BeginInit();
            ((ISupportInitialize)(this.nodeParticleIndexNumericUpDown)).BeginInit();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(1184, 24);
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.openToolStripMenuItem, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.closeToolStripMenuItem });
            this.fileToolStripMenuItem.Text = "File";
            //
            // openToolStripMenuItem
            //
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            //
            // saveToolStripMenuItem
            //
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            //
            // saveAsToolStripMenuItem
            //
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            //
            // closeToolStripMenuItem
            //
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            //
            // chunkEditorPanel
            //
            this.chunkEditorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.chunkEditorPanel.BorderStyle = BorderStyle.FixedSingle;
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
            this.chunkEditorPanel.Location = new Point(12, 30);
            this.chunkEditorPanel.Size = new Size(1160, 72);
            //
            // chunk header controls
            //
            this.chunkLabel.AutoSize = true;
            this.chunkLabel.Location = new Point(10, 13);
            this.chunkLabel.Text = "Particle Chunk";
            this.chunkComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.chunkComboBox.Location = new Point(88, 10);
            this.chunkComboBox.Size = new Size(280, 21);
            this.chunkComboBox.SelectedIndexChanged += new System.EventHandler(this.chunkComboBox_SelectedIndexChanged);
            this.chunkNameLabel.AutoSize = true;
            this.chunkNameLabel.Location = new Point(386, 13);
            this.chunkNameLabel.Text = "Name";
            this.chunkNameTextBox.Location = new Point(430, 10);
            this.chunkNameTextBox.Size = new Size(220, 20);
            this.chunkPathLabel.AutoSize = true;
            this.chunkPathLabel.Location = new Point(10, 44);
            this.chunkPathLabel.Text = "Path";
            this.chunkPathTextBox.Location = new Point(88, 41);
            this.chunkPathTextBox.Size = new Size(562, 20);
            this.saveChunkMetaButton.Location = new Point(666, 9);
            this.saveChunkMetaButton.Size = new Size(104, 23);
            this.saveChunkMetaButton.Text = "Apply Metadata";
            this.saveChunkMetaButton.Click += new System.EventHandler(this.saveChunkMetaButton_Click);
            this.addChunkButton.Location = new Point(776, 9);
            this.addChunkButton.Size = new Size(90, 23);
            this.addChunkButton.Text = "Add Chunk";
            this.addChunkButton.Click += new System.EventHandler(this.addChunkButton_Click);
            this.deleteChunkButton.Location = new Point(872, 9);
            this.deleteChunkButton.Size = new Size(94, 23);
            this.deleteChunkButton.Text = "Delete Chunk";
            this.deleteChunkButton.Click += new System.EventHandler(this.deleteChunkButton_Click);
            this.editReferencesButton.Location = new Point(972, 9);
            this.editReferencesButton.Size = new Size(173, 23);
            this.editReferencesButton.Text = "Edit Linked Chunks";
            this.editReferencesButton.Click += new System.EventHandler(this.editReferencesButton_Click);
            //
            // particleTabControl
            //
            this.particleTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.particleTabControl.Controls.Add(this.managerTabPage);
            this.particleTabControl.Controls.Add(this.resourceTabPage);
            this.particleTabControl.Controls.Add(this.positionTabPage);
            this.particleTabControl.Controls.Add(this.forceFieldTabPage);
            this.particleTabControl.Controls.Add(this.nodeTabPage);
            this.particleTabControl.Location = new Point(12, 108);
            this.particleTabControl.Size = new Size(1160, 610);
            //
            // tabs
            //
            this.managerTabPage.Text = "Particle Settings";
            this.resourceTabPage.Text = "Resources";
            this.positionTabPage.Text = "Bone / Position";
            this.forceFieldTabPage.Text = "Force Fields";
            this.nodeTabPage.Text = "Frames / Nodes";
            this.managerTabPage.UseVisualStyleBackColor = true;
            this.resourceTabPage.UseVisualStyleBackColor = true;
            this.positionTabPage.UseVisualStyleBackColor = true;
            this.forceFieldTabPage.UseVisualStyleBackColor = true;
            this.nodeTabPage.UseVisualStyleBackColor = true;
            BuildPropertyTab(this.managerTabPage, this.managerListBox, this.managerHintLabel, "Grouped to match the particle panel: core settings plus alpha, scale, color, and timing.", this.managerAnimationLabel, "Animation Chunk", this.managerAnimationComboBox, this.managerEntryIndexLabel, "Particle Index", this.managerEntryIndexNumericUpDown, null, null, this.managerPropertyGrid, this.managerAddButton, this.managerDuplicateButton, this.managerDeleteButton, this.managerSaveButton, this.managerListBox_SelectedIndexChanged, this.managerAnimationComboBox_SelectedIndexChanged, this.managerEntryIndexNumericUpDown_ValueChanged, null, this.managerAddButton_Click, this.managerDuplicateButton_Click, this.managerDeleteButton_Click, this.managerSaveButton_Click);
            BuildPropertyTab(this.resourceTabPage, this.resourceListBox, this.resourceHintLabel, "Select the referenced effect chunk by name instead of editing a raw index.", this.resourceEffectLabel, "Effect Chunk", this.resourceEffectComboBox, this.resourceParticleIndexLabel, "Particle Index", this.resourceParticleIndexNumericUpDown, null, null, this.resourcePropertyGrid, this.resourceAddButton, this.resourceDuplicateButton, this.resourceDeleteButton, this.resourceSaveButton, this.resourceListBox_SelectedIndexChanged, this.resourceEffectComboBox_SelectedIndexChanged, this.resourceParticleIndexNumericUpDown_ValueChanged, null, this.resourceAddButton_Click, this.resourceDuplicateButton_Click, this.resourceDeleteButton_Click, this.resourceSaveButton_Click);
            BuildPropertyTab(this.positionTabPage, this.positionListBox, this.positionHintLabel, "Coord and clump references are explicit selectors. Remaining values stay grouped in the editor.", this.positionCoordLabel, "Coord Chunk", this.positionCoordComboBox, this.positionParticleIndexLabel, "Particle Index", this.positionParticleIndexNumericUpDown, this.positionClumpLabel, this.positionClumpComboBox, this.positionPropertyGrid, this.positionAddButton, this.positionDuplicateButton, this.positionDeleteButton, this.positionSaveButton, this.positionListBox_SelectedIndexChanged, this.positionCoordComboBox_SelectedIndexChanged, this.positionParticleIndexNumericUpDown_ValueChanged, this.positionClumpComboBox_SelectedIndexChanged, this.positionAddButton_Click, this.positionDuplicateButton_Click, this.positionDeleteButton_Click, this.positionSaveButton_Click);
            BuildPropertyTab(this.forceFieldTabPage, this.forceFieldListBox, this.forceFieldHintLabel, "Force fields use named coord and clump selectors so the user can switch actual referenced chunks.", this.forceFieldCoordLabel, "Coord Chunk", this.forceFieldCoordComboBox, this.forceFieldParticleIndexLabel, "Particle Index", this.forceFieldParticleIndexNumericUpDown, this.forceFieldClumpLabel, this.forceFieldClumpComboBox, this.forceFieldPropertyGrid, this.forceFieldAddButton, this.forceFieldDuplicateButton, this.forceFieldDeleteButton, this.forceFieldSaveButton, this.forceFieldListBox_SelectedIndexChanged, this.forceFieldCoordComboBox_SelectedIndexChanged, this.forceFieldParticleIndexNumericUpDown_ValueChanged, this.forceFieldClumpComboBox_SelectedIndexChanged, this.forceFieldAddButton_Click, this.forceFieldDuplicateButton_Click, this.forceFieldDeleteButton_Click, this.forceFieldSaveButton_Click);
            //
            // node tab
            //
            this.nodeListBox.Location = new Point(6, 6);
            this.nodeListBox.Size = new Size(290, 498);
            this.nodeListBox.SelectedIndexChanged += new System.EventHandler(this.nodeListBox_SelectedIndexChanged);
            this.nodeAddButton.Location = new Point(6, 513);
            this.nodeAddButton.Size = new Size(64, 24);
            this.nodeAddButton.Text = "Add";
            this.nodeAddButton.Click += new System.EventHandler(this.nodeAddButton_Click);
            this.nodeDuplicateButton.Location = new Point(76, 513);
            this.nodeDuplicateButton.Size = new Size(75, 24);
            this.nodeDuplicateButton.Text = "Duplicate";
            this.nodeDuplicateButton.Click += new System.EventHandler(this.nodeDuplicateButton_Click);
            this.nodeDeleteButton.Location = new Point(157, 513);
            this.nodeDeleteButton.Size = new Size(62, 24);
            this.nodeDeleteButton.Text = "Delete";
            this.nodeDeleteButton.Click += new System.EventHandler(this.nodeDeleteButton_Click);
            this.nodeSaveButton.Location = new Point(225, 513);
            this.nodeSaveButton.Size = new Size(71, 24);
            this.nodeSaveButton.Text = "Apply";
            this.nodeSaveButton.Click += new System.EventHandler(this.nodeSaveButton_Click);
            this.nodeHintLabel.AutoSize = true;
            this.nodeHintLabel.Location = new Point(314, 11);
            this.nodeHintLabel.MaximumSize = new Size(820, 0);
            this.nodeHintLabel.Text = "Each row is one timeline point for the selected node. Use On / Spawn and Off / Despawn with the time value instead of editing the packed frame integers directly.";
            this.nodeParticleIndexLabel.AutoSize = true;
            this.nodeParticleIndexLabel.Location = new Point(314, 44);
            this.nodeParticleIndexLabel.Text = "Particle Index";
            this.nodeParticleIndexNumericUpDown.Location = new Point(400, 41);
            this.nodeParticleIndexNumericUpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            this.nodeParticleIndexNumericUpDown.Size = new Size(120, 20);
            this.nodeParticleIndexNumericUpDown.ValueChanged += new System.EventHandler(this.nodeParticleIndexNumericUpDown_ValueChanged);
            this.nodeEventsGrid.Location = new Point(317, 72);
            this.nodeEventsGrid.Size = new Size(825, 432);
            this.nodeEventsGrid.AllowUserToAddRows = false;
            this.nodeEventsGrid.AllowUserToDeleteRows = false;
            this.nodeEventsGrid.RowHeadersVisible = false;
            this.nodeEventsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.nodeAddEventButton.Location = new Point(317, 513);
            this.nodeAddEventButton.Size = new Size(92, 24);
            this.nodeAddEventButton.Text = "Add Timing";
            this.nodeAddEventButton.Click += new System.EventHandler(this.nodeAddEventButton_Click);
            this.nodeDeleteEventButton.Location = new Point(415, 513);
            this.nodeDeleteEventButton.Size = new Size(95, 24);
            this.nodeDeleteEventButton.Text = "Delete Timing";
            this.nodeDeleteEventButton.Click += new System.EventHandler(this.nodeDeleteEventButton_Click);
            this.nodeTabPage.Controls.Add(this.nodeListBox);
            this.nodeTabPage.Controls.Add(this.nodeAddButton);
            this.nodeTabPage.Controls.Add(this.nodeDuplicateButton);
            this.nodeTabPage.Controls.Add(this.nodeDeleteButton);
            this.nodeTabPage.Controls.Add(this.nodeSaveButton);
            this.nodeTabPage.Controls.Add(this.nodeHintLabel);
            this.nodeTabPage.Controls.Add(this.nodeParticleIndexLabel);
            this.nodeTabPage.Controls.Add(this.nodeParticleIndexNumericUpDown);
            this.nodeTabPage.Controls.Add(this.nodeEventsGrid);
            this.nodeTabPage.Controls.Add(this.nodeAddEventButton);
            this.nodeTabPage.Controls.Add(this.nodeDeleteEventButton);
            //
            // Tool_ParticleEditor
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1184, 730);
            this.Controls.Add(this.particleTabControl);
            this.Controls.Add(this.chunkEditorPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new Size(1200, 769);
            this.Name = "Tool_ParticleEditor";
            this.Text = "Particle Chunk Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.chunkEditorPanel.ResumeLayout(false);
            this.chunkEditorPanel.PerformLayout();
            this.particleTabControl.ResumeLayout(false);
            ((ISupportInitialize)(this.managerEntryIndexNumericUpDown)).EndInit();
            ((ISupportInitialize)(this.resourceParticleIndexNumericUpDown)).EndInit();
            ((ISupportInitialize)(this.positionParticleIndexNumericUpDown)).EndInit();
            ((ISupportInitialize)(this.forceFieldParticleIndexNumericUpDown)).EndInit();
            ((ISupportInitialize)(this.nodeEventsGrid)).EndInit();
            ((ISupportInitialize)(this.nodeParticleIndexNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private static void BuildPropertyTab(TabPage tabPage, ListBox listBox, Label hintLabel, string hintText, Label combo1Label, string combo1Text, ComboBox combo1, Label indexLabel, string indexText, NumericUpDown indexNumericUpDown, Label combo2Label, ComboBox combo2, PropertyGrid propertyGrid, Button addButton, Button duplicateButton, Button deleteButton, Button saveButton, System.EventHandler listChanged, System.EventHandler combo1Changed, System.EventHandler indexChanged, System.EventHandler combo2Changed, System.EventHandler addClick, System.EventHandler duplicateClick, System.EventHandler deleteClick, System.EventHandler saveClick)
        {
            listBox.Location = new Point(6, 6);
            listBox.Size = new Size(290, 498);
            listBox.SelectedIndexChanged += listChanged;
            addButton.Location = new Point(6, 513);
            addButton.Size = new Size(64, 24);
            addButton.Text = "Add";
            addButton.Click += addClick;
            duplicateButton.Location = new Point(76, 513);
            duplicateButton.Size = new Size(75, 24);
            duplicateButton.Text = "Duplicate";
            duplicateButton.Click += duplicateClick;
            deleteButton.Location = new Point(157, 513);
            deleteButton.Size = new Size(62, 24);
            deleteButton.Text = "Delete";
            deleteButton.Click += deleteClick;
            saveButton.Location = new Point(225, 513);
            saveButton.Size = new Size(71, 24);
            saveButton.Text = "Apply";
            saveButton.Click += saveClick;
            hintLabel.AutoSize = true;
            hintLabel.Location = new Point(314, 11);
            hintLabel.MaximumSize = new Size(820, 0);
            hintLabel.Text = hintText;
            combo1Label.AutoSize = true;
            combo1Label.Location = new Point(314, 44);
            combo1Label.Text = combo1Text;
            combo1.DropDownStyle = ComboBoxStyle.DropDownList;
            combo1.Location = new Point(400, 41);
            combo1.Size = new Size(300, 21);
            combo1.SelectedIndexChanged += combo1Changed;
            indexLabel.AutoSize = true;
            indexLabel.Location = new Point(314, 73);
            indexLabel.Text = indexText;
            indexNumericUpDown.Location = new Point(400, 71);
            indexNumericUpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            indexNumericUpDown.Size = new Size(120, 20);
            indexNumericUpDown.ValueChanged += indexChanged;
            propertyGrid.Location = new Point(317, 100);
            propertyGrid.Size = new Size(825, 437);
            propertyGrid.HelpVisible = true;
            propertyGrid.PropertySort = PropertySort.Categorized;
            propertyGrid.ToolbarVisible = false;
            tabPage.Controls.Add(listBox);
            tabPage.Controls.Add(addButton);
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
                combo2Label.Location = new Point(722, 44);
                combo2Label.Text = "Clump Chunk";
                combo2.DropDownStyle = ComboBoxStyle.DropDownList;
                combo2.Location = new Point(807, 41);
                combo2.Size = new Size(335, 21);
                combo2.SelectedIndexChanged += combo2Changed;
                tabPage.Controls.Add(combo2Label);
                tabPage.Controls.Add(combo2);
            }
            tabPage.Controls.Add(propertyGrid);
        }
    }
}
