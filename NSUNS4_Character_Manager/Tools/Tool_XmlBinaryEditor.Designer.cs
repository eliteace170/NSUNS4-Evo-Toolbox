using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_XmlBinaryEditor
    {
        private IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ListBox chunkListBox;
        private Button addChunkButton;
        private Button duplicateChunkButton;
        private Button deleteChunkButton;
        private Button copyChunkButton;
        private Button pasteChunkButton;
        private Button saveChunkButton;
        private Panel editorPanel;
        private Label chunkNameLabel;
        private Label chunkPathLabel;
        private TextBox chunkNameTextBox;
        private TextBox chunkPathTextBox;
        private TextBox xmlTextBox;
        private ListBox suggestionListBox;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;

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
            this.chunkListBox = new System.Windows.Forms.ListBox();
            this.addChunkButton = new System.Windows.Forms.Button();
            this.duplicateChunkButton = new System.Windows.Forms.Button();
            this.deleteChunkButton = new System.Windows.Forms.Button();
            this.copyChunkButton = new System.Windows.Forms.Button();
            this.pasteChunkButton = new System.Windows.Forms.Button();
            this.saveChunkButton = new System.Windows.Forms.Button();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.suggestionListBox = new System.Windows.Forms.ListBox();
            this.xmlTextBox = new System.Windows.Forms.TextBox();
            this.chunkPathTextBox = new System.Windows.Forms.TextBox();
            this.chunkNameTextBox = new System.Windows.Forms.TextBox();
            this.chunkPathLabel = new System.Windows.Forms.Label();
            this.chunkNameLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.editorPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1343, 24);
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
            // chunkListBox
            // 
            this.chunkListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chunkListBox.FormattingEnabled = true;
            this.chunkListBox.HorizontalScrollbar = true;
            this.chunkListBox.Location = new System.Drawing.Point(12, 31);
            this.chunkListBox.Name = "chunkListBox";
            this.chunkListBox.Size = new System.Drawing.Size(308, 498);
            this.chunkListBox.TabIndex = 1;
            this.chunkListBox.SelectedIndexChanged += new System.EventHandler(this.chunkListBox_SelectedIndexChanged);
            // 
            // addChunkButton
            // 
            this.addChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addChunkButton.Location = new System.Drawing.Point(12, 541);
            this.addChunkButton.Name = "addChunkButton";
            this.addChunkButton.Size = new System.Drawing.Size(43, 25);
            this.addChunkButton.TabIndex = 2;
            this.addChunkButton.Text = "Add";
            this.addChunkButton.UseVisualStyleBackColor = true;
            this.addChunkButton.Click += new System.EventHandler(this.addChunkButton_Click);
            // 
            // duplicateChunkButton
            // 
            this.duplicateChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateChunkButton.Location = new System.Drawing.Point(59, 541);
            this.duplicateChunkButton.Name = "duplicateChunkButton";
            this.duplicateChunkButton.Size = new System.Drawing.Size(62, 25);
            this.duplicateChunkButton.TabIndex = 3;
            this.duplicateChunkButton.Text = "Dup";
            this.duplicateChunkButton.UseVisualStyleBackColor = true;
            this.duplicateChunkButton.Click += new System.EventHandler(this.duplicateChunkButton_Click);
            // 
            // deleteChunkButton
            // 
            this.deleteChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteChunkButton.Location = new System.Drawing.Point(272, 541);
            this.deleteChunkButton.Name = "deleteChunkButton";
            this.deleteChunkButton.Size = new System.Drawing.Size(48, 25);
            this.deleteChunkButton.TabIndex = 7;
            this.deleteChunkButton.Text = "Delete";
            this.deleteChunkButton.UseVisualStyleBackColor = true;
            this.deleteChunkButton.Click += new System.EventHandler(this.deleteChunkButton_Click);
            // 
            // copyChunkButton
            // 
            this.copyChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyChunkButton.Location = new System.Drawing.Point(125, 541);
            this.copyChunkButton.Name = "copyChunkButton";
            this.copyChunkButton.Size = new System.Drawing.Size(45, 25);
            this.copyChunkButton.TabIndex = 4;
            this.copyChunkButton.Text = "Copy";
            this.copyChunkButton.UseVisualStyleBackColor = true;
            this.copyChunkButton.Click += new System.EventHandler(this.copyChunkButton_Click);
            // 
            // pasteChunkButton
            // 
            this.pasteChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pasteChunkButton.Location = new System.Drawing.Point(174, 541);
            this.pasteChunkButton.Name = "pasteChunkButton";
            this.pasteChunkButton.Size = new System.Drawing.Size(45, 25);
            this.pasteChunkButton.TabIndex = 5;
            this.pasteChunkButton.Text = "Paste";
            this.pasteChunkButton.UseVisualStyleBackColor = true;
            this.pasteChunkButton.Click += new System.EventHandler(this.pasteChunkButton_Click);
            // 
            // saveChunkButton
            // 
            this.saveChunkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveChunkButton.Location = new System.Drawing.Point(223, 541);
            this.saveChunkButton.Name = "saveChunkButton";
            this.saveChunkButton.Size = new System.Drawing.Size(45, 25);
            this.saveChunkButton.TabIndex = 6;
            this.saveChunkButton.Text = "Save";
            this.saveChunkButton.UseVisualStyleBackColor = true;
            this.saveChunkButton.Click += new System.EventHandler(this.saveChunkButton_Click);
            // 
            // editorPanel
            // 
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorPanel.Controls.Add(this.suggestionListBox);
            this.editorPanel.Controls.Add(this.xmlTextBox);
            this.editorPanel.Controls.Add(this.chunkPathTextBox);
            this.editorPanel.Controls.Add(this.chunkNameTextBox);
            this.editorPanel.Controls.Add(this.chunkPathLabel);
            this.editorPanel.Controls.Add(this.chunkNameLabel);
            this.editorPanel.Location = new System.Drawing.Point(326, 31);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(1005, 535);
            this.editorPanel.TabIndex = 6;
            // 
            // suggestionListBox
            // 
            this.suggestionListBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.suggestionListBox.FormattingEnabled = true;
            this.suggestionListBox.IntegralHeight = false;
            this.suggestionListBox.ItemHeight = 14;
            this.suggestionListBox.Location = new System.Drawing.Point(11, 57);
            this.suggestionListBox.Name = "suggestionListBox";
            this.suggestionListBox.Size = new System.Drawing.Size(360, 132);
            this.suggestionListBox.TabIndex = 6;
            this.suggestionListBox.Visible = false;
            this.suggestionListBox.DoubleClick += new System.EventHandler(this.suggestionListBox_DoubleClick);
            // 
            // xmlTextBox
            // 
            this.xmlTextBox.AcceptsReturn = true;
            this.xmlTextBox.AcceptsTab = true;
            this.xmlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.xmlTextBox.Location = new System.Drawing.Point(11, 57);
            this.xmlTextBox.Multiline = true;
            this.xmlTextBox.Name = "xmlTextBox";
            this.xmlTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.xmlTextBox.Size = new System.Drawing.Size(979, 463);
            this.xmlTextBox.TabIndex = 4;
            this.xmlTextBox.WordWrap = false;
            this.xmlTextBox.TextChanged += new System.EventHandler(this.xmlTextBox_TextChanged);
            this.xmlTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.xmlTextBox_KeyDown);
            this.xmlTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.xmlTextBox_KeyUp);
            // 
            // chunkPathTextBox
            // 
            this.chunkPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chunkPathTextBox.Location = new System.Drawing.Point(82, 31);
            this.chunkPathTextBox.Name = "chunkPathTextBox";
            this.chunkPathTextBox.Size = new System.Drawing.Size(908, 20);
            this.chunkPathTextBox.TabIndex = 3;
            // 
            // chunkNameTextBox
            // 
            this.chunkNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chunkNameTextBox.Location = new System.Drawing.Point(82, 7);
            this.chunkNameTextBox.Name = "chunkNameTextBox";
            this.chunkNameTextBox.Size = new System.Drawing.Size(908, 20);
            this.chunkNameTextBox.TabIndex = 2;
            // 
            // chunkPathLabel
            // 
            this.chunkPathLabel.AutoSize = true;
            this.chunkPathLabel.Location = new System.Drawing.Point(8, 34);
            this.chunkPathLabel.Name = "chunkPathLabel";
            this.chunkPathLabel.Size = new System.Drawing.Size(62, 13);
            this.chunkPathLabel.TabIndex = 1;
            this.chunkPathLabel.Text = "Chunk path";
            // 
            // chunkNameLabel
            // 
            this.chunkNameLabel.AutoSize = true;
            this.chunkNameLabel.Location = new System.Drawing.Point(8, 10);
            this.chunkNameLabel.Name = "chunkNameLabel";
            this.chunkNameLabel.Size = new System.Drawing.Size(67, 13);
            this.chunkNameLabel.TabIndex = 0;
            this.chunkNameLabel.Text = "Chunk name";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 575);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1343, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Tool_XmlBinaryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 597);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.saveChunkButton);
            this.Controls.Add(this.pasteChunkButton);
            this.Controls.Add(this.copyChunkButton);
            this.Controls.Add(this.deleteChunkButton);
            this.Controls.Add(this.duplicateChunkButton);
            this.Controls.Add(this.addChunkButton);
            this.Controls.Add(this.chunkListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(850, 520);
            this.Name = "Tool_XmlBinaryEditor";
            this.Text = "XML Binary Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
