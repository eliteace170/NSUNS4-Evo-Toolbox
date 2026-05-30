using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_ParticleChunkReferenceEditor
    {
        private IContainer components = null;
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel headerLayout;
        private Label titleLabel;
        private Label hintLabel;
        private Label searchLabel;
        private TextBox searchTextBox;
        private FlowLayoutPanel summaryPanel;
        private Label countLabel;
        private Label selectionLabel;
        private SplitContainer editorSplitContainer;
        private DataGridView referencesGrid;
        private DataGridViewTextBoxColumn rowNumberColumn;
        private DataGridViewTextBoxColumn referenceNameColumn;
        private DataGridViewComboBoxColumn referenceTypeColumn;
        private DataGridViewTextBoxColumn referencePathColumn;
        private GroupBox detailsGroupBox;
        private TableLayoutPanel detailsLayout;
        private Label indexCaptionLabel;
        private Label indexValueLabel;
        private Label originalIndexCaptionLabel;
        private Label originalIndexValueLabel;
        private Label nameFieldLabel;
        private TextBox nameTextBox;
        private Label typeFieldLabel;
        private ComboBox typeComboBox;
        private Label pathFieldLabel;
        private TextBox pathTextBox;
        private TableLayoutPanel buttonPanel;
        private Button addButton;
        private Button duplicateButton;
        private Button deleteButton;
        private Button moveUpButton;
        private Button moveDownButton;
        private Button okButton;
        private Button cancelButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolTip toolTip;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.mainLayout = new TableLayoutPanel();
            this.headerLayout = new TableLayoutPanel();
            this.titleLabel = new Label();
            this.hintLabel = new Label();
            this.searchLabel = new Label();
            this.searchTextBox = new TextBox();
            this.summaryPanel = new FlowLayoutPanel();
            this.countLabel = new Label();
            this.selectionLabel = new Label();
            this.editorSplitContainer = new SplitContainer();
            this.referencesGrid = new DataGridView();
            this.rowNumberColumn = new DataGridViewTextBoxColumn();
            this.referenceNameColumn = new DataGridViewTextBoxColumn();
            this.referenceTypeColumn = new DataGridViewComboBoxColumn();
            this.referencePathColumn = new DataGridViewTextBoxColumn();
            this.detailsGroupBox = new GroupBox();
            this.detailsLayout = new TableLayoutPanel();
            this.indexCaptionLabel = new Label();
            this.indexValueLabel = new Label();
            this.originalIndexCaptionLabel = new Label();
            this.originalIndexValueLabel = new Label();
            this.nameFieldLabel = new Label();
            this.nameTextBox = new TextBox();
            this.typeFieldLabel = new Label();
            this.typeComboBox = new ComboBox();
            this.pathFieldLabel = new Label();
            this.pathTextBox = new TextBox();
            this.buttonPanel = new TableLayoutPanel();
            this.addButton = new Button();
            this.duplicateButton = new Button();
            this.deleteButton = new Button();
            this.moveUpButton = new Button();
            this.moveDownButton = new Button();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();
            this.toolTip = new ToolTip(this.components);
            this.mainLayout.SuspendLayout();
            this.headerLayout.SuspendLayout();
            this.summaryPanel.SuspendLayout();
            ((ISupportInitialize)(this.editorSplitContainer)).BeginInit();
            this.editorSplitContainer.Panel1.SuspendLayout();
            this.editorSplitContainer.Panel2.SuspendLayout();
            this.editorSplitContainer.SuspendLayout();
            ((ISupportInitialize)(this.referencesGrid)).BeginInit();
            this.detailsGroupBox.SuspendLayout();
            this.detailsLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // mainLayout
            //
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.headerLayout, 0, 0);
            this.mainLayout.Controls.Add(this.summaryPanel, 0, 1);
            this.mainLayout.Controls.Add(this.editorSplitContainer, 0, 2);
            this.mainLayout.Controls.Add(this.buttonPanel, 0, 3);
            this.mainLayout.Controls.Add(this.statusStrip, 0, 4);
            this.mainLayout.Dock = DockStyle.Fill;
            this.mainLayout.Location = new Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new Padding(12, 10, 12, 0);
            this.mainLayout.RowCount = 5;
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            this.mainLayout.Size = new Size(1000, 580);
            this.mainLayout.TabIndex = 0;
            //
            // headerLayout
            //
            this.headerLayout.ColumnCount = 2;
            this.headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270F));
            this.headerLayout.Controls.Add(this.titleLabel, 0, 0);
            this.headerLayout.Controls.Add(this.hintLabel, 0, 1);
            this.headerLayout.Controls.Add(this.searchLabel, 1, 0);
            this.headerLayout.Controls.Add(this.searchTextBox, 1, 1);
            this.headerLayout.Dock = DockStyle.Fill;
            this.headerLayout.Location = new Point(12, 10);
            this.headerLayout.Margin = new Padding(0);
            this.headerLayout.Name = "headerLayout";
            this.headerLayout.RowCount = 2;
            this.headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            this.headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.headerLayout.Size = new Size(976, 62);
            this.headerLayout.TabIndex = 0;
            //
            // titleLabel
            //
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = DockStyle.Fill;
            this.titleLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new Point(0, 0);
            this.titleLabel.Margin = new Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new Size(706, 24);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Linked chunks";
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // hintLabel
            //
            this.hintLabel.Dock = DockStyle.Fill;
            this.hintLabel.ForeColor = SystemColors.GrayText;
            this.hintLabel.Location = new Point(0, 24);
            this.hintLabel.Margin = new Padding(0, 0, 12, 0);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new Size(694, 38);
            this.hintLabel.TabIndex = 1;
            this.hintLabel.Text = "Edit the named chunks this particle page can reference. Reordering changes stored reference indexes used by managers, resources, positions, and force fields.";
            //
            // searchLabel
            //
            this.searchLabel.AutoSize = true;
            this.searchLabel.Dock = DockStyle.Fill;
            this.searchLabel.Location = new Point(706, 0);
            this.searchLabel.Margin = new Padding(0);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new Size(270, 24);
            this.searchLabel.TabIndex = 2;
            this.searchLabel.Text = "Find";
            this.searchLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // searchTextBox
            //
            this.searchTextBox.Dock = DockStyle.Top;
            this.searchTextBox.Location = new Point(706, 27);
            this.searchTextBox.Margin = new Padding(0, 3, 0, 0);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new Size(270, 20);
            this.searchTextBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.searchTextBox, "Search by name, type, or path.");
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            //
            // summaryPanel
            //
            this.summaryPanel.Controls.Add(this.countLabel);
            this.summaryPanel.Controls.Add(this.selectionLabel);
            this.summaryPanel.Dock = DockStyle.Fill;
            this.summaryPanel.Location = new Point(12, 72);
            this.summaryPanel.Margin = new Padding(0);
            this.summaryPanel.Name = "summaryPanel";
            this.summaryPanel.Size = new Size(976, 30);
            this.summaryPanel.TabIndex = 1;
            this.summaryPanel.WrapContents = false;
            //
            // countLabel
            //
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new Point(0, 6);
            this.countLabel.Margin = new Padding(0, 6, 24, 0);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new Size(85, 13);
            this.countLabel.TabIndex = 0;
            this.countLabel.Text = "0 linked chunks";
            //
            // selectionLabel
            //
            this.selectionLabel.AutoSize = true;
            this.selectionLabel.Location = new Point(109, 6);
            this.selectionLabel.Margin = new Padding(0, 6, 0, 0);
            this.selectionLabel.Name = "selectionLabel";
            this.selectionLabel.Size = new Size(78, 13);
            this.selectionLabel.TabIndex = 1;
            this.selectionLabel.Text = "Selected: none";
            //
            // editorSplitContainer
            //
            this.editorSplitContainer.Dock = DockStyle.Fill;
            this.editorSplitContainer.FixedPanel = FixedPanel.Panel2;
            this.editorSplitContainer.Location = new Point(12, 102);
            this.editorSplitContainer.Margin = new Padding(0);
            this.editorSplitContainer.Name = "editorSplitContainer";
            //
            // editorSplitContainer.Panel1
            //
            this.editorSplitContainer.Panel1.Controls.Add(this.referencesGrid);
            //
            // editorSplitContainer.Panel2
            //
            this.editorSplitContainer.Panel2.Controls.Add(this.detailsGroupBox);
            this.editorSplitContainer.Size = new Size(976, 412);
            this.editorSplitContainer.SplitterDistance = 670;
            this.editorSplitContainer.TabIndex = 2;
            //
            // referencesGrid
            //
            this.referencesGrid.AllowUserToAddRows = false;
            this.referencesGrid.AllowUserToDeleteRows = false;
            this.referencesGrid.AllowUserToResizeRows = false;
            this.referencesGrid.AutoGenerateColumns = false;
            this.referencesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.referencesGrid.BackgroundColor = SystemColors.Window;
            this.referencesGrid.BorderStyle = BorderStyle.Fixed3D;
            this.referencesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.referencesGrid.Columns.AddRange(new DataGridViewColumn[] {
            this.rowNumberColumn,
            this.referenceNameColumn,
            this.referenceTypeColumn,
            this.referencePathColumn});
            this.referencesGrid.Dock = DockStyle.Fill;
            this.referencesGrid.Location = new Point(0, 0);
            this.referencesGrid.MultiSelect = false;
            this.referencesGrid.Name = "referencesGrid";
            this.referencesGrid.RowHeadersVisible = false;
            this.referencesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.referencesGrid.Size = new Size(670, 412);
            this.referencesGrid.TabIndex = 0;
            //
            // rowNumberColumn
            //
            this.rowNumberColumn.HeaderText = "#";
            this.rowNumberColumn.Name = "rowNumberColumn";
            this.rowNumberColumn.ReadOnly = true;
            this.rowNumberColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.rowNumberColumn.Width = 42;
            //
            // referenceNameColumn
            //
            this.referenceNameColumn.DataPropertyName = "Name";
            this.referenceNameColumn.HeaderText = "Name";
            this.referenceNameColumn.Name = "referenceNameColumn";
            this.referenceNameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.referenceNameColumn.Width = 220;
            //
            // referenceTypeColumn
            //
            this.referenceTypeColumn.DataPropertyName = "Type";
            this.referenceTypeColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            this.referenceTypeColumn.FlatStyle = FlatStyle.Flat;
            this.referenceTypeColumn.HeaderText = "Type";
            this.referenceTypeColumn.Name = "referenceTypeColumn";
            this.referenceTypeColumn.Width = 150;
            //
            // referencePathColumn
            //
            this.referencePathColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.referencePathColumn.DataPropertyName = "Path";
            this.referencePathColumn.HeaderText = "Path";
            this.referencePathColumn.Name = "referencePathColumn";
            this.referencePathColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // detailsGroupBox
            //
            this.detailsGroupBox.Controls.Add(this.detailsLayout);
            this.detailsGroupBox.Dock = DockStyle.Fill;
            this.detailsGroupBox.Location = new Point(0, 0);
            this.detailsGroupBox.Name = "detailsGroupBox";
            this.detailsGroupBox.Padding = new Padding(10);
            this.detailsGroupBox.Size = new Size(302, 412);
            this.detailsGroupBox.TabIndex = 0;
            this.detailsGroupBox.TabStop = false;
            this.detailsGroupBox.Text = "Selected reference";
            //
            // detailsLayout
            //
            this.detailsLayout.ColumnCount = 2;
            this.detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            this.detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.detailsLayout.Controls.Add(this.indexCaptionLabel, 0, 0);
            this.detailsLayout.Controls.Add(this.indexValueLabel, 1, 0);
            this.detailsLayout.Controls.Add(this.originalIndexCaptionLabel, 0, 1);
            this.detailsLayout.Controls.Add(this.originalIndexValueLabel, 1, 1);
            this.detailsLayout.Controls.Add(this.nameFieldLabel, 0, 2);
            this.detailsLayout.Controls.Add(this.nameTextBox, 1, 2);
            this.detailsLayout.Controls.Add(this.typeFieldLabel, 0, 3);
            this.detailsLayout.Controls.Add(this.typeComboBox, 1, 3);
            this.detailsLayout.Controls.Add(this.pathFieldLabel, 0, 4);
            this.detailsLayout.Controls.Add(this.pathTextBox, 1, 4);
            this.detailsLayout.Dock = DockStyle.Fill;
            this.detailsLayout.Location = new Point(10, 23);
            this.detailsLayout.Name = "detailsLayout";
            this.detailsLayout.RowCount = 6;
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            this.detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.detailsLayout.Size = new Size(282, 379);
            this.detailsLayout.TabIndex = 0;
            //
            // indexCaptionLabel
            //
            this.indexCaptionLabel.Dock = DockStyle.Fill;
            this.indexCaptionLabel.Location = new Point(0, 0);
            this.indexCaptionLabel.Margin = new Padding(0);
            this.indexCaptionLabel.Name = "indexCaptionLabel";
            this.indexCaptionLabel.Size = new Size(88, 28);
            this.indexCaptionLabel.TabIndex = 0;
            this.indexCaptionLabel.Text = "Index";
            this.indexCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // indexValueLabel
            //
            this.indexValueLabel.Dock = DockStyle.Fill;
            this.indexValueLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.indexValueLabel.Location = new Point(88, 0);
            this.indexValueLabel.Margin = new Padding(0);
            this.indexValueLabel.Name = "indexValueLabel";
            this.indexValueLabel.Size = new Size(194, 28);
            this.indexValueLabel.TabIndex = 1;
            this.indexValueLabel.Text = "-";
            this.indexValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // originalIndexCaptionLabel
            //
            this.originalIndexCaptionLabel.Dock = DockStyle.Fill;
            this.originalIndexCaptionLabel.Location = new Point(0, 28);
            this.originalIndexCaptionLabel.Margin = new Padding(0);
            this.originalIndexCaptionLabel.Name = "originalIndexCaptionLabel";
            this.originalIndexCaptionLabel.Size = new Size(88, 28);
            this.originalIndexCaptionLabel.TabIndex = 2;
            this.originalIndexCaptionLabel.Text = "Original";
            this.originalIndexCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // originalIndexValueLabel
            //
            this.originalIndexValueLabel.Dock = DockStyle.Fill;
            this.originalIndexValueLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.originalIndexValueLabel.Location = new Point(88, 28);
            this.originalIndexValueLabel.Margin = new Padding(0);
            this.originalIndexValueLabel.Name = "originalIndexValueLabel";
            this.originalIndexValueLabel.Size = new Size(194, 28);
            this.originalIndexValueLabel.TabIndex = 3;
            this.originalIndexValueLabel.Text = "-";
            this.originalIndexValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nameFieldLabel
            //
            this.nameFieldLabel.Dock = DockStyle.Fill;
            this.nameFieldLabel.Location = new Point(0, 56);
            this.nameFieldLabel.Margin = new Padding(0);
            this.nameFieldLabel.Name = "nameFieldLabel";
            this.nameFieldLabel.Size = new Size(88, 34);
            this.nameFieldLabel.TabIndex = 4;
            this.nameFieldLabel.Text = "Name";
            this.nameFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nameTextBox
            //
            this.nameTextBox.Dock = DockStyle.Top;
            this.nameTextBox.Location = new Point(88, 62);
            this.nameTextBox.Margin = new Padding(0, 6, 0, 0);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new Size(194, 20);
            this.nameTextBox.TabIndex = 5;
            this.toolTip.SetToolTip(this.nameTextBox, "The chunk name stored in the XFBIN reference map.");
            //
            // typeFieldLabel
            //
            this.typeFieldLabel.Dock = DockStyle.Fill;
            this.typeFieldLabel.Location = new Point(0, 90);
            this.typeFieldLabel.Margin = new Padding(0);
            this.typeFieldLabel.Name = "typeFieldLabel";
            this.typeFieldLabel.Size = new Size(88, 34);
            this.typeFieldLabel.TabIndex = 6;
            this.typeFieldLabel.Text = "Type";
            this.typeFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // typeComboBox
            //
            this.typeComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.typeComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.typeComboBox.Dock = DockStyle.Top;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new Point(88, 96);
            this.typeComboBox.Margin = new Padding(0, 6, 0, 0);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new Size(194, 21);
            this.typeComboBox.TabIndex = 7;
            this.toolTip.SetToolTip(this.typeComboBox, "Known particle-related chunk types are suggested, but custom values can be typed.");
            //
            // pathFieldLabel
            //
            this.pathFieldLabel.Dock = DockStyle.Fill;
            this.pathFieldLabel.Location = new Point(0, 124);
            this.pathFieldLabel.Margin = new Padding(0);
            this.pathFieldLabel.Name = "pathFieldLabel";
            this.pathFieldLabel.Size = new Size(88, 88);
            this.pathFieldLabel.TabIndex = 8;
            this.pathFieldLabel.Text = "Path";
            this.pathFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pathTextBox
            //
            this.pathTextBox.AcceptsReturn = true;
            this.pathTextBox.Dock = DockStyle.Fill;
            this.pathTextBox.Location = new Point(88, 130);
            this.pathTextBox.Margin = new Padding(0, 6, 0, 0);
            this.pathTextBox.Multiline = true;
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ScrollBars = ScrollBars.Vertical;
            this.pathTextBox.Size = new Size(194, 82);
            this.pathTextBox.TabIndex = 9;
            this.toolTip.SetToolTip(this.pathTextBox, "The chunk path stored in the XFBIN reference map.");
            //
            // buttonPanel
            //
            this.buttonPanel.ColumnCount = 8;
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 106F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));
            this.buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            this.buttonPanel.Controls.Add(this.addButton, 0, 0);
            this.buttonPanel.Controls.Add(this.duplicateButton, 1, 0);
            this.buttonPanel.Controls.Add(this.deleteButton, 2, 0);
            this.buttonPanel.Controls.Add(this.moveUpButton, 3, 0);
            this.buttonPanel.Controls.Add(this.moveDownButton, 4, 0);
            this.buttonPanel.Controls.Add(this.cancelButton, 6, 0);
            this.buttonPanel.Controls.Add(this.okButton, 7, 0);
            this.buttonPanel.Dock = DockStyle.Fill;
            this.buttonPanel.Location = new Point(12, 514);
            this.buttonPanel.Margin = new Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.RowCount = 1;
            this.buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.buttonPanel.Size = new Size(976, 42);
            this.buttonPanel.TabIndex = 3;
            //
            // addButton
            //
            this.addButton.Dock = DockStyle.Fill;
            this.addButton.Location = new Point(0, 8);
            this.addButton.Margin = new Padding(0, 8, 6, 8);
            this.addButton.Name = "addButton";
            this.addButton.Size = new Size(88, 26);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add";
            this.toolTip.SetToolTip(this.addButton, "Add a new linked chunk after the current selection.");
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            //
            // duplicateButton
            //
            this.duplicateButton.Dock = DockStyle.Fill;
            this.duplicateButton.Location = new Point(94, 8);
            this.duplicateButton.Margin = new Padding(0, 8, 6, 8);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new Size(88, 26);
            this.duplicateButton.TabIndex = 1;
            this.duplicateButton.Text = "Duplicate";
            this.toolTip.SetToolTip(this.duplicateButton, "Copy the selected reference into a new linked chunk.");
            this.duplicateButton.UseVisualStyleBackColor = true;
            this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
            //
            // deleteButton
            //
            this.deleteButton.Dock = DockStyle.Fill;
            this.deleteButton.Location = new Point(188, 8);
            this.deleteButton.Margin = new Padding(0, 8, 18, 8);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new Size(88, 26);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Remove";
            this.toolTip.SetToolTip(this.deleteButton, "Remove the selected linked chunk.");
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            //
            // moveUpButton
            //
            this.moveUpButton.Dock = DockStyle.Fill;
            this.moveUpButton.Location = new Point(294, 8);
            this.moveUpButton.Margin = new Padding(0, 8, 6, 8);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new Size(82, 26);
            this.moveUpButton.TabIndex = 3;
            this.moveUpButton.Text = "Move Up";
            this.toolTip.SetToolTip(this.moveUpButton, "Move the selected reference earlier in the index list.");
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            //
            // moveDownButton
            //
            this.moveDownButton.Dock = DockStyle.Fill;
            this.moveDownButton.Location = new Point(382, 8);
            this.moveDownButton.Margin = new Padding(0, 8, 0, 8);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new Size(92, 26);
            this.moveDownButton.TabIndex = 4;
            this.moveDownButton.Text = "Move Down";
            this.toolTip.SetToolTip(this.moveDownButton, "Move the selected reference later in the index list.");
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            //
            // cancelButton
            //
            this.cancelButton.Dock = DockStyle.Fill;
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = new Point(794, 8);
            this.cancelButton.Margin = new Padding(0, 8, 6, 8);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new Size(88, 26);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // okButton
            //
            this.okButton.Dock = DockStyle.Fill;
            this.okButton.Location = new Point(888, 8);
            this.okButton.Margin = new Padding(0, 8, 0, 8);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(88, 26);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Apply";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            //
            // statusStrip
            //
            this.statusStrip.Dock = DockStyle.Fill;
            this.statusStrip.Items.AddRange(new ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new Point(12, 556);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(976, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip";
            //
            // statusLabel
            //
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(273, 19);
            this.statusLabel.Text = "Reference indexes will be remapped when applied.";
            //
            // form
            //
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new Size(1000, 580);
            this.Controls.Add(this.mainLayout);
            this.MinimumSize = new Size(920, 520);
            this.Name = "Tool_ParticleChunkReferenceEditor";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Particle Linked Chunks";
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.headerLayout.ResumeLayout(false);
            this.headerLayout.PerformLayout();
            this.summaryPanel.ResumeLayout(false);
            this.summaryPanel.PerformLayout();
            this.editorSplitContainer.Panel1.ResumeLayout(false);
            this.editorSplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(this.editorSplitContainer)).EndInit();
            this.editorSplitContainer.ResumeLayout(false);
            ((ISupportInitialize)(this.referencesGrid)).EndInit();
            this.detailsGroupBox.ResumeLayout(false);
            this.detailsLayout.ResumeLayout(false);
            this.detailsLayout.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
