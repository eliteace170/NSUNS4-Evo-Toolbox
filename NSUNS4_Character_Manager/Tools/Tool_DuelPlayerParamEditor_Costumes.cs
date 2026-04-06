using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
	public class Tool_DuelPlayerParamEditor_Costumes : Form
	{
		public string[] baseList;
		public string[] awakeList;
		public int str_index;
		public Tool_DuelPlayerParamEditor tool;

		private IContainer components = null;
		private ListBox baseListBox;
		private ListBox awakeListBox;
		private Label baseLabel;
		private Label awakeLabel;
		private Label baseEditLabel;
		private Label awakeEditLabel;
		private TextBox baseTextBox;
		private TextBox awakeTextBox;
		private Button baseSubmitButton;
		private Button awakeSubmitButton;
		private Button applyButton;

		public Tool_DuelPlayerParamEditor_Costumes(string[] baseCostumes, string[] awakeCostumes, Tool_DuelPlayerParamEditor t, int index)
		{
			InitializeComponent();
			baseList = baseCostumes ?? new string[20];
			awakeList = awakeCostumes ?? new string[20];
			tool = t;
			str_index = index;
			ReloadLists();
		}

		private void ReloadLists()
		{
			baseListBox.Items.Clear();
			awakeListBox.Items.Clear();
			for (int i = 0; i < 20; i++)
			{
				baseListBox.Items.Add(i.ToString() + " - " + (string.IsNullOrEmpty(baseList[i]) ? "[null]" : baseList[i]));
				awakeListBox.Items.Add(i.ToString() + " - " + (string.IsNullOrEmpty(awakeList[i]) ? "[null]" : awakeList[i]));
			}
		}

		private void baseListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = baseListBox.SelectedIndex;
			baseTextBox.Text = index >= 0 ? baseList[index] : "";
		}

		private void awakeListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = awakeListBox.SelectedIndex;
			awakeTextBox.Text = index >= 0 ? awakeList[index] : "";
		}

		private void baseSubmitButton_Click(object sender, EventArgs e)
		{
			int index = baseListBox.SelectedIndex;
			if (index == -1)
			{
				baseTextBox.Text = "";
				return;
			}

			baseList[index] = string.IsNullOrWhiteSpace(baseTextBox.Text) ? "" : baseTextBox.Text;
			baseListBox.Items[index] = index.ToString() + " - " + (baseList[index] == "" ? "[null]" : baseList[index]);
		}

		private void awakeSubmitButton_Click(object sender, EventArgs e)
		{
			int index = awakeListBox.SelectedIndex;
			if (index == -1)
			{
				awakeTextBox.Text = "";
				return;
			}

			awakeList[index] = string.IsNullOrWhiteSpace(awakeTextBox.Text) ? "" : awakeTextBox.Text;
			awakeListBox.Items[index] = index.ToString() + " - " + (awakeList[index] == "" ? "[null]" : awakeList[index]);
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			tool.UpdateCostumeEntry(str_index, baseList, false);
			tool.UpdateCostumeEntry(str_index, awakeList, true);
			MessageBox.Show("Costume lists saved correctly.");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.baseListBox = new System.Windows.Forms.ListBox();
			this.awakeListBox = new System.Windows.Forms.ListBox();
			this.baseLabel = new System.Windows.Forms.Label();
			this.awakeLabel = new System.Windows.Forms.Label();
			this.baseEditLabel = new System.Windows.Forms.Label();
			this.awakeEditLabel = new System.Windows.Forms.Label();
			this.baseTextBox = new System.Windows.Forms.TextBox();
			this.awakeTextBox = new System.Windows.Forms.TextBox();
			this.baseSubmitButton = new System.Windows.Forms.Button();
			this.awakeSubmitButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// baseListBox
			// 
			this.baseListBox.FormattingEnabled = true;
			this.baseListBox.Location = new System.Drawing.Point(12, 31);
			this.baseListBox.Name = "baseListBox";
			this.baseListBox.Size = new System.Drawing.Size(300, 329);
			this.baseListBox.TabIndex = 0;
			this.baseListBox.SelectedIndexChanged += new System.EventHandler(this.baseListBox_SelectedIndexChanged);
			// 
			// awakeListBox
			// 
			this.awakeListBox.FormattingEnabled = true;
			this.awakeListBox.Location = new System.Drawing.Point(326, 31);
			this.awakeListBox.Name = "awakeListBox";
			this.awakeListBox.Size = new System.Drawing.Size(300, 329);
			this.awakeListBox.TabIndex = 1;
			this.awakeListBox.SelectedIndexChanged += new System.EventHandler(this.awakeListBox_SelectedIndexChanged);
			// 
			// baseLabel
			// 
			this.baseLabel.AutoSize = true;
			this.baseLabel.Location = new System.Drawing.Point(12, 10);
			this.baseLabel.Name = "baseLabel";
			this.baseLabel.Size = new System.Drawing.Size(79, 15);
			this.baseLabel.TabIndex = 2;
			this.baseLabel.Text = "Base costumes";
			// 
			// awakeLabel
			// 
			this.awakeLabel.AutoSize = true;
			this.awakeLabel.Location = new System.Drawing.Point(323, 10);
			this.awakeLabel.Name = "awakeLabel";
			this.awakeLabel.Size = new System.Drawing.Size(93, 15);
			this.awakeLabel.TabIndex = 3;
			this.awakeLabel.Text = "Awake costumes";
			// 
			// baseEditLabel
			// 
			this.baseEditLabel.AutoSize = true;
			this.baseEditLabel.Location = new System.Drawing.Point(12, 372);
			this.baseEditLabel.Name = "baseEditLabel";
			this.baseEditLabel.Size = new System.Drawing.Size(82, 15);
			this.baseEditLabel.TabIndex = 4;
			this.baseEditLabel.Text = "Edit base entry";
			// 
			// awakeEditLabel
			// 
			this.awakeEditLabel.AutoSize = true;
			this.awakeEditLabel.Location = new System.Drawing.Point(323, 372);
			this.awakeEditLabel.Name = "awakeEditLabel";
			this.awakeEditLabel.Size = new System.Drawing.Size(96, 15);
			this.awakeEditLabel.TabIndex = 5;
			this.awakeEditLabel.Text = "Edit awake entry";
			// 
			// baseTextBox
			// 
			this.baseTextBox.Location = new System.Drawing.Point(12, 389);
			this.baseTextBox.MaxLength = 8;
			this.baseTextBox.Name = "baseTextBox";
			this.baseTextBox.Size = new System.Drawing.Size(219, 23);
			this.baseTextBox.TabIndex = 6;
			// 
			// awakeTextBox
			// 
			this.awakeTextBox.Location = new System.Drawing.Point(326, 389);
			this.awakeTextBox.MaxLength = 8;
			this.awakeTextBox.Name = "awakeTextBox";
			this.awakeTextBox.Size = new System.Drawing.Size(219, 23);
			this.awakeTextBox.TabIndex = 7;
			// 
			// baseSubmitButton
			// 
			this.baseSubmitButton.Location = new System.Drawing.Point(237, 389);
			this.baseSubmitButton.Name = "baseSubmitButton";
			this.baseSubmitButton.Size = new System.Drawing.Size(75, 23);
			this.baseSubmitButton.TabIndex = 8;
			this.baseSubmitButton.Text = "Submit";
			this.baseSubmitButton.UseVisualStyleBackColor = true;
			this.baseSubmitButton.Click += new System.EventHandler(this.baseSubmitButton_Click);
			// 
			// awakeSubmitButton
			// 
			this.awakeSubmitButton.Location = new System.Drawing.Point(551, 389);
			this.awakeSubmitButton.Name = "awakeSubmitButton";
			this.awakeSubmitButton.Size = new System.Drawing.Size(75, 23);
			this.awakeSubmitButton.TabIndex = 9;
			this.awakeSubmitButton.Text = "Submit";
			this.awakeSubmitButton.UseVisualStyleBackColor = true;
			this.awakeSubmitButton.Click += new System.EventHandler(this.awakeSubmitButton_Click);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(12, 423);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(614, 28);
			this.applyButton.TabIndex = 10;
			this.applyButton.Text = "Apply changes";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// Tool_DuelPlayerParamEditor_Costumes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 463);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.awakeSubmitButton);
			this.Controls.Add(this.baseSubmitButton);
			this.Controls.Add(this.awakeTextBox);
			this.Controls.Add(this.baseTextBox);
			this.Controls.Add(this.awakeEditLabel);
			this.Controls.Add(this.baseEditLabel);
			this.Controls.Add(this.awakeLabel);
			this.Controls.Add(this.baseLabel);
			this.Controls.Add(this.awakeListBox);
			this.Controls.Add(this.baseListBox);
			this.Font = new System.Drawing.Font("Segoe UI", 8.5F);
			this.MaximizeBox = false;
			this.Name = "Tool_DuelPlayerParamEditor_Costumes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit costume lists";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
