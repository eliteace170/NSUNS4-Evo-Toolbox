using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NSUNS4_Character_Manager
{
    public partial class Tool_SpcloadEditor : Form
    {
        public Tool_SpcloadEditor()
        {
            InitializeComponent();
        }

        string fileName = "";
        string prmName = "";
        bool fileOpen = false;
        byte[] fileBytes = new byte[0];
        public int entryCount = 0;
        public List<string> pathList = new List<string>();
        public List<string> nameList = new List<string>();
        public List<int> typeList = new List<int>();
        public List<int> costumeIndexList = new List<int>();
        public List<int> loadcondList = new List<int>();

        private const int LOAD_COMMON = 0x0001;
        private const int LOAD_SUPPORT = 0x0002;
        private const int LOAD_AWAKENING = 0x0004;
        private const int LOAD_JUTSU1 = 0x0008;
        private const int LOAD_JUTSU2 = 0x0010;
        private const int LOAD_JUTSU3 = 0x0020;
        private const int LOAD_JUTSU4 = 0x0040;
        private const int LOAD_JUTSU5 = 0x0080;
        private const int LOAD_ULTIMATE1 = 0x0100;
        private const int LOAD_ULTIMATE2 = 0x0200;
        private const int LOAD_ULTIMATE3 = 0x0400;
        private const int LOAD_ULTIMATE4 = 0x0800;
        private const int LOAD_JUTSU6 = 0x2000;
        private const string ClipboardPrefix = "PRMLOAD_ENTRY_V1";
        private bool syncingConditionControls = false;

        private void UpdateConditionFlagsFromValue(int condition)
        {
            syncingConditionControls = true;
            checkConditionCommon.Checked = (condition & LOAD_COMMON) != 0;
            checkConditionSupport.Checked = (condition & LOAD_SUPPORT) != 0;
            checkConditionAwakening.Checked = (condition & LOAD_AWAKENING) != 0;
            checkConditionJutsu1.Checked = (condition & LOAD_JUTSU1) != 0;
            checkConditionJutsu2.Checked = (condition & LOAD_JUTSU2) != 0;
            checkConditionJutsu3.Checked = (condition & LOAD_JUTSU3) != 0;
            checkConditionJutsu4.Checked = (condition & LOAD_JUTSU4) != 0;
            checkConditionJutsu5.Checked = (condition & LOAD_JUTSU5) != 0;
            checkConditionJutsu6.Checked = (condition & LOAD_JUTSU6) != 0;
            checkConditionUltimate1.Checked = (condition & LOAD_ULTIMATE1) != 0;
            checkConditionUltimate2.Checked = (condition & LOAD_ULTIMATE2) != 0;
            checkConditionUltimate3.Checked = (condition & LOAD_ULTIMATE3) != 0;
            checkConditionUltimate4.Checked = (condition & LOAD_ULTIMATE4) != 0;
            syncingConditionControls = false;
        }

        private int BuildConditionValueFromFlags()
        {
            int condition = 0;
            if (checkConditionCommon.Checked) condition |= LOAD_COMMON;
            if (checkConditionSupport.Checked) condition |= LOAD_SUPPORT;
            if (checkConditionAwakening.Checked) condition |= LOAD_AWAKENING;
            if (checkConditionJutsu1.Checked) condition |= LOAD_JUTSU1;
            if (checkConditionJutsu2.Checked) condition |= LOAD_JUTSU2;
            if (checkConditionJutsu3.Checked) condition |= LOAD_JUTSU3;
            if (checkConditionJutsu4.Checked) condition |= LOAD_JUTSU4;
            if (checkConditionJutsu5.Checked) condition |= LOAD_JUTSU5;
            if (checkConditionJutsu6.Checked) condition |= LOAD_JUTSU6;
            if (checkConditionUltimate1.Checked) condition |= LOAD_ULTIMATE1;
            if (checkConditionUltimate2.Checked) condition |= LOAD_ULTIMATE2;
            if (checkConditionUltimate3.Checked) condition |= LOAD_ULTIMATE3;
            if (checkConditionUltimate4.Checked) condition |= LOAD_ULTIMATE4;
            return condition;
        }

        private void RefreshConditionValueFromFlags()
        {
            if (syncingConditionControls)
                return;
            syncingConditionControls = true;
            numericUpDown1.Value = BuildConditionValueFromFlags();
            syncingConditionControls = false;
        }

        public void OpenFile(string basepath = "")
        {
            OpenFileDialog o = new OpenFileDialog();
            if (basepath != "") {
                o.FileName = basepath;
            } else {
                o.ShowDialog();
            }

            if (o.FileName == "" || File.Exists(o.FileName) == false) return;
            fileName = o.FileName;

            fileBytes = File.ReadAllBytes(fileName);
            int fileSectionIndex = XfbinParser.GetFileSectionIndex(fileBytes);
            int startIndex = fileSectionIndex + 0x1C;
            int fileIndex = startIndex;

            // Check for NUCC in header
            if (!(fileBytes.Length > 0x44 && Main.b_ReadString(fileBytes, 0, 4) == "NUCC"))
            {
                MessageBox.Show("Not a valid .xfbin file.");
                return;
            }

            // Get character name
            prmName = XfbinParser.GetNameList(fileBytes)[0];
            prmName = prmName.Substring(0, prmName.Length - 0x8);
            textBox3.Text = prmName;

            // Get entry count
            entryCount = Main.b_ReadInt(fileBytes, fileSectionIndex + 0x1C);

            for(int x = 0; x < entryCount; x++)
            {
                fileIndex = startIndex + 0x4 + (0x50 * x);

                int strIndex = fileIndex + 0x4;
                string path = Main.b_ReadString(fileBytes, strIndex);
                pathList.Add(path);

                strIndex = strIndex + 0x20;
                string name = Main.b_ReadString(fileBytes, strIndex);
                nameList.Add(name);

                listBox1.Items.Add(path + "/" + name);

                strIndex = strIndex + 0x20;
                typeList.Add(Main.b_ReadInt(fileBytes, strIndex));
                costumeIndexList.Add(Main.b_ReadInt(fileBytes, strIndex + 0x4));
                loadcondList.Add(Main.b_ReadInt(fileBytes, strIndex + 0x8));
            }

            fileOpen = true;
        }

        void CloseFile()
        {
            fileName = "";
            prmName = "";
            fileOpen = false;
            fileBytes = new byte[0];
            entryCount = 0;
            pathList.Clear();
            nameList.Clear();
            typeList.Clear();
            costumeIndexList.Clear();
            loadcondList.Clear();

            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            textBox3.Clear();
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = -1;
            UpdateConditionFlagsFromValue(0);
        }

        void SaveFile(string oldname = "")
        {
            if (fileOpen == false) return;

            if (oldname == "")
            {
                oldname = fileName;
                // Do backup
                if (File.Exists(oldname + ".bak")) File.Delete(oldname + ".bak");
                File.Copy(oldname, oldname + ".bak");
            }

            // Create new file
            List<byte> newFile = new List<byte>();

            // Copy old header
            int pathSectionIndex = XfbinParser.GetPathSectionIndex(fileBytes) + 1;
            for(int x = 0; x < pathSectionIndex; x++) newFile.Add(fileBytes[x]);

            byte[] actualFile = newFile.ToArray();
            int totalSize = actualFile.Length;

            // Create xfbin path and xfbin name strings
            string xfbinPathString = "Z:/param/player/converter/bin/" + prmName + "/" + prmName + "prm_load.bin";
            string xfbinNameString = prmName + "prm_load";

            // Add path section of xfbin
            int newPathSectionSize = xfbinPathString.Length + 1;
            actualFile = Main.b_AddString(actualFile, xfbinPathString);
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

            totalSize = totalSize + newPathSectionSize;

            // Add name section of xfbin
            int newNameSectionSize = 1 + xfbinNameString.Length + 1 + "Page0".Length + 1 + "Index".Length + 1;

            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });
            actualFile = Main.b_AddString(actualFile, xfbinNameString);
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

            actualFile = Main.b_AddString(actualFile, "Page0");
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

            actualFile = Main.b_AddString(actualFile, "index");
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

            totalSize = totalSize + newNameSectionSize;

            // Add extra bytes to have a size divisible by 4
            while (totalSize % 4 != 0)
            {
                actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });
                totalSize++;
            }

            // Add binary 1 section of xfbin
            int newBin1SectionSize = 0x30;
            int newBin1SectionIndex = totalSize;
            for(int x = 0; x < 0x30; x++) actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });
            actualFile[totalSize + 0xF] = 0x1;
            actualFile[totalSize + 0x13] = 0x1;
            actualFile[totalSize + 0x17] = 0x1;
            actualFile[totalSize + 0x1B] = 0x2;
            actualFile[totalSize + 0x23] = 0x2;
            actualFile[totalSize + 0x27] = 0x3;
            actualFile[totalSize + 0x2F] = 0x3;
            totalSize = totalSize + newBin1SectionSize;

            // Add binary 2 section of xfbin
            int newBin2SectionSize = 0x10;
            int newBin2SectionIndex = totalSize;
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03 });
            totalSize = totalSize + newBin2SectionSize;

            // Add file header
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x63, 0x00, 0x00, 0x00, 0x00, 0x08, 0x78,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x63, 0x00, 0x00, 0x00, 0x00, 0x08, 0x74 });
            totalSize = totalSize + 0x1C;

            int prmLoadIndex = totalSize;
            actualFile = Main.b_AddBytes(actualFile, new byte[] { (byte)pathList.Count });
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0x00, 0x00, 0x00});

            for (int x = 0; x < pathList.Count; x++)
            {
                // Add path
                actualFile = Main.b_AddBytes(actualFile, new byte[] { 0x3F, 0x00, 0x00, 0x00 });
                actualFile = Main.b_AddString(actualFile, pathList[x]);
                for (int y = 0; y < 0x20 - pathList[x].Length; y++) actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

                // Add name
                actualFile = Main.b_AddString(actualFile, nameList[x]);
                for (int y = 0; y < 0x20 - nameList[x].Length; y++) actualFile = Main.b_AddBytes(actualFile, new byte[] { 0 });

                // Add type and loading state
                actualFile = Main.b_AddBytes(actualFile, BitConverter.GetBytes(typeList[x]));
                actualFile = Main.b_AddBytes(actualFile, BitConverter.GetBytes(costumeIndexList[x]));
                actualFile = Main.b_AddBytes(actualFile, BitConverter.GetBytes(loadcondList[x]));
            }

            // Add EOF
            actualFile = Main.b_AddBytes(actualFile, new byte[] { 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 });

            // Fix sizes
            actualFile = Main.b_ReplaceBytes(actualFile, BitConverter.GetBytes(newPathSectionSize + 1), 0x28, 1);
            actualFile = Main.b_ReplaceBytes(actualFile, BitConverter.GetBytes(newNameSectionSize), 0x30, 1);
            actualFile = Main.b_ReplaceBytes(actualFile, BitConverter.GetBytes(4 + (pathList.Count * 0x50)), prmLoadIndex - 0x4, 1);
            actualFile = Main.b_ReplaceBytes(actualFile, BitConverter.GetBytes(8 + (pathList.Count * 0x50)), prmLoadIndex - 0x4 - 0xC, 1);
            // Save file
            File.WriteAllBytes(fileName, actualFile);
            MessageBox.Show("File saved.");
        }

        void SaveFileAs()
        {
            SaveFileDialog s = new SaveFileDialog();
            s.ShowDialog();

            if (s.FileName == "") return;

            string oldname = fileName;
            fileName = s.FileName;

            SaveFile(oldname);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen) CloseFile();

            OpenFile();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen == false) return;
            CloseFile();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileOpen == false) return;

            if(listBox1.SelectedIndex == -1)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = -1;
                UpdateConditionFlagsFromValue(0);
            }
            else
            {
                int x = listBox1.SelectedIndex;
                textBox1.Text = pathList[x];
                textBox2.Text = nameList[x];
                comboBox1.SelectedIndex = typeList[x];
                numericUpDown2.Value = costumeIndexList[x];
                numericUpDown1.Value = loadcondList[x];
                UpdateConditionFlagsFromValue(loadcondList[x]);
            }
        }

        // Add entry
        private void button1_Click(object sender, EventArgs e)
        {
            if (fileOpen == false) return;

            pathList.Add("spc");
            nameList.Add("filename");
            typeList.Add(1);
            costumeIndexList.Add(-1);
            loadcondList.Add(1);
            listBox1.Items.Add("spc/filename");
            listBox1.SelectedIndex = pathList.Count() - 1;
        }

        // Remove entry
        private void button2_Click(object sender, EventArgs e)
        {
            if (fileOpen == false || listBox1.SelectedIndex == -1) return;

            int x = listBox1.SelectedIndex;
            pathList.RemoveAt(x);
            nameList.RemoveAt(x);
            typeList.RemoveAt(x);
            costumeIndexList.RemoveAt(x);
            loadcondList.RemoveAt(x);

            if (x == listBox1.Items.Count - 1)
            {
                listBox1.SelectedIndex = x - 1;
                listBox1.Items.RemoveAt(x);
            }
            else
            {
                listBox1.Items.RemoveAt(x);
                listBox1.SelectedIndex = x;
            }
        }

        // Save entry
        private void button3_Click(object sender, EventArgs e)
        {
            if (fileOpen == false || listBox1.SelectedIndex == -1) return;

            int x = listBox1.SelectedIndex;
            pathList[x] = textBox1.Text;
            nameList[x] = textBox2.Text;
            typeList[x] = comboBox1.SelectedIndex;
            costumeIndexList[x] = (int)numericUpDown2.Value;
            loadcondList[x] = BuildConditionValueFromFlags();
            numericUpDown1.Value = loadcondList[x];
            listBox1.Items[x] = textBox1.Text + "/" + textBox2.Text;
        }

        // Save prm name
        private void button4_Click(object sender, EventArgs e)
        {
            if (fileOpen == false || textBox3.Text == "") return;

            prmName = textBox3.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (fileOpen == false || listBox1.SelectedIndex == -1) return;

            int x = listBox1.SelectedIndex;
            string[] payload =
            {
                ClipboardPrefix,
                pathList[x] ?? "",
                nameList[x] ?? "",
                typeList[x].ToString(),
                costumeIndexList[x].ToString(),
                loadcondList[x].ToString()
            };

            Clipboard.SetText(string.Join("\n", payload));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (fileOpen == false) return;
            if (Clipboard.ContainsText() == false) return;

            string[] payload = Clipboard.GetText()
                .Replace("\r\n", "\n")
                .Split(new[] { '\n' }, StringSplitOptions.None);

            if (payload.Length < 6 || payload[0] != ClipboardPrefix)
            {
                MessageBox.Show("Clipboard does not contain a valid PRM load entry.");
                return;
            }

            int typeValue;
            int costumeValue;
            int conditionValue;
            if (!int.TryParse(payload[3], out typeValue) ||
                !int.TryParse(payload[4], out costumeValue) ||
                !int.TryParse(payload[5], out conditionValue))
            {
                MessageBox.Show("Clipboard does not contain a valid PRM load entry.");
                return;
            }

            pathList.Add(payload[1]);
            nameList.Add(payload[2]);
            typeList.Add(typeValue);
            costumeIndexList.Add(costumeValue);
            loadcondList.Add(conditionValue);
            listBox1.Items.Add(payload[1] + "/" + payload[2]);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void saeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen == false) return;

            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileOpen == false) return;

            SaveFileAs();
        }

        private void Tool_SpcloadEditor_Load(object sender, EventArgs e) {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (syncingConditionControls)
                return;
            UpdateConditionFlagsFromValue((int)numericUpDown1.Value);
        }

        private void ConditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshConditionValueFromFlags();
        }
    }
}
