namespace NSUNS4_Character_Manager.Tools
{
    partial class Tool_EvoCustomParamEditor
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByNameButton = new System.Windows.Forms.ToolStripButton();
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.characterCodeLabel = new System.Windows.Forms.Label();
            this.characterCodeTextBox = new System.Windows.Forms.TextBox();
            this.flagsLabel = new System.Windows.Forms.Label();
            this.costumeFlagsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.copyEntryButton = new System.Windows.Forms.Button();
            this.pasteEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.saveEntryButton = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.sortByNameButton});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(534, 26);
            this.menuStrip.TabIndex = 0;
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileMenuItem.Text = "&File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openMenuItem.Text = "&Open...";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsMenuItem.Text = "Save &As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // sortByNameButton
            // 
            this.sortByNameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sortByNameButton.Name = "sortByNameButton";
            this.sortByNameButton.Size = new System.Drawing.Size(32, 19);
            this.sortByNameButton.Text = "Sort";
            this.sortByNameButton.ToolTipText = "Sort all entries A-Z. Use File > Save to write the new order.";
            this.sortByNameButton.Click += new System.EventHandler(this.SortByNameButton_Click);
            // 
            // entryListBox
            // 
            this.entryListBox.HorizontalScrollbar = true;
            this.entryListBox.IntegralHeight = false;
            this.entryListBox.ItemHeight = 15;
            this.entryListBox.Location = new System.Drawing.Point(12, 27);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(166, 416);
            this.entryListBox.TabIndex = 3;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.EntryListBox_SelectedIndexChanged);
            // 
            // searchLabel
            // 
            this.searchLabel.Location = new System.Drawing.Point(9, 452);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(80, 20);
            this.searchLabel.TabIndex = 4;
            this.searchLabel.Text = "Search names";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(98, 449);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(80, 23);
            this.searchTextBox.TabIndex = 2;
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // characterCodeLabel
            // 
            this.characterCodeLabel.Location = new System.Drawing.Point(184, 27);
            this.characterCodeLabel.Name = "characterCodeLabel";
            this.characterCodeLabel.Size = new System.Drawing.Size(65, 22);
            this.characterCodeLabel.TabIndex = 4;
            this.characterCodeLabel.Text = "Characode";
            // 
            // characterCodeTextBox
            // 
            this.characterCodeTextBox.Location = new System.Drawing.Point(255, 29);
            this.characterCodeTextBox.MaxLength = 8;
            this.characterCodeTextBox.Name = "characterCodeTextBox";
            this.characterCodeTextBox.Size = new System.Drawing.Size(272, 23);
            this.characterCodeTextBox.TabIndex = 5;
            this.characterCodeTextBox.TextChanged += new System.EventHandler(this.CharacterCodeTextBox_TextChanged);
            // 
            // flagsLabel
            // 
            this.flagsLabel.Location = new System.Drawing.Point(184, 53);
            this.flagsLabel.Name = "flagsLabel";
            this.flagsLabel.Size = new System.Drawing.Size(100, 22);
            this.flagsLabel.TabIndex = 6;
            this.flagsLabel.Text = "Forced costume Entries:";
            // 
            // costumeFlagsCheckedListBox
            // 
            this.costumeFlagsCheckedListBox.CheckOnClick = true;
            this.costumeFlagsCheckedListBox.IntegralHeight = false;
            this.costumeFlagsCheckedListBox.Items.AddRange(new object[] {
            "Costume index 0",
            "Costume index 1",
            "Costume index 2",
            "Costume index 3",
            "Costume index 4",
            "Costume index 5",
            "Costume index 6",
            "Costume index 7",
            "Costume index 8",
            "Costume index 9",
            "Costume index 10",
            "Costume index 11",
            "Costume index 12",
            "Costume index 13",
            "Costume index 14",
            "Costume index 15",
            "Costume index 16",
            "Costume index 17",
            "Costume index 18",
            "Costume index 19"});
            this.costumeFlagsCheckedListBox.Location = new System.Drawing.Point(187, 78);
            this.costumeFlagsCheckedListBox.Name = "costumeFlagsCheckedListBox";
            this.costumeFlagsCheckedListBox.Size = new System.Drawing.Size(340, 364);
            this.costumeFlagsCheckedListBox.TabIndex = 7;
            this.costumeFlagsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CostumeFlagsCheckedListBox_ItemCheck);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Location = new System.Drawing.Point(12, 478);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(80, 30);
            this.addEntryButton.TabIndex = 8;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.Click += new System.EventHandler(this.AddEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Location = new System.Drawing.Point(12, 550);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(83, 30);
            this.deleteEntryButton.TabIndex = 9;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.Click += new System.EventHandler(this.DeleteEntryButton_Click);
            // 
            // copyEntryButton
            // 
            this.copyEntryButton.Location = new System.Drawing.Point(12, 514);
            this.copyEntryButton.Name = "copyEntryButton";
            this.copyEntryButton.Size = new System.Drawing.Size(80, 30);
            this.copyEntryButton.TabIndex = 10;
            this.copyEntryButton.Text = "Copy";
            this.copyEntryButton.Click += new System.EventHandler(this.CopyEntryButton_Click);
            // 
            // pasteEntryButton
            // 
            this.pasteEntryButton.Location = new System.Drawing.Point(98, 478);
            this.pasteEntryButton.Name = "pasteEntryButton";
            this.pasteEntryButton.Size = new System.Drawing.Size(80, 30);
            this.pasteEntryButton.TabIndex = 11;
            this.pasteEntryButton.Text = "Paste";
            this.pasteEntryButton.Click += new System.EventHandler(this.PasteEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Location = new System.Drawing.Point(98, 514);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(80, 30);
            this.duplicateEntryButton.TabIndex = 12;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.Click += new System.EventHandler(this.DuplicateEntryButton_Click);
            // 
            // saveEntryButton
            // 
            this.saveEntryButton.Location = new System.Drawing.Point(101, 550);
            this.saveEntryButton.Name = "saveEntryButton";
            this.saveEntryButton.Size = new System.Drawing.Size(77, 30);
            this.saveEntryButton.TabIndex = 13;
            this.saveEntryButton.Text = "Save Entry";
            this.saveEntryButton.UseVisualStyleBackColor = true;
            this.saveEntryButton.Click += new System.EventHandler(this.SaveEntryButton_Click);
            // 
            // Tool_EvoCustomParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 586);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.characterCodeLabel);
            this.Controls.Add(this.characterCodeTextBox);
            this.Controls.Add(this.flagsLabel);
            this.Controls.Add(this.costumeFlagsCheckedListBox);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.copyEntryButton);
            this.Controls.Add(this.pasteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.saveEntryButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "Tool_EvoCustomParamEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evo Custom Param";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tool_EvoCustomParamEditor_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripButton sortByNameButton;
        private System.Windows.Forms.ListBox entryListBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label characterCodeLabel;
        private System.Windows.Forms.TextBox characterCodeTextBox;
        private System.Windows.Forms.Label flagsLabel;
        private System.Windows.Forms.CheckedListBox costumeFlagsCheckedListBox;
        private System.Windows.Forms.Button addEntryButton;
        private System.Windows.Forms.Button deleteEntryButton;
        private System.Windows.Forms.Button copyEntryButton;
        private System.Windows.Forms.Button pasteEntryButton;
        private System.Windows.Forms.Button duplicateEntryButton;
        private System.Windows.Forms.Button saveEntryButton;
    }
}
