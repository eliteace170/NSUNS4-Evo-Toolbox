using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NSUNS4_Character_Manager
{
    public class Tool_EvoUnlockItemParamEditor : Form
    {
        private const int FileSectionHeaderSize = 0x20;
        private const int FileSectionCountOffset = 0x1C;
        private const int EntryOffset = 0x20;
        private const int EntrySize = 0x18;
        private const int FileSectionSizeOffsetA = 0x0C;
        private const int FileSectionSizeOffsetB = 0x18;

        public bool FileOpen = false;
        public string FilePath = "";
        public byte[] FileBytes = new byte[0];

        public List<byte[]> EntryList = new List<byte[]>();
        public int EntryCount = 0;

        private IContainer components = null;
        private ListBox ListBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem sortToolStripMenuItem;
        private ToolStripMenuItem sortPresetToolStripMenuItem;
        private ToolStripMenuItem sortDLCToolStripMenuItem;
        private ToolStripMenuItem sortRyoToolStripMenuItem;
        private ToolStripMenuItem displayToolStripMenuItem;
        private ToolStripMenuItem displayHexToolStripMenuItem;
        private ToolStripMenuItem displayDecToolStripMenuItem;
        private Button buttonAdd;
        private Button buttonCopy;
        private Button buttonDelete;
        private Button buttonSaveSelected;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox dlcID;
        private ComboBox unlockType;
        private NumericUpDown costumeID;
        private NumericUpDown version;
        private CheckBox checkResetLock;
        private CheckBox checkNortification;
        private TextBox messageName;
        private ToolTip checkToolTip;
        private bool displayAsHex = true;

        private class UnlockTypeOption
        {
            public byte Value { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private class DlcOption
        {
            public byte Value { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private readonly Dictionary<byte, string> unlockTypeLookup = new Dictionary<byte, string>
        {
            { 0x15, "Money" },
            { 0x19, "Costume/Character" },
            { 0x1F, "Costume Material" },
            { 0x20, "Team Ultimate Jutsu" }
        };

        private readonly Dictionary<byte, string> dlcLookup = new Dictionary<byte, string>
        {
            { 0x00, "Pre Order" },
            { 0x01, "The Last" },
            { 0x02, "Story" },
            { 0x03, "unknow" },
            { 0x04, "Traditional Festival Costume" },
            { 0x05, "Shikamaru's Tale" },
            { 0x06, "Gaara's Tale" },
            { 0x07, "The Sound Four" },
            { 0x08, "Season Pass Bonus" },
            { 0x09, "Road to Boruto" },
            { 0x0A, "Next Generation" },
            { 0xFF, "FF - Reset/Lock" }
        };

        public Tool_EvoUnlockItemParamEditor()
        {
            InitializeComponent();
            InitializeDlcItems();
            InitializeUnlockTypeItems();
            UpdateSortMenuStates(sortPresetToolStripMenuItem);
            UpdateDisplayMenuStates(displayHexToolStripMenuItem);
        }

        public void CloseFile()
        {
            FileOpen = false;
            FilePath = "";
            FileBytes = new byte[0];
            EntryList.Clear();
            EntryCount = 0;
            ListBox1.Items.Clear();
            ListBox1.Items.Add("No file loaded...");
            checkResetLock.Checked = false;
            checkNortification.Checked = false;
            if (ListBox1.Items.Count > 0)
            {
                ListBox1.SelectedIndex = -1;
            }
        }

        public void NewFile()
        {
            CloseFile();
            ListBox1.Items.Clear();
            checkResetLock.Checked = false;
            checkNortification.Checked = false;
            FileOpen = true;
        }

        public void OpenFile(string basepath = "")
        {
            InitializeDlcItems();
            InitializeUnlockTypeItems();

            if (FileOpen)
            {
                CloseFile();
            }

            OpenFileDialog o = new OpenFileDialog
            {
                DefaultExt = ".xfbin",
                Filter = "*.xfbin|*.xfbin"
            };

            if (basepath != "")
                o.FileName = basepath;
            else
                o.ShowDialog();

            if (!(o.FileName != "") || !File.Exists(o.FileName))
            {
                return;
            }

            FilePath = o.FileName;
            FileOpen = true;
            FileBytes = File.ReadAllBytes(FilePath);
            EntryList = new List<byte[]>();
            ListBox1.Items.Clear();

            int fileSection = XfbinParser.GetFileSectionIndex(FileBytes);
            EntryCount = Main.b_byteArrayToInt(Main.b_ReadByteArray(FileBytes, fileSection + FileSectionCountOffset, 4));
            int maxEntries = (FileBytes.Length - 20 - fileSection - EntryOffset) / EntrySize;
            if (EntryCount > maxEntries)
                EntryCount = maxEntries;
            if (EntryCount < 0)
                EntryCount = 0;

            for (int x = 0; x < EntryCount; x++)
            {
                int ptr = fileSection + EntryOffset + (x * EntrySize);
                if (ptr + EntrySize > FileBytes.Length)
                    break;

                byte dlc = FileBytes[ptr];
                byte unlock = FileBytes[ptr + 1];
                byte[] costume = Main.b_ReadByteArray(FileBytes, ptr + 2, 2);
                byte[] versionBytes = Main.b_ReadByteArray(FileBytes, ptr + 4, 4);
                byte[] msg = Main.b_ReadByteArray(FileBytes, ptr + 8, 16);
                int msgTerm = Array.IndexOf(msg, (byte)0);
                if (msgTerm != -1)
                    Array.Resize(ref msg, msgTerm);

                byte[] entry = new byte[24];
                entry[0] = dlc;
                entry[1] = unlock;
                entry[2] = costume[0];
                entry[3] = costume[1];
                entry[4] = versionBytes[0];
                entry[5] = versionBytes[1];
                entry[6] = versionBytes[2];
                entry[7] = versionBytes[3];

                for (int a = 0; a < msg.Length; a++)
                    entry[8 + a] = msg[a];

                EntryList.Add(entry);
                ListBox1.Items.Add(BuildListDisplayEntry(entry));
            }
        }

        private string ReadMessageFromInputs()
        {
            return messageName.Text.Trim();
        }

        private byte[] BuildEntryFromInputs()
        {
            byte[] entry = new byte[EntrySize];
            entry[0] = checkResetLock.Checked ? (byte)0xFF : GetDlcValue();
            entry[1] = GetUnlockTypeValue();
            byte[] costume = BitConverter.GetBytes((short)costumeID.Value);
            entry[2] = costume[0];
            entry[3] = costume[1];
            byte[] versionBytes = BitConverter.GetBytes((int)version.Value);
            if (checkNortification.Checked)
                versionBytes = BitConverter.GetBytes(-1);
            entry[4] = versionBytes[0];
            entry[5] = versionBytes[1];
            entry[6] = versionBytes[2];
            entry[7] = versionBytes[3];

            byte[] msg = Encoding.ASCII.GetBytes(ReadMessageFromInputs());
            for (int x = 0; x < msg.Length && x < 16; x++)
            {
                entry[8 + x] = msg[x];
            }

            return entry;
        }

        private void RefreshSelectedPanel()
        {
            if (ListBox1.SelectedIndex < 0 || ListBox1.SelectedIndex >= EntryList.Count)
            {
                return;
            }

            byte[] selected = EntryList[ListBox1.SelectedIndex];
            SetDlcValue(selected[0]);
            SetUnlockTypeValue(selected[1]);
            costumeID.Value = Main.b_byteArrayToIntTwoBytes(new byte[] { selected[2], selected[3] });
            version.Value = BitConverter.ToInt32(selected, 4);
            byte[] msg = Main.b_ReadByteArray(selected, 8, 16);
            int msgTerm = Array.IndexOf(msg, (byte)0);
            if (msgTerm != -1)
                Array.Resize(ref msg, msgTerm);
            messageName.Text = Encoding.ASCII.GetString(msg);
            checkResetLock.CheckedChanged -= checkResetLock_CheckedChanged;
            checkNortification.CheckedChanged -= checkNortification_CheckedChanged;
            checkResetLock.Checked = selected[0] == (byte)0xFF;
            checkNortification.Checked = BitConverter.ToInt32(selected, 4) == -1;
            checkResetLock.CheckedChanged += checkResetLock_CheckedChanged;
            checkNortification.CheckedChanged += checkNortification_CheckedChanged;
            dlcID.Enabled = !checkResetLock.Checked;
        }

        private string UpdateListItem(int index, byte[] entry)
        {
            return BuildListDisplayEntry(entry);
        }

        private string BuildListDisplayEntry(byte[] entry)
        {
            byte dlc = entry[0];
            byte unlock = entry[1];
            int costume = Main.b_byteArrayToIntTwoBytes(new byte[] { entry[2], entry[3] });
            int versionVal = BitConverter.ToInt32(entry, 4);

            string costumeText = displayAsHex ? costume.ToString("X4") : costume.ToString();
            string dlcText = displayAsHex ? dlc.ToString("X2") : dlc.ToString();
            string versionText = displayAsHex ? versionVal.ToString("X8") : versionVal.ToString();
            string unlockText = unlockTypeLookup.ContainsKey(unlock) ? unlockTypeLookup[unlock] : "???";

            return "Preset: " + costumeText + " | DLC: " + dlcText + " | Unlock Type: " + unlockText + " | Ryo: " + versionText;
        }

        private void RefreshListDisplay()
        {
            for (int x = 0; x < EntryList.Count; x++)
            {
                if (x < ListBox1.Items.Count)
                {
                    ListBox1.Items[x] = UpdateListItem(x, EntryList[x]);
                }
            }
        }

        private void InitializeUnlockTypeItems()
        {
            unlockType.Items.Clear();
            for (int typeValue = 0; typeValue <= 0x20; typeValue++)
            {
                byte value = (byte)typeValue;
                string name = "???";
                if (unlockTypeLookup.TryGetValue(value, out string label))
                    name = label;

                unlockType.Items.Add(new UnlockTypeOption
                {
                    Value = value,
                    Name = $"0x{typeValue:X2} - {name}"
                });
            }

            if (unlockType.Items.Count > 0)
            {
                unlockType.SelectedIndex = 0;
            }
        }

        private void InitializeDlcItems()
        {
            dlcID.Items.Clear();
            foreach (KeyValuePair<byte, string> pair in dlcLookup)
            {
                dlcID.Items.Add(new DlcOption
                {
                    Value = pair.Key,
                    Name = pair.Value
                });
            }

            if (dlcID.Items.Count > 0)
            {
                dlcID.SelectedIndex = 0;
            }
        }

        private DlcOption GetDlcOption(byte value)
        {
            foreach (DlcOption option in dlcID.Items)
            {
                if (option.Value == value)
                    return option;
            }

            DlcOption fallback = new DlcOption
            {
                Value = value,
                Name = value == 0xFF ? "FF - Reset/Lock" : $"0x{value:X2} - Unknown"
            };

            dlcID.Items.Add(fallback);
            return fallback;
        }

        private void SetDlcValue(byte value)
        {
            dlcID.SelectedItem = GetDlcOption(value);
        }

        private byte GetDlcValue()
        {
            if (dlcID.SelectedItem is DlcOption selectedOption)
            {
                return selectedOption.Value;
            }

            return 0;
        }

        private UnlockTypeOption GetUnlockTypeOption(byte value)
        {
            foreach (UnlockTypeOption option in unlockType.Items)
            {
                if (option.Value == value)
                    return option;
            }

            string name = "???" ;
            UnlockTypeOption fallback = new UnlockTypeOption
            {
                Value = value,
                Name = $"0x{value:X2} - {name}"
            };

            unlockType.Items.Add(fallback);
            return fallback;
        }

        private void SetUnlockTypeValue(byte value)
        {
            unlockType.SelectedItem = GetUnlockTypeOption(value);
        }

        private byte GetUnlockTypeValue()
        {
            if (unlockType.SelectedItem is UnlockTypeOption selectedOption)
            {
                return selectedOption.Value;
            }

            return 0;
        }

        public void AddEntry()
        {
            EntryList.Add(BuildEntryFromInputs());
            ListBox1.Items.Add(UpdateListItem(EntryList.Count - 1, EntryList[EntryList.Count - 1]));
            EntryCount = EntryList.Count;
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
            ListBox1.SelectedIndex = EntryList.Count - 1;
        }

        public void CopyEntry()
        {
            if (ListBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            byte[] selected = new byte[EntrySize];
            EntryList[ListBox1.SelectedIndex].CopyTo(selected, 0);
            EntryList.Add(selected);
            ListBox1.Items.Add(UpdateListItem(EntryList.Count - 1, selected));
            EntryCount = EntryList.Count;
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
        }

        public void RemoveEntry()
        {
            if (ListBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No entry selected.");
                return;
            }

            int index = ListBox1.SelectedIndex;
            EntryList.RemoveAt(index);
            ListBox1.Items.RemoveAt(index);
            EntryCount = EntryList.Count;
            if (ListBox1.Items.Count > 0)
            {
                if (index >= ListBox1.Items.Count)
                    index = ListBox1.Items.Count - 1;
                if (index >= 0)
                    ListBox1.SelectedIndex = index;
            }
            else
            {
                ListBox1.SelectedIndex = -1;
            }
        }

        public byte[] ConvertToFile()
        {
            List<byte> file = new List<byte>();
            int fileSection = XfbinParser.GetFileSectionIndex(FileBytes);
            for (int x = 0; x < fileSection; x++)
                file.Add(FileBytes[x]);

            byte[] sectionHeader = new byte[FileSectionHeaderSize];
            for (int x = 0; x < FileSectionHeaderSize && (fileSection + x) < FileBytes.Length; x++)
                sectionHeader[x] = FileBytes[fileSection + x];

            byte[] countBytes = BitConverter.GetBytes(EntryList.Count);
            for (int x = 0; x < 4; x++)
                sectionHeader[FileSectionCountOffset + x] = countBytes[x];
            int entryDataSize = EntryList.Count * EntrySize;
            byte[] sizeBytesA = BitConverter.GetBytes(entryDataSize + 8);
            byte[] sizeBytesB = BitConverter.GetBytes(entryDataSize + 4);
            Array.Reverse(sizeBytesA);
            Array.Reverse(sizeBytesB);
            for (int x = 0; x < 4; x++)
            {
                sectionHeader[FileSectionSizeOffsetA + x] = sizeBytesA[x];
                sectionHeader[FileSectionSizeOffsetB + x] = sizeBytesB[x];
            }

            for (int x = 0; x < sectionHeader.Length; x++)
                file.Add(sectionHeader[x]);

            foreach (byte[] entry in EntryList)
                file.AddRange(entry);

            byte[] finalBytes = new byte[20];
            if (FileBytes.Length >= 20)
            {
                for (int x = 0; x < 20; x++)
                    finalBytes[x] = FileBytes[FileBytes.Length - 20 + x];
            }
            file.AddRange(finalBytes);

            return file.ToArray();
        }

        public void SaveFile()
        {
            if (FilePath != "")
            {
                if (File.Exists(FilePath + ".backup"))
                {
                    File.Delete(FilePath + ".backup");
                }
                File.Copy(FilePath, FilePath + ".backup");
                File.WriteAllBytes(FilePath, ConvertToFile());
                MessageBox.Show("File saved to " + FilePath + ".");
            }
            else
            {
                SaveFileAs();
            }
        }

        public void SaveFileAs(string basepath = "")
        {
            SaveFileDialog s = new SaveFileDialog
            {
                DefaultExt = ".xfbin",
                Filter = "*.xfbin|*.xfbin"
            };

            if (basepath != "")
                s.FileName = basepath;
            else
                s.ShowDialog();

            if (!(s.FileName != ""))
            {
                return;
            }

            if (s.FileName == FilePath)
            {
                if (File.Exists(FilePath + ".backup"))
                {
                    File.Delete(FilePath + ".backup");
                }
                File.Copy(FilePath, FilePath + ".backup");
            }
            else
            {
                FilePath = s.FileName;
            }
            File.WriteAllBytes(FilePath, ConvertToFile());
            MessageBox.Show("File saved to " + FilePath + ".");
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSelectedPanel();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen && MessageBox.Show("Are you sure you want to open another file?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                OpenFile();
            }
            else if (!FileOpen)
            {
                OpenFile();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen)
                SaveFile();
            else
                MessageBox.Show("No file loaded...");
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen)
                SaveFileAs();
            else
                MessageBox.Show("No file loaded...");
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                CloseFile();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                AddEntry();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                CopyEntry();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                RemoveEntry();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }

        private void buttonSaveSelected_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                UpdateListItemData();
            }
            else
            {
                MessageBox.Show("No file loaded...");
            }
        }

        private void UpdateListItemData()
        {
            if (ListBox1.SelectedIndex < 0 || ListBox1.SelectedIndex >= EntryList.Count)
            {
                return;
            }

            int index = ListBox1.SelectedIndex;
            EntryList[index] = BuildEntryFromInputs();
            ListBox1.Items[index] = UpdateListItem(index, EntryList[index]);
            ListBox1.SelectedIndex = index;
        }

        private void ApplySort(string sortType)
        {
            if (!FileOpen || EntryList.Count == 0)
            {
                return;
            }

            byte[] selectedEntry = (ListBox1.SelectedIndex >= 0 && ListBox1.SelectedIndex < EntryList.Count)
                ? EntryList[ListBox1.SelectedIndex]
                : null;

            EntryList.Sort((a, b) =>
            {
                switch (sortType)
                {
                    case "DLC":
                        return a[0].CompareTo(b[0]);
                    case "Ryo":
                        return BitConverter.ToInt32(a, 4).CompareTo(BitConverter.ToInt32(b, 4));
                    default:
                        return Main.b_byteArrayToIntTwoBytes(new byte[] { a[2], a[3] }).CompareTo(
                            Main.b_byteArrayToIntTwoBytes(new byte[] { b[2], b[3] }));
                }
            });

            RefreshListDisplayOnly();

            if (selectedEntry != null)
            {
                for (int x = 0; x < EntryList.Count; x++)
                {
                    if (EntriesMatch(selectedEntry, EntryList[x]))
                    {
                        ListBox1.SelectedIndex = x;
                        return;
                    }
                }
            }

            if (EntryList.Count > 0)
            {
                ListBox1.SelectedIndex = 0;
                RefreshSelectedPanel();
            }
        }

        private bool EntriesMatch(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshListDisplayOnly()
        {
            ListBox1.Items.Clear();
            for (int x = 0; x < EntryList.Count; x++)
            {
                ListBox1.Items.Add(UpdateListItem(x, EntryList[x]));
            }
        }

        private void ApplyDisplayMode(bool asHex)
        {
            if (!FileOpen && ListBox1.Items.Count == 0)
            {
                return;
            }

            displayAsHex = asHex;
            costumeID.Hexadecimal = displayAsHex;
            version.Hexadecimal = displayAsHex;
            UpdateDisplayMenuStates(displayAsHex ? displayHexToolStripMenuItem : displayDecToolStripMenuItem);
            RefreshListDisplay();
            RefreshSelectedPanel();
        }

        private void displayHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyDisplayMode(true);
        }

        private void displayDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyDisplayMode(false);
        }

        private void sortPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplySort("Preset");
            UpdateSortMenuStates(sortPresetToolStripMenuItem);
        }

        private void sortDLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplySort("DLC");
            UpdateSortMenuStates(sortDLCToolStripMenuItem);
        }

        private void sortRyoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplySort("Ryo");
            UpdateSortMenuStates(sortRyoToolStripMenuItem);
        }

        private void UpdateSortMenuStates(ToolStripMenuItem selectedItem)
        {
            sortPresetToolStripMenuItem.Checked = false;
            sortDLCToolStripMenuItem.Checked = false;
            sortRyoToolStripMenuItem.Checked = false;
            if (selectedItem != null)
                selectedItem.Checked = true;
        }

        private void UpdateDisplayMenuStates(ToolStripMenuItem selectedItem)
        {
            displayHexToolStripMenuItem.Checked = false;
            displayDecToolStripMenuItem.Checked = false;
            if (selectedItem != null)
                selectedItem.Checked = true;
        }

        private void checkResetLock_CheckedChanged(object sender, EventArgs e)
        {
            if (checkResetLock.Checked)
            {
                SetDlcValue(0xFF);
                dlcID.Enabled = false;
            }
            else
            {
                dlcID.Enabled = true;
                if (dlcID.SelectedIndex < 0 && dlcID.Items.Count > 0)
                    dlcID.SelectedIndex = 0;
            }
        }

        private void checkNortification_CheckedChanged(object sender, EventArgs e)
        {
            if (checkNortification.Checked)
                version.Value = -1;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ListBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortDLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortRyoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSaveSelected = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dlcID = new System.Windows.Forms.ComboBox();
            this.unlockType = new System.Windows.Forms.ComboBox();
            this.costumeID = new System.Windows.Forms.NumericUpDown();
            this.version = new System.Windows.Forms.NumericUpDown();
            this.checkResetLock = new System.Windows.Forms.CheckBox();
            this.checkNortification = new System.Windows.Forms.CheckBox();
            this.messageName = new System.Windows.Forms.TextBox();
            this.checkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costumeID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.version)).BeginInit();
            this.SuspendLayout();
            // 
            // ListBox1
            // 
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.Location = new System.Drawing.Point(3, 31);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(430, 355);
            this.ListBox1.TabIndex = 0;
            this.ListBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortPresetToolStripMenuItem,
            this.sortDLCToolStripMenuItem,
            this.sortRyoToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // sortPresetToolStripMenuItem
            // 
            this.sortPresetToolStripMenuItem.Name = "sortPresetToolStripMenuItem";
            this.sortPresetToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.sortPresetToolStripMenuItem.Text = "Preset";
            this.sortPresetToolStripMenuItem.Click += new System.EventHandler(this.sortPresetToolStripMenuItem_Click);
            // 
            // sortDLCToolStripMenuItem
            // 
            this.sortDLCToolStripMenuItem.Name = "sortDLCToolStripMenuItem";
            this.sortDLCToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.sortDLCToolStripMenuItem.Text = "DLC";
            this.sortDLCToolStripMenuItem.Click += new System.EventHandler(this.sortDLCToolStripMenuItem_Click);
            // 
            // sortRyoToolStripMenuItem
            // 
            this.sortRyoToolStripMenuItem.Name = "sortRyoToolStripMenuItem";
            this.sortRyoToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.sortRyoToolStripMenuItem.Text = "Ryo";
            this.sortRyoToolStripMenuItem.Click += new System.EventHandler(this.sortRyoToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayHexToolStripMenuItem,
            this.displayDecToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // displayHexToolStripMenuItem
            // 
            this.displayHexToolStripMenuItem.Name = "displayHexToolStripMenuItem";
            this.displayHexToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.displayHexToolStripMenuItem.Text = "HEX";
            this.displayHexToolStripMenuItem.Click += new System.EventHandler(this.displayHexToolStripMenuItem_Click);
            // 
            // displayDecToolStripMenuItem
            // 
            this.displayDecToolStripMenuItem.Name = "displayDecToolStripMenuItem";
            this.displayDecToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.displayDecToolStripMenuItem.Text = "DEC";
            this.displayDecToolStripMenuItem.Click += new System.EventHandler(this.displayDecToolStripMenuItem_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(438, 288);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(145, 23);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(438, 317);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(145, 23);
            this.buttonCopy.TabIndex = 5;
            this.buttonCopy.Text = "Duplicate";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(590, 317);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(145, 23);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSaveSelected
            // 
            this.buttonSaveSelected.Location = new System.Drawing.Point(590, 288);
            this.buttonSaveSelected.Name = "buttonSaveSelected";
            this.buttonSaveSelected.Size = new System.Drawing.Size(145, 23);
            this.buttonSaveSelected.TabIndex = 7;
            this.buttonSaveSelected.Text = "Save Selected";
            this.buttonSaveSelected.UseVisualStyleBackColor = true;
            this.buttonSaveSelected.Click += new System.EventHandler(this.buttonSaveSelected_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(439, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "DLC";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(435, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Unlock Type";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(439, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Preset:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(436, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ryo";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(435, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "MessageInfoString (16 bytes)";
            // 
            // dlcID
            // 
            this.dlcID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dlcID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dlcID.FormattingEnabled = true;
            this.dlcID.Location = new System.Drawing.Point(476, 164);
            this.dlcID.Name = "dlcID";
            this.dlcID.Size = new System.Drawing.Size(264, 21);
            this.dlcID.TabIndex = 8;
            this.dlcID.SelectedIndexChanged += new System.EventHandler(this.dlcID_SelectedIndexChanged);
            // 
            // unlockType
            // 
            this.unlockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unlockType.Location = new System.Drawing.Point(512, 76);
            this.unlockType.Name = "unlockType";
            this.unlockType.Size = new System.Drawing.Size(228, 21);
            this.unlockType.TabIndex = 6;
            this.unlockType.SelectedIndexChanged += new System.EventHandler(this.unlockType_SelectedIndexChanged);
            // 
            // costumeID
            // 
            this.costumeID.Hexadecimal = true;
            this.costumeID.Location = new System.Drawing.Point(512, 31);
            this.costumeID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.costumeID.Name = "costumeID";
            this.costumeID.Size = new System.Drawing.Size(228, 23);
            this.costumeID.TabIndex = 4;
            // 
            // version
            // 
            this.version.Hexadecimal = true;
            this.version.Location = new System.Drawing.Point(476, 119);
            this.version.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.version.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(264, 23);
            this.version.TabIndex = 2;
            this.version.ValueChanged += new System.EventHandler(this.version_ValueChanged);
            // 
            // checkResetLock
            // 
            this.checkResetLock.AutoSize = true;
            this.checkResetLock.Location = new System.Drawing.Point(439, 193);
            this.checkResetLock.Name = "checkResetLock";
            this.checkResetLock.Size = new System.Drawing.Size(84, 19);
            this.checkResetLock.TabIndex = 11;
            this.checkResetLock.Text = "Reset/Lock";
            this.checkToolTip.SetToolTip(this.checkResetLock, "Will reset that unlock slot in the save (and if the unlock type is 0x19, lock the" +
        " costume as well)");
            this.checkResetLock.UseVisualStyleBackColor = true;
            this.checkResetLock.CheckedChanged += new System.EventHandler(this.checkResetLock_CheckedChanged);
            // 
            // checkNortification
            // 
            this.checkNortification.AutoSize = true;
            this.checkNortification.Location = new System.Drawing.Point(439, 218);
            this.checkNortification.Name = "checkNortification";
            this.checkNortification.Size = new System.Drawing.Size(89, 19);
            this.checkNortification.TabIndex = 12;
            this.checkNortification.Text = "Notification";
            this.checkToolTip.SetToolTip(this.checkNortification, "Will unlock the character without adding it to the notification list");
            this.checkNortification.UseVisualStyleBackColor = true;
            this.checkNortification.CheckedChanged += new System.EventHandler(this.checkNortification_CheckedChanged);
            // 
            // messageName
            // 
            this.messageName.Location = new System.Drawing.Point(439, 259);
            this.messageName.MaxLength = 16;
            this.messageName.Name = "messageName";
            this.messageName.Size = new System.Drawing.Size(300, 23);
            this.messageName.TabIndex = 0;
            // 
            // Tool_EvoUnlockItemParamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 410);
            this.Controls.Add(this.messageName);
            this.Controls.Add(this.checkNortification);
            this.Controls.Add(this.checkResetLock);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.version);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.costumeID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.unlockType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dlcID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSaveSelected);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.ListBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Tool_EvoUnlockItemParamEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Evo Unlock  Item Param Editor";
            this.Load += new System.EventHandler(this.Tool_EvoUnlockItemParamEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costumeID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.version)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Tool_EvoUnlockItemParamEditor_Load(object sender, EventArgs e)
        {
            if (File.Exists(Main.unlockEvoItemParamPath))
            {
                displayAsHex = true;
                InitializeDlcItems();
                InitializeUnlockTypeItems();
                costumeID.Hexadecimal = true;
                version.Hexadecimal = true;
                UpdateDisplayMenuStates(displayHexToolStripMenuItem);
                OpenFile(Main.unlockEvoItemParamPath);
            }
            else
            {
                ListBox1.Items.Clear();
                ListBox1.Items.Add("No file loaded...");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void unlockType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dlcID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void version_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
