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
        public List<int> FrameActionUnlockValues = new List<int>();
        public List<int> ActionLengthValues = new List<int>();
        public List<int> Unk1Values = new List<int>();
        public List<float> CircleVelocityValues = new List<float>();
        public List<float> Unk2Values = new List<float>();
        public List<float> CircleVelocityStrengthValues = new List<float>();
        public List<int> MovementFrequencyValues = new List<int>();
        public List<float> ForwardVelocityValues = new List<float>();

        public Tool_PRMEtcEditor(
            Tool_MovesetCoder t,
            List<int> frameActionUnlockValues,
            List<int> actionLengthValues,
            List<int> unk1Values,
            List<float> circleVelocityValues,
            List<float> unk2Values,
            List<float> circleVelocityStrengthValues,
            List<int> movementFrequencyValues,
            List<float> forwardVelocityValues,
            int count)
        {
            InitializeComponent();
            tool = t;
            EntryCount = count;
            FrameActionUnlockValues = frameActionUnlockValues;
            ActionLengthValues = actionLengthValues;
            Unk1Values = unk1Values;
            CircleVelocityValues = circleVelocityValues;
            Unk2Values = unk2Values;
            CircleVelocityStrengthValues = circleVelocityStrengthValues;
            MovementFrequencyValues = movementFrequencyValues;
            ForwardVelocityValues = forwardVelocityValues;

            for (int i = 0; i < EntryCount; i++)
                listBox1.Items.Add(FormatEntry(i));
        }

        private string FormatEntry(int index)
        {
            string name = index < DefaultMovementNames.Length ? DefaultMovementNames[index] : ("Entry " + index.ToString());
            return name +
                   " | Frame Unlock: " + FrameActionUnlockValues[index].ToString("X4") +
                   " | Action Length: " + ActionLengthValues[index].ToString("X4") +
                   " | Forward Velocity: " + ForwardVelocityValues[index].ToString("0.###");
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
            SetNumericValue(nFrameActionUnlock, FrameActionUnlockValues[index]);
            SetNumericValue(nActionLength, ActionLengthValues[index]);
            SetNumericValue(nUnk1, Unk1Values[index]);
            SetNumericValue(nCircleVelocity, (decimal)CircleVelocityValues[index]);
            SetNumericValue(nUnk2, (decimal)Unk2Values[index]);
            SetNumericValue(nCircleVelocityStrength, (decimal)CircleVelocityStrengthValues[index]);
            SetNumericValue(nMovementFrequency, MovementFrequencyValues[index]);
            SetNumericValue(nForwardVelocity, (decimal)ForwardVelocityValues[index]);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FrameActionUnlockValues.Add((int)nFrameActionUnlock.Value);
            ActionLengthValues.Add((int)nActionLength.Value);
            Unk1Values.Add((int)nUnk1.Value);
            CircleVelocityValues.Add((float)nCircleVelocity.Value);
            Unk2Values.Add((float)nUnk2.Value);
            CircleVelocityStrengthValues.Add((float)nCircleVelocityStrength.Value);
            MovementFrequencyValues.Add((int)nMovementFrequency.Value);
            ForwardVelocityValues.Add((float)nForwardVelocity.Value);
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
            FrameActionUnlockValues[index] = (int)nFrameActionUnlock.Value;
            ActionLengthValues[index] = (int)nActionLength.Value;
            Unk1Values[index] = (int)nUnk1.Value;
            CircleVelocityValues[index] = (float)nCircleVelocity.Value;
            Unk2Values[index] = (float)nUnk2.Value;
            CircleVelocityStrengthValues[index] = (float)nCircleVelocityStrength.Value;
            MovementFrequencyValues[index] = (int)nMovementFrequency.Value;
            ForwardVelocityValues[index] = (float)nForwardVelocity.Value;
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
            FrameActionUnlockValues.RemoveAt(index);
            ActionLengthValues.RemoveAt(index);
            Unk1Values.RemoveAt(index);
            CircleVelocityValues.RemoveAt(index);
            Unk2Values.RemoveAt(index);
            CircleVelocityStrengthValues.RemoveAt(index);
            MovementFrequencyValues.RemoveAt(index);
            ForwardVelocityValues.RemoveAt(index);
            listBox1.Items.RemoveAt(index);
            EntryCount--;

            for (int i = index; i < listBox1.Items.Count; i++)
                listBox1.Items[i] = FormatEntry(i);

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = Math.Max(0, index - 1);
        }

        private void saveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tool.prmEtcFrameActionUnlockValue = FrameActionUnlockValues;
            tool.prmEtcActionLengthValue = ActionLengthValues;
            tool.prmEtcUnk1Value = Unk1Values;
            tool.prmEtcCircleVelocityValue = CircleVelocityValues;
            tool.prmEtcUnk2Value = Unk2Values;
            tool.prmEtcCircleVelocityStrengthValue = CircleVelocityStrengthValues;
            tool.prmEtcMovementFrequencyValue = MovementFrequencyValues;
            tool.prmEtcForwardVelocityValue = ForwardVelocityValues;
            tool.prmEtcSecCount = EntryCount;
            tool.prmEtcChanged = true;
            MessageBox.Show("PRM ETC data saved.");
        }
    }
}
