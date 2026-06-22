using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_CommandListCloneDialog : Form
    {
        public string SourceCharacode
        {
            get { return (sourceCharacodeComboBox.Text ?? string.Empty).Trim(); }
        }

        public string TargetCharacode
        {
            get { return (targetCharacodeComboBox.Text ?? string.Empty).Trim(); }
        }

        public Tool_CommandListCloneDialog()
        {
            InitializeComponent();
        }

        public Tool_CommandListCloneDialog(IEnumerable<string> characodes)
            : this()
        {
            foreach (string characode in characodes)
            {
                sourceCharacodeComboBox.Items.Add(characode);
                targetCharacodeComboBox.Items.Add(characode);
            }
        }

        private void cloneButton_Click(object sender, EventArgs e)
        {
            if (SourceCharacode.Length == 0 || TargetCharacode.Length == 0)
            {
                MessageBox.Show(this, "Select both the source and clone Characodes.", "Clone Character Entries",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.Equals(SourceCharacode, TargetCharacode, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Source and clone Characodes must be different.", "Clone Character Entries",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
