namespace NSUNS4_Character_Manager.Tools
{
    partial class Tool_PRMEtcEditor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.labelCancelFrame = new System.Windows.Forms.Label();
            this.nCancelFrame = new System.Windows.Forms.NumericUpDown();
            this.labelAirLength = new System.Windows.Forms.Label();
            this.nAirLength = new System.Windows.Forms.NumericUpDown();
            this.labelCircularAccelerationStartFrame = new System.Windows.Forms.Label();
            this.nCircularAccelerationStartFrame = new System.Windows.Forms.NumericUpDown();
            this.labelCircularAccelerationEndFrame = new System.Windows.Forms.Label();
            this.nCircularAccelerationEndFrame = new System.Windows.Forms.NumericUpDown();
            this.labelCircularAccelerationSpeed = new System.Windows.Forms.Label();
            this.nCircularAccelerationSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelCircularAccelerationSpeedDropoff = new System.Windows.Forms.Label();
            this.nCircularAccelerationSpeedDropoff = new System.Windows.Forms.NumericUpDown();
            this.labelCircularAccelerationSpeedMax = new System.Windows.Forms.Label();
            this.nCircularAccelerationSpeedMax = new System.Windows.Forms.NumericUpDown();
            this.labelForwardAccelerationStartFrame = new System.Windows.Forms.Label();
            this.nForwardAccelerationStartFrame = new System.Windows.Forms.NumericUpDown();
            this.labelForwardAccelerationEndFrame = new System.Windows.Forms.Label();
            this.nForwardAccelerationEndFrame = new System.Windows.Forms.NumericUpDown();
            this.labelForwardAccelerationSpeed = new System.Windows.Forms.Label();
            this.nForwardAccelerationSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelPadding = new System.Windows.Forms.Label();
            this.nPadding = new System.Windows.Forms.NumericUpDown();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAndCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.nCancelFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAirLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationStartFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationEndFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeedDropoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeedMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationStartFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationEndFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPadding)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(12, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(500, 498);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // labelCancelFrame
            // 
            this.labelCancelFrame.AutoSize = true;
            this.labelCancelFrame.Location = new System.Drawing.Point(527, 27);
            this.labelCancelFrame.Name = "labelCancelFrame";
            this.labelCancelFrame.Size = new System.Drawing.Size(72, 13);
            this.labelCancelFrame.TabIndex = 1;
            this.labelCancelFrame.Text = "Cancel Frame";
            // 
            // nCancelFrame
            // 
            this.nCancelFrame.Location = new System.Drawing.Point(530, 43);
            this.nCancelFrame.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nCancelFrame.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nCancelFrame.Name = "nCancelFrame";
            this.nCancelFrame.Size = new System.Drawing.Size(340, 20);
            this.nCancelFrame.TabIndex = 2;
            // 
            // labelAirLength
            // 
            this.labelAirLength.AutoSize = true;
            this.labelAirLength.Location = new System.Drawing.Point(527, 69);
            this.labelAirLength.Name = "labelAirLength";
            this.labelAirLength.Size = new System.Drawing.Size(55, 13);
            this.labelAirLength.TabIndex = 3;
            this.labelAirLength.Text = "Air Length";
            // 
            // nAirLength
            // 
            this.nAirLength.Location = new System.Drawing.Point(530, 85);
            this.nAirLength.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nAirLength.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nAirLength.Name = "nAirLength";
            this.nAirLength.Size = new System.Drawing.Size(340, 20);
            this.nAirLength.TabIndex = 4;
            // 
            // labelCircularAccelerationStartFrame
            // 
            this.labelCircularAccelerationStartFrame.AutoSize = true;
            this.labelCircularAccelerationStartFrame.Location = new System.Drawing.Point(527, 111);
            this.labelCircularAccelerationStartFrame.Name = "labelCircularAccelerationStartFrame";
            this.labelCircularAccelerationStartFrame.Size = new System.Drawing.Size(161, 13);
            this.labelCircularAccelerationStartFrame.TabIndex = 5;
            this.labelCircularAccelerationStartFrame.Text = "Circular Acceleration Start Frame";
            // 
            // nCircularAccelerationStartFrame
            // 
            this.nCircularAccelerationStartFrame.Location = new System.Drawing.Point(530, 127);
            this.nCircularAccelerationStartFrame.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nCircularAccelerationStartFrame.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nCircularAccelerationStartFrame.Name = "nCircularAccelerationStartFrame";
            this.nCircularAccelerationStartFrame.Size = new System.Drawing.Size(340, 20);
            this.nCircularAccelerationStartFrame.TabIndex = 6;
            // 
            // labelCircularAccelerationEndFrame
            // 
            this.labelCircularAccelerationEndFrame.AutoSize = true;
            this.labelCircularAccelerationEndFrame.Location = new System.Drawing.Point(527, 153);
            this.labelCircularAccelerationEndFrame.Name = "labelCircularAccelerationEndFrame";
            this.labelCircularAccelerationEndFrame.Size = new System.Drawing.Size(158, 13);
            this.labelCircularAccelerationEndFrame.TabIndex = 7;
            this.labelCircularAccelerationEndFrame.Text = "Circular Acceleration End Frame";
            // 
            // nCircularAccelerationEndFrame
            // 
            this.nCircularAccelerationEndFrame.Location = new System.Drawing.Point(530, 169);
            this.nCircularAccelerationEndFrame.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nCircularAccelerationEndFrame.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nCircularAccelerationEndFrame.Name = "nCircularAccelerationEndFrame";
            this.nCircularAccelerationEndFrame.Size = new System.Drawing.Size(340, 20);
            this.nCircularAccelerationEndFrame.TabIndex = 8;
            // 
            // labelCircularAccelerationSpeed
            // 
            this.labelCircularAccelerationSpeed.AutoSize = true;
            this.labelCircularAccelerationSpeed.Location = new System.Drawing.Point(527, 195);
            this.labelCircularAccelerationSpeed.Name = "labelCircularAccelerationSpeed";
            this.labelCircularAccelerationSpeed.Size = new System.Drawing.Size(138, 13);
            this.labelCircularAccelerationSpeed.TabIndex = 9;
            this.labelCircularAccelerationSpeed.Text = "Circular Acceleration Speed";
            // 
            // nCircularAccelerationSpeed
            // 
            this.nCircularAccelerationSpeed.DecimalPlaces = 4;
            this.nCircularAccelerationSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nCircularAccelerationSpeed.Location = new System.Drawing.Point(530, 211);
            this.nCircularAccelerationSpeed.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nCircularAccelerationSpeed.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nCircularAccelerationSpeed.Name = "nCircularAccelerationSpeed";
            this.nCircularAccelerationSpeed.Size = new System.Drawing.Size(340, 20);
            this.nCircularAccelerationSpeed.TabIndex = 10;
            // 
            // labelCircularAccelerationSpeedDropoff
            // 
            this.labelCircularAccelerationSpeedDropoff.AutoSize = true;
            this.labelCircularAccelerationSpeedDropoff.Location = new System.Drawing.Point(527, 237);
            this.labelCircularAccelerationSpeedDropoff.Name = "labelCircularAccelerationSpeedDropoff";
            this.labelCircularAccelerationSpeedDropoff.Size = new System.Drawing.Size(176, 13);
            this.labelCircularAccelerationSpeedDropoff.TabIndex = 11;
            this.labelCircularAccelerationSpeedDropoff.Text = "Circular Acceleration Speed Dropoff";
            // 
            // nCircularAccelerationSpeedDropoff
            // 
            this.nCircularAccelerationSpeedDropoff.DecimalPlaces = 4;
            this.nCircularAccelerationSpeedDropoff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nCircularAccelerationSpeedDropoff.Location = new System.Drawing.Point(530, 253);
            this.nCircularAccelerationSpeedDropoff.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nCircularAccelerationSpeedDropoff.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nCircularAccelerationSpeedDropoff.Name = "nCircularAccelerationSpeedDropoff";
            this.nCircularAccelerationSpeedDropoff.Size = new System.Drawing.Size(340, 20);
            this.nCircularAccelerationSpeedDropoff.TabIndex = 12;
            // 
            // labelCircularAccelerationSpeedMax
            // 
            this.labelCircularAccelerationSpeedMax.AutoSize = true;
            this.labelCircularAccelerationSpeedMax.Location = new System.Drawing.Point(527, 279);
            this.labelCircularAccelerationSpeedMax.Name = "labelCircularAccelerationSpeedMax";
            this.labelCircularAccelerationSpeedMax.Size = new System.Drawing.Size(161, 13);
            this.labelCircularAccelerationSpeedMax.TabIndex = 13;
            this.labelCircularAccelerationSpeedMax.Text = "Circular Acceleration Speed Max";
            // 
            // nCircularAccelerationSpeedMax
            // 
            this.nCircularAccelerationSpeedMax.DecimalPlaces = 4;
            this.nCircularAccelerationSpeedMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nCircularAccelerationSpeedMax.Location = new System.Drawing.Point(530, 295);
            this.nCircularAccelerationSpeedMax.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nCircularAccelerationSpeedMax.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nCircularAccelerationSpeedMax.Name = "nCircularAccelerationSpeedMax";
            this.nCircularAccelerationSpeedMax.Size = new System.Drawing.Size(340, 20);
            this.nCircularAccelerationSpeedMax.TabIndex = 14;
            // 
            // labelForwardAccelerationStartFrame
            // 
            this.labelForwardAccelerationStartFrame.AutoSize = true;
            this.labelForwardAccelerationStartFrame.Location = new System.Drawing.Point(527, 321);
            this.labelForwardAccelerationStartFrame.Name = "labelForwardAccelerationStartFrame";
            this.labelForwardAccelerationStartFrame.Size = new System.Drawing.Size(164, 13);
            this.labelForwardAccelerationStartFrame.TabIndex = 15;
            this.labelForwardAccelerationStartFrame.Text = "Forward Acceleration Start Frame";
            // 
            // nForwardAccelerationStartFrame
            // 
            this.nForwardAccelerationStartFrame.Location = new System.Drawing.Point(530, 337);
            this.nForwardAccelerationStartFrame.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nForwardAccelerationStartFrame.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nForwardAccelerationStartFrame.Name = "nForwardAccelerationStartFrame";
            this.nForwardAccelerationStartFrame.Size = new System.Drawing.Size(340, 20);
            this.nForwardAccelerationStartFrame.TabIndex = 16;
            // 
            // labelForwardAccelerationEndFrame
            // 
            this.labelForwardAccelerationEndFrame.AutoSize = true;
            this.labelForwardAccelerationEndFrame.Location = new System.Drawing.Point(527, 363);
            this.labelForwardAccelerationEndFrame.Name = "labelForwardAccelerationEndFrame";
            this.labelForwardAccelerationEndFrame.Size = new System.Drawing.Size(161, 13);
            this.labelForwardAccelerationEndFrame.TabIndex = 17;
            this.labelForwardAccelerationEndFrame.Text = "Forward Acceleration End Frame";
            // 
            // nForwardAccelerationEndFrame
            // 
            this.nForwardAccelerationEndFrame.Location = new System.Drawing.Point(530, 379);
            this.nForwardAccelerationEndFrame.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nForwardAccelerationEndFrame.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nForwardAccelerationEndFrame.Name = "nForwardAccelerationEndFrame";
            this.nForwardAccelerationEndFrame.Size = new System.Drawing.Size(340, 20);
            this.nForwardAccelerationEndFrame.TabIndex = 18;
            // 
            // labelForwardAccelerationSpeed
            // 
            this.labelForwardAccelerationSpeed.AutoSize = true;
            this.labelForwardAccelerationSpeed.Location = new System.Drawing.Point(527, 405);
            this.labelForwardAccelerationSpeed.Name = "labelForwardAccelerationSpeed";
            this.labelForwardAccelerationSpeed.Size = new System.Drawing.Size(141, 13);
            this.labelForwardAccelerationSpeed.TabIndex = 19;
            this.labelForwardAccelerationSpeed.Text = "Forward Acceleration Speed";
            // 
            // nForwardAccelerationSpeed
            // 
            this.nForwardAccelerationSpeed.DecimalPlaces = 4;
            this.nForwardAccelerationSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nForwardAccelerationSpeed.Location = new System.Drawing.Point(530, 421);
            this.nForwardAccelerationSpeed.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nForwardAccelerationSpeed.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nForwardAccelerationSpeed.Name = "nForwardAccelerationSpeed";
            this.nForwardAccelerationSpeed.Size = new System.Drawing.Size(340, 20);
            this.nForwardAccelerationSpeed.TabIndex = 20;
            // 
            // labelPadding
            // 
            this.labelPadding.AutoSize = true;
            this.labelPadding.Location = new System.Drawing.Point(527, 447);
            this.labelPadding.Name = "labelPadding";
            this.labelPadding.Size = new System.Drawing.Size(92, 13);
            this.labelPadding.TabIndex = 21;
            this.labelPadding.Text = "Padding (Unused)";
            // 
            // nPadding
            // 
            this.nPadding.Location = new System.Drawing.Point(530, 463);
            this.nPadding.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nPadding.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.nPadding.Name = "nPadding";
            this.nPadding.Size = new System.Drawing.Size(340, 20);
            this.nPadding.TabIndex = 22;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(530, 498);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(108, 35);
            this.buttonAdd.TabIndex = 23;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(646, 498);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(108, 35);
            this.buttonDelete.TabIndex = 24;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(762, 498);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(108, 35);
            this.buttonSave.TabIndex = 25;
            this.buttonSave.Text = "Save Entry";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAndCloseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAndCloseToolStripMenuItem
            // 
            this.saveAndCloseToolStripMenuItem.Name = "saveAndCloseToolStripMenuItem";
            this.saveAndCloseToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.saveAndCloseToolStripMenuItem.Text = "Save and close";
            this.saveAndCloseToolStripMenuItem.Click += new System.EventHandler(this.saveAndCloseToolStripMenuItem_Click);
            // 
            // Tool_PRMEtcEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 545);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.nPadding);
            this.Controls.Add(this.labelPadding);
            this.Controls.Add(this.nForwardAccelerationSpeed);
            this.Controls.Add(this.labelForwardAccelerationSpeed);
            this.Controls.Add(this.nForwardAccelerationEndFrame);
            this.Controls.Add(this.labelForwardAccelerationEndFrame);
            this.Controls.Add(this.nForwardAccelerationStartFrame);
            this.Controls.Add(this.labelForwardAccelerationStartFrame);
            this.Controls.Add(this.nCircularAccelerationSpeedMax);
            this.Controls.Add(this.labelCircularAccelerationSpeedMax);
            this.Controls.Add(this.nCircularAccelerationSpeedDropoff);
            this.Controls.Add(this.labelCircularAccelerationSpeedDropoff);
            this.Controls.Add(this.nCircularAccelerationSpeed);
            this.Controls.Add(this.labelCircularAccelerationSpeed);
            this.Controls.Add(this.nCircularAccelerationEndFrame);
            this.Controls.Add(this.labelCircularAccelerationEndFrame);
            this.Controls.Add(this.nCircularAccelerationStartFrame);
            this.Controls.Add(this.labelCircularAccelerationStartFrame);
            this.Controls.Add(this.nAirLength);
            this.Controls.Add(this.labelAirLength);
            this.Controls.Add(this.nCancelFrame);
            this.Controls.Add(this.labelCancelFrame);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Tool_PRMEtcEditor";
            this.Text = "PRM ETC editor";
            ((System.ComponentModel.ISupportInitialize)(this.nCancelFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAirLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationStartFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationEndFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeedDropoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircularAccelerationSpeedMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationStartFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationEndFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardAccelerationSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPadding)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label labelCancelFrame;
        private System.Windows.Forms.NumericUpDown nCancelFrame;
        private System.Windows.Forms.Label labelAirLength;
        private System.Windows.Forms.NumericUpDown nAirLength;
        private System.Windows.Forms.Label labelCircularAccelerationStartFrame;
        private System.Windows.Forms.NumericUpDown nCircularAccelerationStartFrame;
        private System.Windows.Forms.Label labelCircularAccelerationEndFrame;
        private System.Windows.Forms.NumericUpDown nCircularAccelerationEndFrame;
        private System.Windows.Forms.Label labelCircularAccelerationSpeed;
        private System.Windows.Forms.NumericUpDown nCircularAccelerationSpeed;
        private System.Windows.Forms.Label labelCircularAccelerationSpeedDropoff;
        private System.Windows.Forms.NumericUpDown nCircularAccelerationSpeedDropoff;
        private System.Windows.Forms.Label labelCircularAccelerationSpeedMax;
        private System.Windows.Forms.NumericUpDown nCircularAccelerationSpeedMax;
        private System.Windows.Forms.Label labelForwardAccelerationStartFrame;
        private System.Windows.Forms.NumericUpDown nForwardAccelerationStartFrame;
        private System.Windows.Forms.Label labelForwardAccelerationEndFrame;
        private System.Windows.Forms.NumericUpDown nForwardAccelerationEndFrame;
        private System.Windows.Forms.Label labelForwardAccelerationSpeed;
        private System.Windows.Forms.NumericUpDown nForwardAccelerationSpeed;
        private System.Windows.Forms.Label labelPadding;
        private System.Windows.Forms.NumericUpDown nPadding;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAndCloseToolStripMenuItem;
    }
}
