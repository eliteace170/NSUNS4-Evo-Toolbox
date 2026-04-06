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
            this.label1 = new System.Windows.Forms.Label();
            this.nFrameActionUnlock = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nActionLength = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nUnk1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nCircleVelocity = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nUnk2 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nCircleVelocityStrength = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nMovementFrequency = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nForwardVelocity = new System.Windows.Forms.NumericUpDown();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAndCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.nFrameActionUnlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nActionLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUnk1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircleVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUnk2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircleVelocityStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMovementFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardVelocity)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(12, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(527, 355);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(542, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Frame Action Unlock";
            // 
            // nFrameActionUnlock
            // 
            this.nFrameActionUnlock.Hexadecimal = true;
            this.nFrameActionUnlock.Location = new System.Drawing.Point(545, 43);
            this.nFrameActionUnlock.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nFrameActionUnlock.Name = "nFrameActionUnlock";
            this.nFrameActionUnlock.Size = new System.Drawing.Size(250, 20);
            this.nFrameActionUnlock.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(542, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action Length";
            // 
            // nActionLength
            // 
            this.nActionLength.Hexadecimal = true;
            this.nActionLength.Location = new System.Drawing.Point(545, 88);
            this.nActionLength.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nActionLength.Name = "nActionLength";
            this.nActionLength.Size = new System.Drawing.Size(250, 20);
            this.nActionLength.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(542, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Unk 1";
            // 
            // nUnk1
            // 
            this.nUnk1.Hexadecimal = true;
            this.nUnk1.Location = new System.Drawing.Point(545, 133);
            this.nUnk1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nUnk1.Name = "nUnk1";
            this.nUnk1.Size = new System.Drawing.Size(250, 20);
            this.nUnk1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(542, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Circle Velocity";
            // 
            // nCircleVelocity
            // 
            this.nCircleVelocity.DecimalPlaces = 4;
            this.nCircleVelocity.Location = new System.Drawing.Point(545, 178);
            this.nCircleVelocity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nCircleVelocity.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nCircleVelocity.Name = "nCircleVelocity";
            this.nCircleVelocity.Size = new System.Drawing.Size(250, 20);
            this.nCircleVelocity.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(542, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Unk 2";
            // 
            // nUnk2
            // 
            this.nUnk2.DecimalPlaces = 4;
            this.nUnk2.Location = new System.Drawing.Point(545, 223);
            this.nUnk2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nUnk2.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nUnk2.Name = "nUnk2";
            this.nUnk2.Size = new System.Drawing.Size(250, 20);
            this.nUnk2.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(542, 252);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Circle Velocity Strength";
            // 
            // nCircleVelocityStrength
            // 
            this.nCircleVelocityStrength.DecimalPlaces = 4;
            this.nCircleVelocityStrength.Location = new System.Drawing.Point(545, 268);
            this.nCircleVelocityStrength.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nCircleVelocityStrength.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nCircleVelocityStrength.Name = "nCircleVelocityStrength";
            this.nCircleVelocityStrength.Size = new System.Drawing.Size(250, 20);
            this.nCircleVelocityStrength.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(542, 297);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Movement Frequency";
            // 
            // nMovementFrequency
            // 
            this.nMovementFrequency.Hexadecimal = true;
            this.nMovementFrequency.Location = new System.Drawing.Point(545, 313);
            this.nMovementFrequency.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nMovementFrequency.Name = "nMovementFrequency";
            this.nMovementFrequency.Size = new System.Drawing.Size(250, 20);
            this.nMovementFrequency.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(542, 342);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Forward Velocity";
            // 
            // nForwardVelocity
            // 
            this.nForwardVelocity.DecimalPlaces = 4;
            this.nForwardVelocity.Location = new System.Drawing.Point(545, 358);
            this.nForwardVelocity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nForwardVelocity.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nForwardVelocity.Name = "nForwardVelocity";
            this.nForwardVelocity.Size = new System.Drawing.Size(250, 20);
            this.nForwardVelocity.TabIndex = 16;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(545, 395);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(80, 35);
            this.buttonAdd.TabIndex = 17;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(631, 395);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(80, 35);
            this.buttonDelete.TabIndex = 18;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(717, 395);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(78, 35);
            this.buttonSave.TabIndex = 19;
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
            this.menuStrip1.Size = new System.Drawing.Size(798, 24);
            this.menuStrip1.TabIndex = 20;
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
            this.ClientSize = new System.Drawing.Size(798, 447);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.nForwardVelocity);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nMovementFrequency);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nCircleVelocityStrength);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nUnk2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nCircleVelocity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nUnk1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nActionLength);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nFrameActionUnlock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Tool_PRMEtcEditor";
            this.Text = "PRM ETC editor";
            ((System.ComponentModel.ISupportInitialize)(this.nFrameActionUnlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nActionLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUnk1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircleVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUnk2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCircleVelocityStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMovementFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nForwardVelocity)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nFrameActionUnlock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nActionLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nUnk1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nCircleVelocity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nUnk2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nCircleVelocityStrength;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nMovementFrequency;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nForwardVelocity;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAndCloseToolStripMenuItem;
    }
}
