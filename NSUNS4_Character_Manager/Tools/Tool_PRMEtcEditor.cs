using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager.Tools
{
    public partial class Tool_PRMEtcEditor : Form
    {
        private static readonly string[] DefaultMovementNames = new string[]
        {
            "PL_ANM_DSH_FWD_L Base",
            "PL_ANM_DSH_FWD_R Base",
            "PL_ANM_DSH_BK Base",
            "PL_ANM_DSH_L0 Base",
            "PL_ANM_DSH_L1 Base",
            "PL_ANM_DSH_R0 Base",
            "PL_ANM_DSH_R1 Base",
            "PL_ANM_DSH_FWD_L InstantAwakening",
            "PL_ANM_DSH_FWD_R InstantAwakening",
            "PL_ANM_DSH_BK InstantAwakening",
            "PL_ANM_DSH_L0 InstantAwakening",
            "PL_ANM_DSH_L1 InstantAwakening",
            "PL_ANM_DSH_R0 InstantAwakening",
            "PL_ANM_DSH_R1 InstantAwakening",
            "PL_ANM_DSH_FWD_L TrueAwakening",
            "PL_ANM_DSH_FWD_R TrueAwakening",
            "PL_ANM_DSH_BK TrueAwakening",
            "PL_ANM_DSH_L0 TrueAwakening",
            "PL_ANM_DSH_L1 TrueAwakening",
            "PL_ANM_DSH_R0 TrueAwakening",
            "PL_ANM_DSH_R1 TrueAwakening"
        };

        public Tool_MovesetCoder tool;
        public int EntryCount;
        public List<int> CancelFrameValues = new List<int>();
        public List<int> AirLengthValues = new List<int>();
        public List<int> CircularAccelerationStartFrameValues = new List<int>();
        public List<int> CircularAccelerationEndFrameValues = new List<int>();
        public List<float> CircularAccelerationSpeedValues = new List<float>();
        public List<float> CircularAccelerationSpeedDropoffValues = new List<float>();
        public List<float> CircularAccelerationSpeedMaxValues = new List<float>();
        public List<int> ForwardAccelerationStartFrameValues = new List<int>();
        public List<int> ForwardAccelerationEndFrameValues = new List<int>();
        public List<float> ForwardAccelerationSpeedValues = new List<float>();
        public List<int> PaddingValues = new List<int>();

        public Tool_PRMEtcEditor(
            Tool_MovesetCoder t,
            List<int> cancelFrameValues,
            List<int> airLengthValues,
            List<int> circularAccelerationStartFrameValues,
            List<int> circularAccelerationEndFrameValues,
            List<float> circularAccelerationSpeedValues,
            List<float> circularAccelerationSpeedDropoffValues,
            List<float> circularAccelerationSpeedMaxValues,
            List<int> forwardAccelerationStartFrameValues,
            List<int> forwardAccelerationEndFrameValues,
            List<float> forwardAccelerationSpeedValues,
            List<int> paddingValues,
            int count)
        {
            InitializeComponent();
            tool = t;
            EntryCount = count;
            CancelFrameValues = cancelFrameValues;
            AirLengthValues = airLengthValues;
            CircularAccelerationStartFrameValues = circularAccelerationStartFrameValues;
            CircularAccelerationEndFrameValues = circularAccelerationEndFrameValues;
            CircularAccelerationSpeedValues = circularAccelerationSpeedValues;
            CircularAccelerationSpeedDropoffValues = circularAccelerationSpeedDropoffValues;
            CircularAccelerationSpeedMaxValues = circularAccelerationSpeedMaxValues;
            ForwardAccelerationStartFrameValues = forwardAccelerationStartFrameValues;
            ForwardAccelerationEndFrameValues = forwardAccelerationEndFrameValues;
            ForwardAccelerationSpeedValues = forwardAccelerationSpeedValues;
            PaddingValues = paddingValues;

            for (int i = 0; i < EntryCount; i++)
                listBox1.Items.Add(FormatEntry(i));
        }

        private string FormatEntry(int index)
        {
            string name = index < DefaultMovementNames.Length ? DefaultMovementNames[index] : ("Entry " + index.ToString());
            return name +
                   " | Cancel Frame: " + CancelFrameValues[index].ToString() +
                   " | Air Length: " + AirLengthValues[index].ToString() +
                   " | Forward Acceleration Speed: " + ForwardAccelerationSpeedValues[index].ToString("0.###");
        }

        private static void SetNumericValue(NumericUpDown control, decimal value)
        {
            if (value < control.Minimum) value = control.Minimum;
            if (value > control.Maximum) value = control.Maximum;
            control.Value = value;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            int index = listBox1.SelectedIndex;
            SetNumericValue(nCancelFrame, CancelFrameValues[index]);
            SetNumericValue(nAirLength, AirLengthValues[index]);
            SetNumericValue(nCircularAccelerationStartFrame, CircularAccelerationStartFrameValues[index]);
            SetNumericValue(nCircularAccelerationEndFrame, CircularAccelerationEndFrameValues[index]);
            SetNumericValue(nCircularAccelerationSpeed, (decimal)CircularAccelerationSpeedValues[index]);
            SetNumericValue(nCircularAccelerationSpeedDropoff, (decimal)CircularAccelerationSpeedDropoffValues[index]);
            SetNumericValue(nCircularAccelerationSpeedMax, (decimal)CircularAccelerationSpeedMaxValues[index]);
            SetNumericValue(nForwardAccelerationStartFrame, ForwardAccelerationStartFrameValues[index]);
            SetNumericValue(nForwardAccelerationEndFrame, ForwardAccelerationEndFrameValues[index]);
            SetNumericValue(nForwardAccelerationSpeed, (decimal)ForwardAccelerationSpeedValues[index]);
            SetNumericValue(nPadding, PaddingValues[index]);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            CancelFrameValues.Add((int)nCancelFrame.Value);
            AirLengthValues.Add((int)nAirLength.Value);
            CircularAccelerationStartFrameValues.Add((int)nCircularAccelerationStartFrame.Value);
            CircularAccelerationEndFrameValues.Add((int)nCircularAccelerationEndFrame.Value);
            CircularAccelerationSpeedValues.Add((float)nCircularAccelerationSpeed.Value);
            CircularAccelerationSpeedDropoffValues.Add((float)nCircularAccelerationSpeedDropoff.Value);
            CircularAccelerationSpeedMaxValues.Add((float)nCircularAccelerationSpeedMax.Value);
            ForwardAccelerationStartFrameValues.Add((int)nForwardAccelerationStartFrame.Value);
            ForwardAccelerationEndFrameValues.Add((int)nForwardAccelerationEndFrame.Value);
            ForwardAccelerationSpeedValues.Add((float)nForwardAccelerationSpeed.Value);
            PaddingValues.Add((int)nPadding.Value);
            EntryCount++;
            listBox1.Items.Add(FormatEntry(EntryCount - 1));
            listBox1.SelectedIndex = EntryCount - 1;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select entry");
                return;
            }

            int index = listBox1.SelectedIndex;
            CancelFrameValues[index] = (int)nCancelFrame.Value;
            AirLengthValues[index] = (int)nAirLength.Value;
            CircularAccelerationStartFrameValues[index] = (int)nCircularAccelerationStartFrame.Value;
            CircularAccelerationEndFrameValues[index] = (int)nCircularAccelerationEndFrame.Value;
            CircularAccelerationSpeedValues[index] = (float)nCircularAccelerationSpeed.Value;
            CircularAccelerationSpeedDropoffValues[index] = (float)nCircularAccelerationSpeedDropoff.Value;
            CircularAccelerationSpeedMaxValues[index] = (float)nCircularAccelerationSpeedMax.Value;
            ForwardAccelerationStartFrameValues[index] = (int)nForwardAccelerationStartFrame.Value;
            ForwardAccelerationEndFrameValues[index] = (int)nForwardAccelerationEndFrame.Value;
            ForwardAccelerationSpeedValues[index] = (float)nForwardAccelerationSpeed.Value;
            PaddingValues[index] = (int)nPadding.Value;
            listBox1.Items[index] = FormatEntry(index);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select entry");
                return;
            }

            int index = listBox1.SelectedIndex;
            CancelFrameValues.RemoveAt(index);
            AirLengthValues.RemoveAt(index);
            CircularAccelerationStartFrameValues.RemoveAt(index);
            CircularAccelerationEndFrameValues.RemoveAt(index);
            CircularAccelerationSpeedValues.RemoveAt(index);
            CircularAccelerationSpeedDropoffValues.RemoveAt(index);
            CircularAccelerationSpeedMaxValues.RemoveAt(index);
            ForwardAccelerationStartFrameValues.RemoveAt(index);
            ForwardAccelerationEndFrameValues.RemoveAt(index);
            ForwardAccelerationSpeedValues.RemoveAt(index);
            PaddingValues.RemoveAt(index);
            listBox1.Items.RemoveAt(index);
            EntryCount--;

            for (int i = index; i < listBox1.Items.Count; i++)
                listBox1.Items[i] = FormatEntry(i);

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = Math.Max(0, index - 1);
        }

        private void saveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tool.prmEtcCancelFrameValues = CancelFrameValues;
            tool.prmEtcAirLengthValues = AirLengthValues;
            tool.prmEtcCircularAccelerationStartFrameValues = CircularAccelerationStartFrameValues;
            tool.prmEtcCircularAccelerationEndFrameValues = CircularAccelerationEndFrameValues;
            tool.prmEtcCircularAccelerationSpeedValues = CircularAccelerationSpeedValues;
            tool.prmEtcCircularAccelerationSpeedDropoffValues = CircularAccelerationSpeedDropoffValues;
            tool.prmEtcCircularAccelerationSpeedMaxValues = CircularAccelerationSpeedMaxValues;
            tool.prmEtcForwardAccelerationStartFrameValues = ForwardAccelerationStartFrameValues;
            tool.prmEtcForwardAccelerationEndFrameValues = ForwardAccelerationEndFrameValues;
            tool.prmEtcForwardAccelerationSpeedValues = ForwardAccelerationSpeedValues;
            tool.prmEtcPaddingValues = PaddingValues;
            tool.prmEtcSecCount = EntryCount;
            tool.prmEtcChanged = true;
            MessageBox.Show("PRM ETC data saved.");
        }
    }
}
