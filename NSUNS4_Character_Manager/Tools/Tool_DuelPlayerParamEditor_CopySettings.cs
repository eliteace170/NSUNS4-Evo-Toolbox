using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_DuelPlayerParamEditor_CopySettings : Form
    {
        public Tool_DuelPlayerParamEditor_CopySettings()
        {
            InitializeComponent();
        }

        public int SelectedModeIndex
        {
            get { return optionsList.SelectedIndex; }
        }
    }
}
