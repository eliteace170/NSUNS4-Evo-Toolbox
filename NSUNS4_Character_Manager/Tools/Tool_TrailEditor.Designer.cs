using System.ComponentModel;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_TrailEditor
    {
        private IContainer components = null;
        private Label chunkLabel;
        private Label chunkNameLabel;
        private Label chunkPathLabel;
        private TabPage managersTabPage;
        private TabPage resourcesTabPage;
        private TabPage positionsTabPage;
        private TabPage forceFieldsTabPage;
        private TabPage mapIdsTabPage;
        private TabPage nodesTabPage;
        private SplitContainer managersSplitContainer;
        private SplitContainer resourcesSplitContainer;
        private SplitContainer positionsSplitContainer;
        private SplitContainer forceFieldsSplitContainer;
        private SplitContainer mapIdsSplitContainer;
        private SplitContainer nodesSplitContainer;
        private SplitContainer nodeDetailsSplitContainer;
        private SplitContainer framesSplitContainer;
        private FlowLayoutPanel managersButtonsPanel;
        private FlowLayoutPanel resourcesButtonsPanel;
        private FlowLayoutPanel positionsButtonsPanel;
        private FlowLayoutPanel forceFieldsButtonsPanel;
        private FlowLayoutPanel mapIdsButtonsPanel;
        private FlowLayoutPanel nodesButtonsPanel;
        private FlowLayoutPanel framesButtonsPanel;
        private Button addManagerButton;
        private Button copyManagerButton;
        private Button pasteManagerButton;
        private Button duplicateManagerButton;
        private Button deleteManagerButton;
        private Button addResourceButton;
        private Button copyResourceButton;
        private Button pasteResourceButton;
        private Button duplicateResourceButton;
        private Button deleteResourceButton;
        private Button addPositionButton;
        private Button copyPositionButton;
        private Button pastePositionButton;
        private Button duplicatePositionButton;
        private Button deletePositionButton;
        private Button addForceFieldButton;
        private Button copyForceFieldButton;
        private Button pasteForceFieldButton;
        private Button duplicateForceFieldButton;
        private Button deleteForceFieldButton;
        private Button addMapButton;
        private Button copyMapButton;
        private Button pasteMapButton;
        private Button duplicateMapButton;
        private Button deleteMapButton;
        private Button addNodeButton;
        private Button copyNodeButton;
        private Button pasteNodeButton;
        private Button duplicateNodeButton;
        private Button deleteNodeButton;
        private Button addFrameButton;
        private Button copyFrameButton;
        private Button pasteFrameButton;
        private Button duplicateFrameButton;
        private Button deleteFrameButton;
        private Label nodeDetailsLabel;
        private Label frameDetailsLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (backend != null)
                {
                    backend.Dispose();
                    backend = null;
                }
                if (components != null)
                    components.Dispose();
            }
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
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.managersTabPage = new System.Windows.Forms.TabPage();
            this.managersSplitContainer = new System.Windows.Forms.SplitContainer();
            this.managersListBox = new System.Windows.Forms.ListBox();
            this.managersButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addManagerButton = new System.Windows.Forms.Button();
            this.copyManagerButton = new System.Windows.Forms.Button();
            this.pasteManagerButton = new System.Windows.Forms.Button();
            this.duplicateManagerButton = new System.Windows.Forms.Button();
            this.deleteManagerButton = new System.Windows.Forms.Button();
            this.managersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.resourcesTabPage = new System.Windows.Forms.TabPage();
            this.resourcesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.resourcesListBox = new System.Windows.Forms.ListBox();
            this.resourcesButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addResourceButton = new System.Windows.Forms.Button();
            this.copyResourceButton = new System.Windows.Forms.Button();
            this.pasteResourceButton = new System.Windows.Forms.Button();
            this.duplicateResourceButton = new System.Windows.Forms.Button();
            this.deleteResourceButton = new System.Windows.Forms.Button();
            this.resourcesPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.positionsTabPage = new System.Windows.Forms.TabPage();
            this.positionsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.positionsListBox = new System.Windows.Forms.ListBox();
            this.positionsButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addPositionButton = new System.Windows.Forms.Button();
            this.copyPositionButton = new System.Windows.Forms.Button();
            this.pastePositionButton = new System.Windows.Forms.Button();
            this.duplicatePositionButton = new System.Windows.Forms.Button();
            this.deletePositionButton = new System.Windows.Forms.Button();
            this.positionsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.forceFieldsTabPage = new System.Windows.Forms.TabPage();
            this.forceFieldsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.forceFieldsListBox = new System.Windows.Forms.ListBox();
            this.forceFieldsButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addForceFieldButton = new System.Windows.Forms.Button();
            this.copyForceFieldButton = new System.Windows.Forms.Button();
            this.pasteForceFieldButton = new System.Windows.Forms.Button();
            this.duplicateForceFieldButton = new System.Windows.Forms.Button();
            this.deleteForceFieldButton = new System.Windows.Forms.Button();
            this.forceFieldsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mapIdsTabPage = new System.Windows.Forms.TabPage();
            this.mapIdsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mapIdsListBox = new System.Windows.Forms.ListBox();
            this.mapIdsButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addMapButton = new System.Windows.Forms.Button();
            this.copyMapButton = new System.Windows.Forms.Button();
            this.pasteMapButton = new System.Windows.Forms.Button();
            this.duplicateMapButton = new System.Windows.Forms.Button();
            this.deleteMapButton = new System.Windows.Forms.Button();
            this.mapIdsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nodesTabPage = new System.Windows.Forms.TabPage();
            this.nodesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.nodesListBox = new System.Windows.Forms.ListBox();
            this.nodesButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addNodeButton = new System.Windows.Forms.Button();
            this.copyNodeButton = new System.Windows.Forms.Button();
            this.pasteNodeButton = new System.Windows.Forms.Button();
            this.duplicateNodeButton = new System.Windows.Forms.Button();
            this.deleteNodeButton = new System.Windows.Forms.Button();
            this.nodeDetailsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.nodesPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nodeDetailsLabel = new System.Windows.Forms.Label();
            this.framesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.framesListBox = new System.Windows.Forms.ListBox();
            this.framesButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addFrameButton = new System.Windows.Forms.Button();
            this.copyFrameButton = new System.Windows.Forms.Button();
            this.pasteFrameButton = new System.Windows.Forms.Button();
            this.duplicateFrameButton = new System.Windows.Forms.Button();
            this.deleteFrameButton = new System.Windows.Forms.Button();
            this.framesPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.frameDetailsLabel = new System.Windows.Forms.Label();
            this.chunkPathTextBox = new System.Windows.Forms.TextBox();
            this.chunkPathLabel = new System.Windows.Forms.Label();
            this.chunkNameTextBox = new System.Windows.Forms.TextBox();
            this.chunkNameLabel = new System.Windows.Forms.Label();
            this.chunkComboBox = new System.Windows.Forms.ComboBox();
            this.chunkLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.detailsPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.managersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.managersSplitContainer)).BeginInit();
            this.managersSplitContainer.Panel1.SuspendLayout();
            this.managersSplitContainer.Panel2.SuspendLayout();
            this.managersSplitContainer.SuspendLayout();
            this.managersButtonsPanel.SuspendLayout();
            this.resourcesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resourcesSplitContainer)).BeginInit();
            this.resourcesSplitContainer.Panel1.SuspendLayout();
            this.resourcesSplitContainer.Panel2.SuspendLayout();
            this.resourcesSplitContainer.SuspendLayout();
            this.resourcesButtonsPanel.SuspendLayout();
            this.positionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionsSplitContainer)).BeginInit();
            this.positionsSplitContainer.Panel1.SuspendLayout();
            this.positionsSplitContainer.Panel2.SuspendLayout();
            this.positionsSplitContainer.SuspendLayout();
            this.positionsButtonsPanel.SuspendLayout();
            this.forceFieldsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFieldsSplitContainer)).BeginInit();
            this.forceFieldsSplitContainer.Panel1.SuspendLayout();
            this.forceFieldsSplitContainer.Panel2.SuspendLayout();
            this.forceFieldsSplitContainer.SuspendLayout();
            this.forceFieldsButtonsPanel.SuspendLayout();
            this.mapIdsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapIdsSplitContainer)).BeginInit();
            this.mapIdsSplitContainer.Panel1.SuspendLayout();
            this.mapIdsSplitContainer.Panel2.SuspendLayout();
            this.mapIdsSplitContainer.SuspendLayout();
            this.mapIdsButtonsPanel.SuspendLayout();
            this.nodesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodesSplitContainer)).BeginInit();
            this.nodesSplitContainer.Panel1.SuspendLayout();
            this.nodesSplitContainer.Panel2.SuspendLayout();
            this.nodesSplitContainer.SuspendLayout();
            this.nodesButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeDetailsSplitContainer)).BeginInit();
            this.nodeDetailsSplitContainer.Panel1.SuspendLayout();
            this.nodeDetailsSplitContainer.Panel2.SuspendLayout();
            this.nodeDetailsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framesSplitContainer)).BeginInit();
            this.framesSplitContainer.Panel1.SuspendLayout();
            this.framesSplitContainer.Panel2.SuspendLayout();
            this.framesSplitContainer.SuspendLayout();
            this.framesButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1427, 24);
            this.menuStrip1.TabIndex = 1;
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
            // detailsPanel
            // 
            this.detailsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsPanel.Controls.Add(this.tabControl1);
            this.detailsPanel.Controls.Add(this.chunkPathTextBox);
            this.detailsPanel.Controls.Add(this.chunkPathLabel);
            this.detailsPanel.Controls.Add(this.chunkNameTextBox);
            this.detailsPanel.Controls.Add(this.chunkNameLabel);
            this.detailsPanel.Controls.Add(this.chunkComboBox);
            this.detailsPanel.Controls.Add(this.chunkLabel);
            this.detailsPanel.Location = new System.Drawing.Point(12, 27);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(1415, 804);
            this.detailsPanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.managersTabPage);
            this.tabControl1.Controls.Add(this.resourcesTabPage);
            this.tabControl1.Controls.Add(this.positionsTabPage);
            this.tabControl1.Controls.Add(this.forceFieldsTabPage);
            this.tabControl1.Controls.Add(this.mapIdsTabPage);
            this.tabControl1.Controls.Add(this.nodesTabPage);
            this.tabControl1.Location = new System.Drawing.Point(3, 129);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1409, 672);
            this.tabControl1.TabIndex = 0;
            // 
            // managersTabPage
            // 
            this.managersTabPage.Controls.Add(this.managersSplitContainer);
            this.managersTabPage.Location = new System.Drawing.Point(4, 22);
            this.managersTabPage.Name = "managersTabPage";
            this.managersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.managersTabPage.Size = new System.Drawing.Size(1401, 646);
            this.managersTabPage.TabIndex = 0;
            this.managersTabPage.Text = "Trail Settings";
            this.managersTabPage.UseVisualStyleBackColor = true;
            // 
            // managersSplitContainer
            // 
            this.managersSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managersSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.managersSplitContainer.Name = "managersSplitContainer";
            // 
            // managersSplitContainer.Panel1
            // 
            this.managersSplitContainer.Panel1.Controls.Add(this.managersListBox);
            this.managersSplitContainer.Panel1.Controls.Add(this.managersButtonsPanel);
            this.managersSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            this.managersSplitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.managersSplitContainer_Panel1_Paint);
            // 
            // managersSplitContainer.Panel2
            // 
            this.managersSplitContainer.Panel2.Controls.Add(this.managersPropertyGrid);
            this.managersSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.managersSplitContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.managersSplitContainer_Panel2_Paint);
            this.managersSplitContainer.Size = new System.Drawing.Size(1395, 640);
            this.managersSplitContainer.SplitterDistance = 952;
            this.managersSplitContainer.TabIndex = 0;
            // 
            // managersListBox
            // 
            this.managersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managersListBox.FormattingEnabled = true;
            this.managersListBox.IntegralHeight = false;
            this.managersListBox.Location = new System.Drawing.Point(6, 6);
            this.managersListBox.Name = "managersListBox";
            this.managersListBox.Size = new System.Drawing.Size(940, 592);
            this.managersListBox.TabIndex = 1;
            // 
            // managersButtonsPanel
            // 
            this.managersButtonsPanel.Controls.Add(this.addManagerButton);
            this.managersButtonsPanel.Controls.Add(this.copyManagerButton);
            this.managersButtonsPanel.Controls.Add(this.pasteManagerButton);
            this.managersButtonsPanel.Controls.Add(this.duplicateManagerButton);
            this.managersButtonsPanel.Controls.Add(this.deleteManagerButton);
            this.managersButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.managersButtonsPanel.Location = new System.Drawing.Point(6, 598);
            this.managersButtonsPanel.Name = "managersButtonsPanel";
            this.managersButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.managersButtonsPanel.Size = new System.Drawing.Size(940, 36);
            this.managersButtonsPanel.TabIndex = 2;
            // 
            // addManagerButton
            // 
            this.addManagerButton.Location = new System.Drawing.Point(3, 7);
            this.addManagerButton.Name = "addManagerButton";
            this.addManagerButton.Size = new System.Drawing.Size(84, 23);
            this.addManagerButton.TabIndex = 0;
            this.addManagerButton.Text = "Add";
            this.addManagerButton.Click += new System.EventHandler(this.AddManagerButton_Click);
            // 
            // copyManagerButton
            // 
            this.copyManagerButton.Location = new System.Drawing.Point(93, 7);
            this.copyManagerButton.Name = "copyManagerButton";
            this.copyManagerButton.Size = new System.Drawing.Size(84, 23);
            this.copyManagerButton.TabIndex = 1;
            this.copyManagerButton.Text = "Copy";
            this.copyManagerButton.Click += new System.EventHandler(this.CopyManagerButton_Click);
            // 
            // pasteManagerButton
            // 
            this.pasteManagerButton.Location = new System.Drawing.Point(183, 7);
            this.pasteManagerButton.Name = "pasteManagerButton";
            this.pasteManagerButton.Size = new System.Drawing.Size(84, 23);
            this.pasteManagerButton.TabIndex = 2;
            this.pasteManagerButton.Text = "Paste";
            this.pasteManagerButton.Click += new System.EventHandler(this.PasteManagerButton_Click);
            // 
            // duplicateManagerButton
            // 
            this.duplicateManagerButton.Location = new System.Drawing.Point(273, 7);
            this.duplicateManagerButton.Name = "duplicateManagerButton";
            this.duplicateManagerButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateManagerButton.TabIndex = 3;
            this.duplicateManagerButton.Text = "Duplicate";
            this.duplicateManagerButton.Click += new System.EventHandler(this.DuplicateManagerButton_Click);
            // 
            // deleteManagerButton
            // 
            this.deleteManagerButton.Location = new System.Drawing.Point(363, 7);
            this.deleteManagerButton.Name = "deleteManagerButton";
            this.deleteManagerButton.Size = new System.Drawing.Size(84, 23);
            this.deleteManagerButton.TabIndex = 4;
            this.deleteManagerButton.Text = "Remove";
            this.deleteManagerButton.Click += new System.EventHandler(this.DeleteManagerButton_Click);
            // 
            // managersPropertyGrid
            // 
            this.managersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managersPropertyGrid.Location = new System.Drawing.Point(6, 6);
            this.managersPropertyGrid.Name = "managersPropertyGrid";
            this.managersPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.managersPropertyGrid.Size = new System.Drawing.Size(427, 628);
            this.managersPropertyGrid.TabIndex = 1;
            this.managersPropertyGrid.ToolbarVisible = false;
            this.managersPropertyGrid.Click += new System.EventHandler(this.managersPropertyGrid_Click);
            // 
            // resourcesTabPage
            // 
            this.resourcesTabPage.Controls.Add(this.resourcesSplitContainer);
            this.resourcesTabPage.Location = new System.Drawing.Point(4, 22);
            this.resourcesTabPage.Name = "resourcesTabPage";
            this.resourcesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.resourcesTabPage.Size = new System.Drawing.Size(1401, 686);
            this.resourcesTabPage.TabIndex = 1;
            this.resourcesTabPage.Text = "Effects";
            this.resourcesTabPage.UseVisualStyleBackColor = true;
            // 
            // resourcesSplitContainer
            // 
            this.resourcesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourcesSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.resourcesSplitContainer.Name = "resourcesSplitContainer";
            // 
            // resourcesSplitContainer.Panel1
            // 
            this.resourcesSplitContainer.Panel1.Controls.Add(this.resourcesListBox);
            this.resourcesSplitContainer.Panel1.Controls.Add(this.resourcesButtonsPanel);
            this.resourcesSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            this.resourcesSplitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.resourcesSplitContainer_Panel1_Paint);
            // 
            // resourcesSplitContainer.Panel2
            // 
            this.resourcesSplitContainer.Panel2.Controls.Add(this.resourcesPropertyGrid);
            this.resourcesSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.resourcesSplitContainer.Size = new System.Drawing.Size(1395, 680);
            this.resourcesSplitContainer.SplitterDistance = 995;
            this.resourcesSplitContainer.TabIndex = 0;
            // 
            // resourcesListBox
            // 
            this.resourcesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourcesListBox.FormattingEnabled = true;
            this.resourcesListBox.IntegralHeight = false;
            this.resourcesListBox.Location = new System.Drawing.Point(6, 6);
            this.resourcesListBox.Name = "resourcesListBox";
            this.resourcesListBox.Size = new System.Drawing.Size(983, 632);
            this.resourcesListBox.TabIndex = 1;
            this.resourcesListBox.SelectedIndexChanged += new System.EventHandler(this.resourcesListBox_SelectedIndexChanged_1);
            // 
            // resourcesButtonsPanel
            // 
            this.resourcesButtonsPanel.Controls.Add(this.addResourceButton);
            this.resourcesButtonsPanel.Controls.Add(this.copyResourceButton);
            this.resourcesButtonsPanel.Controls.Add(this.pasteResourceButton);
            this.resourcesButtonsPanel.Controls.Add(this.duplicateResourceButton);
            this.resourcesButtonsPanel.Controls.Add(this.deleteResourceButton);
            this.resourcesButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.resourcesButtonsPanel.Location = new System.Drawing.Point(6, 638);
            this.resourcesButtonsPanel.Name = "resourcesButtonsPanel";
            this.resourcesButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.resourcesButtonsPanel.Size = new System.Drawing.Size(983, 36);
            this.resourcesButtonsPanel.TabIndex = 2;
            // 
            // addResourceButton
            // 
            this.addResourceButton.Location = new System.Drawing.Point(3, 7);
            this.addResourceButton.Name = "addResourceButton";
            this.addResourceButton.Size = new System.Drawing.Size(84, 23);
            this.addResourceButton.TabIndex = 0;
            this.addResourceButton.Text = "Add";
            this.addResourceButton.Click += new System.EventHandler(this.AddResourceButton_Click);
            // 
            // copyResourceButton
            // 
            this.copyResourceButton.Location = new System.Drawing.Point(93, 7);
            this.copyResourceButton.Name = "copyResourceButton";
            this.copyResourceButton.Size = new System.Drawing.Size(84, 23);
            this.copyResourceButton.TabIndex = 1;
            this.copyResourceButton.Text = "Copy";
            this.copyResourceButton.Click += new System.EventHandler(this.CopyResourceButton_Click);
            // 
            // pasteResourceButton
            // 
            this.pasteResourceButton.Location = new System.Drawing.Point(183, 7);
            this.pasteResourceButton.Name = "pasteResourceButton";
            this.pasteResourceButton.Size = new System.Drawing.Size(84, 23);
            this.pasteResourceButton.TabIndex = 2;
            this.pasteResourceButton.Text = "Paste";
            this.pasteResourceButton.Click += new System.EventHandler(this.PasteResourceButton_Click);
            // 
            // duplicateResourceButton
            // 
            this.duplicateResourceButton.Location = new System.Drawing.Point(273, 7);
            this.duplicateResourceButton.Name = "duplicateResourceButton";
            this.duplicateResourceButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateResourceButton.TabIndex = 3;
            this.duplicateResourceButton.Text = "Duplicate";
            this.duplicateResourceButton.Click += new System.EventHandler(this.DuplicateResourceButton_Click);
            // 
            // deleteResourceButton
            // 
            this.deleteResourceButton.Location = new System.Drawing.Point(363, 7);
            this.deleteResourceButton.Name = "deleteResourceButton";
            this.deleteResourceButton.Size = new System.Drawing.Size(84, 23);
            this.deleteResourceButton.TabIndex = 4;
            this.deleteResourceButton.Text = "Remove";
            this.deleteResourceButton.Click += new System.EventHandler(this.DeleteResourceButton_Click);
            // 
            // resourcesPropertyGrid
            // 
            this.resourcesPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourcesPropertyGrid.Location = new System.Drawing.Point(6, 6);
            this.resourcesPropertyGrid.Name = "resourcesPropertyGrid";
            this.resourcesPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.resourcesPropertyGrid.Size = new System.Drawing.Size(384, 668);
            this.resourcesPropertyGrid.TabIndex = 1;
            this.resourcesPropertyGrid.ToolbarVisible = false;
            // 
            // positionsTabPage
            // 
            this.positionsTabPage.Controls.Add(this.positionsSplitContainer);
            this.positionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.positionsTabPage.Name = "positionsTabPage";
            this.positionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.positionsTabPage.Size = new System.Drawing.Size(1401, 686);
            this.positionsTabPage.TabIndex = 2;
            this.positionsTabPage.Text = "Attachments";
            this.positionsTabPage.UseVisualStyleBackColor = true;
            // 
            // positionsSplitContainer
            // 
            this.positionsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionsSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.positionsSplitContainer.Name = "positionsSplitContainer";
            // 
            // positionsSplitContainer.Panel1
            // 
            this.positionsSplitContainer.Panel1.Controls.Add(this.positionsListBox);
            this.positionsSplitContainer.Panel1.Controls.Add(this.positionsButtonsPanel);
            this.positionsSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // positionsSplitContainer.Panel2
            // 
            this.positionsSplitContainer.Panel2.Controls.Add(this.positionsPropertyGrid);
            this.positionsSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.positionsSplitContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.positionsSplitContainer_Panel2_Paint);
            this.positionsSplitContainer.Size = new System.Drawing.Size(1395, 680);
            this.positionsSplitContainer.SplitterDistance = 1116;
            this.positionsSplitContainer.TabIndex = 0;
            // 
            // positionsListBox
            // 
            this.positionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionsListBox.FormattingEnabled = true;
            this.positionsListBox.IntegralHeight = false;
            this.positionsListBox.Location = new System.Drawing.Point(6, 6);
            this.positionsListBox.Name = "positionsListBox";
            this.positionsListBox.Size = new System.Drawing.Size(1104, 632);
            this.positionsListBox.TabIndex = 1;
            // 
            // positionsButtonsPanel
            // 
            this.positionsButtonsPanel.Controls.Add(this.addPositionButton);
            this.positionsButtonsPanel.Controls.Add(this.copyPositionButton);
            this.positionsButtonsPanel.Controls.Add(this.pastePositionButton);
            this.positionsButtonsPanel.Controls.Add(this.duplicatePositionButton);
            this.positionsButtonsPanel.Controls.Add(this.deletePositionButton);
            this.positionsButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.positionsButtonsPanel.Location = new System.Drawing.Point(6, 638);
            this.positionsButtonsPanel.Name = "positionsButtonsPanel";
            this.positionsButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.positionsButtonsPanel.Size = new System.Drawing.Size(1104, 36);
            this.positionsButtonsPanel.TabIndex = 2;
            // 
            // addPositionButton
            // 
            this.addPositionButton.Location = new System.Drawing.Point(3, 7);
            this.addPositionButton.Name = "addPositionButton";
            this.addPositionButton.Size = new System.Drawing.Size(84, 23);
            this.addPositionButton.TabIndex = 0;
            this.addPositionButton.Text = "Add";
            this.addPositionButton.Click += new System.EventHandler(this.AddPositionButton_Click);
            // 
            // copyPositionButton
            // 
            this.copyPositionButton.Location = new System.Drawing.Point(93, 7);
            this.copyPositionButton.Name = "copyPositionButton";
            this.copyPositionButton.Size = new System.Drawing.Size(84, 23);
            this.copyPositionButton.TabIndex = 1;
            this.copyPositionButton.Text = "Copy";
            this.copyPositionButton.Click += new System.EventHandler(this.CopyPositionButton_Click);
            // 
            // pastePositionButton
            // 
            this.pastePositionButton.Location = new System.Drawing.Point(183, 7);
            this.pastePositionButton.Name = "pastePositionButton";
            this.pastePositionButton.Size = new System.Drawing.Size(84, 23);
            this.pastePositionButton.TabIndex = 2;
            this.pastePositionButton.Text = "Paste";
            this.pastePositionButton.Click += new System.EventHandler(this.PastePositionButton_Click);
            // 
            // duplicatePositionButton
            // 
            this.duplicatePositionButton.Location = new System.Drawing.Point(273, 7);
            this.duplicatePositionButton.Name = "duplicatePositionButton";
            this.duplicatePositionButton.Size = new System.Drawing.Size(84, 23);
            this.duplicatePositionButton.TabIndex = 3;
            this.duplicatePositionButton.Text = "Duplicate";
            this.duplicatePositionButton.Click += new System.EventHandler(this.DuplicatePositionButton_Click);
            // 
            // deletePositionButton
            // 
            this.deletePositionButton.Location = new System.Drawing.Point(363, 7);
            this.deletePositionButton.Name = "deletePositionButton";
            this.deletePositionButton.Size = new System.Drawing.Size(84, 23);
            this.deletePositionButton.TabIndex = 4;
            this.deletePositionButton.Text = "Remove";
            this.deletePositionButton.Click += new System.EventHandler(this.DeletePositionButton_Click);
            // 
            // positionsPropertyGrid
            // 
            this.positionsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionsPropertyGrid.Location = new System.Drawing.Point(6, 6);
            this.positionsPropertyGrid.Name = "positionsPropertyGrid";
            this.positionsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.positionsPropertyGrid.Size = new System.Drawing.Size(263, 668);
            this.positionsPropertyGrid.TabIndex = 1;
            this.positionsPropertyGrid.ToolbarVisible = false;
            // 
            // forceFieldsTabPage
            // 
            this.forceFieldsTabPage.Controls.Add(this.forceFieldsSplitContainer);
            this.forceFieldsTabPage.Location = new System.Drawing.Point(4, 22);
            this.forceFieldsTabPage.Name = "forceFieldsTabPage";
            this.forceFieldsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.forceFieldsTabPage.Size = new System.Drawing.Size(1401, 686);
            this.forceFieldsTabPage.TabIndex = 3;
            this.forceFieldsTabPage.Text = "Force Fields";
            this.forceFieldsTabPage.UseVisualStyleBackColor = true;
            // 
            // forceFieldsSplitContainer
            // 
            this.forceFieldsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forceFieldsSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.forceFieldsSplitContainer.Name = "forceFieldsSplitContainer";
            // 
            // forceFieldsSplitContainer.Panel1
            // 
            this.forceFieldsSplitContainer.Panel1.Controls.Add(this.forceFieldsListBox);
            this.forceFieldsSplitContainer.Panel1.Controls.Add(this.forceFieldsButtonsPanel);
            this.forceFieldsSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // forceFieldsSplitContainer.Panel2
            // 
            this.forceFieldsSplitContainer.Panel2.Controls.Add(this.forceFieldsPropertyGrid);
            this.forceFieldsSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.forceFieldsSplitContainer.Size = new System.Drawing.Size(1395, 680);
            this.forceFieldsSplitContainer.SplitterDistance = 1116;
            this.forceFieldsSplitContainer.TabIndex = 0;
            // 
            // forceFieldsListBox
            // 
            this.forceFieldsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forceFieldsListBox.FormattingEnabled = true;
            this.forceFieldsListBox.IntegralHeight = false;
            this.forceFieldsListBox.Location = new System.Drawing.Point(6, 6);
            this.forceFieldsListBox.Name = "forceFieldsListBox";
            this.forceFieldsListBox.Size = new System.Drawing.Size(1104, 632);
            this.forceFieldsListBox.TabIndex = 1;
            // 
            // forceFieldsButtonsPanel
            // 
            this.forceFieldsButtonsPanel.Controls.Add(this.addForceFieldButton);
            this.forceFieldsButtonsPanel.Controls.Add(this.copyForceFieldButton);
            this.forceFieldsButtonsPanel.Controls.Add(this.pasteForceFieldButton);
            this.forceFieldsButtonsPanel.Controls.Add(this.duplicateForceFieldButton);
            this.forceFieldsButtonsPanel.Controls.Add(this.deleteForceFieldButton);
            this.forceFieldsButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.forceFieldsButtonsPanel.Location = new System.Drawing.Point(6, 638);
            this.forceFieldsButtonsPanel.Name = "forceFieldsButtonsPanel";
            this.forceFieldsButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.forceFieldsButtonsPanel.Size = new System.Drawing.Size(1104, 36);
            this.forceFieldsButtonsPanel.TabIndex = 2;
            // 
            // addForceFieldButton
            // 
            this.addForceFieldButton.Location = new System.Drawing.Point(3, 7);
            this.addForceFieldButton.Name = "addForceFieldButton";
            this.addForceFieldButton.Size = new System.Drawing.Size(84, 23);
            this.addForceFieldButton.TabIndex = 0;
            this.addForceFieldButton.Text = "Add";
            this.addForceFieldButton.Click += new System.EventHandler(this.AddForceFieldButton_Click);
            // 
            // copyForceFieldButton
            // 
            this.copyForceFieldButton.Location = new System.Drawing.Point(93, 7);
            this.copyForceFieldButton.Name = "copyForceFieldButton";
            this.copyForceFieldButton.Size = new System.Drawing.Size(84, 23);
            this.copyForceFieldButton.TabIndex = 1;
            this.copyForceFieldButton.Text = "Copy";
            this.copyForceFieldButton.Click += new System.EventHandler(this.CopyForceFieldButton_Click);
            // 
            // pasteForceFieldButton
            // 
            this.pasteForceFieldButton.Location = new System.Drawing.Point(183, 7);
            this.pasteForceFieldButton.Name = "pasteForceFieldButton";
            this.pasteForceFieldButton.Size = new System.Drawing.Size(84, 23);
            this.pasteForceFieldButton.TabIndex = 2;
            this.pasteForceFieldButton.Text = "Paste";
            this.pasteForceFieldButton.Click += new System.EventHandler(this.PasteForceFieldButton_Click);
            // 
            // duplicateForceFieldButton
            // 
            this.duplicateForceFieldButton.Location = new System.Drawing.Point(273, 7);
            this.duplicateForceFieldButton.Name = "duplicateForceFieldButton";
            this.duplicateForceFieldButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateForceFieldButton.TabIndex = 3;
            this.duplicateForceFieldButton.Text = "Duplicate";
            this.duplicateForceFieldButton.Click += new System.EventHandler(this.DuplicateForceFieldButton_Click);
            // 
            // deleteForceFieldButton
            // 
            this.deleteForceFieldButton.Location = new System.Drawing.Point(363, 7);
            this.deleteForceFieldButton.Name = "deleteForceFieldButton";
            this.deleteForceFieldButton.Size = new System.Drawing.Size(84, 23);
            this.deleteForceFieldButton.TabIndex = 4;
            this.deleteForceFieldButton.Text = "Remove";
            this.deleteForceFieldButton.Click += new System.EventHandler(this.DeleteForceFieldButton_Click);
            // 
            // forceFieldsPropertyGrid
            // 
            this.forceFieldsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forceFieldsPropertyGrid.Location = new System.Drawing.Point(6, 6);
            this.forceFieldsPropertyGrid.Name = "forceFieldsPropertyGrid";
            this.forceFieldsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.forceFieldsPropertyGrid.Size = new System.Drawing.Size(263, 668);
            this.forceFieldsPropertyGrid.TabIndex = 1;
            this.forceFieldsPropertyGrid.ToolbarVisible = false;
            // 
            // mapIdsTabPage
            // 
            this.mapIdsTabPage.Controls.Add(this.mapIdsSplitContainer);
            this.mapIdsTabPage.Location = new System.Drawing.Point(4, 22);
            this.mapIdsTabPage.Name = "mapIdsTabPage";
            this.mapIdsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mapIdsTabPage.Size = new System.Drawing.Size(1401, 686);
            this.mapIdsTabPage.TabIndex = 4;
            this.mapIdsTabPage.Text = "Linked Chunks";
            this.mapIdsTabPage.UseVisualStyleBackColor = true;
            // 
            // mapIdsSplitContainer
            // 
            this.mapIdsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapIdsSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.mapIdsSplitContainer.Name = "mapIdsSplitContainer";
            // 
            // mapIdsSplitContainer.Panel1
            // 
            this.mapIdsSplitContainer.Panel1.Controls.Add(this.mapIdsListBox);
            this.mapIdsSplitContainer.Panel1.Controls.Add(this.mapIdsButtonsPanel);
            this.mapIdsSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // mapIdsSplitContainer.Panel2
            // 
            this.mapIdsSplitContainer.Panel2.Controls.Add(this.mapIdsPropertyGrid);
            this.mapIdsSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.mapIdsSplitContainer.Size = new System.Drawing.Size(1395, 680);
            this.mapIdsSplitContainer.SplitterDistance = 1116;
            this.mapIdsSplitContainer.TabIndex = 0;
            // 
            // mapIdsListBox
            // 
            this.mapIdsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapIdsListBox.FormattingEnabled = true;
            this.mapIdsListBox.IntegralHeight = false;
            this.mapIdsListBox.Location = new System.Drawing.Point(6, 6);
            this.mapIdsListBox.Name = "mapIdsListBox";
            this.mapIdsListBox.Size = new System.Drawing.Size(1104, 632);
            this.mapIdsListBox.TabIndex = 1;
            // 
            // mapIdsButtonsPanel
            // 
            this.mapIdsButtonsPanel.Controls.Add(this.addMapButton);
            this.mapIdsButtonsPanel.Controls.Add(this.copyMapButton);
            this.mapIdsButtonsPanel.Controls.Add(this.pasteMapButton);
            this.mapIdsButtonsPanel.Controls.Add(this.duplicateMapButton);
            this.mapIdsButtonsPanel.Controls.Add(this.deleteMapButton);
            this.mapIdsButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mapIdsButtonsPanel.Location = new System.Drawing.Point(6, 638);
            this.mapIdsButtonsPanel.Name = "mapIdsButtonsPanel";
            this.mapIdsButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.mapIdsButtonsPanel.Size = new System.Drawing.Size(1104, 36);
            this.mapIdsButtonsPanel.TabIndex = 2;
            // 
            // addMapButton
            // 
            this.addMapButton.Location = new System.Drawing.Point(3, 7);
            this.addMapButton.Name = "addMapButton";
            this.addMapButton.Size = new System.Drawing.Size(84, 23);
            this.addMapButton.TabIndex = 0;
            this.addMapButton.Text = "Add";
            this.addMapButton.Click += new System.EventHandler(this.AddMapButton_Click);
            // 
            // copyMapButton
            // 
            this.copyMapButton.Location = new System.Drawing.Point(93, 7);
            this.copyMapButton.Name = "copyMapButton";
            this.copyMapButton.Size = new System.Drawing.Size(84, 23);
            this.copyMapButton.TabIndex = 1;
            this.copyMapButton.Text = "Copy";
            this.copyMapButton.Click += new System.EventHandler(this.CopyMapButton_Click);
            // 
            // pasteMapButton
            // 
            this.pasteMapButton.Location = new System.Drawing.Point(183, 7);
            this.pasteMapButton.Name = "pasteMapButton";
            this.pasteMapButton.Size = new System.Drawing.Size(84, 23);
            this.pasteMapButton.TabIndex = 2;
            this.pasteMapButton.Text = "Paste";
            this.pasteMapButton.Click += new System.EventHandler(this.PasteMapButton_Click);
            // 
            // duplicateMapButton
            // 
            this.duplicateMapButton.Location = new System.Drawing.Point(273, 7);
            this.duplicateMapButton.Name = "duplicateMapButton";
            this.duplicateMapButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateMapButton.TabIndex = 3;
            this.duplicateMapButton.Text = "Duplicate";
            this.duplicateMapButton.Click += new System.EventHandler(this.DuplicateMapButton_Click);
            // 
            // deleteMapButton
            // 
            this.deleteMapButton.Location = new System.Drawing.Point(363, 7);
            this.deleteMapButton.Name = "deleteMapButton";
            this.deleteMapButton.Size = new System.Drawing.Size(84, 23);
            this.deleteMapButton.TabIndex = 4;
            this.deleteMapButton.Text = "Remove";
            this.deleteMapButton.Click += new System.EventHandler(this.DeleteMapButton_Click);
            // 
            // mapIdsPropertyGrid
            // 
            this.mapIdsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapIdsPropertyGrid.Location = new System.Drawing.Point(6, 6);
            this.mapIdsPropertyGrid.Name = "mapIdsPropertyGrid";
            this.mapIdsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.mapIdsPropertyGrid.Size = new System.Drawing.Size(263, 668);
            this.mapIdsPropertyGrid.TabIndex = 1;
            this.mapIdsPropertyGrid.ToolbarVisible = false;
            // 
            // nodesTabPage
            // 
            this.nodesTabPage.Controls.Add(this.nodesSplitContainer);
            this.nodesTabPage.Location = new System.Drawing.Point(4, 22);
            this.nodesTabPage.Name = "nodesTabPage";
            this.nodesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.nodesTabPage.Size = new System.Drawing.Size(1401, 686);
            this.nodesTabPage.TabIndex = 5;
            this.nodesTabPage.Text = "Timelines";
            this.nodesTabPage.UseVisualStyleBackColor = true;
            // 
            // nodesSplitContainer
            // 
            this.nodesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodesSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.nodesSplitContainer.Name = "nodesSplitContainer";
            // 
            // nodesSplitContainer.Panel1
            // 
            this.nodesSplitContainer.Panel1.Controls.Add(this.nodesListBox);
            this.nodesSplitContainer.Panel1.Controls.Add(this.nodesButtonsPanel);
            this.nodesSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // nodesSplitContainer.Panel2
            // 
            this.nodesSplitContainer.Panel2.Controls.Add(this.nodeDetailsSplitContainer);
            this.nodesSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.nodesSplitContainer.Size = new System.Drawing.Size(1395, 680);
            this.nodesSplitContainer.SplitterDistance = 1116;
            this.nodesSplitContainer.TabIndex = 0;
            // 
            // nodesListBox
            // 
            this.nodesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodesListBox.FormattingEnabled = true;
            this.nodesListBox.IntegralHeight = false;
            this.nodesListBox.Location = new System.Drawing.Point(6, 6);
            this.nodesListBox.Name = "nodesListBox";
            this.nodesListBox.Size = new System.Drawing.Size(1104, 632);
            this.nodesListBox.TabIndex = 0;
            // 
            // nodesButtonsPanel
            // 
            this.nodesButtonsPanel.Controls.Add(this.addNodeButton);
            this.nodesButtonsPanel.Controls.Add(this.copyNodeButton);
            this.nodesButtonsPanel.Controls.Add(this.pasteNodeButton);
            this.nodesButtonsPanel.Controls.Add(this.duplicateNodeButton);
            this.nodesButtonsPanel.Controls.Add(this.deleteNodeButton);
            this.nodesButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.nodesButtonsPanel.Location = new System.Drawing.Point(6, 638);
            this.nodesButtonsPanel.Name = "nodesButtonsPanel";
            this.nodesButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.nodesButtonsPanel.Size = new System.Drawing.Size(1104, 36);
            this.nodesButtonsPanel.TabIndex = 1;
            // 
            // addNodeButton
            // 
            this.addNodeButton.Location = new System.Drawing.Point(3, 7);
            this.addNodeButton.Name = "addNodeButton";
            this.addNodeButton.Size = new System.Drawing.Size(84, 23);
            this.addNodeButton.TabIndex = 0;
            this.addNodeButton.Text = "Add Timeline";
            this.addNodeButton.Click += new System.EventHandler(this.AddNodeButton_Click);
            // 
            // copyNodeButton
            // 
            this.copyNodeButton.Location = new System.Drawing.Point(93, 7);
            this.copyNodeButton.Name = "copyNodeButton";
            this.copyNodeButton.Size = new System.Drawing.Size(84, 23);
            this.copyNodeButton.TabIndex = 1;
            this.copyNodeButton.Text = "Copy";
            this.copyNodeButton.Click += new System.EventHandler(this.CopyNodeButton_Click);
            // 
            // pasteNodeButton
            // 
            this.pasteNodeButton.Location = new System.Drawing.Point(183, 7);
            this.pasteNodeButton.Name = "pasteNodeButton";
            this.pasteNodeButton.Size = new System.Drawing.Size(84, 23);
            this.pasteNodeButton.TabIndex = 2;
            this.pasteNodeButton.Text = "Paste";
            this.pasteNodeButton.Click += new System.EventHandler(this.PasteNodeButton_Click);
            // 
            // duplicateNodeButton
            // 
            this.duplicateNodeButton.Location = new System.Drawing.Point(273, 7);
            this.duplicateNodeButton.Name = "duplicateNodeButton";
            this.duplicateNodeButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateNodeButton.TabIndex = 3;
            this.duplicateNodeButton.Text = "Duplicate";
            this.duplicateNodeButton.Click += new System.EventHandler(this.DuplicateNodeButton_Click);
            // 
            // deleteNodeButton
            // 
            this.deleteNodeButton.Location = new System.Drawing.Point(363, 7);
            this.deleteNodeButton.Name = "deleteNodeButton";
            this.deleteNodeButton.Size = new System.Drawing.Size(84, 23);
            this.deleteNodeButton.TabIndex = 4;
            this.deleteNodeButton.Text = "Remove";
            this.deleteNodeButton.Click += new System.EventHandler(this.DeleteNodeButton_Click);
            // 
            // nodeDetailsSplitContainer
            // 
            this.nodeDetailsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeDetailsSplitContainer.Location = new System.Drawing.Point(6, 6);
            this.nodeDetailsSplitContainer.Name = "nodeDetailsSplitContainer";
            this.nodeDetailsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // nodeDetailsSplitContainer.Panel1
            // 
            this.nodeDetailsSplitContainer.Panel1.Controls.Add(this.nodesPropertyGrid);
            this.nodeDetailsSplitContainer.Panel1.Controls.Add(this.nodeDetailsLabel);
            this.nodeDetailsSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // nodeDetailsSplitContainer.Panel2
            // 
            this.nodeDetailsSplitContainer.Panel2.Controls.Add(this.framesSplitContainer);
            this.nodeDetailsSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.nodeDetailsSplitContainer.Size = new System.Drawing.Size(263, 668);
            this.nodeDetailsSplitContainer.SplitterDistance = 446;
            this.nodeDetailsSplitContainer.TabIndex = 0;
            // 
            // nodesPropertyGrid
            // 
            this.nodesPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodesPropertyGrid.Location = new System.Drawing.Point(6, 30);
            this.nodesPropertyGrid.Name = "nodesPropertyGrid";
            this.nodesPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.nodesPropertyGrid.Size = new System.Drawing.Size(251, 410);
            this.nodesPropertyGrid.TabIndex = 0;
            this.nodesPropertyGrid.ToolbarVisible = false;
            // 
            // nodeDetailsLabel
            // 
            this.nodeDetailsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nodeDetailsLabel.Location = new System.Drawing.Point(6, 6);
            this.nodeDetailsLabel.Name = "nodeDetailsLabel";
            this.nodeDetailsLabel.Size = new System.Drawing.Size(251, 24);
            this.nodeDetailsLabel.TabIndex = 1;
            this.nodeDetailsLabel.Text = "Timeline Details";
            // 
            // framesSplitContainer
            // 
            this.framesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesSplitContainer.Location = new System.Drawing.Point(6, 6);
            this.framesSplitContainer.Name = "framesSplitContainer";
            // 
            // framesSplitContainer.Panel1
            // 
            this.framesSplitContainer.Panel1.Controls.Add(this.framesListBox);
            this.framesSplitContainer.Panel1.Controls.Add(this.framesButtonsPanel);
            this.framesSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // framesSplitContainer.Panel2
            // 
            this.framesSplitContainer.Panel2.Controls.Add(this.framesPropertyGrid);
            this.framesSplitContainer.Panel2.Controls.Add(this.frameDetailsLabel);
            this.framesSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6);
            this.framesSplitContainer.Size = new System.Drawing.Size(251, 206);
            this.framesSplitContainer.SplitterDistance = 115;
            this.framesSplitContainer.TabIndex = 0;
            // 
            // framesListBox
            // 
            this.framesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesListBox.FormattingEnabled = true;
            this.framesListBox.IntegralHeight = false;
            this.framesListBox.Location = new System.Drawing.Point(6, 6);
            this.framesListBox.Name = "framesListBox";
            this.framesListBox.Size = new System.Drawing.Size(103, 158);
            this.framesListBox.TabIndex = 0;
            // 
            // framesButtonsPanel
            // 
            this.framesButtonsPanel.Controls.Add(this.addFrameButton);
            this.framesButtonsPanel.Controls.Add(this.copyFrameButton);
            this.framesButtonsPanel.Controls.Add(this.pasteFrameButton);
            this.framesButtonsPanel.Controls.Add(this.duplicateFrameButton);
            this.framesButtonsPanel.Controls.Add(this.deleteFrameButton);
            this.framesButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.framesButtonsPanel.Location = new System.Drawing.Point(6, 48);
            this.framesButtonsPanel.Name = "framesButtonsPanel";
            this.framesButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.framesButtonsPanel.Size = new System.Drawing.Size(103, 152);
            this.framesButtonsPanel.TabIndex = 1;
            // 
            // addFrameButton
            // 
            this.addFrameButton.Location = new System.Drawing.Point(3, 7);
            this.addFrameButton.Name = "addFrameButton";
            this.addFrameButton.Size = new System.Drawing.Size(84, 23);
            this.addFrameButton.TabIndex = 0;
            this.addFrameButton.Text = "Add Frame";
            this.addFrameButton.Click += new System.EventHandler(this.AddFrameButton_Click);
            // 
            // copyFrameButton
            // 
            this.copyFrameButton.Location = new System.Drawing.Point(3, 36);
            this.copyFrameButton.Name = "copyFrameButton";
            this.copyFrameButton.Size = new System.Drawing.Size(84, 23);
            this.copyFrameButton.TabIndex = 1;
            this.copyFrameButton.Text = "Copy";
            this.copyFrameButton.Click += new System.EventHandler(this.CopyFrameButton_Click);
            // 
            // pasteFrameButton
            // 
            this.pasteFrameButton.Location = new System.Drawing.Point(3, 65);
            this.pasteFrameButton.Name = "pasteFrameButton";
            this.pasteFrameButton.Size = new System.Drawing.Size(84, 23);
            this.pasteFrameButton.TabIndex = 2;
            this.pasteFrameButton.Text = "Paste";
            this.pasteFrameButton.Click += new System.EventHandler(this.PasteFrameButton_Click);
            // 
            // duplicateFrameButton
            // 
            this.duplicateFrameButton.Location = new System.Drawing.Point(3, 94);
            this.duplicateFrameButton.Name = "duplicateFrameButton";
            this.duplicateFrameButton.Size = new System.Drawing.Size(84, 23);
            this.duplicateFrameButton.TabIndex = 3;
            this.duplicateFrameButton.Text = "Duplicate";
            this.duplicateFrameButton.Click += new System.EventHandler(this.DuplicateFrameButton_Click);
            // 
            // deleteFrameButton
            // 
            this.deleteFrameButton.Location = new System.Drawing.Point(3, 123);
            this.deleteFrameButton.Name = "deleteFrameButton";
            this.deleteFrameButton.Size = new System.Drawing.Size(84, 23);
            this.deleteFrameButton.TabIndex = 4;
            this.deleteFrameButton.Text = "Remove";
            this.deleteFrameButton.Click += new System.EventHandler(this.DeleteFrameButton_Click);
            // 
            // framesPropertyGrid
            // 
            this.framesPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesPropertyGrid.Location = new System.Drawing.Point(6, 30);
            this.framesPropertyGrid.Name = "framesPropertyGrid";
            this.framesPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.framesPropertyGrid.Size = new System.Drawing.Size(120, 170);
            this.framesPropertyGrid.TabIndex = 0;
            this.framesPropertyGrid.ToolbarVisible = false;
            // 
            // frameDetailsLabel
            // 
            this.frameDetailsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.frameDetailsLabel.Location = new System.Drawing.Point(6, 6);
            this.frameDetailsLabel.Name = "frameDetailsLabel";
            this.frameDetailsLabel.Size = new System.Drawing.Size(120, 24);
            this.frameDetailsLabel.TabIndex = 1;
            this.frameDetailsLabel.Text = "Frame Details";
            // 
            // chunkPathTextBox
            // 
            this.chunkPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chunkPathTextBox.Location = new System.Drawing.Point(77, 84);
            this.chunkPathTextBox.Name = "chunkPathTextBox";
            this.chunkPathTextBox.Size = new System.Drawing.Size(663, 20);
            this.chunkPathTextBox.TabIndex = 1;
            this.chunkPathTextBox.TextChanged += new System.EventHandler(this.chunkPathTextBox_TextChanged);
            // 
            // chunkPathLabel
            // 
            this.chunkPathLabel.AutoSize = true;
            this.chunkPathLabel.Location = new System.Drawing.Point(3, 84);
            this.chunkPathLabel.Name = "chunkPathLabel";
            this.chunkPathLabel.Size = new System.Drawing.Size(63, 13);
            this.chunkPathLabel.TabIndex = 2;
            this.chunkPathLabel.Text = "Chunk Path";
            // 
            // chunkNameTextBox
            // 
            this.chunkNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chunkNameTextBox.Location = new System.Drawing.Point(77, 52);
            this.chunkNameTextBox.Name = "chunkNameTextBox";
            this.chunkNameTextBox.Size = new System.Drawing.Size(663, 20);
            this.chunkNameTextBox.TabIndex = 3;
            this.chunkNameTextBox.TextChanged += new System.EventHandler(this.chunkNameTextBox_TextChanged);
            // 
            // chunkNameLabel
            // 
            this.chunkNameLabel.AutoSize = true;
            this.chunkNameLabel.Location = new System.Drawing.Point(3, 55);
            this.chunkNameLabel.Name = "chunkNameLabel";
            this.chunkNameLabel.Size = new System.Drawing.Size(69, 13);
            this.chunkNameLabel.TabIndex = 4;
            this.chunkNameLabel.Text = "Chunk Name";
            // 
            // chunkComboBox
            // 
            this.chunkComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chunkComboBox.FormattingEnabled = true;
            this.chunkComboBox.Location = new System.Drawing.Point(77, 15);
            this.chunkComboBox.Name = "chunkComboBox";
            this.chunkComboBox.Size = new System.Drawing.Size(663, 21);
            this.chunkComboBox.TabIndex = 5;
            this.chunkComboBox.SelectedIndexChanged += new System.EventHandler(this.chunkComboBox_SelectedIndexChanged);
            // 
            // chunkLabel
            // 
            this.chunkLabel.AutoSize = true;
            this.chunkLabel.Location = new System.Drawing.Point(3, 18);
            this.chunkLabel.Name = "chunkLabel";
            this.chunkLabel.Size = new System.Drawing.Size(38, 13);
            this.chunkLabel.TabIndex = 6;
            this.chunkLabel.Text = "Chunk";
            // 
            // Tool_TrailEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1427, 843);
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1080, 700);
            this.Name = "Tool_TrailEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trail Chunk Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.managersTabPage.ResumeLayout(false);
            this.managersSplitContainer.Panel1.ResumeLayout(false);
            this.managersSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.managersSplitContainer)).EndInit();
            this.managersSplitContainer.ResumeLayout(false);
            this.managersButtonsPanel.ResumeLayout(false);
            this.resourcesTabPage.ResumeLayout(false);
            this.resourcesSplitContainer.Panel1.ResumeLayout(false);
            this.resourcesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resourcesSplitContainer)).EndInit();
            this.resourcesSplitContainer.ResumeLayout(false);
            this.resourcesButtonsPanel.ResumeLayout(false);
            this.positionsTabPage.ResumeLayout(false);
            this.positionsSplitContainer.Panel1.ResumeLayout(false);
            this.positionsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.positionsSplitContainer)).EndInit();
            this.positionsSplitContainer.ResumeLayout(false);
            this.positionsButtonsPanel.ResumeLayout(false);
            this.forceFieldsTabPage.ResumeLayout(false);
            this.forceFieldsSplitContainer.Panel1.ResumeLayout(false);
            this.forceFieldsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forceFieldsSplitContainer)).EndInit();
            this.forceFieldsSplitContainer.ResumeLayout(false);
            this.forceFieldsButtonsPanel.ResumeLayout(false);
            this.mapIdsTabPage.ResumeLayout(false);
            this.mapIdsSplitContainer.Panel1.ResumeLayout(false);
            this.mapIdsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapIdsSplitContainer)).EndInit();
            this.mapIdsSplitContainer.ResumeLayout(false);
            this.mapIdsButtonsPanel.ResumeLayout(false);
            this.nodesTabPage.ResumeLayout(false);
            this.nodesSplitContainer.Panel1.ResumeLayout(false);
            this.nodesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nodesSplitContainer)).EndInit();
            this.nodesSplitContainer.ResumeLayout(false);
            this.nodesButtonsPanel.ResumeLayout(false);
            this.nodeDetailsSplitContainer.Panel1.ResumeLayout(false);
            this.nodeDetailsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nodeDetailsSplitContainer)).EndInit();
            this.nodeDetailsSplitContainer.ResumeLayout(false);
            this.framesSplitContainer.Panel1.ResumeLayout(false);
            this.framesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.framesSplitContainer)).EndInit();
            this.framesSplitContainer.ResumeLayout(false);
            this.framesButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private Panel detailsPanel;
        private ComboBox chunkComboBox;
        private TextBox chunkNameTextBox;
        private TextBox chunkPathTextBox;
        private TabControl tabControl1;
        private ListBox managersListBox;
        private ListBox resourcesListBox;
        private ListBox positionsListBox;
        private ListBox forceFieldsListBox;
        private ListBox mapIdsListBox;
        private ListBox nodesListBox;
        private ListBox framesListBox;
        private PropertyGrid managersPropertyGrid;
        private PropertyGrid resourcesPropertyGrid;
        private PropertyGrid positionsPropertyGrid;
        private PropertyGrid forceFieldsPropertyGrid;
        private PropertyGrid mapIdsPropertyGrid;
        private PropertyGrid nodesPropertyGrid;
        private PropertyGrid framesPropertyGrid;
    }
}
