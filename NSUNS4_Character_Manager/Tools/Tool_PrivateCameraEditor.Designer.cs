namespace NSUNS4_Character_Manager.Tools
{
    partial class Tool_PrivateCameraEditor
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
            this.entryListBox = new System.Windows.Forms.ListBox();
            this.addEntryButton = new System.Windows.Forms.Button();
            this.copyEntryButton = new System.Windows.Forms.Button();
            this.pasteEntryButton = new System.Windows.Forms.Button();
            this.duplicateEntryButton = new System.Windows.Forms.Button();
            this.deleteEntryButton = new System.Windows.Forms.Button();
            this.saveEntryButton = new System.Windows.Forms.Button();
            this.cameraDistanceLabel = new System.Windows.Forms.Label();
            this.cameraDistanceTextBox = new System.Windows.Forms.TextBox();
            this.cameraSpeedLabel = new System.Windows.Forms.Label();
            this.cameraSpeedTextBox = new System.Windows.Forms.TextBox();
            this.cameraMovementLabel = new System.Windows.Forms.Label();
            this.cameraMovementTextBox = new System.Windows.Forms.TextBox();
            this.unk1Label = new System.Windows.Forms.Label();
            this.unk1TextBox = new System.Windows.Forms.TextBox();
            this.cameraHeightLabel = new System.Windows.Forms.Label();
            this.cameraHeightTextBox = new System.Windows.Forms.TextBox();
            this.cameraAngleLabel = new System.Windows.Forms.Label();
            this.cameraAngleTextBox = new System.Windows.Forms.TextBox();
            this.cameraHeight2Label = new System.Windows.Forms.Label();
            this.cameraHeight2TextBox = new System.Windows.Forms.TextBox();
            this.fovLabel = new System.Windows.Forms.Label();
            this.fovTextBox = new System.Windows.Forms.TextBox();
            this.unk2Label = new System.Windows.Forms.Label();
            this.unk2TextBox = new System.Windows.Forms.TextBox();
            this.cameraDistance2Label = new System.Windows.Forms.Label();
            this.cameraDistance2TextBox = new System.Windows.Forms.TextBox();
            this.fov2Label = new System.Windows.Forms.Label();
            this.fov2TextBox = new System.Windows.Forms.TextBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(612, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
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
            // entryListBox
            // 
            this.entryListBox.IntegralHeight = false;
            this.entryListBox.ItemHeight = 15;
            this.entryListBox.Location = new System.Drawing.Point(12, 27);
            this.entryListBox.Name = "entryListBox";
            this.entryListBox.Size = new System.Drawing.Size(205, 429);
            this.entryListBox.TabIndex = 3;
            this.entryListBox.SelectedIndexChanged += new System.EventHandler(this.EntryListBox_SelectedIndexChanged);
            // 
            // addEntryButton
            // 
            this.addEntryButton.Location = new System.Drawing.Point(12, 462);
            this.addEntryButton.Name = "addEntryButton";
            this.addEntryButton.Size = new System.Drawing.Size(62, 30);
            this.addEntryButton.TabIndex = 5;
            this.addEntryButton.Text = "Add";
            this.addEntryButton.Click += new System.EventHandler(this.AddEntryButton_Click);
            // 
            // copyEntryButton
            // 
            this.copyEntryButton.Location = new System.Drawing.Point(12, 498);
            this.copyEntryButton.Name = "copyEntryButton";
            this.copyEntryButton.Size = new System.Drawing.Size(62, 30);
            this.copyEntryButton.TabIndex = 6;
            this.copyEntryButton.Text = "Copy";
            this.copyEntryButton.Click += new System.EventHandler(this.CopyEntryButton_Click);
            // 
            // pasteEntryButton
            // 
            this.pasteEntryButton.Location = new System.Drawing.Point(77, 498);
            this.pasteEntryButton.Name = "pasteEntryButton";
            this.pasteEntryButton.Size = new System.Drawing.Size(65, 30);
            this.pasteEntryButton.TabIndex = 7;
            this.pasteEntryButton.Text = "Paste";
            this.pasteEntryButton.Click += new System.EventHandler(this.PasteEntryButton_Click);
            // 
            // duplicateEntryButton
            // 
            this.duplicateEntryButton.Location = new System.Drawing.Point(77, 462);
            this.duplicateEntryButton.Name = "duplicateEntryButton";
            this.duplicateEntryButton.Size = new System.Drawing.Size(65, 30);
            this.duplicateEntryButton.TabIndex = 8;
            this.duplicateEntryButton.Text = "Duplicate";
            this.duplicateEntryButton.Click += new System.EventHandler(this.DuplicateEntryButton_Click);
            // 
            // deleteEntryButton
            // 
            this.deleteEntryButton.Location = new System.Drawing.Point(148, 498);
            this.deleteEntryButton.Name = "deleteEntryButton";
            this.deleteEntryButton.Size = new System.Drawing.Size(69, 30);
            this.deleteEntryButton.TabIndex = 9;
            this.deleteEntryButton.Text = "Delete";
            this.deleteEntryButton.Click += new System.EventHandler(this.DeleteEntryButton_Click);
            // 
            // saveEntryButton
            // 
            this.saveEntryButton.Location = new System.Drawing.Point(148, 462);
            this.saveEntryButton.Name = "saveEntryButton";
            this.saveEntryButton.Size = new System.Drawing.Size(69, 30);
            this.saveEntryButton.TabIndex = 10;
            this.saveEntryButton.Text = "Save Entry";
            this.saveEntryButton.Click += new System.EventHandler(this.SaveEntryButton_Click);
            // 
            // cameraDistanceLabel
            // 
            this.cameraDistanceLabel.Location = new System.Drawing.Point(237, 27);
            this.cameraDistanceLabel.Name = "cameraDistanceLabel";
            this.cameraDistanceLabel.Size = new System.Drawing.Size(160, 23);
            this.cameraDistanceLabel.TabIndex = 13;
            this.cameraDistanceLabel.Text = "Camera distance";
            // 
            // cameraDistanceTextBox
            // 
            this.cameraDistanceTextBox.Location = new System.Drawing.Point(402, 27);
            this.cameraDistanceTextBox.Name = "cameraDistanceTextBox";
            this.cameraDistanceTextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraDistanceTextBox.TabIndex = 14;
            this.cameraDistanceTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraSpeedLabel
            // 
            this.cameraSpeedLabel.Location = new System.Drawing.Point(237, 62);
            this.cameraSpeedLabel.Name = "cameraSpeedLabel";
            this.cameraSpeedLabel.Size = new System.Drawing.Size(160, 23);
            this.cameraSpeedLabel.TabIndex = 15;
            this.cameraSpeedLabel.Text = "Camera speed";
            // 
            // cameraSpeedTextBox
            // 
            this.cameraSpeedTextBox.Location = new System.Drawing.Point(402, 62);
            this.cameraSpeedTextBox.Name = "cameraSpeedTextBox";
            this.cameraSpeedTextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraSpeedTextBox.TabIndex = 16;
            this.cameraSpeedTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraMovementLabel
            // 
            this.cameraMovementLabel.Location = new System.Drawing.Point(237, 97);
            this.cameraMovementLabel.Name = "cameraMovementLabel";
            this.cameraMovementLabel.Size = new System.Drawing.Size(160, 23);
            this.cameraMovementLabel.TabIndex = 17;
            this.cameraMovementLabel.Text = "Camera movement";
            // 
            // cameraMovementTextBox
            // 
            this.cameraMovementTextBox.Location = new System.Drawing.Point(402, 97);
            this.cameraMovementTextBox.Name = "cameraMovementTextBox";
            this.cameraMovementTextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraMovementTextBox.TabIndex = 18;
            this.cameraMovementTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // unk1Label
            // 
            this.unk1Label.Location = new System.Drawing.Point(237, 132);
            this.unk1Label.Name = "unk1Label";
            this.unk1Label.Size = new System.Drawing.Size(160, 23);
            this.unk1Label.TabIndex = 19;
            this.unk1Label.Text = "Unknown 1";
            // 
            // unk1TextBox
            // 
            this.unk1TextBox.Location = new System.Drawing.Point(402, 132);
            this.unk1TextBox.Name = "unk1TextBox";
            this.unk1TextBox.Size = new System.Drawing.Size(195, 23);
            this.unk1TextBox.TabIndex = 20;
            this.unk1TextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraHeightLabel
            // 
            this.cameraHeightLabel.Location = new System.Drawing.Point(237, 167);
            this.cameraHeightLabel.Name = "cameraHeightLabel";
            this.cameraHeightLabel.Size = new System.Drawing.Size(160, 23);
            this.cameraHeightLabel.TabIndex = 21;
            this.cameraHeightLabel.Text = "Camera height";
            // 
            // cameraHeightTextBox
            // 
            this.cameraHeightTextBox.Location = new System.Drawing.Point(402, 167);
            this.cameraHeightTextBox.Name = "cameraHeightTextBox";
            this.cameraHeightTextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraHeightTextBox.TabIndex = 22;
            this.cameraHeightTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraAngleLabel
            // 
            this.cameraAngleLabel.Location = new System.Drawing.Point(237, 202);
            this.cameraAngleLabel.Name = "cameraAngleLabel";
            this.cameraAngleLabel.Size = new System.Drawing.Size(160, 23);
            this.cameraAngleLabel.TabIndex = 23;
            this.cameraAngleLabel.Text = "Camera angle";
            // 
            // cameraAngleTextBox
            // 
            this.cameraAngleTextBox.Location = new System.Drawing.Point(402, 202);
            this.cameraAngleTextBox.Name = "cameraAngleTextBox";
            this.cameraAngleTextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraAngleTextBox.TabIndex = 24;
            this.cameraAngleTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraHeight2Label
            // 
            this.cameraHeight2Label.Location = new System.Drawing.Point(237, 237);
            this.cameraHeight2Label.Name = "cameraHeight2Label";
            this.cameraHeight2Label.Size = new System.Drawing.Size(160, 23);
            this.cameraHeight2Label.TabIndex = 25;
            this.cameraHeight2Label.Text = "Camera height 2";
            // 
            // cameraHeight2TextBox
            // 
            this.cameraHeight2TextBox.Location = new System.Drawing.Point(402, 237);
            this.cameraHeight2TextBox.Name = "cameraHeight2TextBox";
            this.cameraHeight2TextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraHeight2TextBox.TabIndex = 26;
            this.cameraHeight2TextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // fovLabel
            // 
            this.fovLabel.Location = new System.Drawing.Point(237, 272);
            this.fovLabel.Name = "fovLabel";
            this.fovLabel.Size = new System.Drawing.Size(160, 23);
            this.fovLabel.TabIndex = 27;
            this.fovLabel.Text = "FOV";
            // 
            // fovTextBox
            // 
            this.fovTextBox.Location = new System.Drawing.Point(402, 272);
            this.fovTextBox.Name = "fovTextBox";
            this.fovTextBox.Size = new System.Drawing.Size(195, 23);
            this.fovTextBox.TabIndex = 28;
            this.fovTextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // unk2Label
            // 
            this.unk2Label.Location = new System.Drawing.Point(237, 307);
            this.unk2Label.Name = "unk2Label";
            this.unk2Label.Size = new System.Drawing.Size(160, 23);
            this.unk2Label.TabIndex = 29;
            this.unk2Label.Text = "Unknown 2";
            // 
            // unk2TextBox
            // 
            this.unk2TextBox.Location = new System.Drawing.Point(402, 307);
            this.unk2TextBox.Name = "unk2TextBox";
            this.unk2TextBox.Size = new System.Drawing.Size(195, 23);
            this.unk2TextBox.TabIndex = 30;
            this.unk2TextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // cameraDistance2Label
            // 
            this.cameraDistance2Label.Location = new System.Drawing.Point(237, 342);
            this.cameraDistance2Label.Name = "cameraDistance2Label";
            this.cameraDistance2Label.Size = new System.Drawing.Size(160, 23);
            this.cameraDistance2Label.TabIndex = 31;
            this.cameraDistance2Label.Text = "Camera distance 2";
            // 
            // cameraDistance2TextBox
            // 
            this.cameraDistance2TextBox.Location = new System.Drawing.Point(402, 342);
            this.cameraDistance2TextBox.Name = "cameraDistance2TextBox";
            this.cameraDistance2TextBox.Size = new System.Drawing.Size(195, 23);
            this.cameraDistance2TextBox.TabIndex = 32;
            this.cameraDistance2TextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // fov2Label
            // 
            this.fov2Label.Location = new System.Drawing.Point(237, 377);
            this.fov2Label.Name = "fov2Label";
            this.fov2Label.Size = new System.Drawing.Size(160, 23);
            this.fov2Label.TabIndex = 33;
            this.fov2Label.Text = "FOV 2";
            // 
            // fov2TextBox
            // 
            this.fov2TextBox.Location = new System.Drawing.Point(402, 377);
            this.fov2TextBox.Name = "fov2TextBox";
            this.fov2TextBox.Size = new System.Drawing.Size(195, 23);
            this.fov2TextBox.TabIndex = 34;
            this.fov2TextBox.TextChanged += new System.EventHandler(this.CameraValue_TextChanged);
            // 
            // Tool_PrivateCameraEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 529);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.entryListBox);
            this.Controls.Add(this.addEntryButton);
            this.Controls.Add(this.copyEntryButton);
            this.Controls.Add(this.pasteEntryButton);
            this.Controls.Add(this.duplicateEntryButton);
            this.Controls.Add(this.deleteEntryButton);
            this.Controls.Add(this.saveEntryButton);
            this.Controls.Add(this.cameraDistanceLabel);
            this.Controls.Add(this.cameraDistanceTextBox);
            this.Controls.Add(this.cameraSpeedLabel);
            this.Controls.Add(this.cameraSpeedTextBox);
            this.Controls.Add(this.cameraMovementLabel);
            this.Controls.Add(this.cameraMovementTextBox);
            this.Controls.Add(this.unk1Label);
            this.Controls.Add(this.unk1TextBox);
            this.Controls.Add(this.cameraHeightLabel);
            this.Controls.Add(this.cameraHeightTextBox);
            this.Controls.Add(this.cameraAngleLabel);
            this.Controls.Add(this.cameraAngleTextBox);
            this.Controls.Add(this.cameraHeight2Label);
            this.Controls.Add(this.cameraHeight2TextBox);
            this.Controls.Add(this.fovLabel);
            this.Controls.Add(this.fovTextBox);
            this.Controls.Add(this.unk2Label);
            this.Controls.Add(this.unk2TextBox);
            this.Controls.Add(this.cameraDistance2Label);
            this.Controls.Add(this.cameraDistance2TextBox);
            this.Controls.Add(this.fov2Label);
            this.Controls.Add(this.fov2TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "Tool_PrivateCameraEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Private Camera";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tool_PrivateCameraEditor_FormClosing);
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
        private System.Windows.Forms.ListBox entryListBox;
        private System.Windows.Forms.Button addEntryButton;
        private System.Windows.Forms.Button copyEntryButton;
        private System.Windows.Forms.Button pasteEntryButton;
        private System.Windows.Forms.Button duplicateEntryButton;
        private System.Windows.Forms.Button deleteEntryButton;
        private System.Windows.Forms.Button saveEntryButton;
        private System.Windows.Forms.Label cameraDistanceLabel;
        private System.Windows.Forms.TextBox cameraDistanceTextBox;
        private System.Windows.Forms.Label cameraSpeedLabel;
        private System.Windows.Forms.TextBox cameraSpeedTextBox;
        private System.Windows.Forms.Label cameraMovementLabel;
        private System.Windows.Forms.TextBox cameraMovementTextBox;
        private System.Windows.Forms.Label unk1Label;
        private System.Windows.Forms.TextBox unk1TextBox;
        private System.Windows.Forms.Label cameraHeightLabel;
        private System.Windows.Forms.TextBox cameraHeightTextBox;
        private System.Windows.Forms.Label cameraAngleLabel;
        private System.Windows.Forms.TextBox cameraAngleTextBox;
        private System.Windows.Forms.Label cameraHeight2Label;
        private System.Windows.Forms.TextBox cameraHeight2TextBox;
        private System.Windows.Forms.Label fovLabel;
        private System.Windows.Forms.TextBox fovTextBox;
        private System.Windows.Forms.Label unk2Label;
        private System.Windows.Forms.TextBox unk2TextBox;
        private System.Windows.Forms.Label cameraDistance2Label;
        private System.Windows.Forms.TextBox cameraDistance2TextBox;
        private System.Windows.Forms.Label fov2Label;
        private System.Windows.Forms.TextBox fov2TextBox;
    }
}
