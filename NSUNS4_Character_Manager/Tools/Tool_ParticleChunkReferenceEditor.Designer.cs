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
            ((ISupportInitialize)(this.referencesGrid)).BeginInit();
            this.SuspendLayout();
            //
            // referencesGrid
            //
            this.referencesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.referencesGrid.Location = new Point(12, 36);
            this.referencesGrid.Size = new Size(860, 366);
            this.referencesGrid.RowHeadersVisible = false;
            this.referencesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //
            // buttons
            //
            this.addButton.Location = new Point(12, 410);
            this.addButton.Size = new Size(75, 24);
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            this.deleteButton.Location = new Point(93, 410);
            this.deleteButton.Size = new Size(75, 24);
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.okButton.Location = new Point(716, 410);
            this.okButton.Size = new Size(75, 24);
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.cancelButton.Location = new Point(797, 410);
            this.cancelButton.Size = new Size(75, 24);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // hintLabel
            //
            this.hintLabel.AutoSize = true;
            this.hintLabel.Location = new Point(12, 13);
            this.hintLabel.Text = "This window lists every linked chunk the particle page can reference. Add, remove, or reorder entries here, then switch them by name from the main editor.";
            //
            // form
            //
            this.ClientSize = new Size(884, 446);
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
