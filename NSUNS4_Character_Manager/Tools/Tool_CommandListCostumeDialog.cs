using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_CommandListCostumeDialog : Form
    {
        public string Characode
        {
            get { return (characodeComboBox.Text ?? string.Empty).Trim(); }
        }

        public int TotalCostumes
        {
            get { return (int)totalCostumesNumericUpDown.Value; }
        }

        public Tool_CommandListCostumeDialog()
        {
            InitializeComponent();
        }

        public Tool_CommandListCostumeDialog(IEnumerable<string> characodes)
            : this()
        {
            foreach (string characode in characodes)
                characodeComboBox.Items.Add(characode);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (Characode.Length == 0)
            {
                MessageBox.Show(this, "Select a Characode.", "Add Costume Entries",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
