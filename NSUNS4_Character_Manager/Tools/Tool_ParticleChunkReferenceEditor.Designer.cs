using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    partial class Tool_ParticleChunkReferenceEditor
    {
        private IContainer components = null;
        private DataGridView referencesGrid;
        private Button addButton;
        private Button deleteButton;
        private Button okButton;
        private Button cancelButton;
        private Label hintLabel;
        private Button moveUpButton;
        private Button moveDownButton;
        private Label countLabel;
        private Label selectionLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.referencesGrid = new DataGridView();
            this.addButton = new Button();
            this.deleteButton = new Button();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.hintLabel = new Label();
            this.moveUpButton = new Button();
            this.moveDownButton = new Button();
            this.countLabel = new Label();
            this.selectionLabel = new Label();
            ((ISupportInitialize)(this.referencesGrid)).BeginInit();
            this.SuspendLayout();
            //
            // referencesGrid
            //
            this.referencesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.referencesGrid.Location = new Point(12, 66);
            this.referencesGrid.Size = new Size(860, 336);
            this.referencesGrid.RowHeadersVisible = false;
            this.referencesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //
            // buttons
            //
            this.addButton.Location = new Point(12, 410);
            this.addButton.Size = new Size(88, 24);
            this.addButton.Text = "Add Link";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            this.deleteButton.Location = new Point(106, 410);
            this.deleteButton.Size = new Size(96, 24);
            this.deleteButton.Text = "Remove Link";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            this.moveUpButton.Location = new Point(208, 410);
            this.moveUpButton.Size = new Size(76, 24);
            this.moveUpButton.Text = "Move Up";
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            this.moveDownButton.Location = new Point(290, 410);
            this.moveDownButton.Size = new Size(88, 24);
            this.moveDownButton.Text = "Move Down";
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.okButton.Location = new Point(716, 410);
            this.okButton.Size = new Size(75, 24);
            this.okButton.Text = "Apply";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.cancelButton.Location = new Point(797, 410);
            this.cancelButton.Size = new Size(75, 24);
            this.cancelButton.Text = "Close";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // hintLabel
            //
            this.hintLabel.AutoSize = true;
            this.hintLabel.Location = new Point(12, 13);
            this.hintLabel.Text = "Manage the named chunks this particle page can link to. Reordering changes the stored reference indexes used by particle settings and nodes.";
            //
            // countLabel
            //
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new Point(12, 44);
            this.countLabel.Text = "0 linked chunks";
            //
            // selectionLabel
            //
            this.selectionLabel.AutoSize = true;
            this.selectionLabel.Location = new Point(160, 44);
            this.selectionLabel.Text = "Selected: none";
            //
            // form
            //
            this.ClientSize = new Size(884, 446);
            this.Controls.Add(this.selectionLabel);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.referencesGrid);
            this.MinimumSize = new Size(900, 485);
            this.Name = "Tool_ParticleChunkReferenceEditor";
            this.Text = "Particle Linked Chunks";
            ((ISupportInitialize)(this.referencesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
